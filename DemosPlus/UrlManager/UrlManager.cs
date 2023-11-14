using DemosPlus.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemosPlus.Url
{

    public enum City
    {
        None,
        Thetford,
        BridgeWatch,
        Martlock,
        Lymhurst,
        FortSterling,
        Caerleon,
    }

    public enum Item
    {
        None,
        T4_Bag,
        T5_Bag,
        T6_Bag,
        T7_Bag,
        T8_Bag,
        T4_PLANKS,
        T5_PLANKS,
        T6_PLANKS,
        T7_PLANKS,
        T8_PLANKS,
    }

    public struct UrlDate
    {
        public int year;
        public int month;
        public int day;

        public override string ToString()
        {
            return $"{month}-{day}-{year}";
        }
    }

    public class UrlManager
    {
        private const string Url_Prices_Avg = "https://west.albion-online-data.com/api/v2/stats/charts/";
        private const string Url_Buy_Max_Prices = "https://west.albion-online-data.com/api/v2/stats/prices/";

        public string GetPricesAvgUrl(List<Item> items, List<City> citys, UrlDate? startDate, UrlDate? endDate)
        {
            StringBuilder url = new StringBuilder();
            url.Append(Url_Prices_Avg);

            for (int i = 0; i < items.Count; ++i)
            {
                url.Append(items[i]);
                if (i != items.Count - 1)
                {
                    url.Append(',');
                }
            }

            bool hasFirstParam = false;

            if (citys.Count > 0)
            {
                AppendParamSymbol(url, ref hasFirstParam);
                url.Append("locations=");

                for (int i = 0; i < citys.Count; ++i)
                {
                    url.Append(citys[i]);
                    if (i != items.Count - 1)
                    {
                        url.Append(',');
                    }
                }
            }

            if (startDate != null)
            {
                AppendParamSymbol(url, ref hasFirstParam);
                url.Append("date=");
                url.Append(startDate.ToString());
            }

            if (endDate != null)
            {
                AppendParamSymbol(url, ref hasFirstParam);
                url.Append("end_date=");
                url.Append(endDate.ToString());
            }

            return url.ToString();
        }

        private void AppendParamSymbol(StringBuilder sb, ref bool hasFirstParam)
        {
            if (!hasFirstParam)
            {
                sb.Append('?');
                hasFirstParam = true;
            }
            else
            {
                sb.Append('&');
            }
        }

    }

    public class QUrlManager : Query<UrlManager> { }

}
