using DG.Tweening;
using UnityEngine;

public class TextMover : MonoBehaviour
{
    RectTransform myRect;
    Vector2 defaultPosition;
    const float delayTime = 2f;
    const float moveDistance = 730f;

    private void Awake()
    {
        myRect = GetComponent<RectTransform>();
        defaultPosition = myRect.anchoredPosition;
    }

    public void PositionReset()
    {
        myRect.DOKill();
        myRect.anchoredPosition = defaultPosition;
        Move();
    }

    void Move()
    {
        float endValue = defaultPosition.x - myRect.sizeDelta.x + moveDistance;
        if (endValue > defaultPosition.x) endValue = defaultPosition.x;
        myRect.DOAnchorPosX(endValue, 1.5f).SetEase(Ease.Linear).SetDelay(delayTime).OnComplete(() =>
        {
            Invoke(nameof(PositionReset), delayTime);
        });
    }
}
