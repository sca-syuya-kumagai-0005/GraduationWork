using System.Collections;
using UnityEngine;

public class Sinner_009 : MonoBehaviour
{
    [Header("=== Renderer / Fade Alpha ===")]
    [SerializeField] private Renderer targetRenderer;
    [SerializeField, Range(0f, 1f)] private float fromAlpha = 0f;
    [SerializeField, Range(0f, 1f)] private float toAlpha = 1f;
    [SerializeField] private float duration = 1f;

    [Header("=== Shader Materials ===")]
    [SerializeField] private Material[] v_DoorMat;
    [SerializeField] private Material[] e_DoorMat;
    [SerializeField] private Material v_BarrierMat;
    [SerializeField] private Material e_BarrierMat;

    [Header("=== Fade Settings ===")]
    [SerializeField] private float startAlpha = 1f;
    [SerializeField] private float endAlpha = 0f;
    [SerializeField] private float fadeInDuration = 1.0f;
    [SerializeField] private float fadeOutDuration = 1.5f;

    [Header("=== Move Settings ===")]
    [SerializeField] private Transform moveTargetA;
    [SerializeField] private Transform moveTargetB;
    [SerializeField] private Vector3 movePos;
    [SerializeField] private float moveTime = 1.5f;

    [SerializeField] private Vector3 offSetA;
    [SerializeField] private Vector3 offSetB;

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

    [Header("=== Rotation Start Angles (Inspector) ===")]
    [SerializeField] private Vector3 rotA_Start1;
    [SerializeField] private Vector3 rotA_Start2;
    [SerializeField] private Vector3 rotB_Start1;
    [SerializeField] private Vector3 rotB_Start2;

    [Header("=== Flags ===")]
    [SerializeField] private bool useSetA = true;

    [Header("=== Barrier Fade Settings ===")]
    [SerializeField] private float barrierFadeDuration = 1f;
    [SerializeField] private float barrierAlpha = 1f;

    private void OnEnable()
    {
        ResetAll();
        StartCoroutine(ProcessRoutine());
    }

    #region Reset / Initialization

    public void ResetAll()
    {
        if (v_DoorMat != null)
        {
            foreach (var m in v_DoorMat)
            {
                if (m == null) continue;
                if (m.HasProperty("_MainColor"))
                {
                    Color vCol = m.GetColor("_MainColor");
                    vCol.a = 0f;
                    m.SetColor("_MainColor", vCol);
                }
            }
        }

        if (e_DoorMat != null)
        {
            foreach (var m in e_DoorMat)
            {
                if (m == null) continue;
                if (m.HasProperty("_MainColor"))
                {
                    Color eCol = m.GetColor("_MainColor");
                    eCol.a = 0f;
                    m.SetColor("_MainColor", eCol);
                }
            }
        }

        if (v_BarrierMat != null && v_BarrierMat.HasProperty("_DotColor"))
        {
            Color vb = v_BarrierMat.GetColor("_DotColor");
            vb.a = 0f;
            v_BarrierMat.SetColor("_DotColor", vb);
        }

        if (e_BarrierMat != null && e_BarrierMat.HasProperty("_DotColor"))
        {
            Color eb = e_BarrierMat.GetColor("_DotColor");
            eb.a = 0f;
            e_BarrierMat.SetColor("_DotColor", eb);
        }

        if (moveTargetA != null) moveTargetA.position = offSetA;
        if (moveTargetB != null) moveTargetB.position = offSetB;

        if (rotA_Obj1 != null) rotA_Obj1.localRotation = Quaternion.Euler(rotA_Start1);
        if (rotA_Obj2 != null) rotA_Obj2.localRotation = Quaternion.Euler(rotA_Start2);

        if (rotB_Obj1 != null) rotB_Obj1.localRotation = Quaternion.Euler(rotB_Start1);
        if (rotB_Obj2 != null) rotB_Obj2.localRotation = Quaternion.Euler(rotB_Start2);

        if (targetRenderer != null && targetRenderer.material != null)
        {
            Material mat = targetRenderer.material;
            Color startColor = mat.color;
            startColor.a = fromAlpha;
            mat.color = startColor;
        }
    }

    #endregion

    #region Main Routine

    IEnumerator ProcessRoutine()
    {
        yield return StartCoroutine(FadeAlpha());

        Coroutine fadeV = StartCoroutine(FadeMaterials(v_DoorMat, 0f, startAlpha, fadeInDuration));
        Coroutine fadeE = StartCoroutine(FadeMaterials(e_DoorMat, 0f, startAlpha, fadeInDuration));
        yield return fadeV;
        yield return fadeE;

        yield return new WaitForSeconds(0.5f);

        Transform movingTarget = useSetA ? moveTargetA : moveTargetB;
        Material[] nonMovingMats = useSetA ? v_DoorMat : e_DoorMat;
        Vector3 finalOffset = useSetA ? movePos : movePos * -1f;

        Coroutine moveCoroutine = StartCoroutine(MoveObject(movingTarget, finalOffset, moveTime));
        Coroutine fadeOutCoroutine = StartCoroutine(FadeMaterials(nonMovingMats, startAlpha, endAlpha, fadeOutDuration));

        yield return moveCoroutine;
        yield return fadeOutCoroutine;

        yield return new WaitForSeconds(0.75f);

        if (useSetA)
            yield return StartCoroutine(RotateTwo(rotA_Obj1, rotA_Obj2, rotA_To1, rotA_To2, rotA_Time));
        else
            yield return StartCoroutine(RotateTwo(rotB_Obj1, rotB_Obj2, rotB_To1, rotB_To2, rotB_Time));

        yield return StartCoroutine(FadeBarrierAfterRotation());

        yield return new WaitForSeconds(0.25f);

        ResetAll();
        gameObject.SetActive(false);
    }

    #endregion

    #region Coroutines

    private IEnumerator FadeAlpha()
    {
        if (targetRenderer == null || targetRenderer.material == null)
            yield break;

        Material mat = targetRenderer.material;
        Color startColor = mat.color;
        startColor.a = fromAlpha;
        mat.color = startColor;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float ratio = Mathf.Clamp01(t / duration);

            Color c = startColor;
            c.a = Mathf.Lerp(fromAlpha, toAlpha, ratio);
            mat.color = c;

            yield return null;
        }

        Color finalColor = mat.color;
        finalColor.a = toAlpha;
        mat.color = finalColor;
    }

    IEnumerator MoveObject(Transform obj, Vector3 offset, float duration)
    {
        if (obj == null)
            yield break;

        Vector3 startPos = obj.localPosition;
        Vector3 endPos = startPos + offset;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float ratio = Mathf.Clamp01(t / duration);
            obj.localPosition = Vector3.Lerp(startPos, endPos, ratio);
            yield return null;
        }

        obj.localPosition = endPos;
    }

    IEnumerator FadeMaterials(Material[] mats, float fromAlpha, float toAlpha, float duration)
    {
        if (mats == null || mats.Length == 0) yield break;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float ratio = Mathf.Clamp01(t / duration);

            foreach (var m in mats)
            {
                if (m == null || !m.HasProperty("_MainColor")) continue;
                Color c = m.GetColor("_MainColor");
                c.a = Mathf.Lerp(fromAlpha, toAlpha, ratio);
                m.SetColor("_MainColor", c);
            }

            yield return null;
        }

        foreach (var m in mats)
        {
            if (m == null || !m.HasProperty("_MainColor")) continue;
            Color c = m.GetColor("_MainColor");
            c.a = toAlpha;
            m.SetColor("_MainColor", c);
        }
    }

    IEnumerator RotateTwo(Transform obj1, Transform obj2, Vector3 to1, Vector3 to2, float duration)
    {
        if (obj1 == null || obj2 == null)
            yield break;

        Quaternion s1 = obj1.localRotation;
        Quaternion s2 = obj2.localRotation;

        Quaternion e1 = Quaternion.Euler(to1);
        Quaternion e2 = Quaternion.Euler(to2);

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float ratio = Mathf.Clamp01(t / duration);

            obj1.localRotation = Quaternion.Lerp(s1, e1, ratio);
            obj2.localRotation = Quaternion.Lerp(s2, e2, ratio);

            yield return null;
        }

        obj1.localRotation = e1;
        obj2.localRotation = e2;
    }

    IEnumerator FadeBarrierAfterRotation()
    {
        Material barrierMat = useSetA ? e_BarrierMat : v_BarrierMat;
        if (barrierMat == null || !barrierMat.HasProperty("_DotColor")) yield break;

        float fromAlpha = barrierMat.GetColor("_DotColor").a;
        float t = 0f;

        while (t < barrierFadeDuration)
        {
            t += Time.deltaTime;
            float ratio = Mathf.Clamp01(t / barrierFadeDuration);

            Color c = barrierMat.GetColor("_DotColor");
            c.a = Mathf.Lerp(fromAlpha, barrierAlpha, ratio);
            barrierMat.SetColor("_DotColor", c);

            yield return null;
        }

        Color finalColor = barrierMat.GetColor("_DotColor");
        finalColor.a = barrierAlpha;
        barrierMat.SetColor("_DotColor", finalColor);
    }

    #endregion
}
