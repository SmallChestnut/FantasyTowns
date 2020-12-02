using System;
using UnityEngine;

public class Collect : MonoBehaviour, ICollect
{
    public CollectType collectType;  // 资源类型
    public float maxTime;            // 采集资源需要的时间
    public int maxNumber;            // 资源总数
    public GameObject corpse;        // 采集后剩下的尸体
    [Tooltip("可采集资源的名字")]
    public string collectName;       // 资源名字

    /// <summary>
    /// 资源被采集完触发该事件
    /// </summary>
    public event Action<object, CollectEventArgs> OncollectDestroy;


    private float remainingTime;
    private float clock = 0; // 记录开采的时间
    private float collectNumber; // 10%的时间需要返回的资源数

    public string Name { get => collectName; set => collectName = value; }

    public InteractionType InteractionType => InteractionType.collect;

    private void Start()
    {
        remainingTime = maxTime;
    }
    private void Finish()
    {
        Instantiate(corpse, transform.position, transform.rotation);
        OncollectDestroy?.Invoke(gameObject, new CollectEventArgs(collectType));
        Destroy(gameObject);
        Destroy(transform.parent?.gameObject);
    }

    public CollectData GetCollectData(float time)
    {
        int tempCollectNumber; // 要返回的资源数量
        bool isDone = false;
        remainingTime -= time;
        clock += time;
        if (remainingTime <= 0)
        {
            isDone = true;
            Finish();
            collectNumber += maxNumber / 10f;
        }
        if (clock >= maxTime * 0.1)
        {
            collectNumber += maxNumber / 10f; // 计算10%的资源
            clock = 0;

        }
        tempCollectNumber = (int)collectNumber;
        collectNumber -= tempCollectNumber;
        CollectData collectData = new CollectData(collectType, maxTime, remainingTime, tempCollectNumber, isDone);
        return collectData;
    }
}
public class CollectEventArgs : EventArgs
{
    public CollectType collectType;
    public CollectEventArgs(CollectType collectType)
    {
        this.collectType = collectType;
    }


}

