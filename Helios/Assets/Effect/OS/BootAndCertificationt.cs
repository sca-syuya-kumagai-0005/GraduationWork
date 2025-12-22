using UnityEngine;
using System.Collections;

public class BootAndCertificationt : MonoBehaviour
{
    // ==================================================
    // First Flash (Boot)
    // ==================================================
    [Header("=== Boot Flash Material ===")]
    [SerializeField] private Renderer bootRenderer;

    [Header("Boot Alpha")]
    [SerializeField] private float bootStartAlpha = 0f;
    [SerializeField] private float bootEndAlpha = 1f;

    [Header("Boot Emission")]
    [ColorUsage(true, true)]
    [SerializeField] private Color bootStartEmissionColor = Color.black;
    [ColorUsage(true, true)]
    [SerializeField] private Color bootEndEmissionColor = Color.white;

    [SerializeField] private float bootStartEmissionIntensity = 0f;
    [SerializeField] private float bootEndEmissionIntensity = 3f;

    [SerializeField] private float bootFlashDuration = 0.3f;

    // ==================================================
    // Scale
    // ==================================================
    [Header("=== Scale Target ===")]
    [SerializeField] private Transform scaleTarget;
    [SerializeField] private Vector3 scaleStart = Vector3.one * 0.8f;
    [SerializeField] private Vector3 scaleEnd = Vector3.one;
    [SerializeField] private float scaleDuration = 0.25f;

    // ==================================================
    // Target Object Material (After Scale)
    // ==================================================
    [Header("=== Target Object Material ===")]
    [SerializeField] private Renderer targetRenderer;

    [Header("Target Alpha")]
    [SerializeField] private float targetStartAlpha = 0f;
    [SerializeField] private float targetEndAlpha = 1f;

    [Header("Target Emission")]
    [ColorUsage(true, true)]
    [SerializeField] private Color targetStartEmissionColor = Color.black;
    [ColorUsage(true, true)]
    [SerializeField] private Color targetEndEmissionColor = Color.cyan;

    [SerializeField] private float targetStartEmissionIntensity = 0f;
    [SerializeField] private float targetEndEmissionIntensity = 2.5f;

    [SerializeField] private float targetFlashDuration = 0.25f;

    // ==================================================
    // Runtime Materials
    // ==================================================
    private Material bootMat;
    private Material targetMat;

    [SerializeField] private GameObject Step0OBJ;
    [SerializeField] private GameObject Step1OBJ;

    private void Awake()
    {
        if (bootRenderer != null)
        {
            bootMat = bootRenderer.material;
            SetAlpha(bootMat, bootStartAlpha);
            SetEmission(bootMat, bootStartEmissionColor, bootStartEmissionIntensity);
        }

        if (targetRenderer != null)
        {
            targetMat = targetRenderer.material;
            SetAlpha(targetMat, targetStartAlpha);
            SetEmission(targetMat, targetStartEmissionColor, targetStartEmissionIntensity);
        }

        if (scaleTarget != null)
        {
            scaleTarget.localScale = scaleStart;
        }
    }

    private void Start()
    {
        StartCoroutine(Sequence());
    }

    // ==================================================
    // Sequence
    // ==================================================
    private IEnumerator Sequence()
    {
        // Boot Flash
        if (bootMat != null)
            yield return StartCoroutine(FlashMaterial(
                bootMat,
                bootStartAlpha,
                bootEndAlpha,
                bootStartEmissionColor,
                bootEndEmissionColor,
                bootStartEmissionIntensity,
                bootEndEmissionIntensity,
                bootFlashDuration
            ));

        // Scale
        if (scaleTarget != null)
            yield return StartCoroutine(ScaleRoutine());

        // Target Object Material Flash
        if (targetMat != null)
            yield return StartCoroutine(FlashMaterial(
                targetMat,
                targetStartAlpha,
                targetEndAlpha,
                targetStartEmissionColor,
                targetEndEmissionColor,
                targetStartEmissionIntensity,
                targetEndEmissionIntensity,
                targetFlashDuration
            ));
        Step0OBJ.SetActive(false);
        
    }

    // ==================================================
    // Material Flash
    // ==================================================
    private IEnumerator FlashMaterial(
        Material mat,
        float startAlpha,
        float endAlpha,
        Color startEmissionColor,
        Color endEmissionColor,
        float startEmissionIntensity,
        float endEmissionIntensity,
        float duration)
    {
        float t = 0f;

        while (t < duration)
        {
            float rate = t / duration;

            float alpha = Mathf.Lerp(startAlpha, endAlpha, rate);
            float intensity = Mathf.Lerp(startEmissionIntensity, endEmissionIntensity, rate);
            Color emissionColor = Color.Lerp(startEmissionColor, endEmissionColor, rate);

            SetAlpha(mat, alpha);
            SetEmission(mat, emissionColor, intensity);

            t += Time.deltaTime;
            yield return null;
        }

        SetAlpha(mat, endAlpha);
        SetEmission(mat, endEmissionColor, endEmissionIntensity);
    }

    // ==================================================
    // Scale
    // ==================================================
    private IEnumerator ScaleRoutine()
    {
        float t = 0f;

        while (t < scaleDuration)
        {
            scaleTarget.localScale = Vector3.Lerp(scaleStart, scaleEnd, t / scaleDuration);
            t += Time.deltaTime;
            yield return null;
        }

        scaleTarget.localScale = scaleEnd;
    }

    // ==================================================
    // Material Helpers
    // ==================================================
    private void SetAlpha(Material mat, float alpha)
    {
        Color c = mat.GetColor("_BaseColor");
        c.a = alpha;
        mat.SetColor("_BaseColor", c);
    }

    private void SetEmission(Material mat, Color color, float intensity)
    {
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", color * intensity);
    }
}
