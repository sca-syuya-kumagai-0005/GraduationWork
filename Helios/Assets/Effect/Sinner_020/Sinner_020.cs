using UnityEngine;
using System.Collections;

public class Sinner_020 : MonoBehaviour
{
    [Header("=== MagicCircle Scale (1st) ===")]
    [SerializeField] private Transform magicCircle;
    [SerializeField] private Vector3 magicCircleStartScale = Vector3.one * 0.5f;
    [SerializeField] private Vector3 magicCircleEndScale = Vector3.one * 1.5f;
    [SerializeField] private float magicCircleDuration = 1.5f;

    [Header("=== MagicCircle Scale (2nd) ===")]
    [SerializeField] private Vector3 magicCircleStartScale2 = Vector3.one * 0.5f;
    [SerializeField] private Vector3 magicCircleEndScale2 = Vector3.one * 1.5f;
    [SerializeField] private float magicCircleDuration2 = 1.5f;

    [Header("=== Stone Scale Settings ===")]
    [SerializeField] private Transform stone;
    [SerializeField] private Vector3 stoneStartScale = Vector3.one * 0.7f;
    [SerializeField] private Vector3 stoneEndScale = Vector3.one * 2.0f;
    [SerializeField] private float stoneDuration = 2.0f;

    [Header("=== Stone Scale Down Settings ===")]
    [SerializeField] private Vector3 stoneScaleDownStart = Vector3.one * 2.0f;
    [SerializeField] private Vector3 stoneScaleDownEnd = Vector3.one * 0.7f;
    [SerializeField] private float stoneDownDuration = 2.0f;

    [Header("=== MagicCircle Rotation (Curve) ===")]
    [SerializeField] private float rotationSpeedStart = 100f;
    [SerializeField] private float rotationSpeedEnd = 800f;
    [SerializeField] private float rotationChangeDuration = 3f;
    [SerializeField] private AnimationCurve rotationCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [Header("=== Bomb ===")]
    [SerializeField] private GameObject bombEffect;

    private float currentRotationSpeed;

    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioClip sinner_020;

    private void OnEnable()
    {
        // èâä˙âªÅiÉäÉZÉbÉgÅj
        if (magicCircle != null) magicCircle.localScale = magicCircleStartScale;
        if (stone != null) stone.localScale = stoneStartScale;
        currentRotationSpeed = rotationSpeedStart;
        bombEffect.SetActive(false);

        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
        audioManager.PlaySE(sinner_020);

        // ÉAÉjÉÅÅ[ÉVÉáÉìäJén
        StartCoroutine(FlowProcess());
    }


    IEnumerator FlowProcess()
    {
        //--á@ Stone ägëÂ
        StartCoroutine(ScaleUp(stone, stoneStartScale, stoneEndScale, stoneDuration));

        //--áA MagicCircle ägëÂ
        yield return StartCoroutine(ScaleUp(magicCircle, magicCircleStartScale, magicCircleEndScale, magicCircleDuration));

        //--áB âÒì]äJén & ë¨ìxè„è∏Curve
        StartCoroutine(ChangeRotationSpeedWithCurve());
        StartCoroutine(RotateMagicCircleLoop());

        yield return new WaitForSeconds(2f);

        //--áC îöî≠ + Stoneèkè¨äJén
        StartCoroutine(Bomb());
        StartCoroutine(ScaleDown(stone, stoneScaleDownStart, stoneScaleDownEnd, stoneDownDuration));

        yield return new WaitForSeconds(1f);

        //--áD MagicCircle Ç‡Ç§àÍìxägëÂ
        yield return StartCoroutine(ScaleUp(magicCircle, magicCircleStartScale2, magicCircleEndScale2, magicCircleDuration2));
        gameObject.SetActive(false);
    }


    IEnumerator Bomb()
    {
        bombEffect.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        bombEffect.SetActive(false);
    }


    IEnumerator ScaleUp(Transform obj, Vector3 from, Vector3 to, float duration)
    {
        float t = 0;
        obj.localScale = from;
        while (t < 1)
        {
            t += Time.deltaTime / duration;
            obj.localScale = Vector3.Lerp(from, to, t);
            yield return null;
        }
    }

    IEnumerator ScaleDown(Transform obj, Vector3 from, Vector3 to, float duration)
    {
        float t = 0;
        obj.localScale = from;
        while (t < 1)
        {
            t += Time.deltaTime / duration;
            obj.localScale = Vector3.Lerp(from, to, t);
            yield return null;
        }
    }

    IEnumerator ChangeRotationSpeedWithCurve()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / rotationChangeDuration;
            currentRotationSpeed = Mathf.Lerp(rotationSpeedStart, rotationSpeedEnd, rotationCurve.Evaluate(t));
            yield return null;
        }
    }

    IEnumerator RotateMagicCircleLoop()
    {
        while (true)
        {
            magicCircle.Rotate(0, 0, currentRotationSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
