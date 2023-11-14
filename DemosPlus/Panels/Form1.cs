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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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

            var label = "";
            foreach (var a in avg)
            {
                label += a.ToString();
                label += '\n';
            }

            label1.Text = label;

        }

    }
}
