using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManage : MonoBehaviour
{
    public GameObject GameMenu;
    public Button SaveButton;
    public Button QuitGame;
    public Text messageText;
    public Canvas canvas;
    private PlayerBulid playerBulid;
    void Start()
    {
        SaveButton.onClick.AddListener(SaveGame);
        QuitGame.onClick.AddListener(QuitGameFunction);
        playerBulid = PlayerInputManage.single.GetComponent<PlayerBulid>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameMenu.SetActive(!GameMenu.activeSelf);
            if (GameMenu.activeSelf)
            {
                Time.timeScale = 0;
                if (playerBulid.bulidTemplet != null)
                {
                    playerBulid.bulidTemplet.SetActive(false);
                }
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Time.timeScale = 1;
                if (playerBulid.bulidTemplet != null)
                {
                    playerBulid.bulidTemplet.SetActive(true);
                }
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
    public void SaveGame()
    {
        GameSave.Single.gameData.playerData._NPC = PlayerInputManage.single.GetComponent<PlayerInteraction>().NPCQueue.Count;
        GameSave.Single.gameData.playerData.itemDatas.Clear();
        foreach (var x in PlayerInputManage.single.GetComponent<PlayerInteraction>().box.box)
        {
            GameSave.Single.gameData.playerData.itemDatas.Add(x.itemData);
        }
        GameSave.Single.gameData.playerData.position = PlayerInputManage.single.transform.position;
        GameSave.Single.gameData.playerData.rotation = PlayerInputManage.single.transform.rotation;
        GameSave.Single.SaveGameData();
        Instantiate(messageText, canvas.transform).GetComponent<TextShow>().SetMessageText("保存成功");
    }
    // 迷之bug不要这功能了
    public void QuitToMainMenuFunction()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartGame");
    }
    public void QuitGameFunction()
    {
        Application.Quit();
    }
}
