using UnityEngine;
using System.Collections;
public class Sinner_016 : MonoBehaviour
{
    #region === Scale Settings ===

    [Header("=== Scale Settings 1 ===")]
    [SerializeField] private Vector3 startScale1 = Vector3.one;
    [SerializeField] private Vector3 endScale1 = Vector3.one * 2f;
    [SerializeField] private float duration1 = 1.0f;

    [Header("=== Scale Settings 2 ===")]
    [SerializeField] private Vector3 startScale2 = Vector3.one * 2f;
    [SerializeField] private Vector3 endScale2 = Vector3.one * 0.5f;
    [SerializeField] private float duration2 = 1.0f;

    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioClip sinner_016;

    #endregion

    #region === Clip Settings ===

    [Header("=== Clip Settings 1 ===")]
    [SerializeField] private float startClip1 = 0f;
    [SerializeField] private float endClip1 = 1f;
    [SerializeField] private float clipDuration1 = 1.0f;
    [SerializeField] private Material eye_Mat;

    [Header("=== Clip Settings 2 ===")]
    [SerializeField] private float startClip2 = 1f;
    [SerializeField] private float endClip2 = 0f;
    [SerializeField] private float clipDuration2 = 1.0f;
    [SerializeField] private Material eyeDistortion_Mat;

    [Header("=== Clip Settings 3 ===")]
    [SerializeField] private float startClip3 = 0f;
    [SerializeField] private float endClip3 = 1f;
    [SerializeField] private float clipDuration3 = 1.0f;
    [SerializeField] private Material outLine;

    [Header("=== Clip Settings 4 ===")]
    [SerializeField] private float startClip4 = 0f;
    [SerializeField] private float endClip4 = 1f;
    [SerializeField] private float clipDuration4 = 1.0f;

    [Header("=== Clip Settings 5 ===")]
    [SerializeField] private float startClip5 = 0f;
    [SerializeField] private float endClip5 = 1f;
    [SerializeField] private float clipDuration5 = 1.0f;

    #endregion

    #region === Extra Scale Settings ===

    [Header("=== Extra Scale Settings ===")]
    [Tooltip("追加でスケールアニメーションさせるオブジェクト")]
    [SerializeField] private Transform extraTarget;
    [SerializeField] private Vector3 extraStartScale = Vector3.one;
    [SerializeField] private Vector3 extraEndScale = Vector3.one * 3f;
    [SerializeField] private float extraDuration = 1.0f;

    #endregion

    #region === Move Settings ===

    [Header("=== Move Settings 1 ===")]
    [SerializeField] private Transform moveTarget;
    [SerializeField] private Vector3 startPos1 = Vector3.zero;
    [SerializeField] private Vector3 endPos1 = Vector3.one;
    [SerializeField] private float moveDuration1 = 1f;

    [Header("=== Move Settings 2 ===")]
    [SerializeField] private Vector3 startPos2 = Vector3.zero;
    [SerializeField] private Vector3 endPos2 = Vector3.one * 2f;
    [SerializeField] private float moveDuration2 = 1f;

    [Header("=== Move Settings 3 ===")]
    [SerializeField] private Vector3 startPos3 = Vector3.zero;
    [SerializeField] private Vector3 endPos3 = Vector3.one * 3f;
    [SerializeField] private float moveDuration3 = 1f;

    #endregion

    #region === Color Settings ===

    [Header("=== Move Color Change Settings ===")]
    [SerializeField] private Material colorMat1;
    [SerializeField] private Material colorMat2;

    [Tooltip("HDR 対応カラー (各ステップごと: 0,1,2...)")]
    [SerializeField] private Color[] mat1_StartColors = new Color[3];
    [SerializeField] private Color[] mat1_EndColors = new Color[3];
    [SerializeField] private Color[] mat2_StartColors = new Color[3];
    [SerializeField] private Color[] mat2_EndColors = new Color[3];

    [Header("=== Move Color Intensity Settings ===")]
    [SerializeField] private float[] mat1_StartIntensity = new float[3];
    [SerializeField] private float[] mat1_EndIntensity = new float[3];
    [SerializeField] private float[] mat2_StartIntensity = new float[3];
    [SerializeField] private float[] mat2_EndIntensity = new float[3];

    [SerializeField] private float colorDuration = 0.5f;

    #endregion

    #region === Runtime cache ===

    /// <summary>メインの目オブジェクト（DeagonEye）</summary>
    private Transform eyeTransform;

    /// <summary>歪み用の目オブジェクト（DeagonEye_Distortion）</summary>
    private Transform eyeDistortionTransform;

    #endregion

    #region === Unity Callbacks ===

    private void Start()
    {
        // 初回起動時にターゲットをシーンから取得
        eyeTransform = GameObject.Find("DeagonEye")?.transform;
        eyeDistortionTransform = GameObject.Find("DeagonEye_Distortion")?.transform;

        if (eyeTransform == null || eyeDistortionTransform == null)
        {
            // 必要なオブジェクトが見つからない場合は何もせず終了
            return;
        }

        // 例: 最初はセット1で拡大し、その後セット2で縮小
        StartCoroutine(ScaleSequence());
    }

    private void OnEnable()
    {
        if (!Application.isPlaying) return;

        // シーン再有効化時にも再取得（リロードなどのケース対応）
        eyeTransform = GameObject.Find("DeagonEye")?.transform;
        eyeDistortionTransform = GameObject.Find("DeagonEye_Distortion")?.transform;

        if (eyeTransform == null || eyeDistortionTransform == null)
        {
            return;
        }

        // --- スケール初期化 ---
        eyeTransform.localScale = startScale1;
        eyeDistortionTransform.localScale = startScale2;

        if (extraTarget != null)
        {
            extraTarget.localScale = extraStartScale;
        }

        // --- Clip の初期化 ---
        if (eye_Mat != null) eye_Mat.SetFloat("_Clip", startClip1);
        if (eyeDistortion_Mat != null) eyeDistortion_Mat.SetFloat("_Clip", startClip2);
        if (outLine != null) outLine.SetFloat("_Clip", startClip3);

        // --- Move の初期化 ---
        if (moveTarget != null)
        {
            moveTarget.localPosition = startPos1;
        }

        // --- Color 初期化 (index = 0 を初期色とする) ---
        if (colorMat1 != null && mat1_StartColors.Length > 0 && mat1_StartIntensity.Length > 0)
        {
            Color c = mat1_StartColors[0] * mat1_StartIntensity[0];
            colorMat1.SetColor("_Color", c);
        }

        if (colorMat2 != null && mat2_StartColors.Length > 0 && mat2_StartIntensity.Length > 0)
        {
            Color c = mat2_StartColors[0] * mat2_StartIntensity[0];
            colorMat2.SetColor("_Color", c);
        }

        //audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
        //audioManager.PlaySE(sinner_016);

        // --- コルーチン再スタート ---
        StopAllCoroutines();
        StartCoroutine(ScaleSequence());
    }

    #endregion

    #region === Main Sequence ===

    /// <summary>
    /// 目のスケール・クリップ・移動・カラー変化など
    /// 一連の演出シーケンスを管理するコルーチン。
    /// </summary>
    private IEnumerator ScaleSequence()
    {
        // アウトラインの最初の Clip アニメーション
        StartCoroutine(OutLine(outLine, startClip3, endClip3, clipDuration3));

        // --- 1セット目：目のスケール ---
        Coroutine eye = StartCoroutine(ScaleRoutine(eyeTransform, startScale1, endScale1, duration1));
        Coroutine distortion = StartCoroutine(ScaleRoutine(eyeDistortionTransform, startScale2, endScale2, duration2));
        yield return eye;
        yield return distortion;

        yield return new WaitForSeconds(0.2f);

        // --- 目のクリップアニメーション ---
        Coroutine clip1 = StartCoroutine(ClipRoutine(eye_Mat, startClip1, endClip1, clipDuration1));
        Coroutine clip2 = StartCoroutine(ClipRoutine(eyeDistortion_Mat, startClip2, endClip2, clipDuration2));
        yield return clip1;
        yield return clip2;

        yield return new WaitForSeconds(0.3f);

        // --- 追加スケールアップ ---
        yield return StartCoroutine(ExtraScaleRoutine());

        // --- 位置移動１ + アウトラインズーム + カラーStep0 ---
        yield return new WaitForSeconds(0.75f);
        Coroutine zoom = StartCoroutine(ZoomOutLine(outLine, startClip4, endClip4, clipDuration4));
        Coroutine move = StartCoroutine(MoveRoutine(moveTarget, startPos1, endPos1, moveDuration1));
        Coroutine color = StartCoroutine(ColorRoutine(0));
        yield return zoom;
        yield return move;
        yield return color;

        // --- 位置移動２ + アウトラインズーム + カラーStep1 ---
        zoom = StartCoroutine(ZoomOutLine(outLine, startClip4, endClip4, clipDuration4));
        move = StartCoroutine(MoveRoutine(moveTarget, startPos2, endPos2, moveDuration2));
        color = StartCoroutine(ColorRoutine(1));
        yield return zoom;
        yield return move;
        yield return color;

        // --- 位置移動３ + アウトラインズーム + カラーStep2 ---
        zoom = StartCoroutine(ZoomOutLine(outLine, startClip4, endClip4, clipDuration4));
        move = StartCoroutine(MoveRoutine(moveTarget, startPos3, endPos3, moveDuration3));
        color = StartCoroutine(ColorRoutine(2));
        yield return zoom;
        yield return move;
        yield return color;

        // --- 終盤演出 ---
        yield return new WaitForSeconds(0.2f);
        yield return ExtraScaleReverseRoutine();
        yield return new WaitForSeconds(0.2f);

        // 目を元に戻す演出（Clip & Scale 逆再生）
        StartCoroutine(ClipRoutine(eye_Mat, endClip1, startClip1, clipDuration1));
        StartCoroutine(ClipRoutine(eyeDistortion_Mat, endClip2, startClip2, clipDuration2));
        StartCoroutine(ScaleRoutine(eyeTransform, endScale1, startScale1, duration1));
        StartCoroutine(ScaleRoutine(eyeDistortionTransform, endScale2, startScale2, duration2));

        // 最後にアウトラインをフェードアウトしてオブジェクトを無効化
        yield return StartCoroutine(FadeOutOutLine(outLine, startClip5, endClip5, clipDuration5));
    }

    #endregion

    #region === Basic Scale / Move / Clip Coroutines ===

    private IEnumerator ScaleRoutine(Transform target, Vector3 startScale, Vector3 endScale, float duration)
    {
        target.localScale = startScale;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float rate = t / duration;

            target.localScale = Vector3.Lerp(startScale, endScale, rate);
            yield return null;
        }

        target.localScale = endScale;
    }

    private IEnumerator ExtraScaleRoutine()
    {
        if (extraTarget == null)
            yield break;

        extraTarget.localScale = extraStartScale;

        float t = 0f;
        while (t < extraDuration)
        {
            t += Time.deltaTime;
            float rate = t / extraDuration;

            extraTarget.localScale = Vector3.Lerp(extraStartScale, extraEndScale, rate);
            yield return null;
        }

        extraTarget.localScale = extraEndScale;
    }

    private IEnumerator ClipRoutine(Material mat, float start, float end, float duration)
    {
        if (mat == null) yield break;

        mat.SetFloat("_Clip", start);

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float rate = t / duration;

            float value = Mathf.Lerp(start, end, rate);
            mat.SetFloat("_Clip", value);

            yield return null;
        }

        mat.SetFloat("_Clip", end);
    }

    private IEnumerator OutLine(Material mat, float start, float end, float duration)
    {
        if (mat == null) yield break;

        mat.SetFloat("_Clip", start);

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float rate = t / duration;

            float value = Mathf.Lerp(start, end, rate);
            mat.SetFloat("_Clip", value);

            yield return null;
        }
    }

    private IEnumerator ZoomOutLine(Material mat, float start, float end, float duration)
    {
        if (mat == null) yield break;

        float t = 0f;
        mat.SetFloat("_Clip", start);

        // ---- 前半：start → end ----
        while (t < duration)
        {
            t += Time.deltaTime;
            float rate = t / duration;

            mat.SetFloat("_Clip", Mathf.Lerp(start, end, rate));
            yield return null;
        }

        mat.SetFloat("_Clip", end);

        // ---- 後半：end → start ----
        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float rate = t / duration;

            mat.SetFloat("_Clip", Mathf.Lerp(end, start, rate));
            yield return null;
        }
    }

    private IEnumerator FadeOutOutLine(Material mat, float start, float end, float duration)
    {
        if (mat == null) yield break;

        mat.SetFloat("_Clip", start);

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float rate = t / duration;
            mat.SetFloat("_Clip", Mathf.Lerp(start, end, rate));
            yield return null;
        }

        // 絶対に end に固定
        mat.SetFloat("_Clip", end);

        // コルーチン実行後にこのオブジェクト無効化
        gameObject.SetActive(false);
    }

    private IEnumerator MoveRoutine(Transform target, Vector3 start, Vector3 end, float duration)
    {
        if (target == null) yield break;

        target.localPosition = start;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float rate = t / duration;

            target.localPosition = Vector3.Lerp(start, end, rate);

            yield return null;
        }

        target.localPosition = end;
    }

    private IEnumerator ExtraScaleReverseRoutine()
    {
        if (extraTarget == null)
            yield break;

        extraTarget.localScale = extraEndScale;

        float t = 0f;
        while (t < extraDuration)
        {
            t += Time.deltaTime;
            float rate = t / extraDuration;

            extraTarget.localScale = Vector3.Lerp(extraEndScale, extraStartScale, rate);
            yield return null;
        }

        extraTarget.localScale = extraStartScale;
    }

    #endregion

    #region === Color Routine ===

    /// <summary>
    /// 指定インデックスのカラーと強度を、
    /// colorDuration の間で Lerp する。
    /// </summary>
    private IEnumerator ColorRoutine(int index)
    {
        if (index < 0 || index >= mat1_StartColors.Length) yield break;

        float t = 0f;

        // === マテリアル1 ===
        Color c1_start = mat1_StartColors[index];
        Color c1_end = mat1_EndColors[index];

        float i1_start = mat1_StartIntensity[index];
        float i1_end = mat1_EndIntensity[index];

        // === マテリアル2 ===
        Color c2_start = mat2_StartColors[index];
        Color c2_end = mat2_EndColors[index];

        float i2_start = mat2_StartIntensity[index];
        float i2_end = mat2_EndIntensity[index];

        // 初期値適用（結果としては c * intensity のみが使われる）
        if (colorMat1 != null)
        {
            colorMat1.SetColor("_Color", c1_start * i1_start);
        }

        if (colorMat2 != null)
        {
            colorMat2.SetColor("_Color", c2_start * i2_start);
        }

        // --- Lerp ---
        while (t < colorDuration)
        {
            t += Time.deltaTime;
            float rate = t / colorDuration;

            // Mat1
            if (colorMat1 != null)
            {
                Color c = Color.Lerp(c1_start, c1_end, rate);
                float intensity = Mathf.Lerp(i1_start, i1_end, rate);
                colorMat1.SetColor("_Color", c * intensity);
            }

            // Mat2
            if (colorMat2 != null)
            {
                Color c = Color.Lerp(c2_start, c2_end, rate);
                float intensity = Mathf.Lerp(i2_start, i2_end, rate);
                colorMat2.SetColor("_Color", c * intensity);
            }

            yield return null;
        }

        // 最終値固定
        if (colorMat1 != null)
        {
            colorMat1.SetColor("_Color", c1_end * i1_end);
        }

        if (colorMat2 != null)
        {
            colorMat2.SetColor("_Color", c2_end * i2_end);
        }
    }

    #endregion
}
