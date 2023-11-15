using DemosPlus.Modules;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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

            List<string> tiers = new List<string>();
            List<string> result = new List<string>();

            if (tierMin == 0 || tierMax == 0)
            {
                tiers.Add(itemKey);
            }
            else
            {
                for (int i = tierMin; i <= tierMax; ++i)
                {
                    var withTier = $"T{i}_" + itemKey;
                    tiers.Add(withTier);
                }
            }

            if (enchantMin == 0 || enchantMax == 0)
            {
                for (int i = 0; i < tiers.Count; ++i)
                {
                    result.Add(tiers[i]);
                }
            }
            else
            {
                var enchantType = QExcelUtil.Instance.GetEnchantType(itemKey);

                result.AddRange(tiers);

                for (int i = enchantMin; i <= enchantMax; ++i)
                {
                    for (int j = 0; j < tiers.Count; ++j)
                    {
                        var name = tiers[i];
                        name += $"{(enchantType == 1 ? $"@{i}" : $"_LEVEL{i}@{i}")}";
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

        #endregion

        #region Event

        private void OnClickCalculate(object sender, EventArgs e)
        {

            var url = QUrlManager.Instance.GetPricesAvgUrl(
                GetItems(dropItem.Text),
                GetCitys(),
                new UrlDate
                {
                    day = 9,
                    month = 11,
                    year = 2023,
                },
                new UrlDate
                {
                    day = 11,
                    month = 11,
                    year = 2023,
                },
                Quality.None);

            var result = QNetwork.Instance.GetResult(url);
            var avg = QJsonManager.Instance.GetPricesAvg(result);

            //url = QUrlManager.Instance.GetBuyMaxPricesUrl(
            //    GetItems(dropItem.Text),
            //    new List<City> { City.Caerleon, City.Thetford },
            //    new UrlDate
            //    {
            //        day = 9,
            //        month = 11,
            //        year = 2023,
            //    },
            //    new UrlDate
            //    {
            //        day = 11,
            //        month = 11,
            //        year = 2023,
            //    },
            //    Quality.None);
            //result = QNetwork.Instance.GetResult(url);
            //var max = QJsonManager.Instance.GetBuyMaxPrices(result);

            var sb = new StringBuilder();
            sb.Append(url);

            foreach (var a in avg)
            {
                sb.Append('\n');
                sb.Append('\n');

                sb.Append(a.city);
                sb.Append(' ');
                sb.Append(a.quality);
                sb.Append(' ');
                sb.Append(a.item);
                sb.Append('\n');

                int index = 0;
                foreach (var p in a.prices_avg)
                {
                    ++index;
                    sb.Append(p);
                    sb.Append(' ');

                    if (index == 4)
                    {
                        sb.Append('\n');
                        index = 0;
                    }
                }

            }

            label1.Text = sb.ToString();

        }

        #endregion

    }
}
