using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public class SlingerMove : EasingMethods
{
    float z;
    [SerializeField] float moveAngle;
    [SerializeField] float moveTime;
    public bool isMove {  get; private set; }
    RectTransform myRect;

    void Awake()
    {
        z = GetComponent<RectTransform>().rotation.z;
        isMove = false;
        myRect = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        myRect.localRotation = Quaternion.Euler(0, 0, 0);
    }

    public IEnumerator Move(float _dir)
    {
        var sequence = DOTween.Sequence();
        isMove = true;
        float time = 0f;
        while (isMove)
        {
            yield return null;
            time += Time.deltaTime;
            if (time >= moveTime)
            {
                isMove = false;
                time = moveTime;
            }
            float angle = (moveAngle * _dir) * EaseOutBack(EaseInCubic(time / moveTime));
            myRect.localRotation = Quaternion.Euler(0, 0, angle + z);
        }
        z += moveAngle * _dir;
        gameObject.transform.DOShakePosition(0.3f,0.7f,10,60,false);
    }
}
