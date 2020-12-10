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
        if (itemData.number == 0)
            return 0;
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

    /// <summary>
    /// 从箱子里拿对于的物品出来
    /// </summary>
    /// <param name="itemData">需要拿的物品数据</param>
    /// <returns></returns>
    public int GetItem(ItemData itemData)
    {
        int itemSum = 0; // 物品的累加
        List<ArticleUI> temp = new List<ArticleUI>(); // 记录要删除的物品
        for(int i = 0; i < box.Count; i++)
        {
            // 如果需要获取的物品数量足够了就返回不查找
            if (itemData.number == 0) break;
            if (itemData.number < 0) throw new Exception();

            if(box[i].itemData.name == itemData.name)
            {
                // 如果箱子里的道具比需要的道具还要大就直接取需要的返回
                if(box[i].itemData.number > itemData.number)
                {
                    itemSum += itemData.number;
                    box[i].itemData.number -= itemData.number;
                    itemData.number = 0;
                    box[i].UpdateNumberText();
                    break;
                }
                // 否则需要销毁箱子里这个道具UI并且累加
                else
                {
                    itemSum += box[i].itemData.number;
                    itemData.number -= box[i].itemData.number;
                    temp.Add(box[i]); // 该物品的数量用完了，记录一下
                }
            }
        }
        foreach(var x in temp)
        {
            box.Remove(x);
            x.ArticleGrid.Article = null; // 让格子取消对物品的引用
            Destroy(x.gameObject);
        }
        return itemSum;
    }

    public void OpenBox()
    {
        Cursor.lockState = CursorLockMode.None;
        PlayerInputManage.single.ForbidVMCamera();
        PlayerInputManage.single.ForbidMove();
        gameObject.SetActive(true);
    }
    public void CloseBox()
    {
        Cursor.lockState = CursorLockMode.Locked;
        PlayerInputManage.single.MayVMCamera();
        PlayerInputManage.single.MayMove();
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

    public ItemData()
    {

    }
    public ItemData(string name, int number)
    {
        this.name = name;
        this.number = number;
    }

    public ItemData Clone() => MemberwiseClone() as ItemData;
}
