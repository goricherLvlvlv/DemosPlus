using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DemosPlus.Modules
{
    public class ExcelUtil : IModule
    {
        private List<string> _items;

        private Dictionary<string, (int tierMin, int tierMax, int enchantMin, int enchantMax)> _itemMap;
        private Dictionary<string, int> _itemEnchantType;

        public void OnResolve()
        {
            InitItems();
        }

        public void OnDestroy()
        {
        }

        public List<string> GetItems()
        {
            return _items;
        }

        public (int tierMin, int tierMax, int enchantMin, int enchantMax) GetValues(string itemKey)
        {
            if (!_itemMap.TryGetValue(itemKey, out var value))
            {
                return (0, 0, 0, 0);
            }

            return value;
        }

        public int GetEnchantType(string itemKey)
        {
            if (!_itemEnchantType.TryGetValue(itemKey, out var value))
            {
                return 0;
            }

            return value;
        }

        private void InitItems()
        {
            var app = new Application();

            string itemPath = string.Format("{0}" + Const.ItemPath, Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\")));
            string namePath = string.Format("{0}" + Const.ItemNamePath, Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\")));

            var itemText = File.ReadAllLines(itemPath);
            var nameText = File.ReadAllLines(namePath);

            _items = new List<string>();
            _itemMap = new Dictionary<string, (int tierMin, int tierMax, int enchantMin, int enchantMax)>();
            _itemEnchantType = new Dictionary<string, int>();

            int line = itemText.Length > nameText.Length ? nameText.Length : itemText.Length;

            Regex numberFilter = new Regex(@"\d+");
            Regex tierFilter = new Regex(@"^T\d+_");
            Regex enchantFilter = new Regex(@"@\d+$");
            Regex enchantFilter2 = new Regex(@"_LEVEL\d+@\d+$");

            for (int i = 0; i < line; ++i)
            {
                var item = itemText[i];
                var name = nameText[i];

                int tierNumber = 0;
                int enchantNumber = 0;

                var tier = tierFilter.Match(item);
                if (tier.Success)
                {
                    var number = numberFilter.Match(tier.Value);
                    if (number.Success)
                    {
                        int.TryParse(number.Value, out tierNumber);
                    }
                }

                var enchant = enchantFilter.Match(item);
                int enchantType = 1;
                if (enchant.Success)
                {
                    var number = numberFilter.Match(enchant.Value);
                    int.TryParse(number.Value, out enchantNumber);

                    var enchant2 = enchantFilter2.Match(item);
                    if (enchant2.Success)
                    {
                        enchant = enchant2;
                        enchantType = 2;
                    }
                }

                int startIndex = tier.Length;
                int length = item.Length - startIndex - enchant.Length;
                var itemKey = item.Substring(startIndex, length);
                _itemEnchantType[itemKey] = enchantType;

                if (!_itemMap.ContainsKey(itemKey))
                {
                    _itemMap.Add(itemKey, (0, 0, 0, 0));
                    _items.Add(itemKey);
                }

                (int tierMin, int tierMax, int enchantMin, int enchantMax) = _itemMap[itemKey];
                if (tierNumber > 0)
                {
                    tierMin = tierMin == 0 ? tierNumber :
                        (tierNumber <= tierMin ? tierNumber : tierMin);

                    tierMax = tierMax == 0 ? tierNumber :
                        (tierNumber >= tierMax ? tierNumber : tierMax);
                }

                if (enchantNumber > 0)
                {
                    enchantMin = enchantMin == 0 ? enchantNumber :
                        (enchantNumber <= enchantMin ? enchantNumber : enchantMin);

                    enchantMax = enchantMax == 0 ? enchantNumber :
                        (enchantNumber >= enchantMax ? enchantNumber : enchantMax);
                }

                _itemMap[itemKey] = (tierMin, tierMax, enchantMin, enchantMax);
            }
        }

    }

    public class QExcelUtil : Query<ExcelUtil> { }
}
