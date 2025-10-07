using UnityEngine;

public class ECGWaveCircleController : MonoBehaviour
{
    public Material waveMaterial;

    public enum WaveState
    {
        Calm,
        SlightStress,
        Stress,
        Death
    }

    [Header("Calm 設定")]
    [ColorUsage(true, true)] public Color calmColor = new Color(0f, 1f, 0f, 1f);
    [ColorUsage(true, true)] public Color calmCenterLineColor = new Color(1f, 1f, 1f, 0.5f);
    float calmSpeed = 0.1f;
    float calmHeight = 0.01f;
    float calmRandomRange = 0.5f;
    float calmWaveWidth = 1.2f;
    float calmWaveCount = 15f;
    float calmLineWidthMin = 3.0f;
    float calmLineWidthMax = 5.0f;
    float calmLineWidthPeriod = 1.0f;

    [Header("Slight Stress 設定")]
    [ColorUsage(true, true)] public Color slightStressColor = new Color(1f, 1f, 0f, 1f);
    [ColorUsage(true, true)] public Color slightStressCenterLineColor = new Color(1f, 1f, 1f, 0.5f);
    float slightStressSpeed = 1.3f;
    float slightStressHeight = 0.1f;
    float slightStressRandomRange = 0.5f;
    float slightStressWaveWidth = 1.2f;
    float slightStressWaveCount = 15f;
    float slightStressLineWidthMin = 3.0f;
    float slightStressLineWidthMax = 5.0f;
    float slightStressLineWidthPeriod = 1.0f;

    [Header("Stress 設定")]
    [ColorUsage(true, true)] public Color stressColor = new Color(1f, 0f, 0f, 1f);
    [ColorUsage(true, true)] public Color stressCenterLineColor = new Color(1f, 1f, 1f, 0.5f);
    float stressSpeed = 2.0f;
    float stressHeight = 0.2f;
    float stressRandomRange = 0.5f;
    float stressWaveWidth = 1.2f;
    float stressWaveCount = 15f;
    float stressLineWidthMin = 5.0f;
    float stressLineWidthMax = 20f;
    float stressLineWidthPeriod = 0.6f;

    [Header("Death 設定")]
    [ColorUsage(true, true)] public Color deathColor = new Color(0.5f, 0f, 0f, 1f);
    [ColorUsage(true, true)] public Color deathCenterLineColor = new Color(1f, 1f, 1f, 0.2f);
    float deathSpeed = 3f;
    float deathHeight = 0.1f;
    float deathRandomRange = 0.1f;
    float deathWaveWidth = 5f;
    float deathWaveCount = 30f;
    float deathLineWidthMin = 10f;
    float deathLineWidthMax = 30f;
    float deathLineWidthPeriod = 0.3f;

    [Header("補間速度")]
    public float transitionSpeed = 2.0f;

    public WaveState targetState = WaveState.Calm;

    // 現在値（補間用）
    private Color currentColor;
    private Color currentCenterLineColor;
    private float currentSpeed;
    private float currentHeight;
    private float currentRandomRange;
    private float currentWaveWidth;
    private float currentWaveCount;
    private float currentLineWidth;

    // 状態ごとの LineWidth パラメータ
    private float targetLineWidthMin;
    private float targetLineWidthMax;
    private float targetLineWidthPeriod;

    void Start()
    {
        SetTargetValues(targetState,
            out currentColor,
            out currentCenterLineColor,
            out currentSpeed,
            out currentHeight,
            out currentRandomRange,
            out currentWaveWidth,
            out currentWaveCount,
            out targetLineWidthMin,
            out targetLineWidthMax,
            out targetLineWidthPeriod);

        currentLineWidth = (targetLineWidthMin + targetLineWidthMax) / 2f;
        ApplyToMaterial(currentColor, currentCenterLineColor, currentSpeed, currentHeight, currentRandomRange, currentWaveWidth, currentWaveCount, currentLineWidth);
    }

    void Update()
    {
        Color targetColor;
        Color targetCenterLineColor;
        float targetSpeed;
        float targetHeight;
        float targetRandomRange;
        float targetWaveWidth;
        float targetWaveCount;

        SetTargetValues(targetState,
            out targetColor,
            out targetCenterLineColor,
            out targetSpeed,
            out targetHeight,
            out targetRandomRange,
            out targetWaveWidth,
            out targetWaveCount,
            out targetLineWidthMin,
            out targetLineWidthMax,
            out targetLineWidthPeriod);

        // 補間
        currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * transitionSpeed);
        currentCenterLineColor = Color.Lerp(currentCenterLineColor, targetCenterLineColor, Time.deltaTime * transitionSpeed);
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * transitionSpeed);
        currentHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * transitionSpeed);
        currentRandomRange = Mathf.Lerp(currentRandomRange, targetRandomRange, Time.deltaTime * transitionSpeed);
        currentWaveWidth = Mathf.Lerp(currentWaveWidth, targetWaveWidth, Time.deltaTime * transitionSpeed);
        currentWaveCount = Mathf.Lerp(currentWaveCount, targetWaveCount, Time.deltaTime * transitionSpeed);

        // LineWidth は時間で周期的に変化させる
        float t = Mathf.PingPong(Time.time / targetLineWidthPeriod, 1f);
        currentLineWidth = Mathf.Lerp(targetLineWidthMin, targetLineWidthMax, t);

        ApplyToMaterial(currentColor, currentCenterLineColor, currentSpeed, currentHeight, currentRandomRange, currentWaveWidth, currentWaveCount, currentLineWidth);
    }

    void SetTargetValues(WaveState state,
        out Color col,
        out Color centerCol,
        out float speed,
        out float height,
        out float randomRange,
        out float waveWidth,
        out float waveCount,
        out float lineWidthMin,
        out float lineWidthMax,
        out float lineWidthPeriod)
    {
        switch (state)
        {
            case WaveState.Calm:
                col = calmColor;
                centerCol = calmCenterLineColor;
                speed = calmSpeed;
                height = calmHeight;
                randomRange = calmRandomRange;
                waveWidth = calmWaveWidth;
                waveCount = calmWaveCount;
                lineWidthMin = calmLineWidthMin;
                lineWidthMax = calmLineWidthMax;
                lineWidthPeriod = calmLineWidthPeriod;
                break;
            case WaveState.SlightStress:
                col = slightStressColor;
                centerCol = slightStressCenterLineColor;
                speed = slightStressSpeed;
                height = slightStressHeight;
                randomRange = slightStressRandomRange;
                waveWidth = slightStressWaveWidth;
                waveCount = slightStressWaveCount;
                lineWidthMin = slightStressLineWidthMin;
                lineWidthMax = slightStressLineWidthMax;
                lineWidthPeriod = slightStressLineWidthPeriod;
                break;
            case WaveState.Stress:
                col = stressColor;
                centerCol = stressCenterLineColor;
                speed = stressSpeed;
                height = stressHeight;
                randomRange = stressRandomRange;
                waveWidth = stressWaveWidth;
                waveCount = stressWaveCount;
                lineWidthMin = stressLineWidthMin;
                lineWidthMax = stressLineWidthMax;
                lineWidthPeriod = stressLineWidthPeriod;
                break;
            case WaveState.Death:
                col = deathColor;
                centerCol = deathCenterLineColor;
                speed = deathSpeed;
                height = deathHeight;
                randomRange = deathRandomRange;
                waveWidth = deathWaveWidth;
                waveCount = deathWaveCount;
                lineWidthMin = deathLineWidthMin;
                lineWidthMax = deathLineWidthMax;
                lineWidthPeriod = deathLineWidthPeriod;
                break;
            default:
                col = calmColor;
                centerCol = calmCenterLineColor;
                speed = calmSpeed;
                height = calmHeight;
                randomRange = calmRandomRange;
                waveWidth = calmWaveWidth;
                waveCount = calmWaveCount;
                lineWidthMin = calmLineWidthMin;
                lineWidthMax = calmLineWidthMax;
                lineWidthPeriod = calmLineWidthPeriod;
                break;
        }
    }

    void ApplyToMaterial(Color col, Color centerCol, float speed, float height, float randomRange, float waveWidth, float waveCount, float lineWidth)
    {
        if (waveMaterial == null)
        {
            Debug.LogWarning("Material not assigned!");
            return;
        }

        waveMaterial.SetColor("_LineColor", col);
        waveMaterial.SetColor("_CenterLineColor", centerCol);
        waveMaterial.SetFloat("_Speed", speed);
        waveMaterial.SetFloat("_WaveHeight", height);
        waveMaterial.SetFloat("_RandomRange", randomRange);
        waveMaterial.SetFloat("_WaveWidth", waveWidth);
        waveMaterial.SetFloat("_WaveCount", waveCount);
        waveMaterial.SetFloat("_LineWidth", lineWidth);
    }

    public void ChangeState(WaveState newState)
    {
        targetState = newState;
    }
}
