using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManage : MonoBehaviour
{
    private PlayerState playerState;
    public GameObject playerDie;
    private void Start()
    {
        playerState = PlayerInputManage.single.GetComponent<PlayerState>();
    }
    void Update()
    {
        if(playerState.playerData.life <= 0 || playerState.playerData.satiety <= 0)
        {
            playerDie.SetActive(true);
            playerDie.transform.position = playerState.transform.position;
            playerDie.transform.rotation = playerState.transform.rotation;
            playerState.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
