using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulid : MonoBehaviour
{
    public LayerMask layer;
    [HideInInspector]
    public GameObject bulidTemplet;  // 房子的样板
    private bool isBulid;            // 是否启用建造模式
    void Start()
    {
        
    }

    
    void Update()
    {
        if (!isBulid) return;
        RayBulid();
    }
    /// <summary>
    /// 启用建造
    /// </summary>
    /// <param name="bulidObj">需要建造的物体</param>
    public void StartBulid(GameObject bulidObj)
    {
        if (bulidTemplet != null) Destroy(bulidTemplet);

        bulidTemplet = bulidObj;
        isBulid = true;
    }
    /// <summary>
    /// 关闭建造模式
    /// </summary>
    public void CloseBulid()
    {
        if (bulidTemplet != null)
        {
            bulidTemplet.GetComponent<Collider>().isTrigger = false;
        }
        bulidTemplet = null;
        isBulid = false;
    }
    // 用射线进行投射建造
    private void RayBulid()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, 20, layer))
        {
            bulidTemplet.transform.position = hit.point;
        }
        if(Input.GetMouseButtonDown(0))
        {
            // 判断是否满足建造要求
            if (bulidTemplet.GetComponent<HouseBulid>().IsBulid)
            {
                CloseBulid();
            }
        }
        else if(Input.GetMouseButtonDown(1))
        {
            bulidTemplet.transform.Rotate(Vector3.up * 90);
        }
    }
}
