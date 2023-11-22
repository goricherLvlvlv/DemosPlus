using System;
using System.Collections.Generic;
using System.IO;

namespace DemosPlus.Modules
{
    public class ExcelUtil : IModule
    {
        private Dictionary<string, ConfigItem> _itemMap;

        private Dictionary<ItemType, Dictionary<string, ConfigItem>> _typeItemMap;

        public const int HasEnchantLevel = 4;

        public void OnResolve()
        {
            InitItems();
        }

        public void OnDestroy()
        {
        }

        public List<string> GetItemKeys(ItemType itemType = ItemType.None)
        {
            List<string> res = new List<string>();

            if (itemType == ItemType.None)
            {
                foreach (var item in _itemMap)
                {
                    res.Add(item.Key);
                }
            }
            else
            {
                var map = _typeItemMap[itemType];
                foreach (var item in map)
                {
                    res.Add(item.Key);
                }
            }

            return res;
        }

        public List<City> GetCitys()
        {
            return new List<City> { City.Thetford, City.BridgeWatch, City.Martlock, City.Lymhurst, City.FortSterling, City.Caerleon, City.BlackMarket };
        }

        public string GetItem(string itemKey, int tier, int enchant)
        {
            var config = QExcelUtil.Instance.GetConfig(itemKey);
            int tierMin = config.tierMin;
            int tierMax = config.tierMax;
            int enchantMin = config.enchantMin;
            int enchantMax = config.enchantMax;
            var enchantType = config.enchantType;

            string name = itemKey;
            if (tierMin != 0 && tierMax != 0)
            {
                name = $"T{tier}_" + itemKey;
            }

            if (tier >= HasEnchantLevel && enchant != 0 && enchantMin != 0 && enchantMax != 0)
            {
                name += $"{(enchantType == EnchantType.JustAt ? $"@{enchant}" : $"_LEVEL{enchant}@{enchant}")}";
            }

            return name;
        }

        public List<string> GetItems(string itemKey)
        {
            List<string> result = new List<string>();

            var config = QExcelUtil.Instance.GetConfig(itemKey);
            int tierMin = config.tierMin;
            int tierMax = config.tierMax;
            int enchantMin = config.enchantMin;
            int enchantMax = config.enchantMax;
            for (int tier = tierMin; tier <= tierMax; ++tier)
            {
                result.Add(GetItem(itemKey, tier, 0));
                if (tier >= HasEnchantLevel)
                {
                    for (int enchant = enchantMin; enchant <= enchantMax; ++enchant)
                    {
                        result.Add(GetItem(itemKey, tier, enchant));
                    }
                }
            }

            return result;
        }

        public ConfigItem GetConfig(string itemKey)
        {
            if (!_itemMap.TryGetValue(itemKey, out var item))
            {
                return null;
            }

            return item;
        }

        public EnchantType GetEnchantType(string itemKey)
        {
            var item = GetConfig(itemKey);
            if (item == null)
            {
                return EnchantType.None;
            }

            return item.enchantType;
        }

        private void InitItems()
        {
            _itemMap = new Dictionary<string, ConfigItem>();
            _typeItemMap = new Dictionary<ItemType, Dictionary<string, ConfigItem>>();

            foreach (ItemType itemType in Enum.GetValues(typeof(ItemType)))
            {
                if (itemType == ItemType.None)
                {
                    continue;
                }

                _typeItemMap[itemType] = new Dictionary<string, ConfigItem>();
                string itemPath = string.Format("{0}" + string.Format(Const.ItemTypePath, itemType), Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\")));
                var json = File.ReadAllText(itemPath);
                var items = QJsonManager.Instance.GetItems(json);

                foreach (var item in items)
                {
                    _itemMap[item.key] = item;
                    _typeItemMap[itemType][item.key] = item;
                }

            }

        }

    }

    public class QExcelUtil : Query<ExcelUtil> { }
}
