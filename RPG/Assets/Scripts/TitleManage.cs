using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class TitleManage : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    public void StartGame()
    {
        SceneManager.LoadSceneAsync("MyGame");
        GameSave.Single.isLoad = false;
        gameObject.SetActive(false);
        EnvironmentCreate.environmentCreates.Clear();
        GameSave.Single.gameData = new GameData();
    }
    public void ContinueGame()
    {
        string path = Application.dataPath + "\\GameData.xml";
        if (!File.Exists(path))
            return;
        SceneManager.LoadScene("MyGame");
        GameSave.Single.isLoad = true;
        gameObject.SetActive(false);
        EnvironmentCreate.environmentCreates.Clear();
        GameSave.Single.gameData = new GameData();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
