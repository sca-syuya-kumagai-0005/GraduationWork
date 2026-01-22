using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CelestialSteed : MonoBehaviour
{
    private float timer;
    private float timeLimit;
    private GameObject effectObject;
    public GameObject SetEffectObject { set { effectObject = value; } }
    private GameObject[] plot = new GameObject[9];
    private SpriteRenderer spriteRenderer;
    private Sprite iconSprite;
    public Sprite SetSprite { set { iconSprite = value; } }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new Vector3(1000, 1000, 0);
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = iconSprite;
        for (int i = 0; i < plot.Length; i++)
        {
            plot[i] = GameObject.Find("Address_" + i);
        }
        timeLimit = 10.0f;
        timer = timeLimit;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > timeLimit)
        {
            timer -= timeLimit;
            StartCoroutine(Warp(0));
        }
    }
     
    private IEnumerator Warp(int plotNumber)
    {
        float waitTime = 1.0f;
        yield return new WaitForSeconds(waitTime);
        gameObject.transform.parent = plot[plotNumber].transform;
        gameObject.transform.localPosition = new Vector3(31.0f, -53.0f, 0.0f);
        Debug.Log("è’åÇîgÉ@!");
    }
}
