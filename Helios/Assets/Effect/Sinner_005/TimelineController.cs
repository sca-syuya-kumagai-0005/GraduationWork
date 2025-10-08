using System.Collections;
using UnityEngine;

public class TimelineController : MonoBehaviour
{
    [Header("=== �ړ��ݒ� ===")]
    public Vector3 moveTargetPosition = new Vector3(0f, -3f, 0f);
    public float moveDuration = 2f;
    public AnimationCurve moveEase = AnimationCurve.Linear(0, 0, 1, 1);
    public float moveDelay = 0f;

    [Header("=== ���v�̐j ��]�ݒ� ===")]
    public Transform clockHand;
    public float handRotationDelta = 3600f;
    public float handRotationDuration = 3f;
    public AnimationCurve handEase = AnimationCurve.Linear(0, 0, 1, 1);
    public float handDelay = 0f;

    [Header("=== ���g��]�ݒ� ===")]
    public bool rotateSelf = true;
    public float selfRotationDelta = 3600f;
    public float selfRotationDuration = 3f;
    public AnimationCurve selfEase = AnimationCurve.Linear(0, 0, 1, 1);
    public float selfDelay = 0f;

    [Header("=== ��A�C�R�� �t�F�[�h�ݒ� ===")]
    public SpriteRenderer[] nightIcons;
    public float fadeDuration = 3f;
    public float fadeDelay = 0f;

    [Header("=== �㏸�ݒ� ===")]
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

    private void Start()
    {
        StartCoroutine(Sequence());
    }

    private IEnumerator Sequence()
    {
        // ================== ���� ==================
        if (moveDelay > 0f) yield return new WaitForSeconds(moveDelay);

        Coroutine moveDown = StartCoroutine(MoveCoroutine(moveTargetPosition, moveDuration, moveEase));
        StartCoroutine(AnimateProperty(featherForward, true));
        StartCoroutine(AnimateProperty(fadeForward, true));

        yield return moveDown; // �������I���܂őҋ@

        // ================== �j��] + ��A�C�R���t�F�[�h + ���g��] ==================

        // �j
        Coroutine handCoroutine = null;
        if (clockHand != null)
            handCoroutine = StartCoroutine(StartAfterDelay(handDelay, RotateClockHand(clockHand, handRotationDelta, handRotationDuration, handEase)));

        // ���g
        Coroutine selfCoroutine = null;
        if (rotateSelf)
            selfCoroutine = StartCoroutine(StartAfterDelay(selfDelay, RotateWithChildren(transform, selfRotationDelta, selfRotationDuration, selfEase)));

        // ��A�C�R��
        Coroutine[] iconCoroutines = new Coroutine[nightIcons.Length];
        for (int i = 0; i < nightIcons.Length; i++)
        {
            if (nightIcons[i] != null)
                iconCoroutines[i] = StartCoroutine(StartAfterDelay(fadeDelay, FadeSprite(nightIcons[i], fadeDuration)));
        }

        // �S���I���܂őҋ@
        if (handCoroutine != null) yield return handCoroutine;
        if (selfCoroutine != null) yield return selfCoroutine;
        foreach (var c in iconCoroutines)
            if (c != null) yield return c;

        // ================== �㏸ ==================
        if (upDelay > 0f) yield return new WaitForSeconds(upDelay);

        StartCoroutine(AnimateProperty(featherReverse, false));
        StartCoroutine(AnimateProperty(fadeReverse, false));
        yield return StartCoroutine(MoveCoroutine(transform.position + upTargetOffset, upDuration, upEase));
    }

    // -------- �w���p�[: �x�����Ă���J�n --------
    private IEnumerator StartAfterDelay(float delay, IEnumerator coroutine)
    {
        if (delay > 0f) yield return new WaitForSeconds(delay);
        yield return StartCoroutine(coroutine);
    }

    // -------- �ړ� --------
    private IEnumerator MoveCoroutine(Vector3 target, float duration, AnimationCurve ease)
    {
        Vector3 start = transform.position;
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

    // -------- ���v�j��] --------
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

    // -------- ���g��] + �q�I�u�W�F�N�g --------
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

    // -------- �X�v���C�g�t�F�[�h --------
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

    // -------- BlackFade --------
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
