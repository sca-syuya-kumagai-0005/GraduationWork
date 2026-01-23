using UnityEngine;
using UnityEngine.InputSystem;
using System.Runtime.InteropServices;
using System.Collections;
public class MouseNoise : MonoBehaviour
{
    [SerializeField]
    float radius = 3f;

    Vector2 noise = Vector2.zero;
    Vector2 noisePosition = Vector2.zero;

    private bool isNoise;
    public bool IsNoise { set { isNoise = value; } }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isNoise = false;
        StartCoroutine(DrunkCorsor());
    }
    private IEnumerator DrunkCorsor()
    {
        while (true)
        {
            if (isNoise)
            {
                noisePosition += new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 0.1f;
                noise = new Vector2(Mathf.PerlinNoise(noisePosition.x, 0f), Mathf.PerlinNoise(0f, noisePosition.y)) * 2f - Vector2.one;
                Vector3 mousePos = Input.mousePosition;
                Mouse.current.WarpCursorPosition(new Vector2(mousePos.x, mousePos.y) + noise * radius);
                yield return null;
            }
            yield return null;
        }
    }
}
