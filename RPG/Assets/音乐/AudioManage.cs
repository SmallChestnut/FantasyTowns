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
    private AudioClip music = null;

    void Start()
    {
        single = this;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(ReplaceAudioClip());
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
        if(music != audioClips[0])
        {
            music = audioClips[0];
            audioSource.clip = music;
            audioSource.Stop();
        }
        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        
    }
    private void PlayerTown()
    {
        if (music != TownAudioClips[TownAudioIndex])
        {
            music = TownAudioClips[TownAudioIndex];
        }
        else if (!audioSource.isPlaying && !isReplace && audioSource.clip != null)
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
        if (music != audioClips[1])
        {
            music = audioClips[1];
            audioSource.clip = music;
            audioSource.Stop();
        }
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
    private void PlayDanger()
    {
        if (music != audioClips[2])
        {
            music = audioClips[2];
        }
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
    // 更换音乐
    IEnumerator ReplaceAudioClip()
    {
        AudioClip temp;
        while(true)
        {
            // 如果当前音乐不等于需要的音乐就替换
            if(audioSource.clip != music)
            {
                // 开始更换
                isReplace = true;
                temp = music;
                while (audioSource.volume > 0)
                {
                    audioSource.volume -= 0.01f;
                    // 如果在替换的过程中被更换了音乐就退出重新替换
                    if(temp != music)
                    {
                        break;
                    }
                    yield return new WaitForSeconds(0.07f);
                }
                yield return new WaitForSeconds(3f);
                audioSource.Stop();
                audioSource.clip = music;
                audioSource.Play();
                while (audioSource.volume < 1)
                {
                    audioSource.volume += 0.01f;
                    // 如果在替换的过程中被更换了音乐就退出重新替换
                    if (temp != music)
                    {
                        while (audioSource.volume > 0)
                        {
                            audioSource.volume -= 0.01f;
                            yield return new WaitForSeconds(0.07f);
                        }
                        break;
                    }
                    yield return new WaitForSeconds(0.07f);
                }
                // 结束更换
                isReplace = false;
            }
            // 音量不够就添加
            else if(audioSource.volume < 1)
            {
                while (audioSource.volume < 1)
                {
                    audioSource.volume += 0.01f;
                    yield return new WaitForSeconds(0.07f);
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
        
    }
}

public enum AudioState
{
    标题,
    安全地带,
    危险地带,
    死亡,
}
