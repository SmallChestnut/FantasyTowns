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

    private ResourcePath()
    {
        articleUIs.Add(wood.GetComponent<ArticleUI>().itemData.name, wood);
        articleUIs.Add(stone.GetComponent<ArticleUI>().itemData.name, stone);
        articleUIs.Add(food.GetComponent<ArticleUI>().itemData.name, food);
    }
}
