using DemosPlus.Modules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DemosPlus
{
    public partial class MainWindow : Form
    {
        private Tab _currentTab;

        public MainWindow()
        {
            InitializeComponent();

            resourceTab.Checked = true;
        }

        #region Init

        private void InitDuration()
        {
            dropDuration.Items.Clear();
            var arr = Enum.GetValues(typeof(Duration));
            List<object> durations = new List<object>();
            foreach (var i in arr)
            {
                durations.Add(i);
            }
            dropDuration.Items.AddRange(durations.ToArray());
            dropDuration.SelectedIndex = 0;
        }

        private void InitReturn()
        {
            if (_currentTab == Tab.Cost || _currentTab == Tab.Profit)
            {
                inputReturn.Enabled = true;
                inputReturn.Text = 0.248f.ToString();
            }
            else
            {
                inputReturn.Enabled = false;
            }
        }

        private void InitTax()
        {
            if (_currentTab == Tab.Profit)
            {
                dropTax.Enabled = true;
                dropTax.Items.Clear();
                var arr = Enum.GetValues(typeof(Tax));
                List<object> taxs = new List<object>();
                foreach (var i in arr)
                {
                    taxs.Add(i);
                }
                dropTax.Items.AddRange(taxs.ToArray());
                dropTax.SelectedIndex = 0;
            }
            else
            {
                dropTax.Enabled = false;
            }
        }

        private void InitSaleMode()
        {
            if (_currentTab == Tab.Profit)
            {
                dropSaleMode.Enabled = true;
                dropSaleMode.Items.Clear();
                var arr = Enum.GetValues(typeof(SaleMode));
                List<object> saleModes = new List<object>();
                foreach (var i in arr)
                {
                    saleModes.Add(i);
                }
                dropSaleMode.Items.AddRange(saleModes.ToArray());
                dropSaleMode.SelectedIndex = 0;
            }
            else
            {
                dropSaleMode.Enabled = false;
            }
        }

        private void InitItem()
        {
            List<ConfigItem> items = null;

            switch (_currentTab)
            {
                case Tab.Resource:
                    items = QExcelUtil.Instance.GetConfigItems(ItemType.Resource);
                    break;
                case Tab.Artifact:
                    items = QExcelUtil.Instance.GetConfigItems(ItemType.Artifact);
                    break;
                case Tab.Gear:
                case Tab.Cost:
                case Tab.Profit:
                    items = QExcelUtil.Instance.GetConfigItems(ItemType.Gear);
                    break;
            }

            dropItem.Items.Clear();
            dropItem.Items.AddRange(items.ToArray());
        }

        #endregion

        #region Tab

        private void SelectTab(Tab tab)
        {
            _currentTab = tab;

            InitDuration();
            InitReturn();
            InitTax();
            InitSaleMode();
            InitItem();
        }


        private void OnClickResourceTab(object sender, EventArgs e)
        {
            SelectTab(Tab.Resource);
        }

        private void OnClickGearTab(object sender, EventArgs e)
        {
            SelectTab(Tab.Gear);
        }

        private void OnClickArtifactTab(object sender, EventArgs e)
        {
            SelectTab(Tab.Artifact);
        }

        private void OnClickProfitTab(object sender, EventArgs e)
        {
            SelectTab(Tab.Profit);
        }

        private void OnClickCostTab(object sender, EventArgs e)
        {
            SelectTab(Tab.Cost);
        }

        private void OnSelectItem(object sender, EventArgs e)
        {
            var item = (ConfigItem)dropItem.SelectedItem;
            foreach (var name in item.nameMap)
            {
                txtItemName.Text = name.Key;
                break;
            }
        }

        #endregion

        #region Core

        private void CalculatePrice()
        {
            if (dropDuration.SelectedItem == null)
            {
                return;
            }
            var duration = (Duration)dropDuration.SelectedItem;
            var config = (ConfigItem)dropItem.SelectedItem;
            var avgMap = QCalculator.Instance.GetAvgPrices(config.key, duration);

            if (avgMap == null)
            {
                return;
            }

            dumpView.DataSource = null;
            dumpView.Columns.Clear();
            dumpView.Rows.Clear();

            var citys = QExcelUtil.Instance.GetCitys();
            foreach (var city in citys)
            {
                dumpView.Columns.Add(city.ToString(), city.ToString());
            }
            dumpView.Columns.Add("Average", "Average");

            var items = QExcelUtil.Instance.GetItems(config.key);
            foreach (var item in items)
            {
                var rowView = new DataGridViewRow();
                rowView.HeaderCell.Value = item;

                var rowIndex = dumpView.Rows.Add(rowView);
                var row = dumpView.Rows[rowIndex];

                var columnIndex = 0;
                double sum = 0d;
                int count = 0;

                foreach (var city in citys)
                {
                    avgMap.TryGetValue((item, city), out var avgPrice);
                    if (avgPrice.price > 0d)
                    {
                        row.Cells[columnIndex].Value = avgPrice.price.ToString("f2");

                        if (city != City.Caerleon && city != City.BlackMarket)
                        {
                            sum += avgPrice.price;
                            ++count;
                        }
                    }

                    ++columnIndex;
                }

                row.Cells[columnIndex].Value = (sum / count).ToString("f2");
            }

            dumpView.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        }

        private void CalculateCost()
        {
            if (!double.TryParse(inputReturn.Text, out var returnRate))
            {
                MessageBox.Show("请输入正确的返还率");
                return;
            }

            var duration = (Duration)dropDuration.SelectedItem;

            var config = (ConfigItem)dropItem.SelectedItem;
            var costMap = QCalculator.Instance.CalCost(config.key, returnRate, duration);

            if (costMap == null)
            {
                return;
            }

            dumpView.DataSource = null;
            dumpView.Columns.Clear();
            dumpView.Rows.Clear();
            dumpView.Columns.Add("Price1", "Price1");
            dumpView.Columns.Add("Price2", "Price2");
            dumpView.Columns.Add("Price3", "Price3");
            dumpView.Columns.Add("Price4", "Price4");

            var items = QExcelUtil.Instance.GetItems(config.key);
            foreach (var item in items)
            {
                var rowView = new DataGridViewRow();
                rowView.HeaderCell.Value = item;

                var rowIndex = dumpView.Rows.Add(rowView);
                var row = dumpView.Rows[rowIndex];

                if (!costMap.TryGetValue(item, out var costs))
                {
                    continue;
                }

                for (int i = 0; i < costs.Count; ++i)
                {
                    row.Cells[i].Value = costs[i].ToString("f2");
                }
            }

            dumpView.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

        }

        private void CalculateProfit()
        {
            if (!double.TryParse(inputReturn.Text, out var returnRate))
            {
                MessageBox.Show("请输入正确的返还率");
                return;
            }

            var duration = (Duration)dropDuration.SelectedItem;
            var saleMode = (SaleMode)dropSaleMode.SelectedItem;
            var tax = (Tax)dropTax.SelectedItem;

            var config = (ConfigItem)dropItem.SelectedItem;
            var profitMap = QCalculator.Instance.CalProfit(config.key, saleMode, returnRate, duration, tax);
            if (profitMap == null)
            {
                return;
            }

            dumpView.DataSource = null;
            dumpView.Columns.Clear();
            dumpView.Rows.Clear();

            var citys = QExcelUtil.Instance.GetCitys();
            int blackSalesIndex = -1;
            foreach (var city in citys)
            {
                dumpView.Columns.Add(city.ToString(), city.ToString());

                if (city == City.BlackMarket)
                {
                    blackSalesIndex = dumpView.Columns.Add("BalckSales", "BalckSales");
                }
            }

            var items = QExcelUtil.Instance.GetItems(config.key);
            foreach (var item in items)
            {
                var rowView = new DataGridViewRow();
                rowView.HeaderCell.Value = item;

                var rowIndex = dumpView.Rows.Add(rowView);
                var row = dumpView.Rows[rowIndex];

                for (int i = 0; i < citys.Count; ++i)
                {
                    if (profitMap.TryGetValue((item, citys[i]), out var profit))
                    {
                        row.Cells[i].Value = profit.percent.ToString("f2");
                    }
                    else
                    {
                        row.Cells[i].Value = "";
                    }

                    if (citys[i] == City.BlackMarket)
                    {
                        row.Cells[blackSalesIndex].Value = profit.number;
                    }
                }
            }

            dumpView.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        }

        #endregion

        #region Tool

        #endregion

        #region Event

        private void OnClickCalculate(object sender, EventArgs e)
        {
            dumpView.DataSource = null;
            dumpView.Columns.Clear();
            dumpView.Rows.Clear();

            switch (_currentTab)
            {
                case Tab.Resource:
                case Tab.Artifact:
                case Tab.Gear:
                    CalculatePrice();
                    break;
                case Tab.Cost:
                    CalculateCost();
                    break;
                case Tab.Profit:
                    CalculateProfit();
                    break;
            }

        }

        #endregion

        #region Enum

        public enum Tab
        {
            Resource,
            Artifact,
            Gear,
            Cost,
            Profit,
        }

        #endregion

    }

}
