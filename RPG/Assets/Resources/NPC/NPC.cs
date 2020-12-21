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

    [HideInInspector]
    public NavMeshAgent meshAgent;  // 寻路导航

    [HideInInspector]
    public NPCAnimation anima;      // 动画    

    [HideInInspector]
    public ItemData itemData;       // 用于收集资源

    [HideInInspector]
    public GameObject tool;         // 工具

    [HideInInspector]
    public CollectType collectType;   // 记录采集的类型

    [HideInInspector]
    public EnvironmentCreate environmentCreate;    //记录采集的资源地

    [HideInInspector]
    public Transform commender;            //  记录发送命令者

    private NPC_FSM FSM;

    private void Awake()
    {
        anima = new NPCAnimation(GetComponent<Animator>());
        FSM = new NPC_FSM(new NPCIdle(FSM), this);
    }

    void Start()
    {
        meshAgent = GetComponent<NavMeshAgent>();
        

    }

    private void Update()
    {
        FSM.Update();
    }
    public void SetTask(CollectType collectType, EnvironmentCreate environmentCreate, Transform commender)
    {
        this.collectType = collectType;
        this.environmentCreate = environmentCreate;
        this.commender = commender;
        FSM.SetState(new NPCFindCollect(FSM));
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
/// <summary>
/// 休息
/// </summary>
public class NPCIdle : IState
{
    public NPC_FSM _FSM { get; set; }

    public NPCIdle(NPC_FSM _FSM)
    {
        this._FSM = _FSM;
    }
    public void Update()
    {
        _FSM.NPC_Obj.anima.Idle();
    }
}
/// <summary>
/// 状态机
/// </summary>
public class NPC_FSM
{
    public NPC NPC_Obj;
    private IState state;

    public NPC_FSM(IState state, NPC NPC_Obj)
    {
        this.NPC_Obj = NPC_Obj;
        this.state = state;
    }
    /// <summary>
    /// 设置状态
    /// </summary>
    /// <param name="state">实现状态接口的类</param>
    public void SetState(IState state)
    {
        this.state = state;
    }
    /// <summary>
    /// 每帧执行状态
    /// </summary>
    public void Update()
    {
        state.Update();
    }
}
/// <summary>
/// 状态机接口
/// </summary>
public interface IState
{
    NPC_FSM _FSM { get; set; }
    void Update();
}
/// <summary>
/// NPC对资源进行采集
/// </summary>
public class NPCCollect : IState
{
    public NPC_FSM _FSM { get; set; }
    private Collect target;
    public NPCCollect(Transform target, NPC_FSM _FSM)
    {
        this._FSM = _FSM;
        this.target = target.GetComponent<Collect>();
        switch (_FSM.NPC_Obj.collectType)
        {
            case CollectType.石头:
                _FSM.NPC_Obj.anima.Mine();
                break;
            case CollectType.木头:
                _FSM.NPC_Obj.anima.Lumbering();
                break;
            case CollectType.食物:
                _FSM.NPC_Obj.anima.Gather();
                break;
            case CollectType.回复药:
                _FSM.NPC_Obj.anima.Gather();
                break;
            default:
                break;
        }
        _FSM.NPC_Obj.itemData = new ItemData(this.target.collectType.ToString(), 0);
        this.target.OncollectDestroy += Finish;
    }

    public void Update()
    {
        _FSM.NPC_Obj.transform.LookAt(new Vector3(target.transform.position.x, _FSM.NPC_Obj.transform.position.y, target.transform.position.z));
        CollectData collectData = target.GetCollectData(Time.deltaTime);
        if (collectData.number > 0)
        {
            _FSM.NPC_Obj.itemData.number += collectData.number;
            if(collectData.isDone)
            {
                // 再检查一次背包是否有东西，因为在物资累加之前会先触发事件回调
                Finish(this, new CollectEventArgs(_FSM.NPC_Obj.collectType));
            }
        }
       
    }
    // 采集完成
    private void Finish(object obj, CollectEventArgs e)
    {
        // 判断一下背包是否有物资
        if(_FSM.NPC_Obj.itemData.number == 0)
        {
            _FSM.SetState(new NPCFindCollect(_FSM));
        }
        else
        {
            // 将篮子换成装好物资的篮子
            switch (_FSM.NPC_Obj.collectType)
            {
                case CollectType.石头:
                    _FSM.NPC_Obj.shoulder.sharedMesh = ResourcePath.Single.stoneBasket.GetComponent<MeshFilter>().sharedMesh;
                    break;
                case CollectType.木头:
                    _FSM.NPC_Obj.shoulder.sharedMesh = ResourcePath.Single.woodBasket.GetComponent<MeshFilter>().sharedMesh;
                    break;
                case CollectType.食物:
                    _FSM.NPC_Obj.shoulder.sharedMesh = ResourcePath.Single.foodBasket.GetComponent<MeshFilter>().sharedMesh;
                    break;
                case CollectType.回复药:
                    _FSM.NPC_Obj.shoulder.sharedMesh = ResourcePath.Single.medicineBasket.GetComponent<MeshFilter>().sharedMesh;
                    break;
                default:
                    break;
            }
            _FSM.SetState(new NPCMoveHome(_FSM));
        }
    }
}
/// <summary>
/// 搜索资源
/// </summary>
public class NPCFindCollect : IState
{
    public NPC_FSM _FSM { get; set; }
    private List<Transform> collects;
    public NPCFindCollect(NPC_FSM _FSM)
    {
        this._FSM = _FSM;
        _FSM.NPC_Obj.anima.Find();
        _FSM.NPC_Obj.shoulder.sharedMesh = ResourcePath.Single.emptyBasket.GetComponent<MeshFilter>().sharedMesh;
        if(_FSM.NPC_Obj.tool != null)
        {
            GameObject.Destroy(_FSM.NPC_Obj.tool);
            _FSM.NPC_Obj.tool = null;
        }
        // 根据物资查找类型更换对应的物资工具，并且确定要搜索的物资列表
        switch (_FSM.NPC_Obj.collectType)
        {
            case CollectType.石头:
                collects = _FSM.NPC_Obj.environmentCreate.stoneListPosition;
                _FSM.NPC_Obj.tool = GameObject.Instantiate(ResourcePath.Single.pickaxe, _FSM.NPC_Obj.hend);
                _FSM.NPC_Obj.tool.transform.position = _FSM.NPC_Obj.hend.position;
                break;
            case CollectType.木头:
                collects = _FSM.NPC_Obj.environmentCreate.woodListPosition;
                _FSM.NPC_Obj.tool = GameObject.Instantiate(ResourcePath.Single.axe, _FSM.NPC_Obj.hend);
                _FSM.NPC_Obj.tool.transform.position = _FSM.NPC_Obj.hend.position;
                break;
            case CollectType.食物:
                collects = _FSM.NPC_Obj.environmentCreate.foodListPosition;
                // 空手
                break;
            case CollectType.回复药:
                collects = _FSM.NPC_Obj.environmentCreate.medicineListPosition;
                // 空手
                break;
            default:
                break;
        }
    }
    int index = 0;
    float distance = float.MaxValue;    // 记录最近的值
    Transform target = null;
    public void Update()
    {
        _FSM.NPC_Obj.meshAgent.SetDestination(_FSM.NPC_Obj.transform.position);
        if (collects.Count == 0)
        {
            _FSM.SetState(new NPCMoveHome(_FSM));
        }
        else if(index >= collects.Count)
        {
            _FSM.SetState(new NPCMoveCollect(target, _FSM));
        }
        else if(Vector3.Distance(_FSM.NPC_Obj.transform.position, collects[index].position) < distance)
        {
            target = collects[index];
            distance = Vector3.Distance(_FSM.NPC_Obj.transform.position, collects[index].position);
        }
        index++;
    }
}
/// <summary>
/// NPC向物资移动
/// </summary>
public class NPCMoveCollect : IState
{
    public NPC_FSM _FSM { get; set; }
    private Transform target;
    public NPCMoveCollect(Transform target, NPC_FSM _FSM)
    {
        this._FSM = _FSM;
        _FSM.NPC_Obj.anima.Run();
        this.target = target;
        try
        {
            _FSM.NPC_Obj.meshAgent.SetDestination(target.position);
            target.GetComponent<Collect>().OncollectDestroy += Finish;
        }
        catch (MissingReferenceException)
        {

            _FSM.SetState(new NPCMoveHome(_FSM));
        }
       
    }


    public void Update()
    {
        Ray ray = new Ray(_FSM.NPC_Obj.transform.position, _FSM.NPC_Obj.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 1, LayerMask.GetMask("Collect")))
        {
            // 到达目的地
            if(hit.transform == target)
            {
                _FSM.NPC_Obj.meshAgent.SetDestination(_FSM.NPC_Obj.transform.position);
                _FSM.SetState(new NPCCollect(target, _FSM));
            }
        }
    }
    // 当物资被别人采集完后
    private void Finish(object obj, CollectEventArgs e)
    {
        _FSM.SetState(new NPCFindCollect(_FSM));
    }
}
/// <summary>
/// NPC往家里移动
/// </summary>
public class NPCMoveHome : IState
{
    public NPC_FSM _FSM { get; set; }

    public NPCMoveHome(NPC_FSM _FSM)
    {
        this._FSM = _FSM;
        _FSM.NPC_Obj.anima.Run();
        _FSM.NPC_Obj.meshAgent.SetDestination(_FSM.NPC_Obj.commender.position);
    }

    public void Update()
    {
        //_FSM.NPC_Obj.transform.LookAt(new Vector3(_FSM.NPC_Obj.commender.position.x, _FSM.NPC_Obj.transform.position.y, _FSM.NPC_Obj.commender.position.z));
        /*
        Ray ray = new Ray(_FSM.NPC_Obj.transform.position, _FSM.NPC_Obj.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3, LayerMask.GetMask("House")))
        {
            Debug.Log(hit.collider.name);
            // 到达目的地
            if (hit.transform == _FSM.NPC_Obj.commender)
            {
                _FSM.NPC_Obj.commender.GetComponent<House>().AddCollect(_FSM.NPC_Obj.itemData.Clone());
                _FSM.NPC_Obj.commender.GetComponent<House>().NPCQueue.Enqueue(_FSM.NPC_Obj);
                _FSM.SetState(new NPCIdle(_FSM));
                _FSM.NPC_Obj.gameObject.SetActive(false);
            }
        }
        */
        if(Vector3.Distance(_FSM.NPC_Obj.transform.position, _FSM.NPC_Obj.commender.position) < 8)
        {
            _FSM.NPC_Obj.commender.GetComponent<House>().AddCollect(_FSM.NPC_Obj.itemData.Clone());
            _FSM.NPC_Obj.commender.GetComponent<House>().NPCQueue.Enqueue(_FSM.NPC_Obj);
            _FSM.SetState(new NPCIdle(_FSM));
            _FSM.NPC_Obj.gameObject.SetActive(false);
        }
    }
}

