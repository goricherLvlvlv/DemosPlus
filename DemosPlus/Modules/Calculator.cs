using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static DemosPlus.Modules.ConfigItem;

namespace DemosPlus.Modules
{
    public class Calculator : IModule
    {
        public double returnRate = 0.52d;

        public void OnResolve()
        {
        }

        public void OnDestroy()
        {
        }

        #region Interface

        public Dictionary<string, double> CalCost(string itemKey)
        {
            var config = QExcelUtil.Instance.GetConfig(itemKey);
            if (config.crafts == null || config.crafts.Count <= 0)
            {
                return null;
            }

            var prices = new Dictionary<string, ItemAvgPrice>();
            var pricesCache = new HashSet<string>();

            // 记录所有的道具价格
            foreach (var craft in config.crafts)
            {
                for (int i = 0; i < craft.resourceList.Count; ++i)
                {
                    var resourceItemKey = craft.resourceList[i];
                    if (pricesCache.Contains(resourceItemKey))
                    {
                        continue;
                    }

                    AppendItemKeyPrices(Duration.SevenDays, resourceItemKey, ref prices);

                    pricesCache.Add(resourceItemKey);
                }
            }

            var result = new Dictionary<string, double>();
            int tierMin = config.tierMin;
            int tierMax = config.tierMax;
            int enchantMin = config.enchantMin;
            int enchantMax = config.enchantMax;

            foreach (var craft in config.crafts)
            {
                for (int tier = tierMin; tier <= tierMax; ++tier)
                {
                    var item = QExcelUtil.Instance.GetItem(itemKey, tier, 0);
                    var cost = CalCost(craft, tier, 0, prices);
                    result[item] = cost;

                    if (tier >= ExcelUtil.HasEnchantLevel)
                    {
                        for (int enchant = enchantMin; enchant <= enchantMax; ++enchant)
                        {
                            item = QExcelUtil.Instance.GetItem(itemKey, tier, enchant);
                            cost = CalCost(craft, tier, enchant, prices);
                            result[item] = cost;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 获取 道具, 城市 二维价格表
        /// </summary>
        public Dictionary<(string item, City city), double> GetAvgMap(string itemKey, Duration duration)
        {
            var items = QExcelUtil.Instance.GetItems(itemKey);
            if (items == null || items.Count <= 0)
            {
                return null;
            }

            var (startDate, endDate) = GetUrlDate(duration);
            var quality = Quality.None;
            var citys = QExcelUtil.Instance.GetCitys();
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
            return avgMap;
        }

        /// <summary>
        /// 获取该类型所有物品的均价
        /// </summary>
        public Dictionary<string, ItemAvgPrice> GetTypePrices(Duration duration, ItemType type)
        {
            var (startDate, endDate) = GetUrlDate(duration);
            var quality = Quality.None;
            var citys = QExcelUtil.Instance.GetCitys();
            var itemKeys = QExcelUtil.Instance.GetItemKeys(type);

            var res = new Dictionary<string, ItemAvgPrice>();

            foreach (var itemKey in itemKeys)
            {
                var items = QExcelUtil.Instance.GetItems(itemKey);
                if (items == null || items.Count <= 0)
                {
                    continue;
                }

                var url = QUrlManager.Instance.GetPricesAvgUrl(items, citys, startDate, endDate, quality);
                var result = QNetwork.Instance.GetResult(url);
                var avgList = QJsonManager.Instance.GetPricesAvg(result);
                ProcessAvg_ForCost(ref res, avgList);

            }

            return res;
        }

        /// <summary>
        /// 获得该itemKey所有阶级/附魔的价格
        /// </summary>
        public void AppendItemKeyPrices(Duration duration, string itemKey, ref Dictionary<string, ItemAvgPrice> res)
        {
            var (startDate, endDate) = GetUrlDate(duration);
            var quality = Quality.None;
            var citys = QExcelUtil.Instance.GetCitys();

            var items = QExcelUtil.Instance.GetItems(itemKey);
            if (items == null || items.Count <= 0)
            {
                return;
            }

            var url = QUrlManager.Instance.GetPricesAvgUrl(items, citys, startDate, endDate, quality);
            var result = QNetwork.Instance.GetResult(url);
            var avgList = QJsonManager.Instance.GetPricesAvg(result);
            ProcessAvg_ForCost(ref res, avgList);
        }

        #endregion

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
            else if (name.Equals("Black Market", StringComparison.OrdinalIgnoreCase))
            {
                return City.BlackMarket;
            }

            return City.None;
        }

        private Dictionary<(string item, City city), double> ProcessAvg(List<NetPricesAvg> avgs)
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

        private Dictionary<string, ItemAvgPrice> ProcessAvg_ForCost(ref Dictionary<string, ItemAvgPrice> result, List<NetPricesAvg> avgs)
        {
            var map = ProcessAvg(avgs);
            var resultSum = new Dictionary<string, (double price, int count)>();
            foreach (var avg in map)
            {
                if (!resultSum.ContainsKey(avg.Key.item))
                {
                    resultSum[avg.Key.item] = (0d, 0);
                }

                var sum = resultSum[avg.Key.item];
                sum.price += avg.Value;
                sum.count += 1;
                resultSum[avg.Key.item] = sum;
            }

            foreach (var res in resultSum)
            {
                var sum = resultSum[res.Key];
                result.Add(res.Key, new ItemAvgPrice()
                {
                    item = res.Key,
                    price = sum.price / sum.count
                });
            }

            return result;
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

        private double CalCost(CraftExt craft, int tier, int enchant, Dictionary<string, ItemAvgPrice> prices)
        {
            var cost = 0d;

            for (int i = 0; i < craft.resourceList.Count; ++i)
            {
                var resourceItemKey = craft.resourceList[i];
                var count = craft.countList[i];
                var canReturn = craft.canReturnList[i];

                var resourcePrice = GetResourcePrice(resourceItemKey, tier, enchant, prices);

                cost += resourcePrice * count * (canReturn ? (1d - returnRate) : 1d);
            }

            return cost;
        }

        private double GetResourcePrice(string resourceKey, int tier, int enchant, Dictionary<string, ItemAvgPrice> prices)
        {
            var item = QExcelUtil.Instance.GetItem(resourceKey, tier, enchant);
            prices.TryGetValue(item, out var price);
            return price.price;
        }

    }

    public class QCalculator : Query<Calculator> { }

    public struct ItemAvgPrice
    {
        public string item;

        public double price;

    }

}
