﻿using DemosPlus.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DemosPlus.Modules
{
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

    public class UrlManager : IModule
    {


        public void OnResolve()
        {
        }

        public void OnDestroy()
        {
        }

        public string GetPricesAvgUrl(Server server, List<string> items, List<City> citys, UrlDate? startDate, UrlDate? endDate, Quality quality)
        {
            StringBuilder url = new StringBuilder();
            url.Append(server == Server.West ? Const.Url_Prices_Avg : Const.Url_Prices_Avg_East);
            AddItemParam(url, items);

            bool hasFirstParam = false;
            AppendStartDateParam(url, startDate, ref hasFirstParam);
            AppendEndDateParam(url, endDate, ref hasFirstParam);
            AppendQualityParam(url, quality, ref hasFirstParam);

            return url.ToString();
        }

        public string GetBuyMaxPricesUrl(Server server, List<string> items, List<City> citys, UrlDate? startDate, UrlDate? endDate, Quality quality)
        {
            StringBuilder url = new StringBuilder();
            url.Append(server == Server.West ? Const.Url_Buy_Max_Prices : Const.Url_Buy_Max_Prices_East);
            AddItemParam(url, items);

            bool hasFirstParam = false;
            AppendStartDateParam(url, startDate, ref hasFirstParam);
            AppendEndDateParam(url, endDate, ref hasFirstParam);
            AppendQualityParam(url, quality, ref hasFirstParam);

            return url.ToString();
        }

        private void AddItemParam(StringBuilder url, List<string> items)
        {
            for (int i = 0; i < items.Count; ++i)
            {
                url.Append(items[i]);
                if (i != items.Count - 1)
                {
                    url.Append(',');
                }
            }
        }

        /// <summary>
        /// 会有portal, 直接不加城市限制
        /// </summary>
        private void AddCityParam(StringBuilder url, List<City> citys, ref bool hasFirstParam)
        {
            if (citys.Count > 0)
            {
                AppendParamSymbol(url, ref hasFirstParam);
                url.Append("locations=");

                for (int i = 0; i < citys.Count; ++i)
                {
                    url.Append(citys[i]);
                    if (i != citys.Count - 1)
                    {
                        url.Append(',');
                    }
                }
            }
        }

        private void AppendStartDateParam(StringBuilder url, UrlDate? startDate, ref bool hasFirstParam)
        {
            if (startDate != null)
            {
                AppendParamSymbol(url, ref hasFirstParam);
                url.Append("date=");
                url.Append(startDate.ToString());
            }
        }

        private void AppendEndDateParam(StringBuilder url, UrlDate? endDate, ref bool hasFirstParam)
        {
            if (endDate != null)
            {
                AppendParamSymbol(url, ref hasFirstParam);
                url.Append("end_date=");
                url.Append(endDate.ToString());
            }
        }

        private void AppendQualityParam(StringBuilder url, Quality quality, ref bool hasFirstParam)
        {
            if (quality != Quality.None)
            {
                AppendParamSymbol(url, ref hasFirstParam);
                url.Append("qualities=");
                url.Append((int)quality);
            }
        }

        private void AppendParamSymbol(StringBuilder url, ref bool hasFirstParam)
        {
            if (!hasFirstParam)
            {
                url.Append('?');
                hasFirstParam = true;
            }
            else
            {
                url.Append('&');
            }
        }

    }

    public class QUrlManager : Query<UrlManager> { }

}
