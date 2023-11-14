using DemosPlus.Modules;
using DemosPlus.Json;
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
using DemosPlus.Url;

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
            var arr = Enum.GetValues(typeof(Item));
            List<object> items = new List<object>();
            foreach (var i in arr)
            {
                items.Add(i);
            }
            dropItem.Items.AddRange(items.ToArray());
        }

        private void TestBtn_Click(object sender, EventArgs e)
        {

            var url = QUrlManager.Instance.GetPricesAvgUrl(
                new List<Item> { Item.T4_PLANKS, Item.T5_PLANKS },
                new List<City> { City.Caerleon, City.Thetford },
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
            var avg = JsonManager.GetPricesAvg(result);

            url = QUrlManager.Instance.GetBuyMaxPricesUrl(
                new List<Item> { Item.T4_PLANKS, Item.T5_PLANKS },
                new List<City> { City.Caerleon, City.Thetford },
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
            result = QNetwork.Instance.GetResult(url);
            var max = JsonManager.GetBuyMaxPrices(result);

            var label = "";
            foreach (var a in max)
            {
                label += a.buy_price_max.ToString();
                label += '\n';
            }

            label1.Text = label;

        }

    }
}
