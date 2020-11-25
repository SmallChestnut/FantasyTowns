using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Box : MonoBehaviour
{
    public List<ArticleUI> box = new List<ArticleUI>();
    public GameObject boxGrids;

    /// <summary>
    /// 添加物品到箱子里
    /// </summary>
    /// <param name="itemData">物品的数据</param>
    /// <returns>返回没有添加进去的物品数量</returns>
    public int AddItem(ItemData itemData)
    {
        // 先判断箱子里是否已经有重复的物品，如果有就叠加在一起
        for(int i = 0; i < box.Count; i++)
        {
            // 找到重复物品并且还有叠加的位置
            if(box[i].itemData.name == itemData.name && box[i].itemData.number < box[i].maxNumber)
            {
                // 如果物品的叠加数量超过最大限制就进行叠加后将多余的物品继续进行下一轮查找
                if(box[i].itemData.number + itemData.number > box[i].maxNumber)
                {
                    itemData.number = box[i].itemData.number + itemData.number - box[i].maxNumber;
                    box[i].itemData.number = box[i].maxNumber;
                    box[i].UpdateNumberText();
                }
                // 否则就能够填满
                else
                {
                    box[i].itemData.number = box[i].itemData.number + itemData.number;
                    box[i].UpdateNumberText();
                    return 0;
                }
            }
        }
        //  查找空的格子进行装填
        for (int i = 0; i < boxGrids.transform.childCount; i++)
        {
            GridUI gridUI = boxGrids.transform.GetChild(i).GetComponent<GridUI>();
            if (gridUI.Article == null)
            {
                GameObject temp = Instantiate(ResourcePath.Single.articleUIs[itemData.name]);
                ArticleUI articleUI = temp.GetComponent<ArticleUI>();

                articleUI.ChangeGrid(gridUI);
                box.Add(articleUI);
                // 如果可以叠加完的话
                if (itemData.number <= articleUI.maxNumber)
                {
                    articleUI.itemData = itemData.Clone();
                    articleUI.UpdateNumberText();
                    return 0;
                }
                // 不能叠加还得继续找空格子创建物品
                else
                {
                    articleUI.itemData = itemData.Clone();
                    articleUI.itemData.number = articleUI.maxNumber;
                    itemData.number -= articleUI.maxNumber;
                    articleUI.UpdateNumberText();
                }
            }
        }
        // 找不到空格子后，返回还剩下的物品数量
        return itemData.number;
    }

    public void OpenBox()
    {
        Cursor.lockState = CursorLockMode.None;
        PlayerInputManage.single.ForbidVMCamera();
        gameObject.SetActive(true);
    }
    public void CloseBox()
    {
        Cursor.lockState = CursorLockMode.Locked;
        PlayerInputManage.single.MayVMCamera();
        gameObject.SetActive(false);
    }
}

/// <summary>
/// 物品通用数据
/// </summary>
[Serializable]
public class ItemData
{
    public string name;                 // 物品名字
    public int number = 1;              // 物品数量

    public ItemData Clone() => MemberwiseClone() as ItemData;
}
