using DG.Tweening;
using UnityEngine;

public class Sorrow : MonoBehaviour
{
    RectTransform rectTransform;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        OnMove();
    }

    private void OnMove()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(rectTransform.DOAnchorPosY(115f, 1.5f).SetDelay(2.0f))
                .SetLoops(-1, LoopType.Restart);
        sequence.Play();
    }
}
