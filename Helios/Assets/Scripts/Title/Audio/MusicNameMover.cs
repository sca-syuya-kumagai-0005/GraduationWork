using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MusicNameMover : MonoBehaviour
{
    RectTransform myRect;
    Vector2 defaultPosition = new Vector2(10f, 0f);
    const float delayTime = 2f;
    const float moveDistance = 116f;
    
    private void Awake()
    {
        myRect = GetComponent<RectTransform>();
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
