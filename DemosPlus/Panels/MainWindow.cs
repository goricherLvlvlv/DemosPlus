using DemosPlus.Modules;
using System;
using System.Collections.Generic;
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
            var items = QExcelUtil.Instance.GetItems();
            dropItem.Items.AddRange(items.ToArray());
        }

        #region Tool

        private List<string> GetItems(string itemKey)
        {
            var (tierMin, tierMax, enchantMin, enchantMax) = QExcelUtil.Instance.GetValues(itemKey);

            List<(string name, bool hasEnchant)> tiers = new List<(string, bool)>();
            List<string> result = new List<string>();

            if (tierMin == 0 || tierMax == 0)
            {
                tiers.Add((itemKey, false));
            }
            else
            {
                for (int i = tierMin; i <= tierMax; ++i)
                {
                    var withTier = $"T{i}_" + itemKey;
                    tiers.Add((withTier, i > 3));
                }
            }

            if (enchantMin == 0 || enchantMax == 0)
            {
                for (int i = 0; i < tiers.Count; ++i)
                {
                    result.Add(tiers[i].name);
                }
            }
            else
            {
                var enchantType = QExcelUtil.Instance.GetEnchantType(itemKey);

                for (int i = 0; i < tiers.Count; ++i)
                {
                    result.Add(tiers[i].name);

                    if (!tiers[i].hasEnchant)
                    {
                        continue;
                    }

                    for (int j = enchantMin; j <= enchantMax; ++j)
                    {
                        var name = tiers[i].name;
                        name += $"{(enchantType == 1 ? $"@{j}" : $"_LEVEL{j}@{j}")}";
                        result.Add(name);
                    }
                }
            }

            return result;

        }

        private List<City> GetCitys()
        {
            return new List<City> { City.Thetford, City.BridgeWatch, City.Martlock, City.Lymhurst, City.FortSterling, City.Caerleon };
        }

        private int GetDays(Duration duration)
        {
            switch (duration)
            {
                case Duration.OneDay: return 1;
                case Duration.SevenDays: return 7;
                case Duration.TwoWeeks: return 14;
                case Duration.ThreeWeeks: return 21;
                case Duration.OneMonth: return 30;
                default: return 1;
            }
        }

        private (UrlDate start, UrlDate end) GetUrlDate(Duration duration)
        {
            var today = DateTime.Now;
            int days = GetDays(duration);
            var startDay = today - new TimeSpan(days, 0, 0, 0);

            var start = new UrlDate()
            {
                day = startDay.Day,
                month = startDay.Month,
                year = startDay.Year,
            };

            var end = new UrlDate()
            {
                day = today.Day,
                month = today.Month,
                year = today.Year,
            };

            return (start, end);
        }

        private Dictionary<(string item, City city), double> ProcessAvg(List<JsonPricesAvg> avgs)
        {
            var map = new Dictionary<(string item, City city), double>();
            var map2 = new Dictionary<(string item, City city), int>();
            foreach (var avg in avgs)
            {
                var key = (avg.item, GetCityByName(avg.city));
                if (!map.ContainsKey(key))
                {
                    map[key] = 0d;
                    map2[key] = 0;
                }

                map[key] += avg.GetAverage();
                map2[key] += 1;
            }

            var res = new Dictionary<(string item, City city), double>();
            foreach (var m in map)
            {
                res[m.Key] = map[m.Key] / map2[m.Key];
            }
            return res;
        }

        private City GetCityByName(string name)
        {
            if (Enum.TryParse<City>(name, true, out var city))
            {
                return city;
            }

            Regex portalRegex = new Regex(@" Portal$");
            if (portalRegex.IsMatch(name))
            {
                name = name.Substring(0, name.Length - 7);
                if (Enum.TryParse<City>(name, true, out city))
                {
                    return city;
                }
            }

            if (name.Equals("Fort Sterling", StringComparison.OrdinalIgnoreCase))
            {
                return City.FortSterling;
            }

            return City.None;
        }

        #endregion

        #region Event

        private void OnClickCalculate(object sender, EventArgs e)
        {
            if (dropDuration.SelectedItem == null)
            {
                return;
            }
            var duration = (Duration)dropDuration.SelectedItem;

            var items = GetItems(dropItem.Text);
            if (items == null || items.Count <= 0)
            {
                return;
            }

            var (startDate, endDate) = GetUrlDate(duration);

            var quality = Quality.None;
            var citys = GetCitys();

            var url = QUrlManager.Instance.GetPricesAvgUrl(items, citys, startDate, endDate, quality);

            var result = QNetwork.Instance.GetResult(url);
            var avgList = QJsonManager.Instance.GetPricesAvg(result);
            avgList.Sort((a, b) =>
            {
                if (a.city != b.city)
                {
                    return string.Compare(a.city, b.city);
                }

                if (a.item == b.item) return 0;
                return string.Compare(a.item, b.item);
            });

            var avgMap = ProcessAvg(avgList);

            dumpView.DataSource = null;
            dumpView.Columns.Clear();
            dumpView.Rows.Clear();

            foreach (var city in citys)
            {
                dumpView.Columns.Add(city.ToString(), city.ToString());
            }
            dumpView.Columns.Add("Average", "Average");

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

                        if (city != City.Caerleon)
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
