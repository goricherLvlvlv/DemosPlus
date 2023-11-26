using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemosPlus.Modules
{
    [JsonConverter(typeof(JsonPathConverter))]
    public class NetPricesAvg
    {
        [JsonProperty("location")]
        public string city { get; set; }

        [JsonProperty("item_id")]
        public string item { get; set; }

        [JsonProperty("quality")]
        public int quality { get; set; }

        [JsonProperty("data.prices_avg")]
        public List<double> prices_avg { get; set; }

        [JsonProperty("data.item_count")]
        public List<int> item_count { get; set; }

        public int SaleCount()
        {
            int count = 0;
            foreach (var c in item_count) count += c;
            return count;
        }

        public double GetAverage()
        {
            double sum = 0d;
            foreach (var price in prices_avg) sum += price;
            return prices_avg.Count != 0 ? sum / prices_avg.Count : sum;
        }

    }

    [JsonConverter(typeof(JsonPathConverter))]
    public class NetBuyMaxPrices
    {
        [JsonProperty("city")]
        public string city { get; set; }

        [JsonProperty("item_id")]
        public string item { get; set; }

        [JsonProperty("quality")]
        public int quality { get; set; }

        [JsonProperty("buy_price_max")]
        public int buy_price_max { get; set; }
    }

    [JsonConverter(typeof(JsonPathConverter))]
    public class ConfigItem
    {
        [JsonConverter(typeof(JsonPathConverter))]
        public class CraftExt
        { 
            [JsonProperty("resource_list")]
            public List<string> resourceList { get; set; }

            [JsonProperty("count_list")]
            public List<int> countList { get; set; }

            [JsonProperty("can_return_list")]
            public List<bool> canReturnList { get; set; }

        }

        [JsonProperty("key")]
        public string key { get; set; }

        [JsonProperty("tierMin")]
        public int tierMin { get; set; }

        [JsonProperty("tierMax")]
        public int tierMax { get; set; }

        [JsonProperty("enchantMin")]
        public int enchantMin { get; set; }

        [JsonProperty("enchantMax")]
        public int enchantMax { get; set; }

        [JsonProperty("enchantType")]
        public EnchantType enchantType { get; set; }

        [JsonProperty("name_map")]
        public Dictionary<string, string> nameMap { get; set; }

        [JsonProperty("craft_ext")]
        public List<CraftExt> crafts { get; set; }

        public override string ToString()
        {
            //if (nameMap == null || nameMap.Count <= 0)
            //{
            //    return key;
            //}

            //foreach (var name in nameMap)
            //{
            //    return name.Key;
            //}

            return key;
        }
    }

    public class JsonManager : IModule
    {
        public void OnResolve()
        {
        }

        public void OnDestroy()
        {
        }

        public List<NetPricesAvg> GetPricesAvg(string json)
        {
            return JsonConvert.DeserializeObject<List<NetPricesAvg>>(json);
        }

        public List<NetBuyMaxPrices> GetBuyMaxPrices(string json)
        {
            return JsonConvert.DeserializeObject<List<NetBuyMaxPrices>>(json);
        }

        public List<ConfigItem> GetItems(string json)
        {
            return JsonConvert.DeserializeObject<List<ConfigItem>>(json);
        }

    }

    public class QJsonManager : Query<JsonManager> { }

    public class JsonPathConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);
            var targetObj = Activator.CreateInstance(objectType);

            foreach (var prop in objectType.GetProperties().Where(p => p.CanRead && p.CanWrite))
            {
                var att = prop.GetCustomAttributes(true).OfType<JsonPropertyAttribute>().FirstOrDefault();

                var jsonPath = (att != null ? att.PropertyName : prop.Name);
                var token = jo.SelectToken(jsonPath);

                if (token == null || token.Type == JTokenType.Null) continue;

                var value = token.ToObject(prop.PropertyType, serializer);
                prop.SetValue(targetObj, value, null);
            }

            return targetObj;
        }

        public override bool CanConvert(Type objectType) => false;

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

}
