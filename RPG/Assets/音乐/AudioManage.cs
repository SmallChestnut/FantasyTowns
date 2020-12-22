using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManage : MonoBehaviour
{
    public List<AudioClip> audioClips;

    public List<AudioClip> TownAudioClips;
    public static AudioManage single;
    [HideInInspector]
    public bool isReplace;

    public AudioState audioState;
    private AudioSource audioSource;
    private int TownAudioIndex;         // 记录小镇音乐播放的索引

    void Start()
    {
        single = this;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        switch (audioState)
        {
            case AudioState.标题:
                PlayTitle();
                break;
            case AudioState.安全地带:
                PlayerTown();
                break;
            case AudioState.危险地带:
                PlayDanger();
                break;
            case AudioState.死亡:
                PlayDie();
                break;
            default:
                break;
        }
    }

    private void PlayTitle()
    {
        if(audioSource.clip != audioClips[0])
        {
            audioSource.clip = audioClips[0];
            audioSource.Stop();
        }
        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        
    }
    private void PlayerTown()
    {
        if (audioSource.clip != TownAudioClips[TownAudioIndex] && !isReplace)
        {
            StartCoroutine(ReplaceAudioClip(TownAudioClips[TownAudioIndex]));
        }
        else if (!audioSource.isPlaying && !isReplace)
        {
            TownAudioIndex++;
            if(TownAudioIndex >= TownAudioClips.Count)
            {
                TownAudioIndex = 0;
            }
        }
    }
    private void PlayDie()
    {
        if (audioSource.clip != audioClips[1])
        {
            audioSource.clip = audioClips[1];
            audioSource.Stop();
        }
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
    private void PlayDanger()
    {
        if (audioSource.clip != audioClips[2] && !isReplace)
        {
            StartCoroutine(ReplaceAudioClip(audioClips[2]));
        }
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
    // 更换音乐
    IEnumerator ReplaceAudioClip(AudioClip audioClip)
    {
        // 开始更换
        isReplace = true;
        while(audioSource.volume > 0)
        {
            audioSource.volume -= 0.01f;
            yield return new WaitForSeconds(0.07f);
        }
        audioSource.Stop();
        audioSource.clip = audioClip;
        audioSource.Play();
        while (audioSource.volume < 1)
        {
            audioSource.volume += 0.01f;
            yield return new WaitForSeconds(0.07f);
        }
        // 结束更换
        isReplace = false;
    }
}

public enum AudioState
{
    标题,
    安全地带,
    危险地带,
    死亡,
}
