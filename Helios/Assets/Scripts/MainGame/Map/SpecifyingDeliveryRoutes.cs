using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using static KumagaiLibrary.String;

//これは配達を管理するScriptです
public class SpecifyingDeliveryRoutes : Map
{
    const int driverCount = 4;
    [SerializeField] GameObject map;
    List<int>[] routeObjectsID=new List<int>[driverCount];
    List<int[]>[] routes = new List<int[]>[driverCount];
    List<Vector3>[] routesPosition = new List<Vector3>[driverCount];
    List<GameObject>[] passedObjects = new List<GameObject>[driverCount];

    [SerializeField] GameObject move;
    [SerializeField] GameObject arrows;

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
    bool[] isDriving = new bool[driverCount];
    private int[] deliveryProcess=new int[driverCount];
    [SerializeField]private int tmpDeliveryProcess;
   
    private GameObject[] destination=new GameObject[driverCount];
    private GameObject tmpDestination;
    bool[] canStart = new bool[driverCount];
    bool[] isProcessSetting = new bool[driverCount];
    bool tmpProcessSetting;
    int[] deliveryItems = new int[driverCount];
    [SerializeField] int tmpDeliveryItem;
    bool[] isItemSetting = new bool[driverCount];
    bool tmpItemSetting;
    bool[] isDestinationSetting = new bool[driverCount];
    bool tmpDestinationSetting;

    

    [SerializeField] GameObject writeButton;
    SpriteRenderer writeButtonRenderer;
    [SerializeField] Sprite[] writeSprite;
    [SerializeField] GameObject[] driverSetButton;
    SpriteRenderer[] driverSetButtonRenderer = new SpriteRenderer[driverCount];
    [SerializeField] GameObject[] startButtons;

    [SerializeField] GameObject[] lineObject;

    [SerializeField]Texture2D cursor;
    [SerializeField] float cursorX;
    [SerializeField] float cursorY;

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

        driverType = -1;
        tmpDeliveryItem = -1;
        tmpDeliveryProcess = -1;

        writing = false;
        driverSet = false;
        for(int i=0;i<driver.Length;i++)
        {
            line[i] = lineObject[i].GetComponent<LineRenderer>();
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
            //if (!driverSet) break;
            if(i==driverType)driverSetButtonRenderer[i].color = Color.green;
            else driverSetButtonRenderer[i].color = Color.white;
            canStart[i] = isDestinationSetting[i] && isItemSetting[i]&& isItemSetting[i];
            if (routeObjectsID[i].Count > 0)
            {
                if (routeObjectsID[i][routeObjectsID[i].Count - 1] == 3)
                {
                    startButtons[i].SetActive(true);
                }
                if (isDriving[i])
                {
                    startButtons[i].SetActive(false);
                }
            }
            else
            {
                startButtons[i].SetActive(false);
            }
           
        }

        if (writing)
        {
            Cursor.SetCursor(cursor, new Vector2(cursorX,cursorY), CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        lastdriverType = driverType;
        frame++;
        //Debug.Log(routesPosition[driverType][routesPosition[driverType].Count]);

        for (int i = 0; i < driverCount; i++)
        {
            if (lastRoutesPositionCount[i] != routesPosition[i].Count && routesPosition[i].Count > 1)
            {
                coroutineNumber[i]++;
                StartCoroutine(ArrowMove(routesPosition[i][routesPosition[i].Count - 2], routesPosition[i][routesPosition[i].Count - 1], coroutineNumber[i], frame, i));
            }
            lastRoutesPositionCount[i] = routesPosition[i].Count;
        }
       
        if(driverType>=0)
        {
            if (Input.GetMouseButtonDown(1) && routes[driverType].Count > 0)
            {
                if (!isDriving[driverType])
                {
                    routes[driverType].RemoveAt(routes[driverType].Count - 1);
                    routesPosition[driverType].RemoveAt(routesPosition[driverType].Count - 1);
                    passedObjects[driverType].RemoveAt(passedObjects[driverType].Count - 1);
                    routeObjectsID[driverType].RemoveAt(routeObjectsID[driverType].Count - 1);
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

    }

    public void MemoryRoute(int widthPositionID,int heightPositionID,int objectID,GameObject obj,Vector3 position)
    {
        if (!Input.GetMouseButton(0)) return;
        if(!writing||!driverSet)return;
        if (driverType == -1) return;
        int[] positionID = new int[2];//xとzで二つ
        int routeObjectsIDCount= routeObjectsID[driverType].Count;
        Debug.Log(ColorChanger(routeObjectsIDCount.ToString(),"red"));
        if(routeObjectsIDCount > 0)
        {
            if (routeObjectsID[driverType][routeObjectsIDCount - 1] == 3) return;
        }
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
            if (objectID == 3)
            {
                driverType = -1;
                writing = false;
            }
        }
        
    }

    public void MemoryStart()
    {
        if(!writing||!driverSet) { return;}
        StartCoroutine(Directions(driverType)); 
       
    }

    public void StartDriver(int driverID)
    {
        Debug.Log("押せてはいる");
        if (routeObjectsID[driverID].Count==0) return;
        if (routeObjectsID[driverID][routeObjectsID[driverID].Count - 1] == 3) 
        { 
            Debug.Log(ColorChanger("運転を開始します", "red"));
            isDriving[driverID] = true;
            StartCoroutine(DriverMove(driverID));
            isItemSetting[driverID] = false;
            isProcessSetting[driverID] = false;
            isDestinationSetting[driverID] = false;
        }
       
    }

    private bool NearCheck(List<int[]> list, int[] positionID)
    {
        return Mathf.Abs(list[list.Count - 1][0] - positionID[0]) <= 1 && Mathf.Abs(list[list.Count - 1][1] - positionID[1]) <= 1 && Mathf.Abs(list[list.Count - 1][1] - positionID[1])!= Mathf.Abs(list[list.Count - 1][0] - positionID[0]);
    }

    private IEnumerator DriverMove(int driverID)
    {
        GameObject obj = driver[driverID];
        for (int i=1;i<routesPosition[driverID].Count;i++)
        {
           
            Vector3 dirction = ((routesPosition[driverID][i]+map.transform.localPosition) - obj.transform.position).normalized;
            Vector3 lastDirction = dirction;
            while (lastDirction==dirction)
            {
                lastDirction = dirction;
                Vector3 vec = lastDirction*Time.deltaTime;
                switch(deliveryProcess[driverID])
                {
                    case 0:
                        {
                            speed[driverID] = 0.5f;
                        }
                    break;
                    case 1:
                        {
                            speed[driverID] = 1f;
                        }
                        break;
                    case 2:
                        {
                            speed[driverID] = 2f;
                        }
                        break;
                }
                obj.transform.position += vec/speed[driverID];
                dirction = ((routesPosition[driverID][i] + map.transform.localPosition) - obj.transform.position ).normalized;
                yield return null;
            }
            obj.transform.position = routesPosition[driverID][i] + map.transform.localPosition;
        }
        DeliveryCompleted(destination[driverID],driverID);
        yield return new WaitForSeconds(2f);
        for (int i = routesPosition[driverID].Count-2; i >=0; i--)
        {
            Vector3 dirction = ((routesPosition[driverID][i] + map.transform.localPosition) - obj.transform.position).normalized;
            Vector3 lastDirction = dirction;
            while (lastDirction==dirction)
            {
                Debug.Log(ColorChanger("移動しています", "red"));
                lastDirction = dirction;
                Vector3 vec = lastDirction*Time.deltaTime;
                Debug.Log(ColorChanger("加算値は" + vec/speed[driverID] + "です", "red"));
                obj.transform.position += vec / speed[driverID];
                dirction = ((routesPosition[driverID][i] + map.transform.localPosition) - obj.transform.position).normalized;
                yield return null;
            }
            obj.transform.position = routesPosition[driverID][i]+map.transform.localPosition;
        }
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Arrow");
        foreach(GameObject o in objs)
        {
            Destroy(o);
        }
        routes[driverID] = new List<int[]>();
        routesPosition[driverID] = new List<Vector3>();
        line[driverID].positionCount=0;
        routeObjectsID[driverID]=new List<int>();
        isItemSetting[driverID] = false;
        isProcessSetting[driverID] = false;
        isDriving[driverID] = false;
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
        startPosition += map.transform.localPosition;
        
        GameObject obj = Instantiate(move, startPosition,Quaternion.identity);
        obj.transform.parent = arrows.transform;
        float dist = Mathf.Abs(endPosition.magnitude - obj.transform.position.magnitude);
        for (int i = 0; i < frameCount; i++)
        {
            Vector3 dir = (endPosition + map.transform.localPosition - obj.transform.position).normalized;
            Vector2 vec = obj.transform.position + dir * Time.deltaTime;
            dist = Mathf.Abs((endPosition+ map.transform.localPosition) .magnitude - obj.transform.position.magnitude);
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
                Vector3 dir = (endPosition+ map.transform.localPosition - obj.transform.position).normalized;
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
                dist = Mathf.Abs((endPosition+ map.transform.localPosition ).magnitude - obj.transform.position.magnitude);
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
        if (driverType == -1) return;
        writing = !writing;
        Debug.Log(ColorChanger("関数WritingSwitchが呼ばれました。writingは" + writing + "です。", "red"));
    }


    public void DriverSetting(int driverID)
    {
        if (tmpDeliveryItem == -1 || tmpDeliveryProcess == -1) return;
        if (isDriving[driverID]) return;
        Debug.Log("DriverSetting");
        isDestinationSetting[driverID] = tmpDestinationSetting;
        destination[driverID] = tmpDestination;
        isItemSetting[driverID] = tmpItemSetting;
        isProcessSetting[driverID] = tmpProcessSetting;
        //if (!writing)
        //{
        //    driverSet = false;
        //    return;
        //}
        Debug.Log(deliveryItems[driverID]);
        deliveryItems[driverID] = tmpDeliveryItem;
        deliveryProcess[driverID] = tmpDeliveryProcess;
        driverType = driverID;
        driverSet = true;
        tmpDeliveryProcess = -1;
        tmpDeliveryItem = -1;
      
    }

    public void DeliveryItemSetting(int deliveryItem) 
    {
        tmpDeliveryItem=deliveryItem;

        tmpItemSetting = true; 
    }

    public void DeliveryProcessSetting(int deliveryProcessID)
    {
        tmpDeliveryProcess=deliveryProcessID;
        tmpProcessSetting = true;
    }

    private void DeliveryCompleted(GameObject obj,int driverType)
    {
        Debug.Log(obj);
        obj.GetComponent<Sinner>().ReceiveDeliveryItem(deliveryItems[driverType]);
        //↑設計の都合上Setterから関数に変えたので勝手に変更しました
    }
    public void DestinationSetting(GameObject obj)
    {
        tmpDestination=obj;
        tmpDestinationSetting= true;
    }
}
