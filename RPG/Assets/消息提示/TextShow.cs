using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextShow : MonoBehaviour
{
    public float speed;
    public float destorTime;

    public Text message;
    void Start()
    {
        StartCoroutine(DestorTime(destorTime));
    }

    public void SetMessageText(string str)
    {
        message.text = str;
    }
    void Update()
    {
        transform.localPosition += Vector3.up * speed * Time.deltaTime;
    }
    IEnumerator DestorTime(float time)
    {
        float temp = time;
        while(true)
        {
            time -= 0.01f;
            message.color = new Color(message.color.r, message.color.g, message.color.b, time / temp);
            if(time <= 0)
            {
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(gameObject);
    }
}
