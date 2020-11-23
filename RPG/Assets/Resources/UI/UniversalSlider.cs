using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UniversalSlider : MonoBehaviour
{
    private Transform background;
    private Image fillImage;
    private Text message;
    void Start()
    {
        background = transform.Find("背景");
        fillImage = background.GetChild(0).GetComponent<Image>();
        message = transform.Find("文本").GetComponent<Text>();
    }
    /// <summary>
    /// 更新进度条的属性
    /// </summary>
    /// <param name="fillValue"></param>
    /// <param name="text"></param>
    public void UpdateSlider(float fillValue, string text) 
    {
        fillImage.fillAmount = fillValue;
        message.text = text;
    }
}
