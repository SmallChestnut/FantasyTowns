using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : MonoBehaviour
{
    public float maxTime; // 最大消失时间
    private Material[] meshs;
    private float time; // 计时器
    void Start()
    {
        meshs = GetComponent<MeshRenderer>().materials;
        Destroy(gameObject, maxTime);
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time >= maxTime * 0.01)
        {
            foreach(var mesh in meshs)
            {
                mesh.color = new Color(1, 1, 1, mesh.color.a - 0.01f);
            }
            
            time = 0;
        }

    }
    private void OnDestroy()
    {
        Destroy(transform.parent?.gameObject);
    }
}
