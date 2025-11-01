using System.Collections;
using UnityEngine;

public class Sinner009 : MonoBehaviour
{
    [SerializeField] private Renderer targetRenderer;   // フェード対象のRenderer
    [SerializeField, Range(0f, 1f)] private float fromAlpha = 0f; // 開始α
    [SerializeField, Range(0f, 1f)] private float toAlpha = 1f;   // 終了α
    [SerializeField] private float duration = 1f;                  // フェード時間（秒）


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
    [SerializeField] private bool UseSetA = true;
    [SerializeField] private bool UseTarget = false;

    [Header("=== Barrier Fade Settings ===")]
    [SerializeField] private float barrierFadeDuration = 1f;
    [SerializeField] private float barrierAlpha = 1f;

    // ===== 初期値保持 =====
    private Vector3 moveTargetA_StartPos;
    private Vector3 moveTargetB_StartPos;
    private Quaternion rotA_Obj1_StartRot;
    private Quaternion rotA_Obj2_StartRot;
    private Quaternion rotB_Obj1_StartRot;
    private Quaternion rotB_Obj2_StartRot;
    private float[] v_DoorMat_StartAlpha;
    private float[] e_DoorMat_StartAlpha;
    private float v_BarrierMat_StartAlpha;
    private float e_BarrierMat_StartAlpha;
    private Vector3 movePos_Start;
    private float startAlpha_Start;
    private float endAlpha_Start;
    private float fadeInDuration_Start;
    private float fadeOutDuration_Start;
    private float moveTime_Start;
    private float rotA_Time_Start;
    private float rotB_Time_Start;
    private float barrierFadeDuration_Start;
    private float barrierAlpha_Start;

    private void Awake()
    {
        // オブジェクト初期値保存
        moveTargetA_StartPos = moveTargetA.localPosition;
        moveTargetB_StartPos = moveTargetB.localPosition;

        rotA_Obj1_StartRot = rotA_Obj1.localRotation;
        rotA_Obj2_StartRot = rotA_Obj2.localRotation;
        rotB_Obj1_StartRot = rotB_Obj1.localRotation;
        rotB_Obj2_StartRot = rotB_Obj2.localRotation;

        movePos_Start = movePos;
        startAlpha_Start = startAlpha;
        endAlpha_Start = endAlpha;
        fadeInDuration_Start = fadeInDuration;
        fadeOutDuration_Start = fadeOutDuration;
        moveTime_Start = moveTime;
        rotA_Time_Start = rotA_Time;
        rotB_Time_Start = rotB_Time;
        barrierFadeDuration_Start = barrierFadeDuration;
        barrierAlpha_Start = barrierAlpha;

        // マテリアル初期α保存
        v_DoorMat_StartAlpha = new float[v_DoorMat.Length];
        for (int i = 0; i < v_DoorMat.Length; i++)
            v_DoorMat_StartAlpha[i] = v_DoorMat[i].GetColor("_MainColor").a;

        e_DoorMat_StartAlpha = new float[e_DoorMat.Length];
        for (int i = 0; i < e_DoorMat.Length; i++)
            e_DoorMat_StartAlpha[i] = e_DoorMat[i].GetColor("_MainColor").a;

        if (v_BarrierMat) v_BarrierMat_StartAlpha = v_BarrierMat.GetColor("_DotColor").a;
        if (e_BarrierMat) e_BarrierMat_StartAlpha = e_BarrierMat.GetColor("_DotColor").a;
    }

    private void OnEnable()
    {
        ResetAll();
        StartCoroutine(ProcessRoutine());
    }

    // ===== 初期値に戻す =====
    public void ResetAll()
    {
        moveTargetA.localPosition = moveTargetA_StartPos;
        moveTargetB.localPosition = moveTargetB_StartPos;

        rotA_Obj1.localRotation = rotA_Obj1_StartRot;
        rotA_Obj2.localRotation = rotA_Obj2_StartRot;
        rotB_Obj1.localRotation = rotB_Obj1_StartRot;
        rotB_Obj2.localRotation = rotB_Obj2_StartRot;

        movePos = movePos_Start;
        startAlpha = startAlpha_Start;
        endAlpha = endAlpha_Start;
        fadeInDuration = fadeInDuration_Start;
        fadeOutDuration = fadeOutDuration_Start;
        moveTime = moveTime_Start;
        rotA_Time = rotA_Time_Start;
        rotB_Time = rotB_Time_Start;
        barrierFadeDuration = barrierFadeDuration_Start;
        barrierAlpha = barrierAlpha_Start;

        for (int i = 0; i < v_DoorMat.Length; i++)
        {
            Color c = v_DoorMat[i].GetColor("_MainColor");
            c.a = v_DoorMat_StartAlpha[i];
            v_DoorMat[i].SetColor("_MainColor", c);
        }

        for (int i = 0; i < e_DoorMat.Length; i++)
        {
            Color c = e_DoorMat[i].GetColor("_MainColor");
            c.a = e_DoorMat_StartAlpha[i];
            e_DoorMat[i].SetColor("_MainColor", c);
        }

        if (v_BarrierMat)
        {
            Color c = v_BarrierMat.GetColor("_DotColor");
            c.a = v_BarrierMat_StartAlpha;
            v_BarrierMat.SetColor("_DotColor", c);
        }

        if (e_BarrierMat)
        {
            Color c = e_BarrierMat.GetColor("_DotColor");
            c.a = e_BarrierMat_StartAlpha;
            e_BarrierMat.SetColor("_DotColor", c);
        }
    }
    
    // ===== ProcessRoutine 以下は変更なし =====
    IEnumerator ProcessRoutine()
    {
        Coroutine fadeA = StartCoroutine(FadeAlpha());
        yield return fadeA;

        Coroutine fadeV = StartCoroutine(FadeMaterials(v_DoorMat, 0f, startAlpha, fadeInDuration));
        Coroutine fadeE = StartCoroutine(FadeMaterials(e_DoorMat, 0f, startAlpha, fadeInDuration));
        yield return fadeV;
        yield return fadeE;

        yield return new WaitForSeconds(0.5f);

        Transform movingTarget = UseSetA ? moveTargetA : moveTargetB;
        Material[] nonMovingMats = UseSetA ? v_DoorMat : e_DoorMat;
        Vector3 finalOffset = UseTarget ? movePos * -1f : movePos;

       

        Coroutine moveCoroutine = StartCoroutine(MoveObject(movingTarget, finalOffset, moveTime));
        Coroutine fadeOutCoroutine = StartCoroutine(FadeMaterials(nonMovingMats, startAlpha, endAlpha, fadeOutDuration));

        yield return moveCoroutine;
        yield return fadeOutCoroutine;

        yield return new WaitForSeconds(0.75f);

        if (UseSetA)
            yield return StartCoroutine(RotateTwo(rotA_Obj1, rotA_Obj2, rotA_To1, rotA_To2, rotA_Time));
        else
            yield return StartCoroutine(RotateTwo(rotB_Obj1, rotB_Obj2, rotB_To1, rotB_To2, rotB_Time));

        yield return StartCoroutine(FadeBarrierAfterRotation());

        yield return new WaitForSeconds(0.25f);
        ResetAll();
        gameObject.SetActive(false);
    }

    private IEnumerator FadeAlpha()
    {
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

        // 最終値を確実にセット
        Color finalColor = mat.color;
        finalColor.a = toAlpha;
        mat.color = finalColor;
    }

    IEnumerator MoveObject(Transform obj, Vector3 offset, float duration)
    {
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
                if (!m) continue;
                Color c = m.GetColor("_MainColor");
                c.a = Mathf.Lerp(fromAlpha, toAlpha, ratio);
                m.SetColor("_MainColor", c);
            }

            yield return null;
        }

        foreach (var m in mats)
        {
            if (!m) continue;
            Color c = m.GetColor("_MainColor");
            c.a = toAlpha;
            m.SetColor("_MainColor", c);
        }
    }

    IEnumerator RotateTwo(Transform obj1, Transform obj2, Vector3 to1, Vector3 to2, float duration)
    {
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
        Material barrierMat = UseSetA ? e_BarrierMat : v_BarrierMat;

        if (!barrierMat) yield break;

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
}
