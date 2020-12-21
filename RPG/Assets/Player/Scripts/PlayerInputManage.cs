using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManage : MonoBehaviour
{
    public static PlayerInputManage single;

    private PlayerMovePosition playerMove;
    [Tooltip("玩家自由视角智能摄像机")]
    public GameObject VMcamera;

    private void Awake()
    {
        single = this;
    }
    void Start()
    {
        playerMove = GetComponent<PlayerMovePosition>();
    }
    /// <summary>
    /// 禁用玩家移动
    /// </summary>
    public void ForbidMove()
    {
        playerMove.isMove = false;
    }
    /// <summary>
    /// 启用玩家移动
    /// </summary>
    public void MayMove()
    {
        playerMove.isMove = true;
        Menu.Single.CloseMenu();
    }
    /// <summary>
    /// 禁用玩家视角控制
    /// </summary>
    public void ForbidVMCamera()
    {
        VMcamera.SetActive(false);
    }
    /// <summary>
    /// 启用玩家视角控制
    /// </summary>
    public void MayVMCamera()
    {
        VMcamera.SetActive(true);
    }

    
}
