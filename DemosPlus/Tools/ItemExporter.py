import openpyxl
import re
import json

sheet_names = ['Consume', 'Journal', 'Mount', 'Gear', 'Resource', 'Artifact']

class craftExt:
    resource_list = []
    count_list = []
    can_return_list = []

class itemExt:
    idx : int = 0
    type : str = ""
    key : str = ""
    tierMin : int = 0
    tierMax : int = 0
    enchantMin : int = 0
    enchantMax : int = 0
    enchantType : int = 0
    chinese_name : str = ""
    name_map = {}
    craft_ext : craftExt = None

item_map = {}
item_craft_map = {}
uniqueName2ItemKey = {}

numberFilter = "\d+"
tierFilter = "^T\d+_"
enchantFilter = "@\d+$"
enchant2Filter = "_LEVEL\d+@\d+$"

def FetchItemKey(unique_name : str):
    if unique_name in uniqueName2ItemKey:
        return uniqueName2ItemKey[unique_name]

    startIndex = 0
    endIndex = len(unique_name)

    tierResult = re.search(tierFilter, unique_name)
    enchantResult = re.search(enchantFilter, unique_name)

    if  tierResult is not None:
        startIndex = tierResult.end()

    if enchantResult is not None:
        enchant2Result = re.search(enchant2Filter, unique_name)
        if enchant2Result is not None:
            enchantResult = enchant2Result
        endIndex = enchantResult.start()

    item_key = str(unique_name[startIndex:endIndex])
    uniqueName2ItemKey[unique_name] = item_key
    return item_key


def ProcessCraftResource(craft_ext : craftExt, resource):
    craft_ext.resource_list.append(FetchItemKey(resource["@uniquename"]))
    craft_ext.count_list.append(int(resource["@count"]))
    if "@maxreturnamount" in resource:
        craft_ext.can_return_list.append(False)
    else:
        craft_ext.can_return_list.append(True)

def ProcessCraft(unique_name : str, craft_list):
    item_key = FetchItemKey(unique_name)

    if item_key in item_craft_map:
        return

    crafts : list[craftExt] = []
    for craft in craft_list:
        c = craftExt()
        crafts.append(c)
        c.resource_list = []
        c.count_list = []
        c.can_return_list = []

        if type(craft) is list:
            resource_list = craft
            for resource in resource_list:
                ProcessCraftResource(c, resource)
        else:
            ProcessCraftResource(c, craft)
        
    item_craft_map[item_key] = crafts


def ProcessGears(gears):
    for gear in gears:
        unique_name = gear["@uniquename"]

        if "craftingrequirements" in gear:
            requirements = gear["craftingrequirements"]
            if type(requirements) is list:
                craft_list = []
                for requirement in requirements:
                    craft = requirement["craftresource"]
                    craft_list.append(craft)
                ProcessCraft(unique_name, craft_list)
            elif "craftresource" in requirements:
                craft_list = []
                craft = requirements["craftresource"]
                craft_list.append(craft)
                ProcessCraft(unique_name, craft_list)

def ProcessCrafts():
    item_craft_map.clear()
    with open('../Resources/craft.json', 'r') as file:
        json_obj = json.load(file)
        gears = json_obj["items"]["equipmentitem"]
        ProcessGears(gears)
        gears = json_obj["items"]["weapon"]
        ProcessGears(gears)
        gears = json_obj["items"]["transformationweapon"]
        ProcessGears(gears)

def ProcessCell(idx : int, type : str, cell : str, name : str, chinese_name : str):
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
        item_ext : itemExt = itemExt()
        item_map[itemKey] = item_ext
        item_ext.key = itemKey
        item_ext.name_map = {}

    item_ext: itemExt = item_map[itemKey]
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

    item_ext.idx = idx
    item_ext.type = type
    item_ext.tierMin = tierMin
    item_ext.tierMax = tierMax
    item_ext.enchantMin = enchantMin
    item_ext.enchantMax = enchantMax
    item_ext.enchantType = enchantType
    item_ext.name_map[name] = str(tierNumber) + '|' + str(enchantNumber)

    if chinese_name is "":
        chinese_name = itemKey
    item_ext.chinese_name = chinese_name
    if itemKey in item_craft_map:
        item_ext.craft_ext = item_craft_map[itemKey]

    return itemKey

def Export():
    workbook = openpyxl.load_workbook('../Resources/Item.xlsx')
    for name in sheet_names:
        filename = '../Resources/' + name + '.txt'
        
        item_map.clear()

        with open(filename, 'w') as f:
            text = ""
            sheet = workbook[name]
            idx = 0
            for row in sheet.rows:
                cell = str(row[0].value)
                itemName = str(row[1].value)
                chinese_name = ""
                if len(row) >= 3:
                    chinese_name = str(row[2].value)
                ProcessCell(idx, name, cell, itemName, chinese_name)
                idx = idx + 1

            item_list = []
            for itemKey in item_map:
                item = item_map[itemKey]
                item_list.append(item)
            text = json.dumps(item_list, default=lambda o: o.__dict__)

            f.write(text)

ProcessCrafts()
Export()