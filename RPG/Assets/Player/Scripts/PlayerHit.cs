using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    /// <summary>
    /// 攻击动画是否结束
    /// </summary>
    public bool isEndHit { get; private set; } = true;


    private Animator animator;
    private AnimationClip[] animationClips;
    private float hitTimer;         // 连击计时
    private float maxTime;          // 最大可连击时间计时
    private bool isDoubleHit;       // 是否连击
    private Transform hitPoint;       // 攻击点
#pragma warning disable 0649
    private float hitDistance;        // 攻击距离

    private int power;              // 攻击伤害
    private CinemachineCollisionImpulseSource cinemachineCollisionImpulseSource;
    public void SetPlayerHit(Transform hitPoint, float hitDistance, int power, float maxTime)
    {
        this.hitPoint = hitPoint;
        this.power = power;
        this.maxTime = maxTime;
        this.hitDistance = hitDistance;
        animationClips = animator.runtimeAnimatorController.animationClips;
        AnimationEvent animationEvent = new AnimationEvent();
        AnimationEvent startHitEvent = new AnimationEvent();
        AnimationEvent EndHitEvent = new AnimationEvent();
        animationEvent.functionName = "Atk";
        startHitEvent.functionName = "StartAtk";
        EndHitEvent.functionName = "EndAtk";
        foreach (var clip in animationClips)
        {
            if (clip.name == "Atk1" || clip.name == "Atk3" || clip.name == "Atk4")
            {
                startHitEvent.time = 0;                          // 在动画的开始处添加事件
                animationEvent.time = clip.length * 0.5f;        // 在动画的70%处添加事件
                EndHitEvent.time = clip.length;                  // 在动画结束后添加事件
                clip.AddEvent(animationEvent);
                clip.AddEvent(startHitEvent);
                clip.AddEvent(EndHitEvent);
               
            }
            else if (clip.name == "Atk2")
            {
                AnimationEvent animationEvent1 = new AnimationEvent();
                animationEvent1.functionName = "PowerHit";
                animationEvent1.time = clip.length * 0.5f;
                clip.AddEvent(animationEvent1);
            }
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        
    }
    private void Start()
    {
        cinemachineCollisionImpulseSource = GetComponent<CinemachineCollisionImpulseSource>();
    }
    public void Update()
    {
        HitInput();
        if (hitTimer > 0)
        {
            hitTimer -= Time.deltaTime;
            isDoubleHit = true;
            isEndHit = false;
        }
        else
        {
            isDoubleHit = false;
            isEndHit = true;
        }
        if (isDoubleHit)
        {
            animator.SetBool("IsHit", true);
        }
        else
        {
            animator.SetBool("IsHit", false);
        }
    }

    // 攻击输入
    private void HitInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            hitTimer = maxTime;
        }
    }
    private void PowerHit()
    {
        Ray ray = new Ray(hitPoint.position, transform.forward);
        RaycastHit[] hits = Physics.SphereCastAll(ray, 1f, hitDistance, LayerMask.GetMask("Enemy"));
        if (hits != null)
        {
            foreach (var hit in hits)
            {
                ILife life = hit.collider.GetComponent<ILife>();
                if (life != null)
                {
                    life.Reduce(gameObject, power * 2);
                    cinemachineCollisionImpulseSource.GenerateImpulse(new Vector3(1, 2, 0));
                }
            }

        }
    }
    // 攻击敌人，对敌人造成伤害
    private void Atk()
    {
        Ray ray = new Ray(hitPoint.position, transform.forward);
        RaycastHit[] hits = Physics.SphereCastAll(ray, 1f, hitDistance, LayerMask.GetMask("Enemy"));
        if (hits != null)
        {
            foreach(var hit in hits)
            {
                ILife life = hit.collider.GetComponent<ILife>();
                if (life != null)
                {
                    life.Reduce(gameObject, power);
                }
            }
           
        }
    }

    // 攻击动画启动事件处理
    private void StartAtk()
    {
        isEndHit = false;
    }
    // 攻击动画结束事件处理
    private void EndAtk()
    {
        isEndHit = true;

    }

}




public interface ILife
{
    void Reduce(GameObject obj, int value);
}
