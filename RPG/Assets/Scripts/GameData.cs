using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public List<HouseData> houseDatas = new List<HouseData>();
    public List<HouseTemplateData> houseTemplateDatas = new List<HouseTemplateData>();
    public PlayerData playerData;
}

/// <summary>
/// 房子的数据
/// </summary>
public class HouseData
{
    public Vector3 position;             // 位置
    public Quaternion rotation;          // 旋转
    public string name;                  // 名字  
    public int _NPC;                     // NPC数量
    public ItemData itemData;            // 资源数量
}
/// <summary>
/// 房子模版的数据
/// </summary>
public class HouseTemplateData
{
    public Vector3 position;             // 位置
    public Quaternion rotation;          // 旋转
    public string name;                  // 名字  
}
/// <summary>
/// 玩家的数据
/// </summary>
public class PlayerData
{
    public int life;        // 生命值
    public int satiety;     // 饱食度
    public int maxLife;     // 最大生命值
    public int maxSatiety;  // 最大饱食度
    public int _NPC;        // NPC数量
    public Vector3 position;    // 位置
    public Quaternion rotation; // 旋转
    public List<ItemData> itemDatas = new List<ItemData>(); // 背包物品
}
