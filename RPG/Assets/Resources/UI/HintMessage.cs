using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintMessage
{
    public static HintMessage Single { get; set; } = new HintMessage();

    private Text textMessage;
    private GameObject messageObj;
    public HintMessage()
    {
        messageObj = Object.Instantiate(ResourcePath.Single.universalHintMessage,
                     GameObject.Find("Canvas").transform);
        textMessage = messageObj.transform.GetChild(0).GetComponent<Text>();
    }


    public void ShowMessageText(string messageText)
    {
        textMessage.text = messageText;
        messageObj.SetActive(true);
    }
    public void CloseMessageText()
    {
        try
        {
            messageObj.SetActive(false);
        }
        catch (MissingReferenceException)
        {

            Single = new HintMessage();
        }
        
    }
}

