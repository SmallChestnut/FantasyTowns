using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    public LayerMask layer;
    public GameObject textObj;
    public Transform canvas;
    [Tooltip("交互检测球坐标")]
    public Vector3 interactionPosition;
    [Tooltip("检测球半径")]
    public float radius;
    [Tooltip("背包")]
    public Box box;

    [HideInInspector]
    public Queue<NPC> NPCQueue = new Queue<NPC>();

    private ICollect collect;
    private IBulid bulid;
    private IInteractionInterface interaction;
    private PlayerAnimation playerAnimation;
    private UniversalSlider slider;
    private bool isCollect = false;              // 是否正在采集
    private int collectNum;                      // 计数采集的资源数量
    private float timeText;                      // 时间计数器，1.5秒只显示一次采集

    private void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        StartCoroutine(CloseBox());
    }
    IEnumerator CloseBox()
    {
        yield return new WaitForSeconds(0.5f);
        box.CloseBox();
    }
    void Update()
    {
        if (isCollect)
            StartCollect();
        // 采集&建造
        if (Input.GetKeyDown(KeyCode.E))
        {
            // 如果正在采集
            if (isCollect)
            {
                FinishCollectInit();
            }
            // 如果可交互的话
            else if (interaction != null)
            {
                if (interaction.InteractionType == InteractionType.collect)
                {
                    StartCollectInit();
                }
                else if (interaction.InteractionType == InteractionType.bulid)
                {
                    bulid.BulidHouse(box);
                }
            }
        }
        // 建筑销毁
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            if (interaction != null)
            {
                if (interaction.InteractionType == InteractionType.bulid)
                {
                    bulid.DestroyHouse();
                }
            }
        }
        // 打开背包
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (box.gameObject.activeSelf)
            {
                box.CloseBox();
            }
            else
            {
                box.OpenBox();
            }
        }
        RayInspect();
    }

    /// <summary>
    /// 检查是否可以交互
    /// </summary>
    private void RayInspect()
    {
        RaycastHit[] raycastHit = Physics.SphereCastAll(interactionPosition + transform.position, radius, transform.forward, radius / 2, layer);
        if (raycastHit.Length >= 1)
        {
            interaction = raycastHit[0].collider.GetComponent<IInteractionInterface>();
            if (interaction != null)
            {
                if (interaction.InteractionType == InteractionType.collect)
                {
                    collect = interaction as ICollect;
                    HintMessage.Single.ShowMessageText($"{collect.Name}:采集");
                }
                else if (interaction.InteractionType == InteractionType.bulid)
                {
                    HintMessage.Single.ShowMessageText("Q:销毁\nE:建造");
                    bulid = interaction as IBulid;
                }
            }
            else
            {
                House house = raycastHit[0].collider.GetComponent<House>();
                
                if(house != null)
                {
                    
                    if(house.houseMenu.gameObject.activeSelf)
                    {
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            house.CloseMenu();
                        }
                        HintMessage.Single.CloseMessageText();
                    }
                    else
                    {
                        HintMessage.Single.ShowMessageText("E:打开菜单");
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            house.OpenMenu();
                        }
                    }
                }
               
            }
        }
        else
        {
            interaction = null;
            HintMessage.Single.CloseMessageText();
        }

    }

    /// <summary>
    /// 开始采集
    /// </summary>
    private void StartCollect()
    {
        timeText += Time.deltaTime;
        CollectData collectData = collect.GetCollectData(Time.deltaTime);
        slider.UpdateSlider(1 - collectData.remainingTime / collectData.maxTime, $"正在采集{collect.Name}");
        if (collectData.number > 0)
        {
            collectNum += collectData.number;
            if (timeText >= 1.5f)
            {
                Instantiate(textObj, canvas).GetComponent<TextShow>()
                .SetMessageText($"{collectData.collectType}:+{collectNum}");
                box.AddItem(new ItemData() { number = collectNum, name = collectData.collectType.ToString() });
                collectNum = 0;
                timeText = 0;
            }
        }
        if (collectData.isDone)
        {
            FinishCollectInit();
            if (collectNum != 0)
            {
                Instantiate(textObj, canvas).GetComponent<TextShow>()
                .SetMessageText($"{collectData.collectType}:+{collectNum}");
                box.AddItem(new ItemData() { number = collectNum, name = collectData.collectType.ToString() });
            }

            collectNum = 0;
        }

    }
    private void StartCollectInit()
    {
        isCollect = true;
        PlayerInputManage.single.ForbidMove();
        playerAnimation.Busy();
        slider = Instantiate(ResourcePath.Single.universalSlider, canvas).GetComponent<UniversalSlider>();
    }
    private void FinishCollectInit()
    {
        isCollect = false;
        collect = null;
        PlayerInputManage.single.MayMove();
        playerAnimation.CancelBusy();
        Destroy(slider.gameObject);
        slider = null;
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawSphere(interactionPosition + transform.position, radius);
    //}
}
