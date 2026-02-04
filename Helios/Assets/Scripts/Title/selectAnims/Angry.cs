using DG.Tweening;
using UnityEngine;

public class Angry : MonoBehaviour
{
    [SerializeField] RectTransform left;
    [SerializeField] RectTransform right;

    void Start()
    {
        OnMove();
    }

    private void OnMove()
    {
        var seqence = DOTween.Sequence();
        seqence.Append(left.DOScale(Vector3.one,0.5f))
               .Join(right.DOScale(Vector3.one, 0.5f))
               .Append(left.DOScale(Vector3.one * 0.8f, 0.5f))
               .Join(right.DOScale(Vector3.one * 0.8f, 0.5f).OnComplete(() => OnMove()));
    }
}
