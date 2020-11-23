using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePath
{
    public static ResourcePath Single { get; } = new ResourcePath();

    public GameObject universalSlider = Resources.Load("UI\\进度条") as GameObject;
    public GameObject universalHintMessage = Resources.Load("UI\\交互提示") as GameObject;
}
