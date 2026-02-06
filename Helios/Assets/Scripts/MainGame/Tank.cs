using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static Map;

public class Tank : MonoBehaviour
{
    Map map;
    Map.MapData md;
    GameObject tile;
    const float speed = 1.0f;
    string startPosition;
    public string StartPosition {  get { return startPosition; } set { startPosition = value; } }
    private SpriteRenderer spriteRenderer;
    private Sprite iconSprite;
    public Sprite SetSprite { set { iconSprite = value; } }
    private PointBlink point;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        map = GameObject.Find("Map").gameObject.GetComponent<Map>();
        string[] strings = startPosition.Split("_");
        md = map.MapDatas[int.Parse(strings[2])][int.Parse(strings[1])];
        Debug.Log(md.obj.name);
        StartCoroutine(TankMover());

        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = iconSprite;
        point = transform.GetChild(3).GetComponent<PointBlink>();
        StartCoroutine(Blink());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator Blink()
    {
        while (true)
        {
            StartCoroutine(point.Blink(false));
            yield return new WaitForSeconds(1.0f);
        }
    }
    IEnumerator TankMover()
    {
     
        while(true)
        {
            bool dirSetted = false;
            MapData randomMd = new MapData();
            while (!dirSetted)
            {
                yield return null;
                Debug.Log("Tank1"+md.obj.name);
                string[] randomData = { "TOP", "RIGHT", "LEFT", "BOTTOM" };
                string dirction = randomData[Random.Range(0, randomData.Length)];
                switch (dirction)
                {
                    case "TOP":
                        {
                            if (map.MapDatas[md.heightPositionID - 1][md.widthPositionID].objectID > 0)
                            {
                                randomMd = map.MapDatas[md.heightPositionID - 1][md.widthPositionID];
                                dirSetted = true;
                            }
                            break;
                        }
                    case "RIGHT":
                        {
                            if (map.MapDatas[md.heightPositionID][md.widthPositionID + 1].objectID > 0)
                            {
                                randomMd = map.MapDatas[md.heightPositionID][md.widthPositionID + 1];
                                dirSetted = true;
                            }
                            break;
                        }
                    case "LEFT":
                        {
                            if (map.MapDatas[md.heightPositionID][md.widthPositionID - 1].objectID > 0)
                            {
                                randomMd = map.MapDatas[md.heightPositionID][md.widthPositionID - 1];
                                dirSetted = true;
                            }
                            break;
                        }
                    case "BOTTOM":
                        {
                            if (map.MapDatas[md.heightPositionID + 1][md.widthPositionID].objectID > 0)
                            {
                                randomMd = map.MapDatas[md.heightPositionID + 1][md.widthPositionID];
                                dirSetted = true;
                            }
                            break;
                        }

                }

            }

            md = randomMd;
            Vector3 endPos = md.obj.transform.localPosition;

            //float elapsed = 0f;

            Vector3 dir = (endPos + map.transform.localPosition - this.transform.position).normalized;
            Vector3 lastDirction = dir;
            Vector3 vec = lastDirction * Time.deltaTime;
            Debug.Log(md.obj.name);
            while (lastDirction == dir)
            {

                this.transform.position += vec / speed;
                dir = ((endPos + map.transform.localPosition) - this.transform.position).normalized;
                yield return null;
            }
            this.transform.position = endPos + map.transform.localPosition;
            yield return null;
        }
      
    }
}
