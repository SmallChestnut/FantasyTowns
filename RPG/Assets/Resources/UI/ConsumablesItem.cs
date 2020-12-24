using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ConsumablesItem : ArticleUI, IPointerClickHandler, IPointerDownHandler
{
    public ConsumablesType consumablesType;
    [Tooltip("消耗后得到的值")]
    public int value;

    private PlayerState playerState;
    private new void Start()
    {
        base.Start();
        playerState = PlayerInputManage.single.GetComponent<PlayerState>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(Input.GetMouseButtonUp(1))
        {
            Menu.Single.RemoveAllButton();
            Menu.Single.ShowMenu(new Vector2(eventData.pointerEnter.transform.position.x,
                                             eventData.pointerEnter.transform.position.y));
            Menu.Single.AddButton(Use, "使用");
            Menu.Single.AddButton(Discard, "丢弃");
        }
    }

    private void Use()
    {
        switch (consumablesType)
        {
            case ConsumablesType.回复药:
                playerState.AddLife(value);
                break;
            case ConsumablesType.食物:
                playerState.AddSatiety(value);
                break;
            default:
                break;
        }
        ItemData temp = itemData.Clone();
        temp.number = 1;
        box.GetItem(temp);
        Menu.Single.CloseMenu();
    }

    private void Discard()
    {
        box.GetItem(itemData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public enum ConsumablesType
    {
        回复药,
        食物,
    }
}
