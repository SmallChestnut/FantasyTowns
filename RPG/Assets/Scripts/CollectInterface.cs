using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollect : IInteractionInterface
{
    string Name { get; set; }
    CollectData GetCollectData(float time);
}

public class CollectData
{
    public CollectType collectType; // 资源类型
    public float maxTime; // 采集需要的时间
    public float remainingTime; // 剩余时间
    public int number; // 资源数量
    public bool isDone; // 是否采集完成
    public CollectData(CollectType collectType, float maxTime, float remainingTime, int number, bool isDone)
    {
        this.collectType = collectType;
        this.maxTime = maxTime;
        this.remainingTime = remainingTime;
        this.number = number;
        this.isDone = isDone;
    }
}

public enum CollectType
{
    石头,
    木头,
    食物,
    药材,
}
