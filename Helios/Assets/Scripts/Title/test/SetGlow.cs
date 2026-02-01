using UnityEngine;
using UnityEngine.UI;

public class SetGlow : MonoBehaviour
{
    public Color glowColor = new Color(1.0f, 1.0f, 0.5f); // 発光色
    public float glowIntensity = 2.0f; // 発光の強度

    void Start()
    {
        // マテリアルの発光色を設定
        RawImage image = GetComponent<RawImage>();
        image.material.SetTexture("_MainTex", image.texture);
        image.material.SetColor("_Color", glowColor * glowIntensity);
    }
}
