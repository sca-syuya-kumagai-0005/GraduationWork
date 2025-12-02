using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BarberPole2D : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 1f;
    private Material mat;

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        // sharedMaterial を使うと Clone は作られない（元のマテリアルを直接操作）
        mat = sr.sharedMaterial;
        mat.SetFloat("_Speed", scrollSpeed);
    }
}