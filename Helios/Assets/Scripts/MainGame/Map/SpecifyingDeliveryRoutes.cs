using UnityEngine;
using System.Collections.Generic;
using System.Collections;

//これは配達者につけるscriptです。
public class SpecifyingDeliveryRoutes : Map
{
    [SerializeField] List<int[]> routes = new List<int[]>();
    [SerializeField] List<Vector3> routesPosition = new List<Vector3>();
    [SerializeField] List<GameObject> passedObjects = new List<GameObject>();
    [SerializeField] GameObject move;
    [SerializeField] bool memorying = false;
    public bool Memorying { get { return memorying; } }
    GameObject deliveryItem;
    public GameObject DeliveryItem{set{ deliveryItem = value; }}
    GameObject driver;
    [SerializeField]float speed;
    LineRenderer line;
    [SerializeField] float distance;
    [SerializeField]int coroutineNumber;
    int lastRoutesPositionCount;
    [SerializeField]int frame = 0;
    [SerializeField]bool writing;
    [SerializeField] bool dliverSet = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coroutineNumber = 0;
        lastRoutesPositionCount = 0;
        driver = this.gameObject;
        writing = false;
        memorying = false;
        dliverSet = false;
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        frame++;
        if (lastRoutesPositionCount != routesPosition.Count&&routesPosition.Count>1)
        {
            Debug.Log("追加で呼びます");
            coroutineNumber++;
            StartCoroutine(Move(routesPosition[routesPosition.Count-2], routesPosition[routesPosition.Count - 1], coroutineNumber, frame));
        }
        lastRoutesPositionCount = routesPosition.Count; 
        if(memorying&&Input.GetMouseButtonDown(1))
        {
            routes.Clear();
            routesPosition.Clear();
            passedObjects.Clear();
            memorying = false;
            line.positionCount=0;
        }
    }

    public void MemoryRoute(int widthPositionID,int heightPositionID,int objectID,GameObject obj,Vector3 position)
    {
        if(!memorying||!writing)return;
        int[] positionID = new int[2];
        positionID[0] = widthPositionID;
        positionID[1] = heightPositionID;
        if (objectID == 0&&routes.Count==0)
        {
            Debug.Log("routes.Count="+routes.Count);
            routes.Add(positionID);
            routesPosition.Add(position);
            passedObjects.Add(obj);
            line.positionCount++;
            line.SetPosition(line.positionCount-1,position);
        }
        if (NearCheck(routes,positionID))//なぞったオブジェクトが前のオブジェクトと隣接しているなら
        {
            routes.Add(positionID);
            routesPosition.Add(position);
            passedObjects.Add(obj);
            line.positionCount++;
            line.SetPosition(line.positionCount - 1, position);
        }
       
    }

    public void MemoryStart()
    {
        if(!writing||!dliverSet) { return;}
        memorying = true;
        StartCoroutine(Directions());
    }

    public void MemoryEnd(int widthPositionID, int heightPositionID, int objectID)
    {
        
        int[] positionID = new int[2];
        positionID[0] = widthPositionID;
        positionID[1] = heightPositionID;
        if (!NearCheck(routes,positionID))
        {
            return;
        }
        memorying = false;
        StartCoroutine(DriverMove());
    }

    private bool NearCheck(List<int[]> list, int[] positionID)
    {
        Debug.Log(positionID.Length);
        Debug.Log(list.Count);
        return Mathf.Abs(list[list.Count - 1][0] - positionID[0]) <= 1 && Mathf.Abs(list[list.Count - 1][1] - positionID[1]) <= 1 && Mathf.Abs(list[list.Count - 1][1] - positionID[1])!= Mathf.Abs(list[list.Count - 1][0] - positionID[0]);
    }

    private IEnumerator DriverMove()
    {
        for(int i=0;i<routesPosition.Count;i++)
        {
            float dist = Mathf.Abs(routesPosition[i].magnitude - driver.transform.position.magnitude);
            while (dist>0.05f)
            {
                Vector3 dir = (routesPosition[i] - driver.transform.position).normalized;
                Vector2 vec = driver.transform.position+dir*Time.deltaTime;
                dist = Mathf.Abs(routesPosition[i].magnitude - driver.transform.position.magnitude);
                driver.transform.position = vec*speed;
                yield return null;
            }
            driver.transform.position = routesPosition[i];
        }
        yield return new WaitForSeconds(2f);
        for (int i = routesPosition.Count-1; i >=0; i--)
        {
           
            float dist = Mathf.Abs(routesPosition[i].magnitude - driver.transform.position.magnitude);
            while (dist > 0.05f)
            {
                Vector3 dir = (routesPosition[i] - driver.transform.position).normalized;
                Vector2 vec = driver.transform.position + dir * Time.deltaTime;
                dist = Mathf.Abs(routesPosition[i].magnitude - driver.transform.position.magnitude);
                driver.transform.position = vec*speed;
                yield return null;
            }
            driver.transform.position = routesPosition[i];
        }
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Arrow");
        foreach(GameObject obj in objs)
        {
            Destroy(obj);
        }

        routes = new List<int[]>();
        routesPosition = new List<Vector3>();
        line.positionCount=0;
        yield return null;

    }


    IEnumerator Directions()
    {
        yield return null;
        if (routesPosition.Count > 1)
        {
            for (int i = 0; i<routesPosition.Count - 1; i++)
            {
                coroutineNumber++;
                frame=0;
                StartCoroutine(Move(routesPosition[i], routesPosition[i + 1],coroutineNumber,frame));
            }
        }
        else
        {
            StartCoroutine(Directions());
        }
       
    }
    
    IEnumerator Move(Vector3 startPosition,Vector3 endPosition, int coroutineID, int frameCount)
    {
        float lastDist = 0;
        GameObject obj = Instantiate(move, startPosition, Quaternion.identity);
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
            if (routesPosition.Count == 0||obj==null)
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
                lastDist=dist;
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


    public void DliverSeting()
    {
        if (!writing)
        {
            dliverSet = false;
            return;
        }
        dliverSet = !dliverSet;
    }

}
