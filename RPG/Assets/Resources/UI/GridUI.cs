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
        // 如果当前格子上有物品，那么将这个物品移动到用户拖拽过来的物品之前的格子
        Article?.ChangeGrid(eventData.pointerDrag.GetComponent<ArticleUI>().ArticleGrid);

        eventData.pointerDrag.transform.SetParent(transform);        // 新来的物品设置为自身的子物品
        Article = eventData.pointerDrag.GetComponent<ArticleUI>();   // 拿到引用物品
        Article.ArticleGrid = this;                                  // 将物品对格子的引用设为自己       
    }
}
