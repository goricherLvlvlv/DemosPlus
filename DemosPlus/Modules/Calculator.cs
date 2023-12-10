using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using static DemosPlus.Modules.ConfigItem;

namespace DemosPlus.Modules
{
    public class Calculator : IModule
    {
        public void OnResolve()
        {
        }

        public void OnDestroy()
        {
        }

        #region Interface

        public Dictionary<(string item, City city), (double percent, double salePrice, double costPrice, int number)> CalProfit(string itemKey, SaleMode mode, double returnRate, Duration duration = Duration.SevenDays, Tax tax = Tax.Tax_6_26)
        {
            var cost = QCalculator.Instance.CalCost(itemKey, returnRate, duration);
            var prices = mode == SaleMode.SellOrder ?
                QCalculator.Instance.GetAvgPrices(itemKey, duration) :
                QCalculator.Instance.GetBuyMaxPrices(itemKey, duration);

            var citys = QExcelUtil.Instance.GetCitys();
            var taxPercent = QExcelUtil.Instance.GetTax(tax);

            var result = new Dictionary<(string item, City city), (double percent, double salePrice, double costPrice, int number)>();
            foreach (var city in citys)
            {
                foreach (var c in cost)
                {
                    var item = c.Key;
                    var costNum = double.PositiveInfinity;
                    foreach (var min in c.Value)
                    {
                        if (min <= 0d)
                        {
                            continue;
                        }

                        costNum = costNum < min ? costNum : min;
                    }

                    if (costNum == double.PositiveInfinity)
                    {
                        continue;
                    }

                    if (!prices.TryGetValue((item, city), out var saleNum))
                    {
                        continue;
                    }

                    if (saleNum.price == 0)
                    {
                        continue;
                    }

                    var percent = (saleNum.price - costNum) / costNum - taxPercent;
                    result[(item, city)] = (percent, saleNum.price, costNum, saleNum.number);
                }
            }

            return result;
        }

        /// <summary>
        /// 计算某一类道具的成本
        /// </summary>
        public Dictionary<string, List<double>> CalCost(string itemKey, double returnRate, Duration duration = Duration.SevenDays)
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

                    AppendItemKeyPrices(duration, resourceItemKey, ref prices);
                    pricesCache.Add(resourceItemKey);
                }
            }

            var result = new Dictionary<string, List<double>>();
            int tierMin = config.tierMin;
            int tierMax = config.tierMax;
            int enchantMin = config.enchantMin;
            int enchantMax = config.enchantMax;

            foreach (var craft in config.crafts)
            {
                for (int tier = tierMin; tier <= tierMax; ++tier)
                {
                    var item = QExcelUtil.Instance.GetItem(itemKey, tier, 0);
                    var cost = CalCost(craft, tier, 0, returnRate, prices);
                    if (!result.ContainsKey(item))
                    {
                        result[item] = new List<double>();
                    }
                    result[item].Add(cost);

                    if (tier >= ExcelUtil.HasEnchantLevel)
                    {
                        for (int enchant = enchantMin; enchant <= enchantMax; ++enchant)
                        {
                            item = QExcelUtil.Instance.GetItem(itemKey, tier, enchant);
                            cost = CalCost(craft, tier, enchant, returnRate, prices);

                            if (!result.ContainsKey(item))
                            {
                                result[item] = new List<double>();
                            }
                            result[item].Add(cost);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 获取 道具, 城市 二维直接出售价格表
        /// </summary>
        public Dictionary<(string item, City city), (double price, int number)> GetBuyMaxPrices(string itemKey, Duration duration)
        {
            var items = QExcelUtil.Instance.GetItems(itemKey);
            if (items == null || items.Count <= 0)
            {
                return null;
            }

            var (startDate, endDate) = GetUrlDate(duration);
            var quality = Quality.None;
            var citys = QExcelUtil.Instance.GetCitys();
            var url = QUrlManager.Instance.GetBuyMaxPricesUrl(items, citys, startDate, endDate, quality);
            var result = QNetwork.Instance.GetResult(url);
            var buyMaxPrices = QJsonManager.Instance.GetBuyMaxPrices(result);
            buyMaxPrices.Sort((a, b) =>
            {
                if (a.city != b.city)
                {
                    return string.Compare(a.city, b.city);
                }

                if (a.item == b.item) return 0;
                return string.Compare(a.item, b.item);
            });

            var map = ProcessMaxPrice(buyMaxPrices);
            return map;
        }

        /// <summary>
        /// 获取 道具, 城市 二维价格表
        /// </summary>
        public Dictionary<(string item, City city), (double price, int number)> GetAvgPrices(string itemKey, Duration duration)
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
            if (string.IsNullOrEmpty(name))
            {
                return City.None;
            }

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

        /// <summary>
        /// 获得不同城市的物品直接出售价格
        /// </summary>
        /// <param name="maxPrices"></param>
        /// <returns></returns>
        private Dictionary<(string item, City city), (double price, int number)> ProcessMaxPrice(List<NetBuyMaxPrices> maxPrices)
        {
            var map = new Dictionary<(string item, City city), double>();
            var map2 = new Dictionary<(string item, City city), int>();

            foreach (var maxPrice in maxPrices)
            {
                var key = (maxPrice.item, GetCityByName(maxPrice.city));
                if (!map.ContainsKey(key))
                {
                    map[key] = 0;
                    map2[key] = 0;
                }

                map[key] += maxPrice.buy_price_max;
                map2[key] += 1;
            }

            var res = new Dictionary<(string item, City city), (double price, int number)>();
            foreach (var m in map)
            {
                res[m.Key] = (map[m.Key] / map2[m.Key], 1);
            }
            return res;
        }

        /// <summary>
        /// 获取不同城市的物品均价
        /// </summary>
        private Dictionary<(string item, City city), (double price, int number)> ProcessAvg(List<NetPricesAvg> avgs)
        {
            var avgMap = new Dictionary<(string item, City city), (double sum, int cityCount)>();
            var saleCount = new Dictionary<(string item, City city), int>();
            foreach (var avg in avgs)
            {
                var key = (avg.item, GetCityByName(avg.city));
                if (!avgMap.ContainsKey(key))
                {
                    avgMap[key] = (0d, 0);
                }

                if (!saleCount.ContainsKey(key))
                {
                    saleCount[key] = 0;
                }

                avgMap[key] = (avgMap[key].sum + avg.GetAverage(), avgMap[key].cityCount + 1);
                saleCount[key] += avg.SaleCount();
            }

            var res = new Dictionary<(string item, City city), (double price, int number)>();
            foreach (var m in avgMap)
            {
                var price = avgMap[m.Key].sum / avgMap[m.Key].cityCount;
                saleCount.TryGetValue(m.Key, out var number);
                res[m.Key] = (price, number);
            }
            return res;
        }

        /// <summary>
        /// 处理物品均价
        /// key: 物品itemKey, value: 物品价格
        /// </summary>
        private void ProcessAvg_ForCost(ref Dictionary<string, ItemAvgPrice> result, List<NetPricesAvg> avgs)
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
                sum.price += avg.Value.price;
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
        }

        /// <summary>
        /// 获取日期
        /// </summary>
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

        /// <summary>
        /// 计算一定阶级/附魔下的配方的成本
        /// </summary>
        private double CalCost(CraftExt craft, int tier, int enchant, double returnRate, Dictionary<string, ItemAvgPrice> prices)
        {
            var cost = 0d;

            for (int i = 0; i < craft.resourceList.Count; ++i)
            {
                var resourceItemKey = craft.resourceList[i];
                var count = craft.countList[i];
                var canReturn = craft.canReturnList[i];

                var resourcePrice = GetResourcePrice(resourceItemKey, tier, enchant, prices);
                if (resourcePrice <= 0)
                {
                    return -1d;
                }

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
