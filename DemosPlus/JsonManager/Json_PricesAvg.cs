using DemosPlus.Url;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemosPlus.Json
{
    public class JsonPricesAvg
    {
        public City city;
        public Item item;
        public List<double> prices;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder($"{city} {item}\n");
            for (int i = 0; i < prices.Count; ++i)
            {
                if (i % 3 != 0)
                { 
                    sb.Append(' ');
                }
                sb.Append(prices[i].ToString());

                if ((i + 1) % 3 == 0)
                {
                    sb.Append('\n');
                }
            }

            return sb.ToString();
        }
    }

    public partial class JsonManager
    {
        public static List<JsonPricesAvg> GetPricesAvg(string json)
        {
            var root = JsonConvert.DeserializeObject(json) as JArray;
            if (root == null || root.Count <= 0)
            {
                return null;
            }

            var result = new List<JsonPricesAvg>();

            for (int i = 0; i < root.Count; ++i)
            {
                if (!(root[i] is JObject jobject))
                {
                    continue;
                }

                if (!jobject.TryGetValue("data", out var data) || !(data is JObject dataObject))
                {
                    continue;
                }

                City cityEnum = City.None;
                if (jobject.TryGetValue("location", out var city) && city is JValue cityValue)
                {
                    cityEnum = ConvertCity((string)cityValue.Value);
                }

                Item itemEnum = Item.None;
                if (jobject.TryGetValue("item_id", out var item) && item is JValue itemValue)
                {
                    itemEnum = ConvertItem((string)itemValue.Value);
                }

                if (!dataObject.TryGetValue("prices_avg", out var pricesAvg) || !(pricesAvg is JArray pricesAvgArray))
                {
                    continue;
                }

                var prices = new List<double>();

                for (int j = 0; j < pricesAvgArray.Count; ++j)
                {
                    if (!(pricesAvgArray[j] is JValue value))
                    {
                        continue;
                    }

                    prices.Add((double)value.Value);
                }

                result.Add(new JsonPricesAvg()
                {
                    city = cityEnum,
                    item = itemEnum,
                    prices = prices,
                });
            }

            return result;
        }

        private static Item ConvertItem(string item)
        {
            Enum.TryParse<Item>(item, out var result);
            return result;
        }

        private static City ConvertCity(string city)
        {
            Enum.TryParse<City>(city, out var result);
            return result;
        }

    }
}
