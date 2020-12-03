using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class ArticleUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform canvas;
    private Text text;          // 物品数量下标
    public ItemData itemData;
    public ArticleType articleType;     // 物品类型
    [TextArea]
    public string explain;              // 物品的解释
    public int maxNumber = 1;           // 物品最大数量
    [HideInInspector]
    public Box box;
    public GridUI ArticleGrid { get; set; }
    public int Number
    {
        get => itemData.number;
        set
        {
            itemData.number = value;
        }
    }
    /// <summary>
    /// 更新物品数量显示
    /// </summary>
    public void UpdateNumberText()
    {
        if(text == null)
        {
            text = transform.GetChild(0).GetComponent<Text>();
        }
        text.text = itemData.number.ToString();
    }
    public void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GameObject.Find("Canvas").transform;
        Number = itemData.number;
        box = transform.parent.parent.parent.GetComponent<Box>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;

        // 离开格子之前拿到格子的引用并且格子不再引用自己
        ArticleGrid = transform.parent.GetComponent<GridUI>();
        ArticleGrid.Article = null;

        transform.SetParent(canvas);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;

        #region 如果没有更换新的格子就回归原本的格子，更换了就换新的格子
        rectTransform.position = ArticleGrid.rectTransform.position;
        transform.SetParent(ArticleGrid.transform);
        ArticleGrid.Article = this;
        #endregion
    }

    /// <summary>
    /// 替换格子
    /// </summary>
    /// <param name="gridUI">替换的目标格子</param>
    public void ChangeGrid(GridUI gridUI)
    {
        (transform as RectTransform).position = gridUI.rectTransform.position;
        transform.SetParent(gridUI.transform);
        gridUI.Article = this;
        ArticleGrid = gridUI;
        (transform as RectTransform).sizeDelta = new Vector2(-10, -10);
        //(transform as RectTransform).offsetMin = new Vector2(5, 5);
    }
}

public enum ArticleType
{
    consumables, // 消耗品
    bulid,       // 建造物品
}

public class Menu
{
    public static Menu Single { get; } = new Menu();

    public GameObject menu;
    public List<GameObject> buttons = new List<GameObject>();
    public Transform canvas;
    public RectTransform menuRectTransform;

    private Menu()
    {
        canvas = GameObject.Find("Canvas").transform;
        menu = GameObject.Instantiate(ResourcePath.Single.menu, canvas);
        menuRectTransform = menu.GetComponent<RectTransform>();
        menu.SetActive(false);
    }
    /// <summary>
    /// 添加一个按钮选项
    /// </summary>
    /// <param name="call">该选项的回调方法</param>
    /// <param name="buttonText">该选项的名字</param>
    public void AddButton(UnityAction call, string buttonText)
    {
        Button button = GameObject.Instantiate(ResourcePath.Single.button, Single.menu.transform).GetComponent<Button>();
        Text text = button.transform.GetChild(0).GetComponent<Text>();
        button.onClick.AddListener(call);
        text.text = buttonText;
        buttons.Add(button.gameObject);
    }
    /// <summary>
    /// 删除所有按钮
    /// </summary>
    public void RemoveAllButton()
    {
        foreach (var x in buttons)
            GameObject.Destroy(x);
        buttons.Clear();
    }
    /// <summary>
    /// 显示菜单
    /// </summary>
    /// <param name="position">菜单显示的位置</param>
    public void ShowMenu(Vector3 position)
    {
        menu.SetActive(true);
        menu.transform.position = new Vector2(position.x + menuRectTransform.sizeDelta.x / 2, 
                                              position.y - menuRectTransform.sizeDelta.y / 2);
        menu.transform.SetAsLastSibling();
    }
    /// <summary>
    /// 关闭菜单
    /// </summary>
    public void CloseMenu()
    {
        menu.SetActive(false);
        RemoveAllButton();
    }
}
