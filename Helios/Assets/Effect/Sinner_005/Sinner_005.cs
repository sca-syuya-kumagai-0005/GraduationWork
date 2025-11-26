using System.Collections;
using UnityEngine;

public class Sinner_005 : MonoBehaviour
{
    [Header("=== 移動設定 ===")]
    public Vector3 moveTargetPosition = new Vector3(0f, -3f, 0f);
    public float moveDuration = 2f;
    public AnimationCurve moveEase = AnimationCurve.Linear(0, 0, 1, 1);
    public float moveDelay = 0f;

    [Header("=== 時計の針 回転設定 ===")]
    public Transform clockHand;
    public float handRotationDelta = 3600f;
    public float handRotationDuration = 3f;
    public AnimationCurve handEase = AnimationCurve.Linear(0, 0, 1, 1);
    public float handDelay = 0f;

    [Header("=== 自身回転設定 ===")]
    public bool rotateSelf = true;
    public float selfRotationDelta = 3600f;
    public float selfRotationDuration = 3f;
    public AnimationCurve selfEase = AnimationCurve.Linear(0, 0, 1, 1);
    public float selfDelay = 0f;

    [Header("=== 夜アイコン フェード設定 ===")]
    public SpriteRenderer[] nightIcons;
    public float fadeDuration = 3f;
    public float fadeDelay = 0f;
    public Transform[] iconsTransform;

    [Header("=== 上昇設定 ===")]
    public Vector3 upTargetOffset = new Vector3(0, 2f, 0);
    public float upDuration = 2f;
    public AnimationCurve upEase = AnimationCurve.Linear(0, 0, 1, 1);
    public float upDelay = 0f;

    [Header("=== BlackFade Material ===")]
    public Material fadeMaterial;

    [System.Serializable]
    public class FadeSettings
    {
        public string propertyName = "_Fade";
        public float start = 0f;
        public float end = 1f;
        public float duration = 2f;
        public float delay = 0f;
    }

    public FadeSettings featherForward = new FadeSettings { propertyName = "_Feather", start = 0.1f, end = 0.8f, duration = 2f };
    public FadeSettings fadeForward = new FadeSettings { propertyName = "_Fade", start = 0f, end = 1f, duration = 2f };
    public FadeSettings featherReverse = new FadeSettings { propertyName = "_Feather", start = 0.1f, end = 0.8f, duration = 2f };
    public FadeSettings fadeReverse = new FadeSettings { propertyName = "_Fade", start = 0f, end = 1f, duration = 2f };

    private Coroutine sequenceCoroutine;

    private void OnEnable()
    {
        // 既存のコルーチンがあれば停止
        if (sequenceCoroutine != null)
            StopCoroutine(sequenceCoroutine);

        // 初期化
        ResetState();

        // 再生
        sequenceCoroutine = StartCoroutine(Sequence());
    }

    private void ResetState()
    {
        // Transform 初期化（y = 6.8 固定、x/z は現在位置を維持）
        transform.position = new Vector3(transform.position.x, 6.8f, transform.position.z);
        transform.rotation = Quaternion.identity;

        for(int i = 0; i < iconsTransform.Length; i++ )
        {
            iconsTransform[i].rotation = Quaternion.identity;
        }

        // ClockHand 初期化
        if (clockHand != null)
            clockHand.rotation = Quaternion.identity;

        // SpriteRenderer 初期化
        if (nightIcons != null)
        {
            foreach (var sr in nightIcons)
            {
                if (sr != null)
                {
                    Color c = sr.color;
                    sr.color = new Color(c.r, c.g, c.b, 1f); // フェード前はアルファ1
                }
            }
        }

        // Material 初期化
        if (fadeMaterial != null)
        {
            fadeMaterial.SetFloat(featherForward.propertyName, featherForward.start);
            fadeMaterial.SetFloat(fadeForward.propertyName, fadeForward.start);
        }
    }


    private IEnumerator Sequence()
    {
        // ================== 下落 ==================
        if (moveDelay > 0f) yield return new WaitForSeconds(moveDelay);
        Vector3 start = new Vector3(transform.position.x, 6.8f, transform.position.z);
        transform.position = start; // 開始位置にセット
        Coroutine moveDown = StartCoroutine(MoveCoroutineDown(moveTargetPosition, moveDuration, moveEase));
        StartCoroutine(AnimateProperty(featherForward, true));
        StartCoroutine(AnimateProperty(fadeForward, true));

        yield return moveDown; // 下落が終わるまで待機

        // ================== 針回転 + 夜アイコンフェード + 自身回転 ==================
        Coroutine handCoroutine = null;
        if (clockHand != null)
            handCoroutine = StartCoroutine(StartAfterDelay(handDelay, RotateClockHand(clockHand, handRotationDelta, handRotationDuration, handEase)));

        Coroutine selfCoroutine = null;
        if (rotateSelf)
            selfCoroutine = StartCoroutine(StartAfterDelay(selfDelay, RotateWithChildren(transform, selfRotationDelta, selfRotationDuration, selfEase)));

        Coroutine[] iconCoroutines = new Coroutine[nightIcons.Length];
        for (int i = 0; i < nightIcons.Length; i++)
        {
            if (nightIcons[i] != null)
                iconCoroutines[i] = StartCoroutine(StartAfterDelay(fadeDelay, FadeSprite(nightIcons[i], fadeDuration)));
        }

        // 全部終わるまで待機
        if (handCoroutine != null) yield return handCoroutine;
        if (selfCoroutine != null) yield return selfCoroutine;
        foreach (var c in iconCoroutines)
            if (c != null) yield return c;

        // ================== 上昇 ==================
        if (upDelay > 0f) yield return new WaitForSeconds(upDelay);

        StartCoroutine(AnimateProperty(featherReverse, false));
        Coroutine fade = StartCoroutine(AnimateProperty(fadeReverse, false));
        start = new Vector3(transform.position.x, 0f, transform.position.z);
        transform.position = start; // 開始位置にセット
        yield return StartCoroutine(MoveCoroutineUp(transform.position + upTargetOffset, upDuration, upEase));
        yield return fade;
        yield return new WaitForSeconds(0.25f);
        if (transform.parent != null)
        {
            transform.parent.gameObject.SetActive(false);
        }

    }

    private IEnumerator StartAfterDelay(float delay, IEnumerator coroutine)
    {
        if (delay > 0f) yield return new WaitForSeconds(delay);
        yield return StartCoroutine(coroutine);
    }

    private IEnumerator MoveCoroutineDown(Vector3 target, float duration, AnimationCurve ease)
    {
        Vector3 start = new Vector3(transform.position.x, 6.8f, transform.position.z);
        transform.position = start; // 開始位置にセット

        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            transform.position = Vector3.LerpUnclamped(start, target, ease.Evaluate(t));
            yield return null;
        }

        transform.position = target;
    }

    private IEnumerator MoveCoroutineUp(Vector3 target, float duration, AnimationCurve ease)
    {
        Vector3 start = new Vector3(transform.position.x, 1f, transform.position.z);
        transform.position = start; // 開始位置にセット

        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            transform.position = Vector3.LerpUnclamped(start, target, ease.Evaluate(t));
            yield return null;
        }

        transform.position = target;
        
    }


    private IEnumerator RotateClockHand(Transform target, float deltaZ, float duration, AnimationCurve ease)
    {
        float startZ = target.eulerAngles.z;
        float endZ = startZ + deltaZ;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            float z = Mathf.LerpUnclamped(startZ, endZ, ease.Evaluate(t));
            target.rotation = Quaternion.Euler(0, 0, z);
            yield return null;
        }

        target.rotation = Quaternion.Euler(0, 0, endZ);
    }

    private IEnumerator RotateWithChildren(Transform target, float deltaZ, float duration, AnimationCurve ease)
    {
        Quaternion start = target.rotation;
        Quaternion end = start * Quaternion.Euler(0, 0, deltaZ);
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            target.rotation = Quaternion.LerpUnclamped(start, end, ease.Evaluate(t));

            for (int i = 0; i < target.childCount; i++)
            {
                if (i == 5) continue;
                Transform child = target.GetChild(i);
                if (child == null) continue;
                float zOffset = (i == 0) ? -135f : -90f;
                child.rotation = target.rotation * Quaternion.Euler(0, 0, zOffset);
            }
            yield return null;
        }
        target.rotation = end;
    }

    private IEnumerator FadeSprite(SpriteRenderer sr, float duration)
    {
        Color start = sr.color;
        Color end = new Color(start.r, start.g, start.b, 0f);
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            sr.color = Color.Lerp(start, end, Mathf.Clamp01(time / duration));
            yield return null;
        }
        sr.color = end;
    }

    private IEnumerator AnimateProperty(FadeSettings settings, bool forward)
    {
        if (settings.delay > 0f) yield return new WaitForSeconds(settings.delay);

        float from = forward ? settings.start : settings.end;
        float to = forward ? settings.end : settings.start;
        float time = 0f;

        while (time < settings.duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / settings.duration);
            fadeMaterial.SetFloat(settings.propertyName, Mathf.Lerp(from, to, t));
            yield return null;
        }
        fadeMaterial.SetFloat(settings.propertyName, to);

        
    }
}
