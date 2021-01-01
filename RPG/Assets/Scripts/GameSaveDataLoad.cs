using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaveDataLoad : MonoBehaviour
{
    List<HouseData> houseDatas = new List<HouseData>();
    List<HouseTemplateData> houseTemplateDatas = new List<HouseTemplateData>();
    Box box;
    void Start()
    {
        // 没有加载数据就直接不执行
        if(!GameSave.Single.isLoad)
        {
            return;
        }
        else
        {
            GameSave.Single.LoadGameData();
            PlayerInputManage.single.GetComponent<PlayerState>().playerData = GameSave.Single.gameData.playerData;
            PlayerInputManage.single.GetComponent<PlayerState>().UpdateStateUI();
        }
        foreach(var x in GameSave.Single.gameData.houseDatas)
        {
            houseDatas.Add(x);
        }
        foreach (var x in GameSave.Single.gameData.houseTemplateDatas)
        {
            houseTemplateDatas.Add(x);
        }
        GameSave.Single.gameData.houseDatas.Clear();
        GameSave.Single.gameData.houseTemplateDatas.Clear();
        box = PlayerInputManage.single.GetComponent<PlayerInteraction>().box;
        foreach (var x in houseDatas)
        {
            House house = Instantiate(ResourcePath.Single.house[x.name], x.position, x.rotation).GetComponent<House>();
            house.NPCNumber = x._NPC;
            house.warehouse = x.itemData.Clone();
        }
        foreach(var x in houseTemplateDatas)
        {
            GameObject temp = Instantiate(ResourcePath.Single.house[x.name], x.position, x.rotation);
            temp.GetComponent<Collider>().isTrigger = false;
        }
        foreach(var x in GameSave.Single.gameData.playerData.itemDatas)
        {
            box.AddItem(x);
        }
        PlayerInputManage.single.transform.position = GameSave.Single.gameData.playerData.position;
        PlayerInputManage.single.transform.rotation = GameSave.Single.gameData.playerData.rotation;
    }
}
