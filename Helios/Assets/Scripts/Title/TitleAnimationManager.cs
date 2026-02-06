using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.Rendering;

[Serializable]
public struct Cautionary
{
    public Sprite sprite;
    public string header;
    [TextArea] public string text;
    public Color headerColor;
}

public enum CautionaryNum { 
    WARNING = 0,
    SOUND,
    MAX,
}


public class TitleAnimationManager : MonoBehaviour
{
    [SerializeField] Image fadePanel;
    [SerializeField] Image backGround;
    [SerializeField, Header("起動時に表示する注意喚起演出")] Cautionary[] cautionaries;
    [SerializeField] Image cautionaryImage;
    [SerializeField] Text cautionaryHeader;
    [SerializeField] Text cautionaryText;
    [SerializeField] float waitTime;
    [SerializeField, Header("タイトル開始時演出")] GameObject titleStartObj;
    [SerializeField] GameObject titleLogo;
    Image logoImage;
    RectTransform logoRect;
    [SerializeField] GameObject volume;
    [SerializeField] GameObject startBackGround;
    [SerializeField] Text pushText;
    [SerializeField] GameObject rainParticle;
    [SerializeField, Header("選択画面開始時演出")] GameObject titleSelectObj;
    [SerializeField] GameObject selectBackGround;
    [SerializeField] RectTransform slingerRect;
    [SerializeField] RectTransform arrowsRect;
    [SerializeField] ChangePixelColor roadPixelColor;
    [SerializeField] ChangePixelColor fadePixelColor;
    [SerializeField, Header("自室演出")] GameObject myRoomObj;
    [SerializeField] GameObject myRoomBackGround;
    [SerializeField, Header("メモリースリンガー演出")] GameObject memorySlingerObj;
    [SerializeField,Header("動画")] GameObject videoObj;
    [SerializeField] TitleVideoPlayer titleVideoPlayer;

    bool skip = false;
    float time = 0;
    const float speed = 1.5f;
    const float sinLim = 0.8f;
    GameObject nowTitleMode;
    GameObject nowBackGround;

    private void Awake()
    {
        Color clear = new Color(1, 1, 1, 0);
        cautionaryImage.color = clear;
        cautionaryHeader.color = clear;
        cautionaryText.color = clear;
        logoImage = titleLogo.GetComponent<Image>();
        logoRect = titleLogo.GetComponent<RectTransform>();
        logoImage.color = clear;
        pushText.color = clear;
        titleStartObj.SetActive(true);
        nowTitleMode = null;
        nowBackGround = null;
        volume.SetActive(false);
    }

    public IEnumerator FadeAnimation(float _alpha,float _time)
    {
        //フェードのアニメーション次第で変更
        yield return fadePanel.DOFade(_alpha, _time).WaitForCompletion();
        fadePanel.raycastTarget = (_alpha == 1f);
    }

    public IEnumerator CautionaryAnim(CautionaryNum cautionaryNum)
    {
        int num = (int)cautionaryNum;
        cautionaryImage.sprite = cautionaries[num].sprite;
        cautionaryHeader.text = cautionaries[num].header;
        cautionaryHeader.color = cautionaries[num].headerColor;
        cautionaryText.text = cautionaries[num].text;
        var sequence = DOTween.Sequence();
        sequence.Append(cautionaryImage.DOFade(1f, 0.5f))
                .AppendInterval(0.25f)
                .Append(cautionaryHeader.DOFade(0.3f, 0.5f))
                .Join(cautionaryText.DOFade(1f, 0.5f));
        yield return StartCoroutine(WaitAnim(1f));
        sequence.Kill();
        cautionaryImage.DOFade(1f, 0f);
        cautionaryHeader.DOFade(0.3f, 0f);
        cautionaryText.DOFade(1f, 0f);
        yield return StartCoroutine(WaitAnim(waitTime));
        float fadeOutTime = skip ? 0.3f : 0.7f;
        const float delayTime = 0.3f;
        cautionaryImage.DOFade(0f, fadeOutTime);
        cautionaryHeader.DOFade(0f, fadeOutTime);
        cautionaryText.DOFade(0f, fadeOutTime);
        yield return StartCoroutine(WaitAnim(fadeOutTime + delayTime));
        if(cautionaryNum == CautionaryNum.SOUND)
        {
            SetBackGround(startBackGround);
            yield return StartCoroutine(FadeAnimation(0f, fadeOutTime));
        }
        skip = false;
    }

    /// <summary>
    /// アニメーション待機処理(マウス入力時スキップ)
    /// </summary>
    /// <param name="_waitTime"></param>
    /// <returns></returns>
    IEnumerator WaitAnim(float _waitTime)
    {
        while (_waitTime > 0f)
        {
            yield return null;
            _waitTime -= Time.deltaTime;
            for (int i = 0; i < 3; i++)
            {
                if (Input.GetMouseButtonDown(i))
                {
                    skip = true;
                    yield break;
                }
            }
        }
    }

    public IEnumerator TitleStartAnim(AudioClip _selectSE)
    {
        logoImage.DOFade(1f, 0.5f);
        yield return new WaitForSeconds(0.75f);
        pushText.DOFade(1f - sinLim, 0.3f);
        yield return new WaitForSeconds(0.3f);
        bool isPush = false;
        while (!isPush)
        {
            yield return null;
            pushText.color = GetAlphaColor(pushText.color);
            for (int i = 0; i < 3; i++)
            {
                if (Input.GetMouseButtonDown(i))
                {
                    isPush = true;
                    Locator<AudioManager>.Instance.PlaySE(_selectSE);
                    break;
                }
            }
        }
        logoImage.DOFade(0f, 0.75f);
        pushText.DOFade(0f, 0.75f);
        StartCoroutine(FadeAnimation(1f, 0.8f));
        yield return new WaitForSeconds(1f);
        titleStartObj.SetActive(false);
        rainParticle.SetActive(false);
    }

    public IEnumerator TitleSelectDisplayAnim()
    {
        yield return StartCoroutine(FadeAnimation(1f, 0.5f));
        volume.SetActive(true);
        SetBackGround(selectBackGround);
        SetNowTitleMode(titleSelectObj);
        slingerRect.anchoredPosition = new Vector2(-500f,-500f);
        arrowsRect.anchoredPosition = new Vector2(-500f, -500f);
        yield return new WaitForSeconds(0.5f);
        const float fadeTime = 3.0f;
        const float delay = 0.5f;
        roadPixelColor.OnAnim();
        fadePixelColor.OnAnim();
        yield return StartCoroutine(FadeAnimation(0f, fadeTime + delay));
        var sequence = DOTween.Sequence();
        sequence.PrependInterval(delay)
                .Append(slingerRect.DOAnchorPos(Vector2.zero, 1.25f).SetEase(Ease.OutBack))
                //.AppendInterval(delay)
                .Append(arrowsRect.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.OutBack));
        Locator<TitleManager>.Instance.SelectStart();
    }

    public IEnumerator BackTitleSelect()
    {
        yield return StartCoroutine(FadeAnimation(1f, 0.5f));
        volume.SetActive(true);
        SetBackGround(selectBackGround);
        SetNowTitleMode(titleSelectObj);
        yield return StartCoroutine(FadeAnimation(0f, 0.5f));
        Locator<TitleManager>.Instance.SelectStart();
    }

    public IEnumerator MyRoomAnim()
    {
        yield return StartCoroutine(FadeAnimation(1f, 0.5f));
        volume.SetActive(false);
        SetBackGround(myRoomBackGround);
        SetNowTitleMode(myRoomObj);
        yield return StartCoroutine(FadeAnimation(0f, 0.5f));
    }

    public IEnumerator MemorySlingerAnim()
    {
        yield return StartCoroutine(FadeAnimation(1f, 0.5f));
        SetNowTitleMode(memorySlingerObj);
        yield return StartCoroutine(FadeAnimation(0f, 0.5f));
    }

    public IEnumerator VideoAnim()
    {
        yield return StartCoroutine(FadeAnimation(1f, 0.5f));
        volume.SetActive(false);
        SetNowTitleMode(videoObj);
        titleVideoPlayer.PlayVideo();
        yield return StartCoroutine(FadeAnimation(0f, 0.5f));
    }

    void SetNowTitleMode(GameObject _obj)
    {
        if (nowTitleMode == _obj) return;
        _obj.SetActive(true);
        if (nowTitleMode != null) nowTitleMode.SetActive(false);
        nowTitleMode = _obj;
    }

    void SetBackGround(GameObject _obj)
    {
        if (nowBackGround == _obj) return;
        _obj.SetActive(true);
        if (nowBackGround != null) nowBackGround.SetActive(false);
        nowBackGround = _obj;
    }

    Color GetAlphaColor(Color color)
    {
        time += Time.deltaTime * speed;
        color.a = Mathf.Abs(Mathf.Sin(time)) * sinLim + (1f - sinLim);
        return color;
    }
}
