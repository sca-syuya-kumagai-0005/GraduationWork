using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CelestialSteed : MonoBehaviour
{
    private GameObject[] plot = new GameObject[9];
    private SpriteRenderer spriteRenderer;
    private Sprite iconSprite;
    public Sprite SetSprite { set { iconSprite = value; } }
    private SpecifyingDeliveryRoutes specifyingDeliveryRoutes;
    private GameObject[] drivers = new GameObject[4];
    private PointBlink point;
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
        GameObject go = GameObject.Find("Drivers").gameObject;
        specifyingDeliveryRoutes = go.GetComponent<SpecifyingDeliveryRoutes>();
        for (int i = 0; i < drivers.Length; i++)
        {
            drivers[i] = go.transform.GetChild(i).gameObject;
        }
        point = transform.GetChild(3).GetComponent<PointBlink>();
        StartCoroutine(Blink());
    }

    private void Update()
    {
        for (int i = 0; i < drivers.Length; i++)
        {
            if (drivers[i].activeSelf)
            {
                Vector2 pos;
                pos = drivers[i].transform.position;
                float distX = pos.x - transform.position.x;
                float distY = pos.y - transform.position.y;
                float distance = Mathf.Sqrt(distX * distX + distY * distY);
            }
        }
    }

    private IEnumerator Blink()
    {
        int counter = 5;
        while (true)
        {
            counter++;
            bool IsShockWave = counter % 10 == 0;
            StartCoroutine(point.Blink(IsShockWave));
            if (IsShockWave) Warp(0);
            yield return new WaitForSeconds(1.0f);
        }
    }

    private void Warp(int plotNumber)
    {
        gameObject.transform.parent = plot[plotNumber].transform;
        gameObject.transform.localPosition = new Vector3(31.0f, -53.0f, 0.0f);
        ShockWave();
    }

    private void ShockWave()
    {
        for(int i = 0; i < drivers.Length; i++)
        {
            if (drivers[i].activeSelf)
            {
                Vector2 pos;
                pos = drivers[i].transform.position;
                float distX = pos.x - transform.position.x;
                float distY= pos.y - transform.position.y;
                float distance = Mathf.Sqrt(distX * distX + distY * distY);
                if (distance <= 5.0f)
                {
                    specifyingDeliveryRoutes.Breaking[i] = true;
                }
            }
        }
    }
}
