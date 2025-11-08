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
    [SerializeField, Header("起動時に表示する注意喚起演出")] Cautionary[] cautionaries;
    [SerializeField] Image cautionaryImage;
    [SerializeField] Text cautionaryHeader;
    [SerializeField] Text cautionaryText;
    [SerializeField] float waitTime;
    [SerializeField] Image cautionaryPanel;
    [SerializeField, Header("タイトル演出")] GameObject titleLogo;
    Image logoImage;
    RectTransform logoRect;
    [SerializeField] Text pushText;

    bool skip = false;
    float time = 0;
    const float speed = 1.5f;
    const float sinLim = 0.8f;

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
        if(cautionaryNum == CautionaryNum.SOUND) cautionaryPanel.DOFade(0f, fadeOutTime).WaitForCompletion();
        skip = false;
    }

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
        yield return new WaitForSeconds(1f);
    }

    Color GetAlphaColor(Color color)
    {
        time += Time.deltaTime * speed;
        color.a = Mathf.Abs(Mathf.Sin(time)) * sinLim + (1f - sinLim);
        return color;
    }
}
