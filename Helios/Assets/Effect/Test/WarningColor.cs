// File name: WarningColor.cs
using UnityEngine;

public enum WarningColorType
{
    Custom,
    Red,
    Yellow,
    YellowGreen,
    Purple
}

[System.Serializable]
public struct ColorPreset
{
    public WarningColorType type;
    [ColorUsage(true, true)] public Color color; // HDR対応
}

public class WarningColor : MonoBehaviour
{
    [Header("Color Presets")]
    public ColorPreset[] presets;

    [Header("Custom HDR Color")]
    [ColorUsage(true, true)]
    public Color customColor = Color.white;

    [Header("Target Materials (Project内のマテリアル)")]
    public Material[] materials;

    [Header("Select Color")]
    public WarningColorType colorType = WarningColorType.Custom;

    [Header("Color Change Settings")]
    public float transitionTime = 2f; // 色が切り替わるまでの時間

    private Color currentColor;
    private Color nextColor;
    private float timer = 0f;

    private void Start()
    {
        currentColor = GetColor();
        nextColor = GetNextColor();
        ApplyColor(currentColor);
    }

    private void Update()
    {
        if (materials == null || materials.Length == 0) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / transitionTime);
        Color c = Color.Lerp(currentColor, nextColor, t);

        ApplyColor(c);

        if (t >= 1f)
        {
            timer = 0f;
            currentColor = nextColor;
            nextColor = GetNextColor();
        }
    }
    public Color GetColor()
    {
        if (colorType == WarningColorType.Custom)
            return customColor;

        foreach (var preset in presets)
        {
            if (preset.type == colorType)
                return preset.color;
        }
        return Color.white;
    }

    private Color GetNextColor()
    {
        WarningColorType nextType;

        switch (colorType)
        {
            case WarningColorType.Red:
                nextType = WarningColorType.Red;
                break;
            case WarningColorType.Yellow:
                nextType = WarningColorType.Yellow;
                break;
            case WarningColorType.YellowGreen:
                nextType = WarningColorType.YellowGreen;
                break;
            case WarningColorType.Purple:
                nextType = WarningColorType.Purple;
                break;
            default:
                nextType = WarningColorType.Custom;
                break;
        }

        // Enumに対応するColorを取得
        foreach (var preset in presets)
        {
            if (preset.type == nextType)
                return preset.color;
        }

        // 見つからなければ白
        return Color.white;
    }



    private void ApplyColor(Color c)
    {
        foreach (var mat in materials)
        {
            if (mat == null) continue;

            if (mat.HasProperty("_MainColor"))
                mat.SetColor("_MainColor", c);

            if (mat.HasProperty("_TextColor"))
                mat.SetColor("_TextColor", c);

            if (mat.HasProperty("_Color1"))
                mat.SetColor("_Color1", c);
        }
    }


}

