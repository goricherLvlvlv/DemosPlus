import openpyxl
import re
import json

sheet_names = ['Consume', 'Journal', 'Mount', 'Gear', 'Resource', 'Artifact']
workbook = openpyxl.load_workbook('../Resources/Item.xlsx')

class itemExt:
    key = ""
    tierMin = 0
    tierMax = 0
    enchantMin = 0
    enchantMax = 0
    enchantType = 0

    nameMap = {}

    def toJSON(self):
        return json.dumps(self, default=lambda o: o.__dict__, 
            sort_keys=False, indent=4)

item_map = {}

numberFilter = "\d+"
tierFilter = "^T\d+_"
enchantFilter = "@\d+$"
enchant2Filter = "_LEVEL\d+@\d+$"

def ProcessCell(cell, name):

    tierNumber = 0
    enchantNumber = 0
    enchantType = 0

    startIndex = 0
    endIndex = len(cell)

    tierResult = re.search(tierFilter, cell)
    if  tierResult is not None:
        tier = cell[tierResult.start():tierResult.end()]
        numberResult = re.search(numberFilter, tier)
        if numberResult is not None:
            tierNumber = int(tier[numberResult.start():numberResult.end()])

        startIndex = tierResult.end()

    enchantResult = re.search(enchantFilter, cell)
    if enchantResult is not None:
        enchantType = 1
        enchant = cell[enchantResult.start():enchantResult.end()]
        numberResult = re.search(numberFilter, enchant)
        if numberResult is not None:
            enchantNumber = int(enchant[numberResult.start():numberResult.end()])

        enchant2Result = re.search(enchant2Filter, cell)
        if enchant2Result is not None:
            enchantType = 2
            enchantResult = enchant2Result

        endIndex = enchantResult.start()

    itemKey = str(cell[startIndex:endIndex])

    if itemKey not in item_map:
        item_map[itemKey] = itemExt()
        item_map[itemKey].key = itemKey
        item_map[itemKey].nameMap = {}

    item_ext = item_map[itemKey]
    tierMin, tierMax, enchantMin, enchantMax = item_ext.tierMin, item_ext.tierMax, item_ext.enchantMin, item_ext.enchantMax
    if tierNumber > 0:
        if tierMin == 0:
            tierMin = tierNumber
        else:
            tierMin = tierNumber if tierNumber < tierMin else tierMin

        if tierMax == 0:
            tierMax = tierNumber
        else:
            tierMax = tierNumber if tierNumber > tierMax else tierMax

    if enchantNumber > 0:
        if enchantMin == 0:
            enchantMin = enchantNumber
        else:
            enchantMin = enchantNumber if enchantNumber < enchantMin else enchantMin

        if enchantMax == 0:
            enchantMax = enchantNumber
        else:
            enchantMax = enchantNumber if enchantNumber > enchantMax else enchantMax

    item_ext.tierMin = tierMin
    item_ext.tierMax = tierMax
    item_ext.enchantMin = enchantMin
    item_ext.enchantMax = enchantMax
    item_ext.enchantType = enchantType
    item_ext.nameMap[name] = str(tierNumber) + '|' + str(enchantNumber)

    return itemKey

def Export():
    encoder = json.encoder.JSONEncoder()

    for name in sheet_names:
        filename = '../Resources/' + name + '.txt'
        
        item_map.clear()

        with open(filename, 'w') as f:
            text = ""
            sheet = workbook[name]
            for row in sheet.rows:
                cell = str(row[0].value)
                itemName = str(row[1].value)
                ProcessCell(cell, itemName)

            item_list = []
            for itemKey in item_map:
                item = item_map[itemKey]
                item_list.append(item)
            text = json.dumps(item_list, default=lambda o: o.__dict__)
            # for item in item_map:
            #     text += item_map[item].toJSON()
            #     text += '\n'
            f.write(text)

Export()