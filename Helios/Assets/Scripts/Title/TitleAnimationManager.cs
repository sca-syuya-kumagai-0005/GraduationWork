using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using System.Collections;

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
    [SerializeField] Text pushText;
    [SerializeField] GameObject rainParticle;
    [SerializeField, Header("選択画面開始時演出")] GameObject titleSelectObj;
    [SerializeField, Header("自室演出")] GameObject myRoomObj;

    bool skip = false;
    float time = 0;
    const float speed = 1.5f;
    const float sinLim = 0.8f;
    GameObject nowTitleMode;

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
        if(cautionaryNum == CautionaryNum.SOUND) yield return StartCoroutine(FadeAnimation(0f,fadeOutTime));
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

    public IEnumerator TitleStartAnim()
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
        SetNowTitleMode(titleSelectObj);
        yield return StartCoroutine(FadeAnimation(0f,0.5f));
        Locator<TitleManager>.Instance.SelectStart();
    }

    public IEnumerator MyRoomAnim()
    {
        yield return StartCoroutine(FadeAnimation(1f, 0.5f));
        SetNowTitleMode(myRoomObj);
        yield return StartCoroutine(FadeAnimation(0f, 0.5f));
    }

    void SetNowTitleMode(GameObject _obj)
    {
        _obj.SetActive(true);
        if (nowTitleMode != null) nowTitleMode.SetActive(false);
        nowTitleMode = _obj;
    }

    Color GetAlphaColor(Color color)
    {
        time += Time.deltaTime * speed;
        color.a = Mathf.Abs(Mathf.Sin(time)) * sinLim + (1f - sinLim);
        return color;
    }
}
