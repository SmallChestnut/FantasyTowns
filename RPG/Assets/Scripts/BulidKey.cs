using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulidKey : MonoBehaviour
{
    [Tooltip("建造按键")]
    public KeyCode key;
    [Tooltip("要建造的房子")]
    public GameObject bulidObj;

    private PlayerBulid playerBulid;
    private void Start()
    {

        // 拿到绑定在玩家的建造系统
        playerBulid = PlayerInputManage.single.GetComponent<PlayerBulid>();
    }
    void Update()
    {
        if(Input.GetKeyDown(key))
        {
            playerBulid.StartBulid(Instantiate(bulidObj));
        }
    }
}
