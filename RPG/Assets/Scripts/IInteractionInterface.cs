using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractionInterface 
{
    InteractionType InteractionType { get; }
}

public enum InteractionType
{
    /// <summary>
    /// 可采集
    /// </summary>
    collect, 
    /// <summary>
    /// 可建造
    /// </summary>
    bulid
}
