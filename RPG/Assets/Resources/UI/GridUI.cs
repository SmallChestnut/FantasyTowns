using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridUI : MonoBehaviour, IDropHandler
{
    public RectTransform rectTransform;
    public ArticleUI Article { get; set; }
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Article = transform.childCount >= 1 ? transform.GetChild(0).GetComponent<ArticleUI>() : null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        // 如果拖动的是“可拖动UI”就什么也不做
        if (eventData.pointerDrag.tag == "DragUI")
            return;

        // 如果当前格子上有物品
        if(Article != null)
        {
            ArticleUI temp = eventData.pointerDrag.GetComponent<ArticleUI>();
            // 将一样的物品进行堆叠
            if (Article.itemData.name == temp.itemData.name)
            {
                // 将拖拽的物品的格子对该物品的引用置空
                temp.ArticleGrid.Article = null;
                Box box = transform.parent.parent.GetComponent<Box>();
                // 清除箱子对该物品的引用
                box.box.Remove(temp);
                // 进行物品添加
                box.AddItem(temp.itemData.Clone());
                // 删除拖拽的该物品
                Destroy(temp.gameObject);
            }
            // 否则将这个物品移动到用户拖拽过来的物品之前的格子（交换位置）
            else
            {
                Article.ChangeGrid(temp.ArticleGrid);
            }
        }
        

        eventData.pointerDrag.transform.SetParent(transform);        // 新来的物品设置为自身的子物品
        Article = eventData.pointerDrag.GetComponent<ArticleUI>();   // 拿到引用物品
        Article.ArticleGrid = this;                                  // 将物品对格子的引用设为自己       
    }
}
