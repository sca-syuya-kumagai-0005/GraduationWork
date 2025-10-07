﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using static KumagaiLibrary.String;

//これは配達を管理するScriptです
public class SpecifyingDeliveryRoutes : Map
{
    const int driverCount = 3;
    List<int>[] routeObjectsID=new List<int>[driverCount];
    List<int[]>[] routes = new List<int[]>[driverCount];
    List<Vector3>[] routesPosition = new List<Vector3>[driverCount];
    List<GameObject>[] passedObjects = new List<GameObject>[driverCount];
    [SerializeField] GameObject move;
    [SerializeField]int[] deliveryItems;
    [SerializeField]GameObject[] driver;
    [SerializeField]float[] speed;
    private float interfersenceSpeed;
    public float InterfersenceSpeed { set {  interfersenceSpeed = value; } }
    LineRenderer[] line = new LineRenderer[driverCount];
    [SerializeField] float distance;
    int[] coroutineNumber=new int[driverCount];
    int[] lastRoutesPositionCount = new int[driverCount];
    int frame = 0;
    [SerializeField] bool writing;
    public bool Writing { get {  return writing; }  }
    [SerializeField]bool driverSet = false;
    public bool DriverSet {  get { return driverSet; } }
    [SerializeField] int driverType;
    int lastdriverType;
    public int DriverType { set { driverType = value;} }
    [SerializeField]bool[] isDriving = new bool[driverCount];
    private int[] deliveryProcess=new int[driverCount];
   
    [SerializeField]private GameObject[] destination=new GameObject[driverCount];
    [SerializeField] private bool[] canStart = new bool[driverCount];
    [SerializeField]bool[] isProcessSetting = new bool[driverCount];
    [SerializeField]bool[] isItemSetting = new bool[driverCount];
    [SerializeField] bool[] isDestinationSetting = new bool[driverCount];

    [SerializeField] GameObject writeButton;
    SpriteRenderer writeButtonRenderer;
    [SerializeField] Sprite[] writeSprite;
    [SerializeField] GameObject[] driverSetButton;
    SpriteRenderer[] driverSetButtonRenderer = new SpriteRenderer[driverCount];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        writeButtonRenderer=writeButton.GetComponent<SpriteRenderer>();
        for(int i=0;i<driverCount;i++)
        {
            driverSetButtonRenderer[i] = driverSetButton[i].GetComponent<SpriteRenderer>();
            routes[i]=new List<int[]>();
            routesPosition[i] = new List<Vector3>();
            routeObjectsID[i] = new List<int>();
            passedObjects[i] = new List<GameObject>();
            coroutineNumber[i] = 0;
            lastRoutesPositionCount[i] = 0;
            canStart[i] = false;
            Directions(i);
        }


        writing = false;
        driverSet = false;
        for(int i=0;i<driver.Length;i++)
        {
            line[i] = driver[i].GetComponent<LineRenderer>();
        }
      
        for(int i = 0;i<isDriving.Length;i++)
        {
            isDriving[i] = false;  
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (writing) writeButtonRenderer.sprite = writeSprite[0];
        else writeButtonRenderer.sprite = writeSprite[1];
        if(lastdriverType!=driverType)writing = false;
        for(int i=0;i<driverCount;i++)
        {
            if (!driverSet) break;
            if(i==driverType)driverSetButtonRenderer[i].color = Color.green;
            else driverSetButtonRenderer[i].color = Color.white;
        }
       

        lastdriverType = driverType;
        frame++;
        //Debug.Log(routesPosition[driverType][routesPosition[driverType].Count]);

        for (int i = 0; i < driverCount; i++)
        {
            canStart[i] = isItemSetting[i]&&isProcessSetting[i]&&isDestinationSetting[i];
            if (lastRoutesPositionCount[i] != routesPosition[i].Count && routesPosition[i].Count > 1)
            {
                coroutineNumber[i]++;
                StartCoroutine(ArrowMove(routesPosition[i][routesPosition[i].Count - 2], routesPosition[i][routesPosition[i].Count - 1], coroutineNumber[i], frame, i));
            }
            lastRoutesPositionCount[i] = routesPosition[i].Count;
        }
       
        if (Input.GetMouseButtonDown(1) && routes[driverType].Count>0)
        {
            if (!isDriving[driverType])
            {
                routes[driverType].RemoveAt(routes[driverType].Count - 1);
                routesPosition[driverType].RemoveAt(routesPosition[driverType].Count - 1);
                passedObjects[driverType].RemoveAt(passedObjects[driverType].Count - 1);
                routeObjectsID[driverType].RemoveAt(routeObjectsID[driverType].Count-1);
                GameObject[] objs = GameObject.FindGameObjectsWithTag("Arrow");
                line[driverType].positionCount--;
                coroutineNumber[driverType] = 0;
                foreach (GameObject o in objs)
                {
                    Destroy(o);
                }
                for (int d = 0; d < driverCount; d++)
                {
                    for (int i = 0; i < routesPosition[d].Count - 1; i++)
                    {
                        coroutineNumber[d]++;
                        StartCoroutine(ArrowMove(routesPosition[d][i], routesPosition[d][i + 1], coroutineNumber[d], frame, d));
                    }

                }
            }
            
        }

    }

    public void MemoryRoute(int widthPositionID,int heightPositionID,int objectID,GameObject obj,Vector3 position)
    {
        if (!Input.GetMouseButton(0)) return;
        if(!writing||!driverSet)return;
        int routeObjectsIDCount= routeObjectsID[driverType].Count;
        Debug.Log(ColorChanger(routeObjectsIDCount.ToString(),"red"));
        if(routeObjectsIDCount > 0)
        {
            if (routeObjectsID[driverType][routeObjectsIDCount - 1] == 3) return;
        }
       
        int[] positionID = new int[2];//xとzで二つ
        positionID[0] = widthPositionID;
        positionID[1] = heightPositionID;
        Debug.Log(ColorChanger("関数MemoryRouteが呼び出されました", "red"));
        if (objectID == 0 && routes[driverType].Count==0)
        {
            Debug.Log(ColorChanger("開始地点を追加しました","red"));
            routes[driverType].Add(positionID);
            routeObjectsID[driverType].Add(objectID);
            routesPosition[driverType].Add(position);
            passedObjects[driverType].Add(obj);
            line[driverType].positionCount++;
            line[driverType].SetPosition(line[driverType].positionCount-1,position);
        }
        if (NearCheck(routes[driverType],positionID))//なぞったオブジェクトが前のオブジェクトと隣接しているなら
        {
            routes[driverType].Add(positionID);
            routeObjectsID[driverType].Add(objectID);
            routesPosition[driverType].Add(position);
            passedObjects[driverType].Add(obj);
            line[driverType].positionCount++;
            line[driverType].SetPosition(line[driverType].positionCount - 1, position);
        }
        if(objectID==3)
        {
            writing = false;
        }
    }

    public void MemoryStart()
    {
        if(!writing||!driverSet) { return;}
        StartCoroutine(Directions(driverType)); 
       
    }

    public void StartDriver(int driverID)
    {
        Debug.Log("スタート" + canStart[driverID]);
        if (!canStart[driverID]) return;
        if (routeObjectsID[driverType][routeObjectsID[driverType].Count-1]!=3)return;
        if (canStart[driverType])
        {
            Debug.Log(ColorChanger("運転を開始します", "red"));
            isDriving[driverType] = true;
            StartCoroutine(DriverMove(driverType));
        }
        isItemSetting[driverType] = false;
        isProcessSetting[driverType] = false;
        isDestinationSetting[driverType] = false;
    }

    private bool NearCheck(List<int[]> list, int[] positionID)
    {
        return Mathf.Abs(list[list.Count - 1][0] - positionID[0]) <= 1 && Mathf.Abs(list[list.Count - 1][1] - positionID[1]) <= 1 && Mathf.Abs(list[list.Count - 1][1] - positionID[1])!= Mathf.Abs(list[list.Count - 1][0] - positionID[0]);
    }

    private IEnumerator DriverMove(int driverType)
    {
        GameObject obj = driver[driverType];
        for (int i=1;i<routesPosition[driverType].Count;i++)
        {
           
            Vector3 dirction = (routesPosition[driverType][i] - obj.transform.position).normalized;
            Vector3 lastDirction = dirction;
            while (lastDirction==dirction)
            {
                lastDirction = dirction;
                Vector3 vec = lastDirction*Time.deltaTime;
                Debug.Log(ColorChanger("加算値は" + vec / speed[driverType] + "です", "red"));
                obj.transform.position += vec/speed[driverType];
                dirction = (routesPosition[driverType][i] - obj.transform.position).normalized;
                yield return null;
            }
            obj.transform.position = routesPosition[driverType][i];
        }
        DeliveryCompleted(destination[driverType],driverType);
        yield return new WaitForSeconds(2f);
        for (int i = routesPosition[driverType].Count-2; i >=0; i--)
        {
            Vector3 dirction = (routesPosition[driverType][i] - obj.transform.position).normalized;
            Vector3 lastDirction = dirction;
            while (lastDirction==dirction)
            {
                Debug.Log(ColorChanger("移動しています", "red"));
                lastDirction = dirction;
                Vector3 vec = lastDirction*Time.deltaTime;
                Debug.Log(ColorChanger("加算値は" + vec/speed[driverType] + "です", "red"));
                obj.transform.position += vec / speed[driverType];
                dirction = (routesPosition[driverType][i] - obj.transform.position).normalized;
                yield return null;
            }
            obj.transform.position = routesPosition[driverType][i];
        }
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Arrow");
        foreach(GameObject o in objs)
        {
            Destroy(o);
        }
        routes[driverType] = new List<int[]>();
        routesPosition[driverType] = new List<Vector3>();
        line[driverType].positionCount=0;
        routeObjectsID[driverType]=new List<int>();
        writing = false;
        yield return null;

    }


    IEnumerator Directions(int driver)
    {
        yield return null;
        if (routesPosition[driver].Count > 1)
        {
            for (int i = 0; i<routesPosition[driver].Count - 1; i++)
            {
                coroutineNumber[driver]++;
                frame=0;
                StartCoroutine(ArrowMove(routesPosition[driver][i], routesPosition[driver][i + 1], coroutineNumber[driver], frame, driver));
            }
        }
        else
        {
            StartCoroutine(Directions(driver));
        }
    }
    
    IEnumerator ArrowMove(Vector3 startPosition,Vector3 endPosition, int coroutineID, int frameCount,int driver)
    {
        GameObject obj = Instantiate(move, startPosition,Quaternion.identity);
        float dist = Mathf.Abs(endPosition.magnitude - obj.transform.position.magnitude);
        for (int i = 0; i < frameCount; i++)
        {
            Vector3 dir = (endPosition - obj.transform.position).normalized;
            Vector2 vec = obj.transform.position + dir * Time.deltaTime;
            dist = Mathf.Abs(endPosition.magnitude - obj.transform.position.magnitude);
            obj.transform.position = vec;
        }
        int lastX=0;
        int lastY=0;
        while (dist>0.001f)
        {
                if (routesPosition[driver].Count == 0||obj==null)
                {
                    break;
                }
                Vector3 dir = (endPosition - obj.transform.position).normalized;
                if (dir.x == 1)
                {
                    if(lastX==-1)break;
                    lastX = 1;
                    obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                }
                if (dir.x == -1)
                {
                    if(lastX==1)break;
                    lastX=-1;
                    obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                }
                if (dir.y == 1)
                {
                    if(lastY==-1)break;
                    lastY=1;
                    obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                }
                if (dir.y == -1)
                {
                    if(lastY==1)break;
                    lastY=-1;
                    obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                }
                Vector2 vec = obj.transform.position + dir * Time.deltaTime;
                dist = Mathf.Abs(endPosition.magnitude - obj.transform.position.magnitude);
                obj.transform.position = vec;
                yield return null;
        }
        Destroy(obj);
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Arrow");
        if (coroutineID == 1)
        {
            coroutineNumber[driver] = 0;
            StartCoroutine(Directions(driver));
        }
        foreach (GameObject arrow in objs)
        {
            Destroy(arrow);
        }
        if (obj != null) Destroy(obj);
        
    }
    public void WritingSwitch()
    {
        writing = !writing;
        Debug.Log(ColorChanger("関数WritingSwitchが呼ばれました。writingは" + writing + "です。", "red"));
    }


    public void DriverSetting(int driver)
    {
        Debug.Log("DriverSetting");
        //if (!writing)
        //{
        //    driverSet = false;
        //    return;
        //}
        driverType =driver;
        driverSet = true;
    }

    public void DeliveryItemSetting(int deliveryItem) 
    {
        deliveryItems[driverType]=deliveryItem;
        isItemSetting[driverType] = true; 
    }

    public void DeliveryProcessSetting(int deliveryProcessID)
    {
        deliveryProcess[driverType]=deliveryProcessID;
        isProcessSetting[driverType]=true;
    }

    private void DeliveryCompleted(GameObject obj,int driverType)
    {
        Debug.Log(obj);
        obj.GetComponent<Sinner>().ReceiveDeliveryItem(deliveryItems[driverType]);
        //↑設計の都合上Setterから関数に変えたので勝手に変更しました
    }
    public void DestinationSetting(GameObject obj)
    {
        destination[driverType]=obj;
        isDestinationSetting[driverType] = true;
    }
}
