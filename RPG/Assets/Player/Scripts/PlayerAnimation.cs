using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anima;
    void Start()
    {
        anima = GetComponent<Animator>();
    }
    /// <summary>
    /// 播放行走动画
    /// </summary>
    public void Walk()
    {
        anima.SetBool("IsWalk", true);
        anima.SetBool("IsRun", false);
        anima.SetBool("IsMove", true);
    }
    /// <summary>
    /// 播放奔跑动画
    /// </summary>
    public void Run()
    {
        anima.SetBool("IsRun", true);
        anima.SetBool("IsWalk", false);
        anima.SetBool("IsMove", true);
    }
    /// <summary>
    /// 播放等待动画
    /// </summary>
    public void Idle()
    {
        anima.SetInteger("Idle", Random.Range(1, 4));
        anima.SetBool("IsWalk", false);
        anima.SetBool("IsMove", false);
    }
    /// <summary>
    /// 播放无聊的动画
    /// </summary>
    public void Boring()
    {
        string[] boringNames = { "Boring1", "Boring2", "Boring2" };
        anima.SetTrigger(boringNames[Random.Range(0, 3)]);
    }
    /// <summary>
    /// 播放忙碌的动画
    /// </summary>
    public void Busy()
    {
        anima.SetBool("IsWalk", false);
        anima.SetBool("IsMove", false);
        anima.SetBool("IsBusy", true);
    }
    /// <summary>
    /// 取消播放忙碌动画
    /// </summary>
    public void CancelBusy()
    {
        anima.SetBool("IsBusy", false);
    }
}
