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
    private ICollect collect;
    private IInteractionInterface interaction;
    private PlayerMove playerMove;
    private PlayerAnimation playerAnimation;
    private UniversalSlider slider;
    private bool isCollect = false;              // 是否正在采集
    private int collectNum;                      // 计数采集的资源数量
    private float timeText;                      // 时间计数器，1.5秒只显示一次采集

    private void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }
    void Update()
    {
        if (isCollect)
            StartCollect();
        if (Input.GetKeyDown(KeyCode.E))
        {
            // 如果正在采集
            if (isCollect)
            {
                FinishCollectInit();
            }
            // 如果可交互的话
            else if(interaction != null)
            {
               if(interaction.InteractionType == InteractionType.collect)
                {
                    StartCollectInit();
                }
            }
        }
        RayInspect();
    }

    private void RayInspect()
    {
        //Ray ray = new Ray(interactionPosition + transform.position, transform.forward);
        //if (Physics.Raycast(ray, out RaycastHit raycastHit, 5, layer))
        //{
        //    interaction = raycastHit.collider.GetComponent<IInteractionInterface>();
        //    if (interaction != null)
        //    {
        //        if (interaction.InteractionType == InteractionType.collect)
        //        {
        //            collect = interaction as ICollect;
        //            HintMessage.Single.ShowMessageText($"{collect.Name}:采集");
        //        }
        //    }
        //}
        RaycastHit[] raycastHit = Physics.SphereCastAll(interactionPosition + transform.position, radius, transform.forward, radius / 2, layer);
        if (raycastHit.Length >= 1)
        {
            Debug.Log(raycastHit[0].collider.name);
            interaction = raycastHit[0].collider.GetComponent<IInteractionInterface>();
            if (interaction != null)
            {
                if (interaction.InteractionType == InteractionType.collect)
                {
                    collect = interaction as ICollect;
                    HintMessage.Single.ShowMessageText($"{collect.Name}:采集");
                }
            }
        }
        else
        {
            interaction = null;
            HintMessage.Single.CloseMessageText();
        }

    }

    private void StartCollect()
    {
        timeText += Time.deltaTime;
        CollectData collectData = collect.GetCollectData(Time.deltaTime);
        slider.UpdateSlider(1 - collectData.remainingTime / collectData.maxTime, $"正在采集{collect.Name}");
        if(collectData.number > 0)
        {
            collectNum += collectData.number;
            if (timeText >= 1.5f)
            {
                Instantiate(textObj, canvas).GetComponent<TextShow>()
                .SetMessageText($"{collectData.collectType}:+{collectNum}");
                collectNum = 0;
                timeText = 0;
            }
        }
        if (collectData.isDone)
        {
            FinishCollectInit();
            if (collectNum != 0)
                Instantiate(textObj, canvas).GetComponent<TextShow>()
                .SetMessageText($"{collectData.collectType}:+{collectNum}");
            collectNum = 0;
        }
        
    }
    private void StartCollectInit()
    {
        isCollect = true;
        playerMove.isMove = false;
        playerAnimation.Busy();
        slider = Instantiate(ResourcePath.Single.universalSlider, canvas).GetComponent<UniversalSlider>();
    }
    private void FinishCollectInit()
    {
        isCollect = false;
        collect = null;
        playerMove.isMove = true;
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
