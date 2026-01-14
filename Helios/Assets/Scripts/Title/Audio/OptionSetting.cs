using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public enum Sliders
{
    VOLUME = 0,
    MOUSESPEED,
    MAX,
}

public class OptionSetting : MonoBehaviour
{
    [SerializeField] RectTransform optionWindowRest;
    [SerializeField] Slider[] volumeSliders;
    [SerializeField] Text[] volumeTexts;
    [SerializeField] float volumeUpSpeed;

    const float volumeDiff = 80.0f;

    [SerializeField] float moveTime;

    [SerializeField] CursorMover cursorMover;

    [SerializeField] Image[] volumeIcons;
    [SerializeField] Sprite[] volumeSprites;

    [SerializeField] ChangeMouseSpeed changeMouseSpeed;

    [SerializeField] OptionAnimetion optionAnimetion;

    Action<float>[] sliderSetting;
    const int border = 3;
    float delayTimer;
    float delay;
    const float delayTime = 0.1f;
    private void Awake()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }

    private void OnEnable()
    {
        for (int i = 0; i < (int)Audio.MAX; i++)
        {
            float decibel = Mathf.Clamp(20f * Mathf.Log10(volumeSliders[i].minValue), -volumeDiff, 0f);
            volumeSliders[i].value = Locator<AudioManager>.Instance.GetVolume((Audio)i);
            if (Locator<AudioManager>.Instance.stockVolumes[i] != decibel)
            {
                volumeSliders[i].interactable = false;
                volumeIcons[cursorMover.nowButton].sprite = volumeSprites[1];
            }
            ChangeVolumeText((Audio)i);
        }
        sliderSetting = new Action<float>[(int)Sliders.MAX];
        sliderSetting[(int)Sliders.VOLUME] += VolumeSetting;
        sliderSetting[(int)Sliders.MOUSESPEED] += changeMouseSpeed.AddMouseSpeed;

        delay = delayTime;
        delayTimer = delay;
    }

    private void Update()
    {
        if (optionAnimetion.isAnim) return;
        if (cursorMover.nowButton < border && cursorMover.cursorImg != volumeIcons[cursorMover.nowButton].sprite) cursorMover.SetImage(volumeIcons[cursorMover.nowButton].sprite);
        if (cursorMover.move) return;
        SliderSetting();
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (cursorMover.nowButton >= border) return;
            VolumeMuteSetting();
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(Close());
        }
    }

    void SliderSetting()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (Input.GetJoystickNames().Length > 0 && horizontal == 0) horizontal = Input.GetAxisRaw("Debug Horizontal");

        if (horizontal != 0)
        {
            delayTimer += Time.deltaTime;
            if (delayTimer < delay) return;
            delayTimer = 0f;
            int num = (cursorMover.nowButton < border) ? (int)Sliders.VOLUME : (int)Sliders.MOUSESPEED;
            float d = (num == (int)Sliders.VOLUME) ? 0f : 0.05f;
            delay -= delay * delay;
            if (delay < d) delay = d;
            sliderSetting[num](horizontal);
        }
        else
        {
            delay = delayTime;
            delayTimer = delay;
        }
    }

    void VolumeSetting(float _horizontal)
    {
        if (volumeSliders[cursorMover.nowButton].interactable && _horizontal != 0)
        {
            volumeSliders[cursorMover.nowButton].value += _horizontal * Time.deltaTime * volumeUpSpeed;
        }
    }

    void VolumeMuteSetting()
    {
        AudioManager instance = Locator<AudioManager>.Instance;
        volumeSliders[cursorMover.nowButton].interactable = !volumeSliders[cursorMover.nowButton].interactable;
        //音量設定
        instance.SetVolume((Audio)cursorMover.nowButton, instance.GetStockVolume(cursorMover.nowButton));
        //音量ストック
        instance.SetStockVolume(cursorMover.nowButton, volumeSliders[cursorMover.nowButton].value);
        //スライダー変更
        volumeSliders[cursorMover.nowButton].value = instance.GetVolume((Audio)cursorMover.nowButton);
        //テキスト変更
        ChangeVolumeText((Audio)cursorMover.nowButton);

        volumeIcons[cursorMover.nowButton].sprite = volumeSliders[cursorMover.nowButton].interactable ? volumeSprites[0] : volumeSprites[1];
    }

    public void ChangeVolumeText(Audio _audio)
    {
        volumeTexts[(int)_audio].text = (volumeSliders[(int)_audio].value * 100).ToString("f0") + "%";
    }

    public void ChangeVolumeValue(int _audio)
    {
        Locator<AudioManager>.Instance.SetVolume((Audio)_audio, volumeSliders[_audio].value);
        ChangeVolumeText((Audio)_audio);
    }

    public void CloseButton()
    {
        StartCoroutine(Close());
    }

    IEnumerator Close()
    {
        yield return optionWindowRest.DOScale(Vector3.zero, 0.3f).WaitForCompletion();
        Destroy(this.gameObject);
    }
}
