using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OptionAnimetion : MonoBehaviour
{
    [SerializeField] RectTransform optionBaseRect;
    [SerializeField] float moveTime;
    [SerializeField] float dirayTime;
    [SerializeField] MusicNameMover musicNameMover;
    [SerializeField] AudioClip openSE;
    public bool isAnim {  get; private set; }

    private void Awake()
    {
        isAnim = true;
    }

    private void OnEnable()
    {
        optionBaseRect.sizeDelta = Vector2.zero;
        optionBaseRect.localScale = Vector3.one * 0.8f;
        StartCoroutine(OpenAnim());
    }

    IEnumerator OpenAnim()
    {
        Locator<AudioManager>.Instance.PlaySE(openSE);
        yield return optionBaseRect.DOSizeDelta(Vector2.one * 100f, moveTime);
        yield return new WaitForSeconds(dirayTime + moveTime);
        yield return optionBaseRect.DOLocalRotate(new Vector3(0, 0, 360f), moveTime, RotateMode.FastBeyond360);
        yield return optionBaseRect.DOSizeDelta(Vector2.one * 300f, moveTime);
        yield return new WaitForSeconds(dirayTime + moveTime);

        yield return optionBaseRect.DOSizeDelta(new Vector2(1920f, 1080f), moveTime);
        yield return new WaitForSeconds(dirayTime + moveTime);
        yield return optionBaseRect.DOScale(Vector3.one, moveTime + dirayTime);
        isAnim = false;
        musicNameMover.PositionReset();
    }
}
