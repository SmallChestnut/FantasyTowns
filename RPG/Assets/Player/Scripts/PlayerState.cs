using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    [Header("状态栏UI")]
    public Image lifeImage;
    public Image satietyImage;
    public Text lifeText;
    public Text satietyText;
    [Header("多少时间减少1点饱食度")]
    public float maxTime;
    [HideInInspector]
    public PlayerData playerData;

    private float timer;
    private CinemachineCollisionImpulseSource cinemachineCollisionImpulseSource;

    private void Start()
    {
        playerData = GameSave.Single.gameData.playerData;
        // 生成NPC
        for (int i = 0; i < playerData._NPC; i++)
        {
            NPC temp = Instantiate(ResourcePath.Single.NPCList[
                                Random.Range(0, ResourcePath.Single.NPCList.Count)]).GetComponent<NPC>();
            temp.gameObject.SetActive(false);
            PlayerInputManage.single.GetComponent<PlayerInteraction>().NPCQueue.Enqueue(temp);
        }
        cinemachineCollisionImpulseSource = GetComponent<CinemachineCollisionImpulseSource>();
        UpdateStateUI();
        timer = maxTime;
    }
    public void Update()
    {
        timer -= Time.deltaTime;
        // 一定时间内减少一点饱食度，饱食度为零时减少1点生命值
        if(timer <= 0)
        {
            if (playerData.satiety > 0)
                playerData.satiety -= 1;
            else
                playerData.life -= 1;
            UpdateStateUI();
            timer = maxTime;
        }
    }


    /// <summary>
    /// 添加生命值
    /// </summary>
    /// <param name="value">要添加的值</param>
    public void AddLife(int value)
    {
        playerData.life += value;
        if (playerData.life > playerData.maxLife) playerData.life = playerData.maxLife;
        UpdateStateUI();
    }
    /// <summary>
    /// 减少生命值
    /// </summary>
    /// <param name="value">要减少的值</param>
    public void ReduceLife(int value)
    {
        playerData.life -= value;
        if (playerData.life < 0) playerData.life = 0;
        UpdateStateUI();
        cinemachineCollisionImpulseSource.GenerateImpulse();

    }
    /// <summary>
    /// 添加饱食度
    /// </summary>
    /// <param name="value">要添加的值</param>
    public void AddSatiety(int value)
    {
        playerData.satiety += value;
        if (playerData.satiety > playerData.maxSatiety) playerData.satiety = playerData.maxSatiety;
        UpdateStateUI();
    }
    /// <summary>
    /// 减少饱食度
    /// </summary>
    /// <param name="value">要减少的值</param>
    public void ReduceSatiety(int value)
    {
        playerData.satiety -= value;
        if (playerData.satiety < 0) playerData.satiety = 0;
        UpdateStateUI();
    }






    /// <summary>
    /// 更新状态栏的UI
    /// </summary>
    public void UpdateStateUI()
    {
        lifeImage.fillAmount = (float)playerData.life / playerData.maxLife;
        satietyImage.fillAmount = (float)playerData.satiety / playerData.maxSatiety;
        lifeText.text = $"{playerData.life}/{playerData.maxLife}";
        satietyText.text = $"{playerData.satiety}/{playerData.maxSatiety}";
    }


}
