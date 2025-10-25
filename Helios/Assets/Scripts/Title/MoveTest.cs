using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class MoveTest : EasingMethods
{
    KeyCode[] inputKey;
    float z;
    void Start()
    {
        z = GetComponent<RectTransform>().rotation.z;
        inputKey = new KeyCode[2];
        inputKey[0] = KeyCode.A;
        inputKey[1] = KeyCode.D;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0;i < inputKey.Length;i++)
        {
            if (Input.GetKeyDown(inputKey[i]))
            {
                float f = Input.GetAxisRaw("Horizontal");
                StartCoroutine(Move(-f));
            }
        }
    }
    [SerializeField] float moveAngle;
    [SerializeField] float moveTime;
    IEnumerator Move(float _dir)
    {
        RectTransform rect = GetComponent<RectTransform>();
        var sequence = DOTween.Sequence();
        bool move = false;
        float time = 0f;
        while (!move)
        {
            yield return null;
            time += Time.deltaTime;
            if (time >= moveTime)
            {
                move = true;
                time = moveTime;
            }
            float angle = (moveAngle * _dir ) * EaseOutBack(EaseInCubic(time / moveTime));
            rect.localRotation = Quaternion.Euler(0, 0, angle + z);
        }
        z += moveAngle * _dir;
    }
}
