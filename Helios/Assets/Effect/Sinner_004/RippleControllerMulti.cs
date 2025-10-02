using UnityEngine;

public class RippleControllerMulti : MonoBehaviour
{
    public Material rippleMaterial;
    public int maxWaves = 10;

    private Vector4[] wavePositions;
    private float[] waveStartTimes;
    private int activeCount = 0;

    void Start()
    {
        wavePositions = new Vector4[maxWaves];
        waveStartTimes = new float[maxWaves];
    }

    void Update()
    {
        // マウスクリックで波追加
        if (Input.GetMouseButtonDown(0))
        {
            AddRipple(Input.mousePosition);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddRipple(new Vector2(Screen.width / 2f, Screen.height / 2f));
        }

        // 毎フレームシェーダーに反映
        rippleMaterial.SetInt("_ActiveWaveCount", activeCount);
        rippleMaterial.SetVectorArray("_WaveStartPos", wavePositions);
        rippleMaterial.SetFloatArray("_WaveStartTime", waveStartTimes);
    }

    void AddRipple(Vector2 screenPos)
    {
        if (activeCount >= maxWaves) activeCount = 0; // 循環利用

        Vector2 uv = new Vector2(screenPos.x / Screen.width, screenPos.y / Screen.height);

        uv = new Vector2(1f - uv.x, 1f - uv.y);
        wavePositions[activeCount] = new Vector4(uv.x, uv.y, 0, 0);
        waveStartTimes[activeCount] = Time.time;

        activeCount++;
    }
}
