using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LodCorpse : MonoBehaviour
{
    public float maxTime; // 最大消失时间
    private List<Material> meshs = new List<Material>();
    private float time; // 计时器
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            meshs.Add(transform.GetChild(i).GetComponent<MeshRenderer>().material);
        }
        //meshs = GetComponent<MeshRenderer>().materials as 
        Destroy(gameObject, maxTime);
       
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= maxTime * 0.01)
        {
            foreach (var mesh in meshs)
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
