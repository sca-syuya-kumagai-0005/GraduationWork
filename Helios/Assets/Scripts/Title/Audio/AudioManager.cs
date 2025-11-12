using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.Rendering.DebugUI;

[Serializable]
public enum Audio
{
    MASTER,
    BGM,
    SE,
    MAX,
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] AudioSource BGM;
    [SerializeField] AudioSource SE;
    [SerializeField] AudioMixer masterMixer;
    [SerializeField] AudioMixerGroup BGMMixerGroup;
    [SerializeField] AudioMixerGroup SEMixerGroup;

    //const string MasterVolume = "VolumeParam_Master";
    //const string BGMVolume = "VolumeParam_BGM";
    //const string SEVolume = "VolumeParam_SE";

    private static readonly string[] audioTypes = { "VolumeParam_Master", "VolumeParam_BGM", "VolumeParam_SE" };

    float BGMvolume = 0.5f;
    public float[] stockVolumes { get; private set; }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Locator<AudioManager>.Bind(this);
            BGM.outputAudioMixerGroup = BGMMixerGroup;
            SE.outputAudioMixerGroup = SEMixerGroup;
            stockVolumes = new float[(int)Audio.MAX];
            for (int i = 0; i < stockVolumes.Length; i++)
            {
                stockVolumes[i] = -80f;
            }
        }
        else
        {
            Locator<AudioManager>.Unbind(this);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// BGMを設定して再生する
    /// </summary>
    /// <param name="_audioClip">流したいBGM</param>
    public void PlayBGM(AudioClip _audioClip)
    {
        BGM.clip = _audioClip;
        BGM.Play();
    }

    /// <summary>
    /// SEを設定して1回再生する
    /// </summary>
    /// <param name="_audioClip">流したいSE</param>
    public void PlaySE(AudioClip _audioClip)
    {
        SE.PlayOneShot(_audioClip);
    }

    /// <summary>
    /// 止まっているBGMを再生する
    /// </summary>
    public void StartBGM()
    {
        BGM.Play();
    }

    /// <summary>
    /// BGMを止める関数
    /// </summary>
    public void StopBGM()
    {
        BGM.Stop();
    }

    /// <summary>
    /// BGMをフェードインさせる
    /// </summary>
    /// <param name="_time">フェードインにかかる秒数</param>
    public void FadeInBGM(float _time)
    {
        BGM.DOFade(BGMvolume, _time);
        BGMvolume = 0.0f;
    }

    /// <summary>
    /// BGMをフェードアウトさせる
    /// </summary>
    /// <param name="_time">フェードアウトにかかる秒数</param>
    public void FadeOutBGM(float _time)
    {
        BGMvolume = BGM.volume;
        BGM.DOFade(0.0f, _time);
    }

    public float GetVolumeF(Audio _audio)
    {
        masterMixer.GetFloat(audioTypes[(int)_audio], out float value);
        value = Mathf.Clamp01(Mathf.Pow(10, value / 20f));
        Debug.Log(value);
        return value;
    }

    /// <summary>
    /// 音量を変更する関数
    /// </summary>
    /// <param name="_audio">音量を変更したいオーディオタイプ</param>
    /// <param name="_value">設定したい値(-80～0)</param>
    public void SetVolume(Audio _audio, float _value)
    {
        if (_audio == Audio.MAX) return;
        float decibel = Mathf.Clamp(20f * Mathf.Log10(_value), -80f, 0f);
        masterMixer.SetFloat(audioTypes[(int)_audio], decibel);
    }

    /// <summary>
    /// 音量情報を保持させる関数
    /// </summary>
    /// <param name="_num">配列番号</param>
    /// <param name="_value">音量</param>
    public void SetStockVolume(int _num, float _value)
    {
        float decibel = Mathf.Clamp(20f * Mathf.Log10(_value), -80f, 0f);
        stockVolumes[_num] = decibel;
    }

    /// <summary>
    /// スライダーに音量を代入する時に使用する関数
    /// </summary>
    /// <param name="_num">配列番号</param>
    /// <returns>音量情報を0～1の範囲で返す</returns>
    public float GetStockVolumeF(int _num)
    {
        float value = Mathf.Clamp01(Mathf.Pow(10, stockVolumes[_num] / 20f));
        return value;
    }
}
