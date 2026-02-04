using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Happy : MonoBehaviour
{
    RectTransform rectTransform;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        OnMove();
    }

    private void OnMove()
    {
        var seqence = DOTween.Sequence();
        seqence.Append(rectTransform.DORotate(new Vector3(0, 0, 5f), 1.5f).SetEase(Ease.Linear))
               .Append(rectTransform.DORotate(new Vector3(0, 0, -5f), 1.5f).SetEase(Ease.Linear).OnComplete(()=> OnMove()));
    }
}
