using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEditor.Experimental.GraphView;

//これは配達者につけるscriptです。
public class SpecifyingDeliveryRoutes : Map
{
    const int driverCount = 3;
    List<int[]>[] routes = new List<int[]>[driverCount];
    List<Vector3>[] routesPosition = new List<Vector3>[driverCount];
    List<GameObject>[] passedObjects = new List<GameObject>[driverCount];
    [SerializeField] GameObject move;
    [SerializeField] bool memorying = false;
    public bool Memorying { get { return memorying; } }
    int deliveryItem;
    public int DeliveryItem{set{ deliveryItem = value; }}
    [SerializeField]GameObject[] driver;
    [SerializeField]float speed;
    LineRenderer[] line = new LineRenderer[driverCount];
    [SerializeField] float distance;
    [SerializeField]int coroutineNumber;
    int lastRoutesPositionCount;
    [SerializeField] int frame = 0;
    [SerializeField] bool writing;
    [SerializeField] bool driverSet = false;
    [SerializeField] int driverType;
    [SerializeField] int lastdriverType;
    IEnumerator ienum ;
    List<Coroutine> coroutine=new List<Coroutine>();
    bool arrowDelete;
    public int DriverType { set { driverType = value;} }
    [SerializeField]bool[] delivering = new bool[driverCount];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        arrowDelete = false;
        ienum = Directions();
        for(int i=0;i<driverCount;i++)
        {
            routes[i]=new List<int[]>();
            routesPosition[i] = new List<Vector3>();
            passedObjects[i] = new List<GameObject>();
        }
        coroutineNumber = 0;
        lastRoutesPositionCount = 0;
        writing = false;
        memorying = false;
        driverSet = false;
        for(int i=0;i<driver.Length;i++)
        {
            line[i] = driver[i].GetComponent<LineRenderer>();
        }
      
        for(int i = 0;i<delivering.Length;i++)
        {
            delivering[i] = false;  
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(lastdriverType!=driverType)
        {
            lastRoutesPositionCount = 0;
            memorying=false;
        }
        lastdriverType = driverType;
        frame++;
        //Debug.Log(routesPosition[driverType][routesPosition[driverType].Count]);
        if (lastRoutesPositionCount != routesPosition[driverType].Count&&routesPosition[driverType].Count>1)
        {
            coroutineNumber++;
            coroutine.Clear();
            coroutine.Add(StartCoroutine(ArrowMove(routesPosition[driverType][routesPosition[driverType].Count - 2], routesPosition[driverType][routesPosition[driverType].Count - 1], coroutineNumber, frame)));

        }
        lastRoutesPositionCount = routesPosition[driverType].Count;
        if (Input.GetMouseButtonDown(1) && routes[driverType].Count>0)
        {
            Debug.Log("count" + routesPosition[driverType][routesPosition[driverType].Count-1]);
            arrowDelete = true;
            routes[driverType].RemoveAt(routes[driverType].Count-1);
            routesPosition[driverType].RemoveAt(routesPosition[driverType].Count - 1);
            passedObjects[driverType].RemoveAt(passedObjects[driverType].Count - 1);
            GameObject[] objs = GameObject.FindGameObjectsWithTag("Arrow");
            line[driverType].positionCount--;
            coroutineNumber = 0;
            foreach (GameObject o in objs)
            {
                Destroy(o);
            }
            for (int i = 0; i < routesPosition[driverType].Count - 1; i++)
            {
                coroutineNumber++;
                coroutine.Clear();
                coroutine.Add(StartCoroutine(ArrowMove(routesPosition[driverType][i], routesPosition[driverType][i + 1], coroutineNumber, frame)));
                
            }
          
        }
    }

    public void MemoryRoute(int widthPositionID,int heightPositionID,int objectID,GameObject obj,Vector3 position)
    {
        if (!Input.GetMouseButton(0)) return;
        if(!memorying||!writing)return;
        int[] positionID = new int[2];//xとzで二つ
        positionID[0] = widthPositionID;
        positionID[1] = heightPositionID;
        if (objectID == 0 && routes[driverType].Count==0)
        {
            routes[driverType].Add(positionID);
            routesPosition[driverType].Add(position);
            passedObjects[driverType].Add(obj);
            line[driverType].positionCount++;
            line[driverType].SetPosition(line[driverType].positionCount-1,position);
        }
        if (NearCheck(routes[driverType],positionID))//なぞったオブジェクトが前のオブジェクトと隣接しているなら
        {
            routes[driverType].Add(positionID);
            routesPosition[driverType].Add(position);
            passedObjects[driverType].Add(obj);
            line[driverType].positionCount++;
            line[driverType].SetPosition(line[driverType].positionCount - 1, position);
        }
       
    }

    public void MemoryStart()
    {
        if(!writing||!driverSet) { return;}
        memorying = true;
        StartCoroutine(Directions());
    }

    public void MemoryEnd()
    {
        
        //int[] positionID = new int[2];
        //positionID[0] = widthPositionID;
        //positionID[1] = heightPositionID;
        //if (!NearCheck(routes,positionID))
        //{
        //    return;
        //}
          
    }

    public void StartDriver()
    {
        Debug.Log("start");
        StartCoroutine(DriverMove(driverType));//後でdriverTypeを引き数として渡す
    }

    private bool NearCheck(List<int[]> list, int[] positionID)
    {
        return Mathf.Abs(list[list.Count - 1][0] - positionID[0]) <= 1 && Mathf.Abs(list[list.Count - 1][1] - positionID[1]) <= 1 && Mathf.Abs(list[list.Count - 1][1] - positionID[1])!= Mathf.Abs(list[list.Count - 1][0] - positionID[0]);
    }

    private IEnumerator DriverMove(int objID)
    {
        GameObject obj = driver[objID];
        for (int i=0;i<routesPosition[driverType].Count;i++)
        {
            float dist = Mathf.Abs(routesPosition[driverType][i].magnitude - obj.transform.position.magnitude);
            while (dist>0.05f)
            {
                Vector3 dir = (routesPosition[driverType][i] - obj.transform.position).normalized;
                Vector2 vec = obj.transform.position+dir*Time.deltaTime;
                dist = Mathf.Abs(routesPosition[driverType][i].magnitude - obj.transform.position.magnitude);
                obj.transform.position = vec*speed;
                yield return null;
            }
            obj.transform.position = routesPosition[driverType][i];
        }
        yield return new WaitForSeconds(2f);
        for (int i = routesPosition[driverType].Count-1; i >=0; i--)
        {
           
            float dist = Mathf.Abs(routesPosition[driverType][i].magnitude - obj.transform.position.magnitude);
            while (dist > 0.05f)
            {
                Vector3 dir = (routesPosition[driverType][i]- obj.transform.position).normalized;
                Vector2 vec = obj.transform.position + dir * Time.deltaTime;
                dist = Mathf.Abs(routesPosition[driverType][i].magnitude - obj.transform.position.magnitude);
                obj.transform.position = vec*speed;
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
        memorying = false;
        writing = false;
        driverSet = false;
        yield return null;

    }


    IEnumerator Directions()
    {
        yield return null;
        if (routesPosition[driverType].Count > 1)
        {
            for (int i = 0; i<routesPosition[driverType].Count - 1; i++)
            {
                coroutineNumber++;
                frame=0;
                coroutine.Clear();
                coroutine.Add(StartCoroutine(ArrowMove(routesPosition[driverType][i], routesPosition[driverType][i + 1], coroutineNumber,frame)));
            }
        }
        else
        {
            StartCoroutine(Directions());
        }
       
    }
    
    IEnumerator ArrowMove(Vector3 startPosition,Vector3 endPosition, int coroutineID, int frameCount)
    {
        GameObject obj = Instantiate(move, startPosition,Quaternion.identity);
        float dist = Mathf.Abs(endPosition.magnitude - obj.transform.position.magnitude);
        for (int i = 0; i < frameCount; i++)
        {
            Vector3 dir = (endPosition - obj.transform.position).normalized;
            Vector2 vec = obj.transform.position + dir * Time.deltaTime;
            dist = Mathf.Abs(endPosition.magnitude - obj.transform.position.magnitude);
            obj.transform.position = vec * speed;
        }
        int lastX=0;
        int lastY=0;
        while (dist>0.001f)
        {
                if (routesPosition[driverType].Count == 0||obj==null)
                {
                    break;
                }
                if (arrowDelete)
                {
                    arrowDelete = false;    
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
                obj.transform.position = vec * speed;

                yield return null;
        }
        Destroy(obj);
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Arrow");
        if (coroutineID == 1)
        {
            coroutineNumber = 0;
            StartCoroutine(Directions());
        }
        foreach (GameObject arrow in objs)
        {
            Destroy(arrow);
        }
        if (obj != null) Destroy(obj);
        
    }
    public void Writing()
    {
        writing = !writing;
    }


    public void DriverSetting(int driverType)
    {
        if (!writing)
        {
            driverSet = false;
            return;
        }
        driverSet = !driverSet;
    }

}
