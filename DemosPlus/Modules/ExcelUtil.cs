using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace DemosPlus.Modules
{
    public class ExcelUtil : IModule
    {
        private Dictionary<string, ConfigItem> _itemMap;

        public void OnResolve()
        {
            InitItems2();
        }

        public void OnDestroy()
        {
        }

        public List<string> GetItems()
        {
            List<string> res = new List<string>();
            foreach (var item in _itemMap)
            {
                //res.AddRange(item.Value.nameMap.Keys);
                res.Add(item.Key);
            }

            return res;
        }

        public ConfigItem GetItem(string itemKey)
        {
            if (!_itemMap.TryGetValue(itemKey, out var item))
            {
                return null;
            }

            return item;
        }

        public EnchantType GetEnchantType(string itemKey)
        {
            var item = GetItem(itemKey);
            if (item == null)
            {
                return EnchantType.None;
            }

            return item.enchantType;
        }

        private void InitItems2()
        {
            _itemMap = new Dictionary<string, ConfigItem>();

            foreach (var itemType in Enum.GetNames(typeof(ItemType)))
            { 
                string itemPath = string.Format("{0}" + string.Format(Const.ItemTypePath, itemType), Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\")));
                var json = File.ReadAllText(itemPath);
                var items = QJsonManager.Instance.GetItems(json);

                foreach (var item in items)
                {
                    _itemMap[item.key] = item;
                }

            }

        }

    }

    public class QExcelUtil : Query<ExcelUtil> { }
}
