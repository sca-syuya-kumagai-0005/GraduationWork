using UnityEngine;
using UnityEngine.InputSystem;
using System.Runtime.InteropServices;
using System.Collections;
public class MouseTest : MonoBehaviour
{
   // float time;//‰½•bŒJ‚è•Ô‚·‚©
    [SerializeField]
    float radius = 3f;

    Vector2 noise = Vector2.zero;
    Vector2 noisePosition=Vector2.zero;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DrunkCorsor());
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private IEnumerator DrunkCorsor()
    {
        while (true)
        {
            noisePosition += new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 0.1f;
            noise = new Vector2(Mathf.PerlinNoise(noisePosition.x, 0f), Mathf.PerlinNoise(noisePosition.y, 0f)) * 2f - Vector2.one;
            Vector3 mousePos = Input.mousePosition;
            Mouse.current.WarpCursorPosition(new Vector2(mousePos.x,mousePos.y) + noise * radius);
            yield return null;
        }
    }
}
