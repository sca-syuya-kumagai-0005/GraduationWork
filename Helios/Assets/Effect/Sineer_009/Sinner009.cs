using System.Collections;
using UnityEngine;

public class Sinner009 : MonoBehaviour
{
    [Header("=== Shader Materials ===")]
    [SerializeField] private Material[] v_DoorMat;
    [SerializeField] private Material[] e_DoorMat;

    [Header("=== Fade Settings ===")]
    [SerializeField] private float startAlpha = 1f;
    [SerializeField] private float endAlpha = 0f;
    [SerializeField] private float fadeDuration = 1f;

    [Header("=== Move Settings ===")]
    [SerializeField] private Transform moveTargetA;
    [SerializeField] private Transform moveTargetB;
    [SerializeField] private Vector3 movePos;
    [SerializeField] private float moveTime = 1.5f;

    [Header("=== Rotation Set A ===")]
    [SerializeField] private Transform rotA_Obj1;
    [SerializeField] private Transform rotA_Obj2;
    [SerializeField] private Vector3 rotA_To1;
    [SerializeField] private Vector3 rotA_To2;
    [SerializeField] private float rotA_Time = 1f;

    [Header("=== Rotation Set B ===")]
    [SerializeField] private Transform rotB_Obj1;
    [SerializeField] private Transform rotB_Obj2;
    [SerializeField] private Vector3 rotB_To1;
    [SerializeField] private Vector3 rotB_To2;
    [SerializeField] private float rotB_Time = 1f;

    [Header("=== Flags ===")]
    [SerializeField] private bool UseTargetA = false;  // 移動用ターゲット切替
    [SerializeField] private bool UseTarget = false;   // 移動反転
    [SerializeField] private bool UseRotateA = true;   // 回転パターン切替

    private void OnEnable()
    {
        StartCoroutine(ProcessRoutine());
    }

    IEnumerator ProcessRoutine()
    {
        // 1) Shader Alpha フェード
        yield return StartCoroutine(FadeAlpha());

        // 2) Move
        Transform target = UseTargetA ? moveTargetA : moveTargetB;
        Vector3 finalPos = UseTarget ? movePos * -1f : movePos;
        yield return StartCoroutine(MoveObject(target, finalPos, moveTime));

        // 3) Rotate
        if (UseRotateA)
            yield return StartCoroutine(RotateTwo(rotA_Obj1, rotA_Obj2, rotA_To1, rotA_To2, rotA_Time));
        else
            yield return StartCoroutine(RotateTwo(rotB_Obj1, rotB_Obj2, rotB_To1, rotB_To2, rotB_Time));
    }

    IEnumerator FadeAlpha()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float ratio = Mathf.Clamp01(t / fadeDuration);
            float a = Mathf.Lerp(startAlpha, endAlpha, ratio);

            foreach (var m in v_DoorMat)
                if (m)
                {
                    Color c = m.GetColor("_MainColor");
                    c.a = a;
                    m.SetColor("_MainColor", c);
                }

            foreach (var m in e_DoorMat)
                if (m)
                {
                    Color c = m.GetColor("_MainColor");
                    c.a = a;
                    m.SetColor("_MainColor", c);
                }

            yield return null;
        }
    }

    IEnumerator MoveObject(Transform obj, Vector3 offset, float time)
    {
        Vector3 start = obj.localPosition;
        Vector3 end = start + offset;
        float t = 0f;

        while (t < time)
        {
            t += Time.deltaTime;
            obj.localPosition = Vector3.Lerp(start, end, t / time);
            yield return null;
        }
    }

    IEnumerator RotateTwo(Transform obj1, Transform obj2, Vector3 to1, Vector3 to2, float duration)
    {
        Quaternion start1 = obj1.localRotation;
        Quaternion start2 = obj2.localRotation;

        Quaternion end1 = Quaternion.Euler(to1);
        Quaternion end2 = Quaternion.Euler(to2);

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float ratio = Mathf.Clamp01(t / duration);

            obj1.localRotation = Quaternion.Lerp(start1, end1, ratio);
            obj2.localRotation = Quaternion.Lerp(start2, end2, ratio);

            yield return null;
        }

        // 最終値を保証
        obj1.localRotation = end1;
        obj2.localRotation = end2;
    }
}
