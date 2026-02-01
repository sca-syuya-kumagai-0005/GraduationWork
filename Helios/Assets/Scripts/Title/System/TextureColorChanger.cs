using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TextureColorChanger : MonoBehaviour
{
    [SerializeField] RawImage targetRawImage;
    Texture2D texture;
    [SerializeField] Color32 color;

    private void Start()
    {
        if (targetRawImage.texture != null)
        {
            Texture2D mainTex = targetRawImage.texture as Texture2D;
            texture = new Texture2D(mainTex.width, mainTex.height);
            texture.SetPixels(mainTex.GetPixels());
            texture.Apply();
            targetRawImage.texture = texture;
        }
        Set();
    }

    [Button]
    public void Set()
    {
        Debug.Log("start");
        var pixelData = texture.GetPixelData<Color32>(0);
        for (int i = 0; i < pixelData.Length; i++)
        {
            if (pixelData[i].a > 0)
            {
                pixelData[i] = new Color32(color.r, color.g, color.b, pixelData[i].a);
            }
        }
        texture.Apply();
    }
}
