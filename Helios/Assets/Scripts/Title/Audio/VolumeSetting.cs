using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class VolumeSetting : MonoBehaviour
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

    [SerializeField] OptionAnimetion optionAnimetion;

    private void OnEnable()
    {
        for (int i = 0; i < (int)Audio.MAX; i++)
        {
            float decibel = Mathf.Clamp(20f * Mathf.Log10(volumeSliders[i].minValue), -80f, 0f);
            volumeSliders[i].value = Locator<AudioManager>.Instance.GetVolumeF((Audio)i);
            if (Locator<AudioManager>.Instance.stockVolumes[i] != decibel)
            {
                volumeSliders[i].interactable = false;
                volumeIcons[cursorMover.nowButton].sprite = volumeSprites[1];
            }
            ChangeVolumeText((Audio)i);
        }
    }

    private void Update()
    {
        if (optionAnimetion.isAnim) return;
        if (cursorMover.cursorImg != volumeIcons[cursorMover.nowButton].sprite) cursorMover.SetImage(volumeIcons[cursorMover.nowButton].sprite);
        if (cursorMover.move) return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        if (Input.GetJoystickNames().Length > 0 && horizontal == 0) horizontal = Input.GetAxisRaw("Debug Horizontal");

        if (volumeSliders[cursorMover.nowButton].interactable && horizontal != 0)
        {
            volumeSliders[cursorMover.nowButton].value += horizontal * Time.deltaTime * volumeUpSpeed;
            if (volumeSliders[cursorMover.nowButton].value > volumeSliders[cursorMover.nowButton].maxValue)
            {
                volumeSliders[cursorMover.nowButton].value = volumeSliders[cursorMover.nowButton].maxValue;
            }
            else if (volumeSliders[cursorMover.nowButton].value < volumeSliders[cursorMover.nowButton].minValue)
            {
                volumeSliders[cursorMover.nowButton].value = volumeSliders[cursorMover.nowButton].minValue;
            }
        }

        if(Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager instance = Locator<AudioManager>.Instance;
            volumeSliders[cursorMover.nowButton].interactable = !volumeSliders[cursorMover.nowButton].interactable;
            //音量設定
            instance.SetVolume((Audio)cursorMover.nowButton, instance.GetStockVolumeF(cursorMover.nowButton));
            //音量ストック
            instance.SetStockVolume(cursorMover.nowButton, volumeSliders[cursorMover.nowButton].value);
            //スライダー変更
            volumeSliders[cursorMover.nowButton].value = instance.GetVolumeF((Audio)cursorMover.nowButton);
            //テキスト変更
            ChangeVolumeText((Audio)cursorMover.nowButton);

            volumeIcons[cursorMover.nowButton].sprite = volumeSliders[cursorMover.nowButton].interactable ? volumeSprites[0] : volumeSprites[1];
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(Close());
        }
    }

    IEnumerator Close()
    {
        yield return optionWindowRest.DOScale(Vector3.zero, 0.3f).WaitForCompletion();
        if (SceneManager.GetActiveScene().name == "TitleScene")
        {
            //Locator<TitleManager>.Instance.TitleBack();
        }
        Destroy(this.gameObject);
    }

    public void ChangeVolumeText(Audio _audio)
    {
        volumeTexts[(int)_audio].text = (volumeSliders[(int)_audio].value * 100).ToString("f0") + "%";
        //volumeTexts[(int)_audio].text = (volumeDiff + volumeSliders[(int)_audio].value).ToString("f0") + "%";
    }

    public void ChangeVolumeValue(int _audio)
    {
        Locator<AudioManager>.Instance.SetVolume((Audio)_audio, volumeSliders[_audio].value);
        ChangeVolumeText((Audio)_audio);
    }
}
