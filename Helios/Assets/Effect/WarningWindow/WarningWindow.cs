using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarningWindow : MonoBehaviour
{
    //========================
    // 設定データクラス
    //========================
    [System.Serializable]
    public class ScaleData
    {
        public Transform[] targets;
        public Vector3 startScale = Vector3.zero;
        public Vector3 endScale = Vector3.one;
        public float duration = 1f;
    }

    [System.Serializable]
    public class BackgroundData
    {
        public Transform background;
        public Vector3 startScale = Vector3.zero;
        public Vector3 endScale = Vector3.one;
    }

    [System.Serializable]
    public class ExtraScaleData
    {
        public Transform target;
        public Vector3 startScale1 = Vector3.one;
        public Vector3 endScale1 = Vector3.one * 1.5f;
        public float duration1 = 0.5f;

        public Vector3 startScale2 = Vector3.one * 1.5f;
        public Vector3 endScale2 = Vector3.one;
        public float duration2 = 0.5f;
    }

    [System.Serializable]
    public class LoopFadeData
    {
        public SpriteRenderer sprite;
        public float startAlpha = 0f;
        public float endAlpha = 1f;
        public float duration = 0.5f;
        public int count = 3;
    }

    [System.Serializable]
    public class MaterialFadeData
    {
        public Material material;

        [Header("Alpha")]
        public float alphaFrom = 0f;
        public float alphaTo = 1f;
        public float alphaDuration = 1f;

        [Header("NoiseStrength")]
        public float noiseFrom = 0.05f;
        public float noiseTo = 0.15f;
        public float noiseDuration = 1f;

        [Header("BlockWidth")]
        public float blockFrom = 0.05f;
        public float blockTo = 0.1f;
        public float blockDuration = 1f;
    }

    //========================
    // インスペクタ設定
    //========================
    [Header("対象スケール設定")]
    [SerializeField] private ScaleData scaleData;

    [Header("背景スケール設定")]
    [SerializeField] private BackgroundData backgroundData;

    [Header("追加スケール設定")]
    [SerializeField] private ExtraScaleData extraScaleData;

    [Header("ループフェード設定")]
    [SerializeField] private LoopFadeData loopFadeData;

    [Header("マテリアルフェード設定")]
    [SerializeField] private MaterialFadeData matFadeData;

    public enum PlayState { In, Out, Idle }

    [Header("再生状態")]
    [SerializeField] private PlayState playState = PlayState.In;
    private PlayState prevState;

    [Header("戻りアニメーションカーブ")]
    [SerializeField] private AnimationCurve restoreCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Coroutine stateRoutine;

    private Coroutine watcherRoutine;   // 状態監視専用
    private Coroutine playRoutine;      // In/Out 再生専用


    private void Start()
    {
        ResetAllValues();
        prevState = playState;

        watcherRoutine = StartCoroutine(StateWatcher()); // ←監視専用
    }


    private IEnumerator StateWatcher()
    {
        while (true)
        {
            if (playState != prevState)
            {
                prevState = playState;

                // 再生中のアニメを止めるのは playRoutine のみ
                if (playRoutine != null)
                {
                    StopCoroutine(playRoutine);
                    playRoutine = null;
                }

                switch (playState)
                {
                    case PlayState.In:
                        ResetAllValues();
                        playRoutine = StartCoroutine(MainSequence());
                        break;

                    case PlayState.Out:
                        playRoutine = StartCoroutine(RestoreValues(0.25f));
                        break;

                    case PlayState.Idle:
                        break;
                }
            }

            yield return null;
        }
    }


    private void ResetAllValues()
    {
        // Main targets
        foreach (var t in scaleData.targets)
            if (t != null) t.localScale = scaleData.startScale;

        // Background
        if (backgroundData.background != null)
            backgroundData.background.localScale = backgroundData.startScale;

        // Extra
        if (extraScaleData.target != null)
            extraScaleData.target.localScale = extraScaleData.startScale1;

        // Sprite Renderer
        if (loopFadeData.sprite != null)
        {
            var c = loopFadeData.sprite.color;
            c.a = loopFadeData.startAlpha;
            loopFadeData.sprite.color = c;
        }

        // Material
        if (matFadeData.material != null)
        {
            matFadeData.material.SetFloat("_Alpha", matFadeData.alphaFrom);
            matFadeData.material.SetFloat("_NoiseStrength", matFadeData.noiseFrom);
            matFadeData.material.SetFloat("_BlockWidth", matFadeData.blockFrom);
        }
    }

    private IEnumerator MainSequence()
    {
        // --- 同時再生する処理 ---
        List<Coroutine> running = new List<Coroutine>();

        foreach (var t in scaleData.targets)
            if (t != null)
                running.Add(StartCoroutine(ScaleRoutine(t, scaleData.startScale, scaleData.endScale, scaleData.duration)));

        if (backgroundData.background != null)
            running.Add(StartCoroutine(ScaleRoutine(backgroundData.background, backgroundData.startScale, backgroundData.endScale, scaleData.duration)));

        if (loopFadeData.sprite != null)
            running.Add(StartCoroutine(LoopFade(loopFadeData)));

        // --- これらが終わるの待つ ---
        foreach (var c in running)
            yield return c;

        // --- Extra ---
        yield return ExtraScaleSequence();

        yield return new WaitForSeconds(.25f);

        // --- Material Animations ---
        yield return MaterialFloat(matFadeData.material, "_Alpha", matFadeData.alphaFrom, matFadeData.alphaTo, matFadeData.alphaDuration);
        yield return MaterialFloat(matFadeData.material, "_NoiseStrength", matFadeData.noiseFrom, matFadeData.noiseTo, matFadeData.noiseDuration);
        yield return MaterialFloat(matFadeData.material, "_BlockWidth", matFadeData.blockFrom, matFadeData.blockTo, matFadeData.blockDuration);
        yield return new WaitForSeconds(1.0f);

        // --- Restore ---
        yield return RestoreValues(0.25f);
    }

    private IEnumerator ScaleRoutine(Transform target, Vector3 from, Vector3 to, float time)
    {
        float timer = 0;
        target.localScale = from;

        while (timer < time)
        {
            timer += Time.deltaTime;
            target.localScale = Vector3.Lerp(from, to, timer / time);
            yield return null;
        }

        target.localScale = to;
    }

    private IEnumerator ExtraScaleSequence()
    {
        yield return ScaleRoutine(extraScaleData.target, extraScaleData.startScale1, extraScaleData.endScale1, extraScaleData.duration1);
        yield return ScaleRoutine(extraScaleData.target, extraScaleData.startScale2, extraScaleData.endScale2, extraScaleData.duration2);
    }

    private IEnumerator LoopFade(LoopFadeData d)
    {
        Color c = d.sprite.color;

        for (int i = 0; i < d.count; i++)
        {
            float t1 = 0;
            while (t1 < d.duration)
            {
                t1 += Time.deltaTime;
                c.a = Mathf.Lerp(d.startAlpha, d.endAlpha, t1 / d.duration);
                d.sprite.color = c;
                yield return null;
            }

            float t2 = 0;
            while (t2 < d.duration)
            {
                t2 += Time.deltaTime;
                c.a = Mathf.Lerp(d.endAlpha, d.startAlpha, t2 / d.duration);
                d.sprite.color = c;
                yield return null;
            }
        }

        c.a = d.startAlpha;
        d.sprite.color = c;
    }

    private IEnumerator MaterialFloat(Material mat, string prop, float from, float to, float duration)
    {
        float timer = 0;
        mat.SetFloat(prop, from);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            mat.SetFloat(prop, Mathf.Lerp(from, to, timer / duration));
            yield return null;
        }

        mat.SetFloat(prop, to);
    }
    private IEnumerator RestoreValues(float duration)
    {
        float timer = 0;

        // 初期値の記録
        Vector3[] startTargets = new Vector3[scaleData.targets.Length];
        for (int i = 0; i < scaleData.targets.Length; i++)
            if (scaleData.targets[i] != null)
                startTargets[i] = scaleData.targets[i].localScale;

        Vector3 bgStart = backgroundData.background != null ? backgroundData.background.localScale : Vector3.one;
        Vector3 extraStart = extraScaleData.target != null ? extraScaleData.target.localScale : Vector3.one;
        float loopAlphaStart = loopFadeData.sprite != null ? loopFadeData.sprite.color.a : 1f;

        float matA = matFadeData.material.GetFloat("_Alpha");
        float matN = matFadeData.material.GetFloat("_NoiseStrength");
        float matB = matFadeData.material.GetFloat("_BlockWidth");

        // ここから戻す
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = restoreCurve.Evaluate(timer / duration);

            for (int i = 0; i < scaleData.targets.Length; i++)
                if (scaleData.targets[i] != null)
                    scaleData.targets[i].localScale = Vector3.Lerp(startTargets[i], scaleData.startScale, t);

            if (backgroundData.background != null)
                backgroundData.background.localScale = Vector3.Lerp(bgStart, backgroundData.startScale, t);

            if (extraScaleData.target != null)
                extraScaleData.target.localScale = Vector3.Lerp(extraStart, extraScaleData.startScale1, t);

            if (loopFadeData.sprite != null)
            {
                Color c = loopFadeData.sprite.color;
                c.a = Mathf.Lerp(loopAlphaStart, loopFadeData.startAlpha, t);
                loopFadeData.sprite.color = c;
            }

            if (matFadeData.material != null)
            {
                matFadeData.material.SetFloat("_Alpha", Mathf.Lerp(matA, matFadeData.alphaFrom, t));
                matFadeData.material.SetFloat("_NoiseStrength", Mathf.Lerp(matN, matFadeData.noiseFrom, t));
                matFadeData.material.SetFloat("_BlockWidth", Mathf.Lerp(matB, matFadeData.blockFrom, t));
            }

            yield return null;
        }

        ResetAllValues();
        playState = PlayState.Idle;
    }

    public void SetPlayState(PlayState state)
    {
        playState = state;
    }
}
