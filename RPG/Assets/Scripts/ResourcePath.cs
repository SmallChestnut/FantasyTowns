using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePath
{
    public static ResourcePath Single { get; } = new ResourcePath();

    public Dictionary<string, GameObject> articleUIs = new Dictionary<string, GameObject>(); 

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


    private ResourcePath()
    {
        articleUIs.Add(wood.GetComponent<ArticleUI>().itemData.name, wood);
        articleUIs.Add(stone.GetComponent<ArticleUI>().itemData.name, stone);
        articleUIs.Add(food.GetComponent<ArticleUI>().itemData.name, food);
        articleUIs.Add(medicine.GetComponent<ArticleUI>().itemData.name, medicine);
    }
}
