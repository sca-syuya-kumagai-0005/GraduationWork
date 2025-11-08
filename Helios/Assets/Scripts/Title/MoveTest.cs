using DG.Tweening;
using System.Collections;
using UnityEngine;

public class MoveTest : EasingMethods
{
    KeyCode[] inputKey;
    float z;
    [SerializeField] float moveAngle;
    [SerializeField] float moveTime;
    public bool isMove {  get; private set; }
    [SerializeField] GameObject gameObject;

    void Start()
    {
        z = GetComponent<RectTransform>().rotation.z;
        inputKey = new KeyCode[2];
        inputKey[0] = KeyCode.A;
        inputKey[1] = KeyCode.D;
        isMove = false;
    }

    //void Update()
    //{
    //    for (int i = 0; i < inputKey.Length; i++)
    //    {
    //        if (Input.GetKeyDown(inputKey[i]))
    //        {
    //            float f = Input.GetAxisRaw("Horizontal");
    //            StartCoroutine(Move(-f));
    //        }
    //    }
    //}
    public IEnumerator Move(float _dir)
    {
        RectTransform rect = GetComponent<RectTransform>();
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
            rect.localRotation = Quaternion.Euler(0, 0, angle + z);
        }
        z += moveAngle * _dir;
        gameObject.transform.DOShakePosition(0.3f,0.7f,10,60,false);
    }
}
