using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class PlayerDie : MonoBehaviour
{
    public PlayableDirector playable;
    void Start()
    {
        playable.Play();
        AudioManage.single.audioState = AudioState.死亡;
        Destroy(gameObject, 6);
    }


    public void OnDestroy()
    {
        SceneManager.LoadScene("StartGame");
        EnvironmentCreate.environmentCreates.Clear();
    }
}
