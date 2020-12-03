using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BulidItem : ArticleUI, IPointerClickHandler, IPointerDownHandler
{
    private new void Start()
    {
        base.Start();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Input.GetMouseButtonUp(1))
        {
            Menu.Single.RemoveAllButton();
            Menu.Single.ShowMenu(new Vector2(eventData.pointerEnter.transform.position.x,
                                             eventData.pointerEnter.transform.position.y));
            Menu.Single.AddButton(Discard, "丢弃");
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Menu.Single.CloseMenu();
        }
    }

    private void Discard()
    {
        box.GetItem(itemData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }
}
