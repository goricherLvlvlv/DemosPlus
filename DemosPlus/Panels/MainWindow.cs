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
        public MainWindow()
        {
            InitializeComponent();

            InitDuration();
            InitReturn();
            InitTax();
            InitItem();

            var itemKey = "2H_DUALAXE_KEEPER";
            var mode = SaleMode.Sell;

            var profit = QCalculator.Instance.CalProfit(itemKey, mode, Duration.OneMonth);

        }

        private void InitDuration()
        {
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
            inputReturn.Text = 0.248f.ToString();
        }

        private void InitTax()
        {
            var arr = Enum.GetValues(typeof(Tax));
            List<object> taxs = new List<object>();
            foreach (var i in arr)
            {
                taxs.Add(i);
            }
            dropTax.Items.AddRange(taxs.ToArray());
            dropTax.SelectedIndex = 0;
        }

        private void InitItem()
        {
            var items = QExcelUtil.Instance.GetItemKeys();
            dropItem.Items.AddRange(items.ToArray());
        }

        #region Core

        #endregion

        #region Tool

        #endregion

        #region Event

        private void OnClickCalculate(object sender, EventArgs e)
        {
            if (dropDuration.SelectedItem == null)
            {
                return;
            }
            var duration = (Duration)dropDuration.SelectedItem;
            var avgMap = QCalculator.Instance.GetAvgPrices(dropItem.Text, duration);

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

            var items = QExcelUtil.Instance.GetItems(dropItem.Text);
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
                    if (avgPrice > 0d)
                    {
                        row.Cells[columnIndex].Value = avgPrice.ToString("f2");

                        if (city != City.Caerleon && city != City.BlackMarket)
                        {
                            sum += avgPrice;
                            ++count;
                        }
                    }

                    ++columnIndex;
                }

                row.Cells[columnIndex].Value = (sum / count).ToString("f2");
            }

            dumpView.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

        }

        #endregion

    }

}
