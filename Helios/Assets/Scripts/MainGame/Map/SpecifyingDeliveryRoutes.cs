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
    bool memorying = false;
    public bool Memorying { get { return memorying; } }
    GameObject deliveryItem;
    public GameObject DeliveryItem{set{ deliveryItem = value; }}
    GameObject driver;
    [SerializeField]float speed;
    LineRenderer line;
    [SerializeField] float distance;
    IEnumerator coroutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coroutine = DirectionsCreate();
        driver = this.gameObject;
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
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
        
        if(!memorying)return;
        int[] positionID = new int[2];
        positionID[0] = widthPositionID;
        positionID[1] = heightPositionID;
        if (objectID == 0&&routes.Count==0)
        {
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
        memorying = true;
        StartCoroutine(coroutine);
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
        return Mathf.Abs(routes[routes.Count - 1][0] - positionID[0]) <= 1 && Mathf.Abs(routes[routes.Count - 1][1] - positionID[1]) <= 1 && Mathf.Abs(routes[routes.Count - 1][1] - positionID[1])!= Mathf.Abs(routes[routes.Count - 1][0] - positionID[0]);
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
            StopCoroutine(coroutine);
            Destroy(obj);
        }

        routes = new List<int[]>();
        routesPosition = new List<Vector3>();
        line.positionCount=0;
        yield return null;

    }


    IEnumerator Directions()
    {
        if(routesPosition.Count>0)
        {
            GameObject obj = Instantiate(move, routesPosition[0], Quaternion.identity);
            for (int i = 0; i < routesPosition.Count; i++)
            {
                float dist = Mathf.Abs(routesPosition[i].magnitude - obj.transform.position.magnitude);
                while (dist > 0.005f)
                {
                    if(routesPosition.Count==0)
                    {
                        break;
                    }
                    Vector3 dir = (routesPosition[i] - obj.transform.position).normalized;
                    if (dir.x == 1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                    }
                    if (dir.x == -1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                    }
                    if (dir.y == 1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                    }
                    if (dir.y == -1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    }
                    Vector2 vec = obj.transform.position + dir * Time.deltaTime;
                    dist = Mathf.Abs(routesPosition[i].magnitude - obj.transform.position.magnitude);
                    obj.transform.position = vec * speed;
                    yield return null;
                }
                if (routesPosition.Count>i) obj.transform.position = routesPosition[i];
            }
            if(obj!=null)Destroy(obj);
        }
        
    }

    IEnumerator DirectionsCreate()
    {
        while(true)
        {
            yield return new WaitForSeconds(distance);
            if(routesPosition.Count>0)StartCoroutine(Directions());
        }
    }


}
