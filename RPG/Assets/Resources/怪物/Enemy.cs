using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System;

public class Enemy : MonoBehaviour, ILife
{
    [Tooltip("眼睛")]
    public Transform eye;
    [Tooltip("视觉旋转速度")]
    public float eyeSpeed;
    [Tooltip("攻击点")]
    public Transform hitPoint;
    [Tooltip("伤害值")]
    public int hitValue;
    [Tooltip("生命值")]
    public int maxLifeValue;
    [Tooltip("生命值填充")]
    public Image lifeRedImage;
    [Tooltip("生命值流血填充")]
    public Image lifeYellowImage;

    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public NavMeshAgent meshAgent;
    [HideInInspector]
    public Vector3 range;       // 怪物生成的范围，用来作为巡逻的界限
    [HideInInspector]
    public Vector3 monsterCreatorCenter;
    [HideInInspector]
    public event Action OnDie;  // 死亡事件

    private EnemyFsm _FSM;
    private GameObject player; // 记录被谁打了？
    private int lifeValue;     // 当前生命值
    private float lifeImageTimer;   // 黄色生命值缓慢流血计时器


    public void Reduce(GameObject obj, int value)
    {
        player = null;
        if (lifeValue <= 0) return;
        lifeValue -= value;
        Debug.Log("我被打了，当前生命值:" + lifeValue);
        if (lifeValue <= 0)
        {
            _FSM.SetState(new EnemyDie(_FSM));
            lifeValue = 0;
            OnDie?.Invoke();
        }
        else
        {
            _FSM.SetState(new EnemyGetHit(_FSM));
            player = obj;
        }
        lifeRedImage.fillAmount = (float)lifeValue / maxLifeValue;
        lifeImageTimer = 0.8f; // 更新计时器
    }
    IEnumerator ReduceLifeImage()
    {
        while(true)
        {
            
            if(lifeImageTimer <= 0 && lifeYellowImage.fillAmount > lifeRedImage.fillAmount)
            {
                lifeYellowImage.fillAmount -= 0.01f;
            }
            else
            {
                lifeImageTimer -= Time.deltaTime;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        _FSM = new EnemyFsm(this);
        _FSM.SetState(new EnemyIdle(_FSM));
        meshAgent = transform.parent.GetComponent<NavMeshAgent>();
        lifeValue = maxLifeValue;
        StartCoroutine(ReduceLifeImage());
        lifeRedImage.fillAmount = 1;
        lifeYellowImage.fillAmount = 1;
    }
    public void AddAnimationEvent(string functionName, string AnimationName)
    {
        foreach (var x in animator.runtimeAnimatorController.animationClips)
        {
            if (x.name == AnimationName)
            {
                x.events = default;
                AnimationEvent animationEvent = new AnimationEvent();
                animationEvent.time = x.length * 0.5f;
                animationEvent.functionName = functionName;
                x.AddEvent(animationEvent);
            }
        }
    }
    private void EndPlayGetHitAnimation()
    {
        if (lifeValue <= 0) return;

        _FSM.SetState(new EnemyFollow(_FSM, player.transform));
    }
    void Update()
    {
        _FSM.Update();
    }
    // 普通攻击
    private void Hit()
    {
        Ray ray = new Ray(hitPoint.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3, LayerMask.GetMask("Player")))
        {
            hit.collider.GetComponentInParent<PlayerState>().ReduceLife(hitValue);
        }
    }
    // 强力攻击
    private void PowerHit()
    {
        Ray ray = new Ray(hitPoint.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3, LayerMask.GetMask("Player")))
        {
            hit.collider.GetComponentInParent<PlayerState>().ReduceLife(hitValue * 2);
        }
    }
    /// <summary>
    /// 添加动画事件
    /// </summary>
    /// <param name="animationName">动画名字</param>
    /// <param name="animationEvent">动画事件</param>
    /// <param name="isClear">添加之前是否清除全部动画事件</param>
    /// <returns>事件在该动画的时间</returns>
    public float AddAnimationEvent(string animationName, AnimationEvent animationEvent, bool isClear, float percent)
    {
        foreach (var x in animator.runtimeAnimatorController.animationClips)
        {
            if (x.name == animationName)
            {
                if (isClear) x.events = default;
                animationEvent.time = x.length * percent;
                x.AddEvent(animationEvent);
                return x.length * percent;
            }
        }
        throw new System.Exception("没有找到相关动画");
    }

    /// <summary>
    /// 视觉，周围是否有攻击目标
    /// </summary>
    public GameObject Eye()
    {
        Ray ray = new Ray(eye.position, eye.forward);
        eye.Rotate(Vector3.up * Time.deltaTime * eyeSpeed);
        if (Physics.Raycast(ray, out RaycastHit hit, 15, LayerMask.GetMask("Player")))
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(eye.position, transform.position + eye.forward * 15);
    }

}
public class EnemyFsm
{
    public Enemy enemy;
    public IEnemyState state;

    public EnemyFsm(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void SetState(IEnemyState state)
    {
        this.state = state;
        state.FSM = this;
    }
    public void Update()
    {
        state.Update();
    }
}
public interface IEnemyState
{
    EnemyFsm FSM { get; set; }
    void Update();
}
// 被攻击
public class EnemyGetHit : IEnemyState
{
    public EnemyFsm FSM { get; set; }

    public EnemyGetHit(EnemyFsm enemyFsm)
    {
        FSM = enemyFsm;
        FSM.enemy.animator.SetInteger("State", 4);
        enemyFsm.enemy.AddAnimationEvent("EndPlayGetHitAnimation", "GetHit");

    }

    public void Update()
    {

    }

}
// 休息
class EnemyIdle : IEnemyState
{
    public EnemyFsm FSM { get; set; }
    private float timer;
    private GameObject opponent;

    public EnemyIdle(EnemyFsm enemyFsm)
    {
        FSM = enemyFsm;
        FSM.enemy.animator.SetInteger("State", 0);
        timer = UnityEngine.Random.Range(2, 8);
    }

    public void Update()
    {
        opponent = FSM.enemy.Eye();
        if(opponent != null)
        {
            FSM.SetState(new EnemyFollow(FSM, opponent.transform));
            return;
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            FSM.SetState(new EnemyRun(FSM));
        }
    }

}
// 巡逻
public class EnemyRun : IEnemyState
{
    private GameObject opponent;
    private Vector3 target;
    private float maxRang;
    public EnemyRun(EnemyFsm FSM)
    {
        this.FSM = FSM;
        FSM.enemy.animator.SetInteger("State", 3);
        // 生成器的中心与半径range的计算得到在此范围内的随机坐标
        Vector3 temp = new Vector3
            (
            UnityEngine.Random.Range(FSM.enemy.monsterCreatorCenter.x - FSM.enemy.range.x, FSM.enemy.monsterCreatorCenter.x + FSM.enemy.range.x),
            FSM.enemy.monsterCreatorCenter.y,
            UnityEngine.Random.Range(FSM.enemy.monsterCreatorCenter.z - FSM.enemy.range.z, FSM.enemy.monsterCreatorCenter.z + FSM.enemy.range.z)
            );
        Ray ray = new Ray(temp, Vector3.down);
        if(Physics.Raycast(ray, out RaycastHit hit, LayerMask.GetMask("Map")))
        {
            FSM.enemy.meshAgent.SetDestination(hit.point);
            target = hit.point;
        }
        else
        {
            FSM.enemy.meshAgent.SetDestination(FSM.enemy.monsterCreatorCenter);
            target = FSM.enemy.monsterCreatorCenter;
        }
        maxRang = FSM.enemy.range.x > FSM.enemy.range.z ? FSM.enemy.range.x : FSM.enemy.range.z;
    }

    public EnemyFsm FSM { get; set; }

    public void Update()
    {
        // 到达目的地
        if (Vector3.Distance(FSM.enemy.meshAgent.destination, FSM.enemy.transform.parent.position) < 2)
        {
            FSM.SetState(new EnemyIdle(FSM));
            return;
        }
        opponent = FSM.enemy.Eye();
        if (opponent != null && Vector3.Distance(FSM.enemy.transform.parent.position, FSM.enemy.monsterCreatorCenter) < maxRang)
        {
            FSM.SetState(new EnemyFollow(FSM, opponent.transform));
        }
    }
}
// 追击
public class EnemyFollow : IEnemyState
{
    public EnemyFsm FSM { get; set; }
    public Transform target;
    private float maxRang;

    public EnemyFollow(EnemyFsm enemyFsm, Transform transform)
    {
        FSM = enemyFsm;
        target = transform;
        maxRang = FSM.enemy.range.x > FSM.enemy.range.z ? FSM.enemy.range.x : FSM.enemy.range.z;
        FSM.enemy.animator.SetInteger("State", 3);
    }

    public void Update()
    {
        // 如果超出怪物制作中心的范围就回去继续巡逻
        if (Vector3.Distance(FSM.enemy.transform.position, FSM.enemy.monsterCreatorCenter) > maxRang * 2)
        {
            FSM.enemy.meshAgent.SetDestination(FSM.enemy.monsterCreatorCenter);
            FSM.SetState(new EnemyRun(FSM));
        }

        FSM.enemy.meshAgent.SetDestination(target.position);
        if (Vector3.Distance(FSM.enemy.transform.position, target.position) < 3f)
        {
            FSM.enemy.meshAgent.SetDestination(FSM.enemy.transform.parent.position);
            FSM.SetState(new EnemyHit(FSM, target.gameObject));
        }
    }
}
// 攻击
public class EnemyHit : IEnemyState
{
    public EnemyFsm FSM { get; set; }
    private GameObject target;
    private float hitTimer = 1;
    private float animationEventTime;
    private float animationCloseTime = 0;
    private bool isStartHit;
    int animationStateValue;
    public EnemyHit(EnemyFsm enemyFsm, GameObject target)
    {
        FSM = enemyFsm;
        this.target = target;
        float temp = UnityEngine.Random.Range(0f, 1f);

        AnimationEvent animationEvent = new AnimationEvent();
        if (temp >= 0.7f)
        {
            animationEvent.functionName = "Hit";
            animationEventTime = FSM.enemy.AddAnimationEvent("Attack02", animationEvent, true, 0.8f);
            animationStateValue = 1;
        }
        else
        {
            animationEvent.functionName = "PowerHit";
            animationEventTime = FSM.enemy.AddAnimationEvent("Attack01", animationEvent, true, 0.8f);
            animationStateValue = 2;
        }
        
    }

    public void Update()
    {
        FSM.enemy.transform.parent.LookAt(new Vector3(target.transform.position.x, FSM.enemy.transform.position.y, target.transform.position.z));
        // 如果目标离开了攻击范围就切换追击状态
        if(Vector3.Distance(FSM.enemy.transform.position, target.transform.position) > 3)
        {
            FSM.SetState(new EnemyFollow(FSM, target.transform));
        }
        // 攻击前摇
        if (hitTimer > 0)
        {
            hitTimer -= Time.deltaTime;

        }
        // 满足条件就开始攻击
        else if(hitTimer <= 0 && !isStartHit)
        {
            FSM.enemy.animator.SetInteger("State", animationStateValue);
            animationCloseTime = animationEventTime;
            isStartHit = true;
        }
        // 正在攻击
        else if (animationCloseTime > 0)
        {
            animationCloseTime -= Time.deltaTime;
        }
        // 攻击结束
        else if(animationCloseTime <= 0)
        {
            hitTimer = 4;
            isStartHit = false;
            FSM.SetState(new EnemyFollow(FSM, target.transform));
        }
    }
}
// 死亡
public class EnemyDie : IEnemyState
{
   

    public EnemyDie(EnemyFsm enemyFsm)
    {
        enemyFsm.enemy.animator.SetInteger("State", 5);
        GameObject.Destroy(enemyFsm.enemy.meshAgent);
        GameObject.Destroy(enemyFsm.enemy.transform.parent.gameObject, 5);

    }

    public EnemyFsm FSM { get; set; }

    public void Update()
    {
        
    }
}