using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentCreate : MonoBehaviour
{
    /// <summary>
    /// 记录着所有环境生成器
    /// </summary>
    [HideInInspector]
    public static List<EnvironmentCreate> environmentCreates = new List<EnvironmentCreate>();


    [Header("环境生成的范围")]
    public Vector3 crash;

    [Header("资源生成的数量和碰撞范围")]
    public EnvironmentNumber wood;
    public EnvironmentNumber stone;
    public EnvironmentNumber food;
    public EnvironmentNumber medicine;

    [Header("生成射线碰撞层")]
    public LayerMask layer;

    [Header("检查不适合放置的图层")]
    public LayerMask isPut;

    // 用来存放各个资源的位置
    [HideInInspector]
    public List<Transform> woodListPosition = new List<Transform>();
    [HideInInspector]
    public List<Transform> stoneListPosition = new List<Transform>();
    [HideInInspector]
    public List<Transform> foodListPosition = new List<Transform>();
    [HideInInspector]
    public List<Transform> medicineListPosition = new List<Transform>();


    // 用来实例化各项资源的列表
    private List<GameObject> woodList = new List<GameObject>();
    private List<GameObject> stoneList = new List<GameObject>();
    private List<GameObject> foodList = new List<GameObject>();
    private List<GameObject> medicineList = new List<GameObject>();

    // 各项资源的计数
    private int woodSum;
    private int stoneSum;
    private int foodSum;
    private int medicineSum;

    private void Start()
    {
        #region 树
        woodList.Add(ResourcePath.Single.woodObj1);
        woodList.Add(ResourcePath.Single.woodObj2);
        woodList.Add(ResourcePath.Single.woodObj3);
        woodList.Add(ResourcePath.Single.woodObj4);
        woodList.Add(ResourcePath.Single.woodObj5);
        woodList.Add(ResourcePath.Single.woodObj6);
        woodList.Add(ResourcePath.Single.woodObj7);
        #endregion

        #region 石头
        stoneList.Add(ResourcePath.Single.stoneObj);
        #endregion

        #region 食物
        foodList.Add(ResourcePath.Single.foodObj1);
        foodList.Add(ResourcePath.Single.foodObj2);
        foodList.Add(ResourcePath.Single.foodObj3);
        foodList.Add(ResourcePath.Single.foodObj4);
        #endregion

        #region 回复药
        medicineList.Add(ResourcePath.Single.medicineObj1);
        medicineList.Add(ResourcePath.Single.medicineObj2);
        medicineList.Add(ResourcePath.Single.medicineObj3);
        #endregion

        StartCoroutine(CreateWood());
        StartCoroutine(CreateFood());
        StartCoroutine(CreateMedicine());
        StartCoroutine(CreateStone());

        environmentCreates.Add(this);
    }
    IEnumerator CreateWood()
    {
        while(true)
        {
            // 随机生成木头,数量要小于或等于设定的数量
            while (woodSum <= wood.number)
            {
                Vector3 position = GetPonint(wood.crashRange);
                if (position != Vector3.zero)
                {
                    GameObject temp =
                    Instantiate(woodList[UnityEngine.Random.Range(0, woodList.Count)], position, Quaternion.identity);
                    woodSum++;
                    temp.GetComponent<Collect>().OncollectDestroy += OnDestroyCollect;
                    woodListPosition.Add(temp.transform);
                }

                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.1f);
        }
      
    }
    IEnumerator CreateFood()
    {
        while(true)
        {
            // 随机生成食物,数量要小于或等于设定的数量
            while (foodSum <= food.number)
            {
                Vector3 position = GetPonint(food.crashRange);
                if (position != Vector3.zero)
                {
                    GameObject temp =
                    Instantiate(foodList[UnityEngine.Random.Range(0, foodList.Count)], position, Quaternion.identity);
                    foodSum++;
                    temp.GetComponent<Collect>().OncollectDestroy += OnDestroyCollect;
                    foodListPosition.Add(temp.transform);
                }

                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.1f);
        }
      
    }
    IEnumerator CreateMedicine()
    {
        while(true)
        {
            // 随机生成草药,数量要小于或等于设定的数量
            while (medicineSum <= medicine.number)
            {
                Vector3 position = GetPonint(medicine.crashRange);
                if (position != Vector3.zero)
                {
                    GameObject temp =
                    Instantiate(medicineList[UnityEngine.Random.Range(0, medicineList.Count)], position, Quaternion.identity);
                    medicineSum++;
                    temp.GetComponent<Collect>().OncollectDestroy += OnDestroyCollect;
                    medicineListPosition.Add(temp.transform);
                }

                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.1f);
        }
       
    }
    IEnumerator CreateStone()
    {
        while(true)
        {
            // 随机生成石头,数量要小于或等于设定的数量
            while (stoneSum <= stone.number)
            {
                Vector3 position = GetPonint(stone.crashRange);
                if (position != Vector3.zero)
                {

                    //Instantiate(stoneList[UnityEngine.Random.Range(0, stoneList.Count)], position, Quaternion.identity);
                    GameObject temp = Instantiate(stoneList[0], position, Quaternion.identity);
                    stoneSum++;
                    temp.GetComponent<Collect>().OncollectDestroy += OnDestroyCollect;
                    stoneListPosition.Add(temp.transform);
                }

                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.1f);
        }
       
    }

    private void OnDestroyCollect(object obj, CollectEventArgs e)
    {
        Collect collect = obj as Collect;
        switch (e.collectType)
        {
            case CollectType.石头:
                stoneSum -= 1;
                stoneListPosition.Remove(collect.transform);
                break;
            case CollectType.木头:
                woodSum -= 1;
                woodListPosition.Remove(collect.transform);
                break;
            case CollectType.食物:
                foodSum -= 1;
                foodListPosition.Remove(collect.transform);
                break;
            case CollectType.回复药:
                medicineSum -= 1;
                medicineListPosition.Remove(collect.transform);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 发射射线
    /// </summary>
    /// <returns>返回得到的点，如果该点不适合生成资源则返回0</returns>
    private Vector3 GetPonint(float crashRange)
    {
        Ray ray = new Ray(new Vector3(UnityEngine.Random.Range(-crash.x / 2, crash.x / 2) + transform.position.x,
                                      transform.position.y,
                                      UnityEngine.Random.Range(-crash.z / 2, crash.z / 2) + transform.position.z),
                                      -transform.up);
        // 向地面发射射线
        if (Physics.Raycast(ray, out RaycastHit hit, crash.y, layer))
        {
            #region 检查该点周围是否合适放置环境资源
            Ray ray1 = new Ray(hit.point, -Vector3.up);
            RaycastHit[] hits = Physics.SphereCastAll(ray1, crashRange, crashRange * 2, isPut);
            foreach(var x in hits)
            {
                if (x.collider.tag == "Map") continue;
                return Vector3.zero;
            }
            #endregion

            return hit.point;
        }
        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 150, 0, 0.5f);
        Gizmos.DrawCube(transform.position, crash);
    }
}

[Serializable]
public struct EnvironmentNumber
{
    /// <summary>
    /// 资源生成数量
    /// </summary>
    public int number;
    /// <summary>
    /// 资源的碰撞范围
    /// </summary>
    public float crashRange;
}
