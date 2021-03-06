﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePath
{
    public static ResourcePath Single { get; } = new ResourcePath();

    public Dictionary<string, GameObject> articleUIs = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> house = new Dictionary<string, GameObject>();
    public List<GameObject> NPCList = new List<GameObject>();

    public GameObject universalSlider = Resources.Load("UI\\进度条") as GameObject;
    public GameObject universalHintMessage = Resources.Load("UI\\交互提示") as GameObject;
    public GameObject bulidItem = Resources.Load("UI\\材料") as GameObject;
    public GameObject consumablesItem = Resources.Load("UI\\消耗品") as GameObject;
    public GameObject gridUI = Resources.Load("UI\\格子") as GameObject;
    public GameObject wood = Resources.Load("UI\\木头") as GameObject;
    public GameObject stone = Resources.Load("UI\\石头") as GameObject;
    public GameObject food = Resources.Load("UI\\鸡腿") as GameObject;
    public GameObject medicine = Resources.Load("UI\\回复药") as GameObject;
    public GameObject menu = Resources.Load("UI\\菜单") as GameObject;
    public GameObject button = Resources.Load("UI\\按钮") as GameObject;
    public GameObject woodObj1 = Resources.Load("可采集的资源\\木头\\树1") as GameObject;
    public GameObject woodObj2 = Resources.Load("可采集的资源\\木头\\树2") as GameObject;
    public GameObject woodObj3 = Resources.Load("可采集的资源\\木头\\树3") as GameObject;
    public GameObject woodObj4 = Resources.Load("可采集的资源\\木头\\树4") as GameObject;
    public GameObject woodObj5 = Resources.Load("可采集的资源\\木头\\树5") as GameObject;
    public GameObject woodObj6 = Resources.Load("可采集的资源\\木头\\树6") as GameObject;
    public GameObject woodObj7 = Resources.Load("可采集的资源\\木头\\树7") as GameObject;
    public GameObject medicineObj1 = Resources.Load("可采集的资源\\回复\\回血药材1") as GameObject;
    public GameObject medicineObj2 = Resources.Load("可采集的资源\\回复\\回血药材2") as GameObject;
    public GameObject medicineObj3 = Resources.Load("可采集的资源\\回复\\回血药材3") as GameObject;
    public GameObject stoneObj = Resources.Load("可采集的资源\\石头\\石头") as GameObject;
    public GameObject foodObj1 = Resources.Load("可采集的资源\\食物\\灵果") as GameObject;
    public GameObject foodObj2 = Resources.Load("可采集的资源\\食物\\蘑菇(大)") as GameObject;
    public GameObject foodObj3 = Resources.Load("可采集的资源\\食物\\蘑菇(小)") as GameObject;
    public GameObject foodObj4 = Resources.Load("可采集的资源\\食物\\野果") as GameObject;
    public GameObject axe = Resources.Load("NPC\\工具\\斧头") as GameObject;
    public GameObject pickaxe = Resources.Load("NPC\\工具\\镐子") as GameObject;
    public GameObject woodBasket = Resources.Load("NPC\\工具\\木头") as GameObject;
    public GameObject foodBasket = Resources.Load("NPC\\工具\\食物") as GameObject;
    public GameObject stoneBasket = Resources.Load("NPC\\工具\\矿物") as GameObject;
    public GameObject medicineBasket = Resources.Load("NPC\\工具\\药材") as GameObject;
    public GameObject emptyBasket = Resources.Load("NPC\\工具\\空") as GameObject;
    public GameObject NPC1 = Resources.Load("NPC\\NPC1") as GameObject;
    public GameObject NPC2 = Resources.Load("NPC\\NPC2") as GameObject;
    public GameObject NPC3 = Resources.Load("NPC\\NPC3") as GameObject;
    public GameObject NPC4 = Resources.Load("NPC\\NPC4") as GameObject;
    public GameObject NPC5 = Resources.Load("NPC\\NPC5") as GameObject;
    public GameObject foodHouse = Resources.Load("房子\\食物采集小屋") as GameObject;
    public GameObject woodHouse = Resources.Load("房子\\伐木小屋") as GameObject;
    public GameObject stoneHouse = Resources.Load("房子\\矿物采集小屋") as GameObject;
    public GameObject medicineHouse = Resources.Load("房子\\药材采集小屋") as GameObject;
    public GameObject dwellingHouse = Resources.Load("房子\\住宅小屋") as GameObject;
    public GameObject foodHouseTemplate = Resources.Load("房子\\食物采集小屋(模版)") as GameObject;
    public GameObject woodHouseTemplate = Resources.Load("房子\\伐木小屋(模版)") as GameObject;
    public GameObject stoneHouseTemplate = Resources.Load("房子\\矿物采集小屋(模版)") as GameObject;
    public GameObject medicineHouseTemplate = Resources.Load("房子\\药材采集小屋(模版)") as GameObject;
    public GameObject dwellingHouseTemplate = Resources.Load("房子\\住宅小屋(模版)") as GameObject;


    private ResourcePath()
    {
        #region 背包物品的UI字典初始化
        articleUIs.Add(wood.GetComponent<ArticleUI>().itemData.name, wood);
        articleUIs.Add(stone.GetComponent<ArticleUI>().itemData.name, stone);
        articleUIs.Add(food.GetComponent<ArticleUI>().itemData.name, food);
        articleUIs.Add(medicine.GetComponent<ArticleUI>().itemData.name, medicine);
        #endregion
        #region 各类NPC列表初始化
        NPCList.Add(NPC1);
        NPCList.Add(NPC2);
        NPCList.Add(NPC3);
        NPCList.Add(NPC4);
        NPCList.Add(NPC5);
        #endregion
        #region 各类房子的字典初始化
        house.Add("食物采集小屋", foodHouse);
        house.Add("伐木小屋", woodHouse);
        house.Add("矿物采集小屋", stoneHouse);
        house.Add("房子药材采集小屋", medicineHouse);
        house.Add("住宅小屋", dwellingHouse);
        house.Add("食物采集小屋(模版)", foodHouseTemplate);
        house.Add("伐木小屋(模版)", woodHouseTemplate);
        house.Add("矿物采集小屋(模版)", stoneHouseTemplate);
        house.Add("药材采集小屋(模版)", medicineHouseTemplate);
        house.Add("住宅小屋(模版)", dwellingHouseTemplate);
        #endregion
    }
}
