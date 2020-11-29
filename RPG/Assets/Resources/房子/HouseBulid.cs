using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseBulid : MonoBehaviour, IBulid
{
    [Tooltip("房子本体")]
    public GameObject houseObj;
    [Tooltip("需要的石头量")]
    public ItemData stone;
    [Tooltip("需要的木头量")]
    public ItemData wood;
    [Tooltip("忽略的图层")]
    public LayerMask layer;


    private Transform canvas;
    private Text stoneText;
    private Text woodText;
    /// <summary>
    /// 是否可以建造该房子
    /// </summary>
    public bool IsBulid { get; set; } = true;

    private List<Material> materials = new List<Material>();
    private Color color;

    public InteractionType InteractionType => InteractionType.bulid;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            try
            {
                Material temp = transform.GetChild(i).GetComponent<MeshRenderer>().material;
                materials.Add(temp);
            }
            catch (MissingComponentException)
            {

            }
        }
        color = materials[0].color;
        canvas = transform.Find("Canvas");
        stoneText = canvas.Find("石头").GetChild(0).GetComponent<Text>();
        woodText = canvas.Find("木头").GetChild(0).GetComponent<Text>();
        UpdateText();
    }
    private void UpdateText()
    {
        stoneText.text = stone.number.ToString();
        woodText.text = wood.number.ToString();
    }    
    /// <summary>
    /// 拿材料建造房子
    /// </summary>
    /// <param name="box">材料的容器</param>
    public void BulidHouse(Box box)
    {
        int stoneNum = box.GetItem(stone.Clone());
        int woodNum = box.GetItem(wood.Clone());
        stone.number -= stoneNum;
        wood.number -= woodNum;
        if (stone.number == 0 && wood.number == 0)
        {
            Instantiate(houseObj, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        if (stone.number < 0 || wood.number < 0)
            throw new System.Exception();
        UpdateText();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Map")
            return;
        foreach(var x in materials)
        {
            x.color = new Color(255, 0, 0, 0.25f);
        }
        IsBulid = false;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Map")
            return;
        foreach (var x in materials)
        {
            x.color = color;
        }
        IsBulid = true;
    }

    /// <summary>
    /// 销毁房子
    /// </summary>
    public void DestroyHouse()
    {
        Destroy(gameObject);
    }
}
