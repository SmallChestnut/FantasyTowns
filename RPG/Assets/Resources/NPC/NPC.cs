using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{

    [Tooltip("拿工具的手")]
    public Transform hend;
    [Tooltip("肩膀")]
    public SkinnedMeshRenderer shoulder;

    private NavMeshAgent meshAgent;  // 寻路导航
    private NPCAnimation anima;      // 动画    
    private GameObject targetObj;    // 目标对象
    private ItemData itemData;       // 用于收集资源
    private GameObject tool;         // 工具
    private CollectType collectType1;   // 记录采集的类型
    private EnvironmentCreate environmentCreate;    //记录采集的资源地
    private Transform commender;            //  记录发送命令者
    private bool isHome;                // 是否回家

    private void Awake()
    {
        anima = new NPCAnimation(GetComponent<Animator>());
    }

    void Start()
    {
        meshAgent = GetComponent<NavMeshAgent>();
       
        
    }
    private void OnEnable()
    {
        isHome = false;
    }
    private void Update()
    {
        if(isHome)
        {
            if(Vector3.Distance(transform.position, commender.position) < 8)
            {
                House house = commender.GetComponent<House>();
                house.NPCQueue.Enqueue(this);
                house.AddCollect(itemData.Clone());
                itemData = null;
                gameObject.SetActive(false);
            }
           
        }
            
    }
    /// <summary>
    /// 设置资源采集任务
    /// </summary>
    /// <param name="collectType">资源的类型</param>
    /// <param name="environmentCreate">资源区域</param>
    /// <param name="commender">发送命令者</param>
    public void SetCollect(CollectType collectType, EnvironmentCreate environmentCreate, Transform commender)
    {
        this.collectType1 = collectType;
        this.environmentCreate = environmentCreate;
        this.commender = commender;

        Debug.Log("设定目标");
        if (tool != null)
        {
            Destroy(tool);
            tool = null;
        }
        shoulder.sharedMesh = ResourcePath.Single.emptyBasket.GetComponent<MeshFilter>().sharedMesh;
        
        switch (collectType)
        {
            case CollectType.石头:
                StartCoroutine(FindCollect(environmentCreate.stoneListPosition));
                tool = Instantiate(ResourcePath.Single.pickaxe, hend);
                break;
            case CollectType.木头:
                StartCoroutine(FindCollect(environmentCreate.woodListPosition));
                tool = Instantiate(ResourcePath.Single.axe, hend);
                break;
            case CollectType.食物:
                StartCoroutine(FindCollect(environmentCreate.foodListPosition));
                break;
            case CollectType.回复药:
                StartCoroutine(FindCollect(environmentCreate.medicineListPosition));
                break;
            default:
                break;
        }
        if(tool != null)
        {
            tool.transform.position = hend.position;
        }
        
    }
    /// <summary>
    /// 寻找当前资源区域最近的目标
    /// </summary>
    /// <param name="collectList">当前区域的全部资源</param>
    /// <returns></returns>
    private IEnumerator FindCollect(List<Transform> collectList)
    {
        if(collectList.Count <= 0)
        {
            throw new System.Exception("没有资源");
        }

        float minPosition = float.MaxValue; // 用来记录最短的距离，初始化个最大值
        Transform target = null;
        float temp;
        anima.Find();
        for (int i = 0; i < collectList.Count; i++)
        {
            temp = Vector3.Distance(transform.position, collectList[i].position);
            if (temp < minPosition)
            {
                minPosition = temp;
                target = collectList[i];
            }
            yield return new WaitForSeconds(0.01f);
        }
        try
        {
            MoveTargetCollect(target);
            targetObj = target.gameObject;
            targetObj.GetComponent<Collect>().OncollectDestroy += FinishCollect;
        }
        catch (MissingReferenceException)
        {
            MoveTargetCollect(commender);
        }
        Debug.Log("查找");
    }
    /// <summary>
    /// 资源被采集完成后触发的事件处理，背包有东西就回家，没东西就继续找新的资源
    /// </summary>
    /// <param name="obj">资源对象</param>
    /// <param name="e">资源信息</param>
    private void FinishCollect(object obj, CollectEventArgs e)
    {
        
        if (itemData == null || itemData.number == 0)
        {
            Debug.Log("设定值：自己");
            meshAgent.SetDestination(transform.position);
            SetCollect(collectType1, environmentCreate, commender);
        }
        else if(itemData.number > 0)
        {
            isHome = true;
            MoveTargetCollect(commender);// 背包有东西，放回家里
            switch (collectType1)
            {
                case CollectType.石头:
                    // 换成装满石头的篮子
                    shoulder.sharedMesh = ResourcePath.Single.
                                          stoneBasket.GetComponent<MeshFilter>().sharedMesh;
                    break;
                case CollectType.木头:
                    // 换成装满木头的篮子
                    shoulder.sharedMesh = ResourcePath.Single.woodBasket.GetComponent<MeshFilter>().sharedMesh;
                    break;
                case CollectType.食物:
                    // 换成装满食物的篮子
                    shoulder.sharedMesh = ResourcePath.Single.foodBasket.GetComponent<MeshFilter>().sharedMesh;
                    break;
                case CollectType.回复药:
                    // 换成装满药材的篮子
                    shoulder.sharedMesh = ResourcePath.Single.
                                          medicineBasket.GetComponent<MeshFilter>().sharedMesh;
                    break;
                default:
                    break;
            }
            Debug.Log($"完成采集{itemData.name}:{itemData.number}");
        }
        else
        {
            throw new System.Exception();
        }
        
    }

    /// <summary>
    /// 向目标移动
    /// </summary>
    /// <param name="target">目标</param>
    private void MoveTargetCollect(Transform target)
    {
        try
        {
            meshAgent.SetDestination(target.position);
            Debug.Log(target.position);
        }
        catch (System.Exception)
        {
            SetCollect(collectType1, environmentCreate, commender);
        }
        
        anima.Run();
    }
    /// <summary>
    /// 是否到达了目标的旁边
    /// </summary>
    /// <param name="other">碰撞体</param>
    private void OnTriggerEnter(Collider other)
    {
        if (targetObj != null)
        {
            if(other.gameObject == targetObj && !isHome)
            {
                Debug.Log("设定值：自己");
                meshAgent.SetDestination(transform.position);
                anima.Idle();
                StartCoroutine(StartCollect(targetObj.GetComponent<Collect>()));
                targetObj = null;
            }
        }
    }
    /// <summary>
    /// 开始采集该资源
    /// </summary>
    /// <param name="collect">资源类</param>
    /// <returns></returns>
    private IEnumerator StartCollect(Collect collect)
    {
        itemData = new ItemData() { name = collect.collectType.ToString(), number = 0 };
        while(true)
        {
            switch (collect.collectType)
            {
                case CollectType.石头:
                    anima.Mine();
                    break;
                case CollectType.木头:
                    anima.Lumbering();
                    break;
                case CollectType.食物:
                    anima.Gather();
                    break;
                case CollectType.回复药:
                    anima.Gather();
                    break;
                default:
                    break;
            }
            if(collect == null)
            {
                isHome = true;
                MoveTargetCollect(commender);
                break;
            }
            CollectData collectData = collect.GetCollectData(0.01f);
            transform.LookAt(new Vector3(collect.transform.position.x, transform.position.y, collect.transform.position.z));
            if (collectData.number > 0)
            {
                itemData.number += collectData.number;
            }
            if (collectData.isDone)
            {
                switch (collect.collectType)
                {
                    case CollectType.石头:
                        // 换成装满石头的篮子
                        shoulder.sharedMesh = ResourcePath.Single.
                                              stoneBasket.GetComponent<MeshFilter>().sharedMesh;
                        break;
                    case CollectType.木头:
                        // 换成装满木头的篮子
                        shoulder.sharedMesh = ResourcePath.Single.woodBasket.GetComponent<MeshFilter>().sharedMesh;
                        break;
                    case CollectType.食物:
                        // 换成装满食物的篮子
                        shoulder.sharedMesh = ResourcePath.Single.foodBasket.GetComponent<MeshFilter>().sharedMesh;
                        break;
                    case CollectType.回复药:
                        // 换成装满药材的篮子
                        shoulder.sharedMesh = ResourcePath.Single.
                                              medicineBasket.GetComponent<MeshFilter>().sharedMesh;
                        break;
                    default:
                        break;
                }
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(3f);
    }
}

public class NPCAnimation
{
    private Animator animation;
    public NPCAnimation(Animator animation)
    {
        this.animation = animation;
    }
    /// <summary>
    /// 跑
    /// </summary>
    public void Run()
    {
        animation.SetInteger("State", 2);
    }
    /// <summary>
    /// 查看
    /// </summary>
    public void Find()
    {
        animation.SetInteger("State", 3);
    }
    /// <summary>
    /// 休息
    /// </summary>
    public void Idle()
    {
        animation.SetInteger("State", 0);
    }
    /// <summary>
    /// 伐木
    /// </summary>
    public void Lumbering()
    {
        animation.SetInteger("State", 4);
    }
    /// <summary>
    /// 挖矿
    /// </summary>
    public void Mine()
    {
        animation.SetInteger("State", 6);
    }
    /// <summary>
    /// 采集
    /// </summary>
    public void Gather()
    {
        animation.SetInteger("State", 5);
    }
    /// <summary>
    /// 讲话
    /// </summary>
    public void Talk()
    {
        animation.SetInteger("State", 1);
    }
}

