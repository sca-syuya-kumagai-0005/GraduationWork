using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

using static Map;
using static KumagaiLibrary.String;




//これは配達を管理するScriptです
public class SpecifyingDeliveryRoutes : MonoBehaviour
{
    [SerializeField]Sprite[] driverSprite;
    private float clownSpeed=1;
    public float ClownSpeed { set { clownSpeed = value; } }
    private float swordSpeed = 1;
    public float SwordSpeed { set { swordSpeed = value; } }   
    const int driverCount = 4;//トラックの数
    bool[] breaking = new bool[driverCount];
    public bool[] Breaking { get { return breaking; } set { breaking = value; } }
    [SerializeField] private int[] confisonClickCount = new int[driverCount];
    public int[] ConfisonClickCount { get { return confisonClickCount; } set { confisonClickCount = value; } }
    int[] abnormalCount = new int[driverCount];
    int totalAbnormal;
    public int[] AbnormalCount { get { return abnormalCount; } set { abnormalCount = value; } }
    public int TotalAbnormal { get { return totalAbnormal; } set { totalAbnormal = value; } }
    const int CONPANY_WIDTH = 31;
    const int COMPANY_HEIGHT = 52;
    List<int>[] passArea = new List<int>[driverCount];
    public List<int>[] PassArea { get { return passArea; } }
    //[SerializeField]List<int> nowList = new List<int>();
    [SerializeField] private GameObject pen;
    [SerializeField] private Sprite[] driverStateSprite;
    [SerializeField] private SpriteRenderer[] driverStateRenderer;
    [SerializeField] private GameObject[] waitText;
    [SerializeField] private GameObject[] driveText;
    [SerializeField] private GameObject[] confisonText;
    [SerializeField] private GameObject[] breakText;



    [SerializeField] GameObject mapObject;//マップを格納している親オブジェクト
    Map map;
    ShortestPathSearch shortestPathSearch;//最短経路探索のスクリプト
    List<int>[] routeObjectsID = new List<int>[driverCount];//それぞれのトラックが通るオブジェクトを順番通りに格納
    List<int[]>[] routes = new List<int[]>[driverCount];
    List<Vector3>[] routesPosition = new List<Vector3>[driverCount];//
    List<GameObject>[] passedObjects = new List<GameObject>[driverCount];
    [SerializeField] bool[] isConfison = new bool[driverCount];
    public bool[] IsConfison { get { return isConfison; } set { isConfison = value; } }

    [SerializeField] GameObject move;
    [SerializeField] GameObject arrows;

    [SerializeField] GameObject[] driver;
    Collider2D[] driverSetButtonColliders = new Collider2D[driverCount];
    [SerializeField] GameObject[] driverSetButtonBlackBoard;
    [SerializeField] float[] speed;
    LineRenderer[] line = new LineRenderer[driverCount];
    [SerializeField] float distance;
    int[] coroutineNumber = new int[driverCount];
    int[] lastRoutesPositionCount = new int[driverCount];
    int frame = 0;
    [SerializeField] bool writing;
    public bool Writing { get { return writing; } }
    [SerializeField] bool driverSet = false;
    public bool DriverSet { get { return driverSet; } }
    [SerializeField] int driverType;
    int lastdriverType;
    public int DriverType { set { driverType = value; } }
    bool[] isDriving = new bool[driverCount];
    public bool[] IsDriving { get { return isDriving; } }
    private int[] deliveryProcess = new int[driverCount];
    [SerializeField] private int tmpDeliveryProcess;

    private GameObject[] destination = new GameObject[driverCount];
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
    Image writeButtonRenderer;
    [SerializeField] Sprite[] writeSprite;
    [SerializeField] GameObject[] driverSetButton;
    SpriteRenderer[] driverSetButtonRenderer = new SpriteRenderer[driverCount];
    [SerializeField] GameObject[] startButtons;
    [SerializeField] private GameObject[] recoveryButtons;

    [SerializeField] GameObject[] lineObject;

    [SerializeField] Texture2D cursor;
    [SerializeField] float cursorX;
    [SerializeField] float cursorY;

    bool memoring;

    List<int>[] deliveryData = new List<int>[driverCount];//シナー側で使うデータ。配達の時に通ったオブジェクトとそれに隣接するオブジェクトのID
    public List<int>[] DeleveryData { get { return deliveryData; } }
    [SerializeField] List<string> delivery = new List<string>();

    [SerializeField] Dictionary<string, bool>[] sinnerDebuff = new Dictionary<string, bool>[driverCount];
    public Dictionary<string, bool>[] SinnerDebuff { get { return sinnerDebuff; } set { sinnerDebuff = value; } }

    [SerializeField] private GameObject destinationPin;
    //[SerializeField] string[] str;
    RandomTable table;

    List<Vector3>[] tmpRoutePosition = new List<Vector3>[driverCount];
    List<int[]>[] tmpRoutes = new List<int[]>[driverCount];

    Vector3[] driverFirstPosition = new Vector3[driverCount];
    private TutorialMG tutorialMG;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tutorialMG = GameObject.Find("TutorialMG").gameObject.GetComponent<TutorialMG>();
        table = GameObject.Find("RandomTable").gameObject.GetComponent<RandomTable>();
        originalScale = writeButtonBackGround.transform.localScale;
        StartCoroutine(WriteButtonMover());
        StartCoroutine(ScaleCoroutine());
        memoring = false;
        map = mapObject.GetComponent<Map>();
        shortestPathSearch = mapObject.GetComponent<ShortestPathSearch>();
        writeButtonRenderer = writeButton.GetComponent<Image>();
        for (int i = 0; i < driverCount; i++)
        {
            driverSetButtonColliders[i] = driverSetButton[i].GetComponent<Collider2D>();
            driverSetButtonRenderer[i] = driverSetButton[i].GetComponent<SpriteRenderer>();
            routes[i] = new List<int[]>();
            breaking[i] = false;
            routesPosition[i] = new List<Vector3>();
            routeObjectsID[i] = new List<int>();
            passedObjects[i] = new List<GameObject>();
            sinnerDebuff[i] = new Dictionary<string, bool>();
            coroutineNumber[i] = 0;
            lastRoutesPositionCount[i] = 0;
            canStart[i] = false;
            deliveryData[i] = new List<int>();
            tmpRoutePosition[i] = new List<Vector3>();
            tmpRoutes[i] = new List<int[]>();
            passArea[i] = new List<int>();
            Directions(i);
        }
        //SinnerDebuff = AddArray(SinnerDebuff, str, false);
        foreach (KeyValuePair<string, bool> kvp in sinnerDebuff[0])
        {
            Debug.Log(ColorChanger(kvp.Key, "red") + "は" + sinnerDebuff[0][kvp.Key] + "になっている");
        }
        driverType = -1;
        tmpDeliveryItem = -1;
        tmpDeliveryProcess = -1;

        writing = false;
        driverSet = false;
        for (int i = 0; i < driver.Length; i++)
        {
            line[i] = lineObject[i].GetComponent<LineRenderer>();
            driverFirstPosition[i] = driver[i].transform.localPosition;
            driver[i].SetActive(false);
        }

        for (int i = 0; i < isDriving.Length; i++)
        {
            isDriving[i] = false;
        }
    }
    // Update is called once per frame
    void Update()
    {

        pen.SetActive(!writing);
        DriverStateControler();

        for (int i = 0; i < driverCount; i++)
        {
            recovering[i] = false;
        }
        if (Input.GetMouseButtonDown(0)) // 左クリック
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;

            var results = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            //Debug.Log("Raycast hits:");
          
            foreach (var hit in results)
            {
                if(hit.gameObject.name=="RecoveryButton")
                {   
                    Recovery(int.Parse(hit.gameObject.transform.GetChild(0).gameObject.name));
                }
            }
        }
        if (writing) writeButtonRenderer.sprite = writeSprite[0];
        else writeButtonRenderer.sprite = writeSprite[1];
        if (lastdriverType != driverType) writing = false;
        for (int i = 0; i < driverCount; i++)
        {
            //if (!driverSet) break;
            if (!tutorialMG.IsTutorial) recoveryButtons[i].SetActive(isDriving[i]);
            else recoveryButtons[i].SetActive(false);
            driverSetButtonBlackBoard[i].SetActive(!(tmpDeliveryItem != -1 && tmpDeliveryProcess != -1) && !startButtons[i].activeSelf);

            if (i == driverType) driverSetButtonRenderer[i].color = Color.green;
            else driverSetButtonRenderer[i].color = Color.white;
            canStart[i] = isDestinationSetting[i] && isItemSetting[i] && isItemSetting[i];
            if (routeObjectsID[i].Count > 0)
            {
                if ((int)MapObjectID.HOUSE_1 <= routeObjectsID[i][routeObjectsID[i].Count - 1] && routeObjectsID[i][routeObjectsID[i].Count - 1] <= (int)MapObjectID.HOUSE_4)
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
            Cursor.SetCursor(cursor, new Vector2(cursorX, cursorY), CursorMode.Auto);
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
            driverSetButtonColliders[i].enabled = !isDriving[i] && !startButtons[i].activeSelf;
            if (lastRoutesPositionCount[i] != routesPosition[i].Count && routesPosition[i].Count > 1)
            {
                coroutineNumber[i]++;
                StartCoroutine(ArrowMove(routesPosition[i][routesPosition[i].Count - 2], routesPosition[i][routesPosition[i].Count - 1], coroutineNumber[i], frame, i));
            }
            lastRoutesPositionCount[i] = routesPosition[i].Count;
        }

        if (driverType >= 0)
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

    public void MemoryRoute(int widthPositionID, int heightPositionID, int objectID, GameObject obj, Vector3 position)
    {
        if (!Input.GetMouseButton(0)) return;
        if (!memoring) return;
        if (!writing || !driverSet) return;
        if (driverType == -1) return;
        int[] positionID = new int[2];//xとzで二つ
        int routeObjectsIDCount = routeObjectsID[driverType].Count;
        if (routeObjectsIDCount > 0)
        {
            if ((int)MapObjectID.HOUSE_1 <= routeObjectsID[driverType][routeObjectsIDCount - 1] && routeObjectsID[driverType][routeObjectsIDCount - 1] <= (int)MapObjectID.HOUSE_4) return;
        }
        positionID[0] = heightPositionID;
        positionID[1] = widthPositionID;

        if (objectID == 1 && routes[driverType].Count == 0)
        {
            routes[driverType].Add(positionID);
            routeObjectsID[driverType].Add(objectID);
            routesPosition[driverType].Add(position);
            passedObjects[driverType].Add(obj);
            line[driverType].positionCount++;
            line[driverType].SetPosition(line[driverType].positionCount - 1, position);
        }
        if (NearCheck(routes[driverType], positionID))//なぞったオブジェクトが前のオブジェクトと隣接しているなら
        {
            routes[driverType].Add(positionID);
            routeObjectsID[driverType].Add(objectID);
            routesPosition[driverType].Add(position);
            passedObjects[driverType].Add(obj);
            line[driverType].positionCount++;
            line[driverType].SetPosition(line[driverType].positionCount - 1, position);
            if ((int)MapObjectID.HOUSE_1 <= objectID && objectID <= (int)MapObjectID.HOUSE_4)
            {
                if (tutorialMG.IsTutorial && tutorialMG.CurrentState == TutorialMG.TutorialState.DrawLine)
                {
                    tutorialMG.ChangeState(TutorialMG.TutorialState.EndDrawLine);
                }
                writing = false;
                destination[driverType] = obj;
                driverType = -1;
                Instantiate(destinationPin, obj.transform.position, Quaternion.identity, obj.transform);
            }
        }

    }

    public void MemoryStart()
    {
        if (!writing || !driverSet) { return; }

        memoring = true;
        StartCoroutine(Directions(driverType));

    }

    public void StartDriver(int driverID)
    {
        if (routeObjectsID[driverID].Count == 0) return;
        if ((int)MapObjectID.HOUSE_1 <= routeObjectsID[driverID][routeObjectsID[driverID].Count - 1] && routeObjectsID[driverID][routeObjectsID[driverID].Count - 1] <= (int)MapObjectID.HOUSE_4)
        {
            driver[driverID].SetActive(true);
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
        return Mathf.Abs(list[list.Count - 1][0] - positionID[0]) <= 1 && Mathf.Abs(list[list.Count - 1][1] - positionID[1]) <= 1 && Mathf.Abs(list[list.Count - 1][1] - positionID[1]) != Mathf.Abs(list[list.Count - 1][0] - positionID[0]);
    }



    private IEnumerator DriverMove(int driverID)
    {
        GameObject obj = driver[driverID];
        SpriteRenderer sr = driver[driverID].GetComponent<SpriteRenderer>();
        bool lastIsConfison = false;
        List<MapData> lastList = new List<MapData>();
        List<MapData> nowList = new List<MapData>();
        MapData md = map.MapDatas[routes[driverID][1][0]][routes[driverID][1][1]];
        int tableID = 0;
        bool randomed = false;//一度でも混乱状態になっていたら
        bool confisonClear = false;
        tmpRoutePosition[driverID] = routesPosition[driverID];
        tmpRoutes[driverID] = routes[driverID];
        for (int i = 1; i < routesPosition[driverID].Count; i++)
        {
            
            // Vector3 dir = ((routesPosition[driverID][i]+mapObject.transform.localPosition) - obj.transform.position).normalized;
            //Vector3 lastDirction = dir;
            
            switch (deliveryProcess[driverID])
            {
                case 0:
                    {
                        speed[driverID] = 0.35f;
                        sr.sprite = driverSprite[0];
                        driver[driverID].transform.localScale=new Vector3(0.05f,0.05f,0.05f);
                        //ここいじる
                    }
                    break;
                case 1:
                    {
                        speed[driverID] = 0.5f;
                        sr.sprite = driverSprite[1];
                        driver[driverID].transform.localScale = new Vector3(0.015f,0.015f,0.015f);
                    }
                    break;
                case 2:
                    {
                        speed[driverID] = 1f;
                        sr.sprite = driverSprite[2];
                        driver[driverID].transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);
                    }
                    break;
            }

            if (lastIsConfison && !isConfison[driverID] && !confisonClear)
            {
                // ランダム挙動の最後の位置から最短ルートを取得
                //Vector3 currentPos = obj.transform.position - map.transform.localPosition;
                int startWidth = md.widthPositionID;
                int startHeight = md.heightPositionID;
                int goalWidth = routes[driverID][routes[driverID].Count - 1][1];
                int goalHeight = routes[driverID][routes[driverID].Count - 1][0];
                line[driverID].positionCount = 0;
                tmpRoutePosition[driverID] = routesPosition[driverID];
                tmpRoutes[driverID] = routes[driverID];
                List<Vector3> shortestPositions = shortestPathSearch.ShortestPath(startWidth, startHeight, goalWidth, goalHeight, ref routes[driverID]);
                routesPosition[driverID] = shortestPositions;
                StartCoroutine(DriverMove(driverID, shortestPositions));
                yield break;

            }
            if (!isConfison[driverID])
            {
                if (map.MapDatas[routes[driverID][i][0]][routes[driverID][i][1]].objectID < (int)MapObjectID.HOUSE_1 )
                {
                    // Debug.Log(deliveryData[driverID].Count - 1 + ("を追加しました"));
                    string[] str = map.MapDatas[routes[driverID][i][0]][routes[driverID][i][1]].obj.transform.parent.gameObject.name.Split("_");
                    Debug.Log(str[1]);
                    passArea[driverID].Add(int.Parse(str[1]));
                    passArea.Distinct();
                    nowList.Add(map.MapDatas[routes[driverID][i][0]][routes[driverID][i][1]]);
                    nowList.Add(map.MapDatas[routes[driverID][i][0] + 1][routes[driverID][i][1]]);
                    nowList.Add(map.MapDatas[routes[driverID][i][0] - 1][routes[driverID][i][1]]);
                    nowList.Add(map.MapDatas[routes[driverID][i][0]][routes[driverID][i][1] + 1]);
                    nowList.Add(map.MapDatas[routes[driverID][i][0]][routes[driverID][i][1] - 1]);
                    nowList.RemoveAll(x => lastList.Contains(x));
                    for (int c = 0; c < nowList.Count; c++)
                    {
                        deliveryData[driverID].Add(nowList[c].objectID);
                        delivery.Add(nowList[c].name);
                    }
                    lastList = nowList;
                    nowList = new List<MapData>();
                    //Debug.Log(ColorChanger(map.MapDatas))
                }

                Vector3 dir = ((routesPosition[driverID][i] + map.transform.localPosition) - obj.transform.position).normalized;
                Vector3 lastDirction = dir;
                while (lastDirction == dir)
                {
                    while (breaking[driverID])
                    {
                        if (recovering[driverID])
                        {
                            yield break;
                        }
                        yield return null;
                    }
                  
                    lastDirction = dir;
                    Vector3 vec = lastDirction * Time.deltaTime;
                    if (dir.x == 1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                    }
                    if (dir.x == -1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    }
                    if (dir.y == 1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                    }
                    if (dir.y == -1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                    }
                    obj.transform.position += vec / speed[driverID]*clownSpeed*swordSpeed;
                    if (isDriving[driverID])
                    {
                        dir = ((routesPosition[driverID][i] + map.transform.localPosition) - obj.transform.position).normalized;
                    }
                   
                    yield return null;
                }
                obj.transform.position = routesPosition[driverID][i] + map.transform.localPosition;
                md = map.MapDatas[routes[driverID][i][0]][routes[driverID][i][1]];
            }
            else
            {
                bool dirSetted = false;
                randomed = true;
                MapData randomMd = new MapData();
                lastIsConfison = isConfison[driverID];
                string lastDir = "";
                string dirction = "";
                while (!dirSetted)
                {

                    string[] addDir = new string[3];
                    if (lastDir == "TOP")
                    {
                        addDir[0] = "TOP";
                        addDir[1] = "RIGHT";
                        addDir[2] = "LEFT";
                    }
                    else if (lastDir == "RIGHT")
                    {
                        addDir[0] = "TOP";
                        addDir[1] = "RIGHT";
                        addDir[2] = "BOTTOM";
                    }
                    else if (lastDir == "LEFT")
                    {
                        addDir[0] = "TOP";
                        addDir[1] = "LEFT";
                        addDir[2] = "BOTTOM";
                    }
                    else
                    {
                        addDir[0] = "BOTTOM";
                        addDir[1] = "RIGHT";
                        addDir[2] = "LEFT";
                    }
                    string[] randomData = { "TOP", "RIGHT", "LEFT", "BOTTOM", addDir[0], addDir[0], addDir[0], addDir[0], addDir[0], addDir[0], addDir[1], addDir[1], addDir[1], addDir[1], addDir[1], addDir[1], addDir[2], addDir[2], addDir[2], addDir[2], addDir[2], addDir[2] };


                    dirction = randomData[Random.Range(0, randomData.Length)];
                    switch (dirction)
                    {
                        case "TOP":
                            {
                                if (map.MapDatas[md.heightPositionID - 1][md.widthPositionID].objectID >= (int)MapObjectID.STRAIGHT && map.MapDatas[md.heightPositionID - 1][md.widthPositionID].objectID <= (int)Map.MapObjectID.CROSS)
                                {
                                    randomMd = map.MapDatas[md.heightPositionID - 1][md.widthPositionID];
                                    dirSetted = true;
                                }
                                break;
                            }
                        case "RIGHT":
                            {
                                if (map.MapDatas[md.heightPositionID][md.widthPositionID + 1].objectID >= (int)MapObjectID.STRAIGHT && map.MapDatas[md.heightPositionID][md.widthPositionID + 1].objectID <= (int)Map.MapObjectID.CROSS)
                                {
                                    randomMd = map.MapDatas[md.heightPositionID][md.widthPositionID + 1];
                                    dirSetted = true;
                                }
                                break;
                            }
                        case "LEFT":
                            {
                                if (map.MapDatas[md.heightPositionID][md.widthPositionID - 1].objectID >= (int)MapObjectID.STRAIGHT && map.MapDatas[md.heightPositionID][md.widthPositionID - 1].objectID <= (int)Map.MapObjectID.CROSS)
                                {
                                    randomMd = map.MapDatas[md.heightPositionID][md.widthPositionID - 1];
                                    dirSetted = true;
                                }
                                break;
                            }
                        case "BOTTOM":
                            {
                                if (map.MapDatas[md.heightPositionID + 1][md.widthPositionID].objectID >= (int)MapObjectID.STRAIGHT && map.MapDatas[md.heightPositionID + 1][md.widthPositionID].objectID <= (int)Map.MapObjectID.CROSS)
                                {
                                    randomMd = map.MapDatas[md.heightPositionID + 1][md.widthPositionID];
                                    dirSetted = true;
                                }
                                break;
                            }

                    }
                    yield return null;
                }
                lastDir = dirction;

                md = randomMd;
                string[] objectInfo = md.name.Split("_");
                Vector3 endPos = md.obj.transform.localPosition;
                if (md.objectID < (int)MapObjectID.HOUSE_1 )
                {

                    string[] str = map.MapDatas[routes[driverID][i][0]][routes[driverID][i][1]].obj.transform.parent.gameObject.name.Split("_");
                    Debug.Log(str[1]);
                    passArea[driverID].Add(int.Parse(str[1]));
                    passArea.Distinct();
                    nowList.Add(map.MapDatas[md.heightPositionID][md.widthPositionID]);
                    nowList.Add(map.MapDatas[md.heightPositionID + 1][md.widthPositionID]);
                    nowList.Add(map.MapDatas[md.heightPositionID - 1][md.widthPositionID]);
                    nowList.Add(map.MapDatas[md.heightPositionID][md.widthPositionID + 1]);
                    nowList.Add(map.MapDatas[md.heightPositionID][md.widthPositionID - 1]);
                    nowList.RemoveAll(x => lastList.Contains(x));
                    for (int c = 0; c < nowList.Count; c++)
                    {
                        deliveryData[driverID].Add(nowList[c].objectID);
                        delivery.Add(nowList[c].name);
                    }
                    lastList = nowList;
                    nowList = new List<MapData>();
                }
                //float elapsed = 0f;

                Vector3 dir = ((endPos + mapObject.transform.localPosition) - obj.transform.position).normalized;
                Vector3 lastDirction = dir;
                while (lastDirction == dir)
                {
                    while (breaking[driverID])
                    {
                        if (recovering[driverID])
                        {
                            yield break;
                        }
                        yield return null;
                    }
                   
                    lastDirction = dir;
                    Vector3 vec = lastDirction * Time.deltaTime;
                    if (dir.x == 1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                    }
                    if (dir.x == -1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    }
                    if (dir.y == 1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                    }
                    if (dir.y == -1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                    }
                    obj.transform.position += vec / speed[driverID]*clownSpeed*swordSpeed;
                    dir = ((endPos + map.transform.localPosition) - obj.transform.position).normalized;
                    yield return null;
                }
                obj.transform.position = endPos + mapObject.transform.localPosition;
                i--;
                nowList.Clear();
            }
        }
        DeliveryCompleted(destination[driverID], driverID);

        //{
        //    int startWidth = md.widthPositionID;
        //    int startHeight = md.heightPositionID;
        //    int goalWidth = 31;
        //    int goalHeight = 52;
        //    line[driverID].positionCount = 0;
        //    List<Vector3> shortestPositions = shortestPathSearch.ShortestPath(startWidth, startHeight, goalWidth, goalHeight, ref routes[driverID]);
        //    routesPosition[driverID] = shortestPositions;
        //}

        yield return new WaitForSeconds(2f);
        for (int i = routesPosition[driverID].Count - 2; i >= 0; i--)
        {
            if (!isConfison[driverID])
            {

                Vector3 dir = ((routesPosition[driverID][i] + map.transform.localPosition) - obj.transform.position).normalized;
                Vector3 lastDirction = dir;

                if (map.MapDatas[routes[driverID][i][0]][routes[driverID][i][1]].objectID < (int)MapObjectID.HOUSE_1 )
                {
                    // Debug.Log(deliveryData[driverID].Count - 1 + ("を追加しました"));
                    string[] str = map.MapDatas[routes[driverID][i][0]][routes[driverID][i][1]].obj.transform.parent.gameObject.name.Split("_");
                    Debug.Log(str[1]);
                    passArea[driverID].Add(int.Parse(str[1]));
                    passArea.Distinct();
                    nowList.Add(map.MapDatas[routes[driverID][i][0]][routes[driverID][i][1]]);
                    nowList.Add(map.MapDatas[routes[driverID][i][0] + 1][routes[driverID][i][1]]);
                    nowList.Add(map.MapDatas[routes[driverID][i][0] - 1][routes[driverID][i][1]]);
                    nowList.Add(map.MapDatas[routes[driverID][i][0]][routes[driverID][i][1] + 1]);
                    nowList.Add(map.MapDatas[routes[driverID][i][0]][routes[driverID][i][1] - 1]);
                    nowList.RemoveAll(x => lastList.Contains(x));
                    for (int c = 0; c < nowList.Count; c++)
                    {
                        deliveryData[driverID].Add(nowList[c].objectID);
                        delivery.Add(nowList[c].name);
                    }
                    lastList = nowList;
                    nowList = new List<MapData>();
                    //Debug.Log(ColorChanger(map.MapDatas))
                }

                while (lastDirction == dir)
                {
                    while (breaking[driverID])
                    {
                        if (recovering[driverID])
                        {
                            yield break;
                        }
                        yield return null;
                    }
                    if (dir.x == 1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                    }
                    if (dir.x == -1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    }
                    if (dir.y == 1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                    }
                    if (dir.y == -1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                    }
                    lastDirction = dir;
                    Vector3 vec = lastDirction * Time.deltaTime;
                    obj.transform.position += vec / speed[driverID]*clownSpeed*swordSpeed;
                    dir = ((routesPosition[driverID][i] + map.transform.localPosition) - obj.transform.position).normalized;
                    yield return null;
                }
                obj.transform.position = routesPosition[driverID][i] + map.transform.localPosition;
            }

            else
            {
                bool dirSetted = false;
                randomed = true;
                MapData randomMd = new MapData();
                lastIsConfison = isConfison[driverID];
                string lastDir = "";
                string dirction = "";
                while (!dirSetted)
                {
                    string[] addDir = new string[3];
                    if (lastDir == "TOP")
                    {
                        addDir[0] = "TOP";
                        addDir[1] = "RIGHT";
                        addDir[2] = "LEFT";
                    }
                    else if (lastDir == "RIGHT")
                    {
                        addDir[0] = "TOP";
                        addDir[1] = "RIGHT";
                        addDir[2] = "BOTTOM";
                    }
                    else if (lastDir == "LEFT")
                    {
                        addDir[0] = "TOP";
                        addDir[1] = "LEFT";
                        addDir[2] = "BOTTOM";
                    }
                    else
                    {
                        addDir[0] = "BOTTOM";
                        addDir[1] = "RIGHT";
                        addDir[2] = "LEFT";
                    }
                    string[] randomData = { "TOP", "RIGHT", "LEFT", "BOTTOM", addDir[0], addDir[0], addDir[0], addDir[0], addDir[0], addDir[0], addDir[1], addDir[1], addDir[1], addDir[1], addDir[1], addDir[1], addDir[2], addDir[2], addDir[2], addDir[2], addDir[2], addDir[2] };


                    dirction = randomData[Random.Range(0, randomData.Length)];
                    switch (dirction)
                    {
                        case "TOP":
                            {
                                if (map.MapDatas[md.heightPositionID - 1][md.widthPositionID].objectID >= (int)MapObjectID.STRAIGHT && map.MapDatas[md.heightPositionID - 1][md.widthPositionID].objectID <= (int)Map.MapObjectID.CROSS)
                                {
                                    randomMd = map.MapDatas[md.heightPositionID - 1][md.widthPositionID];
                                    dirSetted = true;
                                }
                                break;
                            }
                        case "RIGHT":
                            {
                                if (map.MapDatas[md.heightPositionID][md.widthPositionID + 1].objectID >= (int)MapObjectID.STRAIGHT && map.MapDatas[md.heightPositionID][md.widthPositionID + 1].objectID <= (int)Map.MapObjectID.CROSS)
                                {
                                    randomMd = map.MapDatas[md.heightPositionID][md.widthPositionID + 1];
                                    dirSetted = true;
                                }
                                break;
                            }
                        case "LEFT":
                            {
                                if (map.MapDatas[md.heightPositionID][md.widthPositionID - 1].objectID >= (int)MapObjectID.STRAIGHT && map.MapDatas[md.heightPositionID][md.widthPositionID - 1].objectID <= (int)Map.MapObjectID.CROSS)
                                {
                                    randomMd = map.MapDatas[md.heightPositionID][md.widthPositionID - 1];
                                    dirSetted = true;
                                }
                                break;
                            }
                        case "BOTTOM":
                            {
                                if (map.MapDatas[md.heightPositionID + 1][md.widthPositionID].objectID >= (int)MapObjectID.STRAIGHT && map.MapDatas[md.heightPositionID + 1][md.widthPositionID].objectID <= (int)Map.MapObjectID.CROSS)
                                {
                                    randomMd = map.MapDatas[md.heightPositionID + 1][md.widthPositionID];
                                    dirSetted = true;
                                }
                                break;
                            }

                    }
                    yield return null;
                }
                lastDir = dirction;

                md = randomMd;
                string[] objectInfo = md.name.Split("_");
                Vector3 endPos = md.obj.transform.localPosition;
                if (map.MapDatas[routes[driverID][i][0]][routes[driverID][i][1]].objectID < (int)MapObjectID.HOUSE_1)
                {
                    // Debug.Log(deliveryData[driverID].Count - 1 + ("を追加しました"));
                    string[] str = map.MapDatas[routes[driverID][i][0]][routes[driverID][i][1]].obj.transform.parent.gameObject.name.Split("_");
                    Debug.Log(str[1]);
                    passArea[driverID].Add(int.Parse(str[1]));
                    passArea.Distinct();
                    nowList.Add(map.MapDatas[md.heightPositionID][md.widthPositionID]);
                    nowList.Add(map.MapDatas[md.heightPositionID + 1][md.widthPositionID]);
                    nowList.Add(map.MapDatas[md.heightPositionID - 1][md.widthPositionID]);
                    nowList.Add(map.MapDatas[md.heightPositionID][md.widthPositionID + 1]);
                    nowList.Add(map.MapDatas[md.heightPositionID][md.widthPositionID - 1]);
                    nowList.RemoveAll(x => lastList.Contains(x));
                    for (int c = 0; c < nowList.Count; c++)
                    {
                        deliveryData[driverID].Add(nowList[c].objectID);
                        delivery.Add(nowList[c].name);
                    }
                    lastList = nowList;
                    nowList = new List<MapData>();
                }
                //float elapsed = 0f;

                Vector3 dir = ((endPos + mapObject.transform.localPosition) - obj.transform.position).normalized;
                Vector3 lastDirction = dir;
                while (lastDirction == dir)
                {
                    while (breaking[driverID])
                    {
                        if (recovering[driverID])
                        {
                            yield break;
                        }
                        yield return null;
                    }
                    lastDirction = dir;
                    Vector3 vec = lastDirction * Time.deltaTime;
                    if (dir.x == 1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                    }
                    if (dir.x == -1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    }
                    if (dir.y == 1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                    }
                    if (dir.y == -1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                    }
                    obj.transform.position += vec / speed[driverID]*clownSpeed*swordSpeed;
                    dir = ((endPos + map.transform.localPosition) - obj.transform.position).normalized;
                    yield return null;
                }
                obj.transform.position = endPos + mapObject.transform.localPosition;
                i++;
                nowList.Clear();
            }
        }
        //メモ　 開始地点がおかしい
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Arrow");
        foreach (GameObject o in objs)
        {
            Destroy(o);
        }
        routes[driverID] = new List<int[]>();
        routesPosition[driverID] = new List<Vector3>();
        tmpRoutePosition[driverID] = new List<Vector3>();
        tmpRoutes[driverID] = new List<int[]>();
        line[driverID].positionCount = 0;
        routeObjectsID[driverID] = new List<int>();
        isItemSetting[driverID] = false;
        isProcessSetting[driverID] = false;
        isDriving[driverID] = false;
        driver[driverID].SetActive(false);
        if (tutorialMG.IsTutorial && tutorialMG.CurrentState == TutorialMG.TutorialState.EndDrawLine)
        {
            tutorialMG.ChangeState(TutorialMG.TutorialState.EndDelivery);
        }
        yield return null;

    }


    private IEnumerator DriverMove(int driverID, List<Vector3> positionID)
    {
        SpriteRenderer sr = driver[driverID].GetComponent<SpriteRenderer>();
        GameObject obj = driver[driverID];
        bool lastIsConfison = false;
        List<MapData> lastList = new List<MapData>();
        List<MapData> nowList = new List<MapData>();
        MapData md = map.MapDatas[routes[driverID][1][0]][routes[driverID][1][1]];



        bool confisonClear = false;
        for (int i = 1; i < routesPosition[driverID].Count; i++)
        {
            // Vector3 dir = ((routesPosition[driverID][i]+mapObject.transform.localPosition) - obj.transform.position).normalized;
            //Vector3 lastDirction = dir;
            switch (deliveryProcess[driverID])
            {
                case 0:
                    {
                        speed[driverID] = 0.35f;
                        sr.sprite = driverSprite[0];
                        driver[driverID].transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                    }
                    break;
                case 1:
                    {
                        speed[driverID] = 0.5f;
                        sr.sprite = driverSprite[0];
                        driver[driverID].transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);
                    }
                    break;
                case 2:
                    {
                        speed[driverID] = 1f;
                        sr.sprite = driverSprite[0];
                        driver[driverID].transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);
                    }
                    break;
            }

            if (lastIsConfison && !isConfison[driverID])
            {
                // ランダム挙動の最後の位置から最短ルートを取得
                //Vector3 currentPos = obj.transform.position - map.transform.localPosition;
                Debug.Log("DEBUG>LOG");
                int startWidth = md.widthPositionID;
                int startHeight = md.heightPositionID;
                int goalWidth = routes[driverID][routes[driverID].Count - 1][1];
                int goalHeight = routes[driverID][routes[driverID].Count - 1][0];
                tmpRoutePosition[driverID] = routesPosition[driverID];
                tmpRoutes[driverID] = routes[driverID];
                line[driverID].positionCount = 0;
                List<Vector3> shortestPositions = shortestPathSearch.ShortestPath(startWidth, startHeight, goalWidth, goalHeight, ref routes[driverID]);
                routesPosition[driverID] = shortestPositions;


                StartCoroutine(DriverMove(driverID, shortestPositions));
                yield break;

            }
            if (!isConfison[driverID])
            {
                Debug.Log("i" + i);
                if (map.MapDatas[routes[driverID][i][0]][routes[driverID][i][1]].objectID < (int)MapObjectID.HOUSE_1)
                {
                    // Debug.Log(deliveryData[driverID].Count - 1 + ("を追加しました"));
                    string[] str = map.MapDatas[routes[driverID][i][0]][routes[driverID][i][1]].obj.transform.parent.gameObject.name.Split("_");
                    Debug.Log(str[1]);
                    passArea[driverID].Add(int.Parse(str[1]));
                    passArea.Distinct();
                    nowList.Add(map.MapDatas[routes[driverID][i][0]][routes[driverID][i][1]]);
                    nowList.Add(map.MapDatas[routes[driverID][i][0] + 1][routes[driverID][i][1]]);
                    nowList.Add(map.MapDatas[routes[driverID][i][0] - 1][routes[driverID][i][1]]);
                    nowList.Add(map.MapDatas[routes[driverID][i][0]][routes[driverID][i][1] + 1]);
                    nowList.Add(map.MapDatas[routes[driverID][i][0]][routes[driverID][i][1] - 1]);
                    nowList.RemoveAll(x => lastList.Contains(x));
                    for (int c = 0; c < nowList.Count; c++)
                    {
                        deliveryData[driverID].Add(nowList[c].objectID);
                        delivery.Add(nowList[c].name);
                    }
                    lastList = nowList;
                    nowList = new List<MapData>();
                    //Debug.Log(ColorChanger(map.MapDatas))
                }

                Vector3 dir = ((routesPosition[driverID][i] + map.transform.localPosition) - obj.transform.position).normalized;
                Vector3 lastDirction = dir;
                while (lastDirction == dir)
                {
                    while (breaking[driverID])
                    {
                        if (recovering[driverID])
                        {
                            yield break;
                        }
                        yield return null;
                    }
                    lastDirction = dir;
                    Vector3 vec = lastDirction * Time.deltaTime;
                    if (dir.x == 1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                    }
                    if (dir.x == -1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    }
                    if (dir.y == 1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                    }
                    if (dir.y == -1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                    }
                    obj.transform.position += vec / speed[driverID]*clownSpeed;
                    dir = ((routesPosition[driverID][i] + map.transform.localPosition) - obj.transform.position).normalized;
                    yield return null;
                }
                obj.transform.position = routesPosition[driverID][i] + map.transform.localPosition;
                md = map.MapDatas[routes[driverID][i][0]][routes[driverID][i][1]];
            }
            else
            {
                bool dirSetted = false;
                MapData randomMd = new MapData();
                lastIsConfison = true;
                while (!dirSetted)
                {

                    string[] randomData = { "TOP", "RIGHT", "LEFT", "BOTTOM" };
                    string dirction = randomData[Random.Range(0, randomData.Length)];
                    switch (dirction)
                    {
                        case "TOP":
                            {
                                if (map.MapDatas[md.heightPositionID - 1][md.widthPositionID].objectID >= (int)MapObjectID.STRAIGHT && map.MapDatas[md.heightPositionID - 1][md.widthPositionID].objectID <= (int)Map.MapObjectID.CROSS)
                                {
                                    randomMd = map.MapDatas[md.heightPositionID - 1][md.widthPositionID];
                                    dirSetted = true;
                                }
                                break;
                            }
                        case "RIGHT":
                            {
                                if (map.MapDatas[md.heightPositionID][md.widthPositionID + 1].objectID >= (int)MapObjectID.STRAIGHT && map.MapDatas[md.heightPositionID][md.widthPositionID + 1].objectID <= (int)Map.MapObjectID.CROSS)
                                {
                                    randomMd = map.MapDatas[md.heightPositionID][md.widthPositionID + 1];
                                    dirSetted = true;
                                }
                                break;
                            }
                        case "LEFT":
                            {
                                if (map.MapDatas[md.heightPositionID][md.widthPositionID - 1].objectID >= (int)MapObjectID.STRAIGHT && map.MapDatas[md.heightPositionID][md.widthPositionID - 1].objectID <= (int)Map.MapObjectID.CROSS)
                                {
                                    randomMd = map.MapDatas[md.heightPositionID][md.widthPositionID - 1];
                                    dirSetted = true;
                                }
                                break;
                            }
                        case "BOTTOM":
                            {
                                if (map.MapDatas[md.heightPositionID + 1][md.widthPositionID].objectID >= (int)MapObjectID.STRAIGHT && map.MapDatas[md.heightPositionID + 1][md.widthPositionID].objectID <= (int)Map.MapObjectID.CROSS)
                                {
                                    randomMd = map.MapDatas[md.heightPositionID + 1][md.widthPositionID];
                                    dirSetted = true;
                                }
                                break;
                            }

                    }

                }

                md = randomMd;
                string[] objectInfo = md.name.Split("_");
                Vector3 endPos = md.obj.transform.localPosition;
                if (map.MapDatas[routes[driverID][i][0]][routes[driverID][i][1]].objectID < (int)MapObjectID.HOUSE_1)
                {
                    // Debug.Log(deliveryData[driverID].Count - 1 + ("を追加しました"));
                    string[] str = map.MapDatas[routes[driverID][i][0]][routes[driverID][i][1]].obj.transform.parent.gameObject.name.Split("_");
                    Debug.Log(str[1]);
                    passArea[driverID].Add(int.Parse(str[1]));
                    passArea.Distinct();
                    nowList.Add(map.MapDatas[md.heightPositionID][md.widthPositionID]);
                    nowList.Add(map.MapDatas[md.heightPositionID + 1][md.widthPositionID]);
                    nowList.Add(map.MapDatas[md.heightPositionID - 1][md.widthPositionID]);
                    nowList.Add(map.MapDatas[md.heightPositionID][md.widthPositionID + 1]);
                    nowList.Add(map.MapDatas[md.heightPositionID][md.widthPositionID - 1]);
                    nowList.RemoveAll(x => lastList.Contains(x));
                    for (int c = 0; c < nowList.Count; c++)
                    {
                        deliveryData[driverID].Add(nowList[c].objectID);
                        delivery.Add(nowList[c].name);
                    }
                    lastList = nowList;
                    nowList = new List<MapData>();
                }
                //float elapsed = 0f;

                Vector3 dir = ((endPos + mapObject.transform.localPosition) - obj.transform.position).normalized;
                Vector3 lastDirction = dir;
                while (lastDirction == dir)
                {
                    lastDirction = dir;
                    while (breaking[driverID])
                    {
                        if (recovering[driverID])
                        {
                            yield break;
                        }
                        yield return null;
                    }
                    Vector3 vec = lastDirction * Time.deltaTime;
                    if (dir.x == 1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                    }
                    if (dir.x == -1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    }
                    if (dir.y == 1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                    }
                    if (dir.y == -1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                    }
                    obj.transform.position += vec / speed[driverID]*clownSpeed;
                    dir = ((endPos + map.transform.localPosition) - obj.transform.position).normalized;
                    yield return null;
                }
                obj.transform.position = endPos + mapObject.transform.localPosition;
                i = 0;
                nowList.Clear();
            }
        }
        DeliveryCompleted(destination[driverID], driverID);

        //{
        //    Debug.Log("空虚su");
        //    int goalWidth = 31;
        //    int goalHeight = 52;
        //    int startWidth = routes[driverID][routes[driverID].Count - 2][0];//最後に通過した道路　配列の最後は配達地点のため、その一つ前を取得
        //    int startHeight = routes[driverID][routes[driverID].Count - 2][1];
        //    line[driverID].positionCount = 0;
        //    List<Vector3> shortestPositions = shortestPathSearch.ShortestPath(startHeight, startWidth, goalWidth, goalHeight,ref routes[driverID]);
        //    routesPosition[driverID] = shortestPositions;
        //}
        yield return new WaitForSeconds(2f);

        routesPosition[driverID] = tmpRoutePosition[driverID];
        routes[driverID] = tmpRoutes[driverID];
        Debug.Log("rotuesPosition.Count" + routesPosition[driverID].Count);
        for (int i = routesPosition[driverID].Count - 2; i >= 0; i--)
        {// Vector3 dir = ((routesPosition[driverID][i]+mapObject.transform.localPosition) - obj.transform.position).normalized;
            //Vector3 lastDirction = dir;
            switch (deliveryProcess[driverID])
            {
                case 0:
                    {
                        speed[driverID] = 0.35f;
                    }
                    break;
                case 1:
                    {
                        speed[driverID] = 0.5f;
                    }
                    break;
                case 2:
                    {
                        speed[driverID] = 1f;
                    }
                    break;
            }

            if (lastIsConfison && !isConfison[driverID])
            {
                // ランダム挙動の最後の位置から最短ルートを取得
                //Vector3 currentPos = obj.transform.position - map.transform.localPosition;
                int startWidth = md.widthPositionID;
                int startHeight = md.heightPositionID;
                int goalWidth = routes[driverID][routes[driverID].Count - 1][1];

                int goalHeight = routes[driverID][routes[driverID].Count - 1][0];
                line[driverID].positionCount = 0;
                List<Vector3> shortestPositions = shortestPathSearch.ShortestPath(startWidth, startHeight, 31, 52, ref routes[driverID]);
                routesPosition[driverID] = shortestPositions;
                StartCoroutine(DriverMove(driverID));
                yield break;

            }
            if (!isConfison[driverID])
            {

                if (map.MapDatas[routes[driverID][i][0]][routes[driverID][i][1]].objectID < (int)MapObjectID.HOUSE_1)
                {
                    // Debug.Log(deliveryData[driverID].Count - 1 + ("を追加しました"));
                    string[] str = map.MapDatas[routes[driverID][i][0]][routes[driverID][i][1]].obj.transform.parent.gameObject.name.Split("_");
                    Debug.Log(str[1]);
                    passArea[driverID].Add(int.Parse(str[1]));
                    passArea.Distinct();
                    // Debug.Log(deliveryData[driverID].Count - 1 + ("を追加しました"));
                    nowList.Add(map.MapDatas[routes[driverID][i][0]][routes[driverID][i][1]]);
                    nowList.Add(map.MapDatas[routes[driverID][i][0] + 1][routes[driverID][i][1]]);
                    nowList.Add(map.MapDatas[routes[driverID][i][0] - 1][routes[driverID][i][1]]);
                    nowList.Add(map.MapDatas[routes[driverID][i][0]][routes[driverID][i][1] + 1]);
                    nowList.Add(map.MapDatas[routes[driverID][i][0]][routes[driverID][i][1] - 1]);
                    nowList.RemoveAll(x => lastList.Contains(x));
                    for (int c = 0; c < nowList.Count; c++)
                    {
                        deliveryData[driverID].Add(nowList[c].objectID);
                        delivery.Add(nowList[c].name);
                    }
                    lastList = nowList;
                    nowList = new List<MapData>();
                    //Debug.Log(ColorChanger(map.MapDatas))
                }

                Vector3 dir = ((routesPosition[driverID][i] + map.transform.localPosition) - obj.transform.position).normalized;
                Vector3 lastDirction = dir;
                while (lastDirction == dir)
                {
                    while (breaking[driverID])
                    {
                        if (recovering[driverID])
                        {
                            yield break;
                        }
                        yield return null;
                    }
                    lastDirction = dir;
                    Vector3 vec = lastDirction * Time.deltaTime;
                    if (dir.x == 1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                    }
                    if (dir.x == -1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    }
                    if (dir.y == 1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                    }
                    if (dir.y == -1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                    }
                    obj.transform.position += vec / speed[driverID]*clownSpeed;
                    dir = ((routesPosition[driverID][i] + map.transform.localPosition) - obj.transform.position).normalized;
                    yield return null;
                }
                obj.transform.position = routesPosition[driverID][i] + map.transform.localPosition;
                md = map.MapDatas[routes[driverID][i][0]][routes[driverID][i][1]];
            }
            else
            {
                bool dirSetted = false;
                MapData randomMd = new MapData();
                lastIsConfison = isConfison[driverID];
                while (!dirSetted)
                {

                    string[] randomData = { "TOP", "RIGHT", "LEFT", "BOTTOM" };
                    string dirction = randomData[Random.Range(0, randomData.Length)];
                    switch (dirction)
                    {
                        case "TOP":
                            {
                                if (map.MapDatas[md.heightPositionID - 1][md.widthPositionID].objectID >= (int)MapObjectID.STRAIGHT && map.MapDatas[md.heightPositionID - 1][md.widthPositionID].objectID <= (int)Map.MapObjectID.CROSS)
                                {
                                    randomMd = map.MapDatas[md.heightPositionID - 1][md.widthPositionID];
                                    dirSetted = true;
                                }
                                break;
                            }
                        case "RIGHT":
                            {
                                if (map.MapDatas[md.heightPositionID][md.widthPositionID + 1].objectID >= (int)MapObjectID.STRAIGHT && map.MapDatas[md.heightPositionID][md.widthPositionID + 1].objectID <= (int)Map.MapObjectID.CROSS)
                                {
                                    randomMd = map.MapDatas[md.heightPositionID][md.widthPositionID + 1];
                                    dirSetted = true;
                                }
                                break;
                            }
                        case "LEFT":
                            {
                                if (map.MapDatas[md.heightPositionID][md.widthPositionID - 1].objectID >= (int)MapObjectID.STRAIGHT && map.MapDatas[md.heightPositionID][md.widthPositionID - 1].objectID <= (int)Map.MapObjectID.CROSS)
                                {
                                    randomMd = map.MapDatas[md.heightPositionID][md.widthPositionID - 1];
                                    dirSetted = true;
                                }
                                break;
                            }
                        case "BOTTOM":
                            {
                                if (map.MapDatas[md.heightPositionID + 1][md.widthPositionID].objectID >= (int)MapObjectID.STRAIGHT && map.MapDatas[md.heightPositionID + 1][md.widthPositionID].objectID <= (int)Map.MapObjectID.CROSS)
                                {
                                    randomMd = map.MapDatas[md.heightPositionID + 1][md.widthPositionID];
                                    dirSetted = true;
                                }
                                break;
                            }

                    }

                }

                md = randomMd;
                string[] objectInfo = md.name.Split("_");
                Vector3 endPos = md.obj.transform.localPosition;
                if (map.MapDatas[routes[driverID][i][0]][routes[driverID][i][1]].objectID < (int)MapObjectID.HOUSE_1)
                {
                    // Debug.Log(deliveryData[driverID].Count - 1 + ("を追加しました"));
                    string[] str = map.MapDatas[routes[driverID][i][0]][routes[driverID][i][1]].obj.transform.parent.gameObject.name.Split("_");
                    Debug.Log(str[1]);
                    passArea[driverID].Add(int.Parse(str[1]));
                    passArea.Distinct();
                    nowList.Add(map.MapDatas[md.heightPositionID][md.widthPositionID]);
                    nowList.Add(map.MapDatas[md.heightPositionID + 1][md.widthPositionID]);
                    nowList.Add(map.MapDatas[md.heightPositionID - 1][md.widthPositionID]);
                    nowList.Add(map.MapDatas[md.heightPositionID][md.widthPositionID + 1]);
                    nowList.Add(map.MapDatas[md.heightPositionID][md.widthPositionID - 1]);
                    nowList.RemoveAll(x => lastList.Contains(x));
                    for (int c = 0; c < nowList.Count; c++)
                    {
                        deliveryData[driverID].Add(nowList[c].objectID);
                        delivery.Add(nowList[c].name);
                    }
                    lastList = nowList;
                    nowList = new List<MapData>();
                }
                //float elapsed = 0f;

                Vector3 dir = ((endPos + mapObject.transform.localPosition) - obj.transform.position).normalized;
                Vector3 lastDirction = dir;
                while (lastDirction == dir)
                {
                    while (breaking[driverID])
                    {
                        if (recovering[driverID])
                        {
                            yield break;
                        }
                        yield return null;
                    }
                    lastDirction = dir;
                    Vector3 vec = lastDirction * Time.deltaTime;
                    if (dir.x == 1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                    }
                    if (dir.x == -1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    }
                    if (dir.y == 1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                    }
                    if (dir.y == -1)
                    {
                        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                    }
                    obj.transform.position += vec / speed[driverID]*clownSpeed;
                    dir = ((endPos + map.transform.localPosition) - obj.transform.position).normalized;
                    yield return null;
                }
                obj.transform.position = endPos + mapObject.transform.localPosition;
                i--;
                nowList.Clear();
            }
        }
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Arrow");
        foreach (GameObject o in objs)
        {
            Destroy(o);
        }
        routes[driverID] = new List<int[]>();
        tmpRoutePosition[driverID] = new List<Vector3>();
        tmpRoutes[driverID] = new List<int[]>();

        routesPosition[driverID] = new List<Vector3>();
        line[driverID].positionCount = 0;
        routeObjectsID[driverID] = new List<int>();
        isItemSetting[driverID] = false;
        isProcessSetting[driverID] = false;
        isDriving[driverID] = false;
        driver[driverID].SetActive(false);
        yield return null;

    }



    IEnumerator Directions(int driver)
    {
        yield return null;
        if (routesPosition[driver].Count > 1)
        {
            for (int i = 0; i < routesPosition[driver].Count - 1; i++)
            {
                coroutineNumber[driver]++;
                frame = 0;
                StartCoroutine(ArrowMove(routesPosition[driver][i], routesPosition[driver][i + 1], coroutineNumber[driver], frame, driver));
            }
        }
        else
        {
            StartCoroutine(Directions(driver));
        }
    }

    IEnumerator ArrowMove(Vector3 startPosition, Vector3 endPosition, int coroutineID, int frameCount, int driverID)
    {
        startPosition += mapObject.transform.localPosition;

        GameObject obj = Instantiate(move, startPosition, Quaternion.identity);
        obj.transform.parent = arrows.transform;
        float dist = Mathf.Abs(endPosition.magnitude - obj.transform.position.magnitude);
        for (int i = 0; i < frameCount; i++)
        {
            Vector3 dirction = (endPosition + mapObject.transform.localPosition - obj.transform.position).normalized;
            Vector2 vec = obj.transform.position + dirction * Time.deltaTime;
            dist = Mathf.Abs((endPosition + mapObject.transform.localPosition).magnitude - obj.transform.position.magnitude);
            obj.transform.position = vec;
        }
        int lastX = 0;
        int lastY = 0;
        Vector3 dir = ((endPosition + mapObject.transform.localPosition) - obj.transform.position).normalized;
        Vector3 lastDirction = dir;
        while (true)
        {

            if (routesPosition[driverID].Count == 0 || obj == null)
            {
                break;
            }
            dir = (endPosition + mapObject.transform.localPosition - obj.transform.position).normalized;
            if (dir.x == 1)
            {
                if (lastX == -1) break;
                lastX = 1;
                obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            }
            if (dir.x == -1)
            {
                if (lastX == 1) break;
                lastX = -1;
                obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
            }
            if (dir.y == 1)
            {
                if (lastY == -1) break;
                lastY = 1;
                obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            }
            if (dir.y == -1)
            {
                if (lastY == 1) break;
                lastY = -1;
                obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            lastDirction = dir;
            Vector3 vec = lastDirction * Time.deltaTime;
            obj.transform.position += vec;
            dir = ((endPosition + mapObject.transform.localPosition) - obj.transform.position).normalized;
            if (endPosition + mapObject.transform.localPosition == obj.transform.position) break;
            yield return null;
        }
        Destroy(obj);
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Arrow");
        if (coroutineID == 1)
        {
            coroutineNumber[driverID] = 0;
            StartCoroutine(Directions(driverID));
        }
        foreach (GameObject arrow in objs)
        {
            Destroy(arrow);
        }
        if (obj != null) Destroy(obj);

    }
    public void WritingSwitch()
    {
        if (tutorialMG.IsTutorial && tutorialMG.CurrentState == TutorialMG.TutorialState.PushButton)
        {
            tutorialMG.ChangeState(TutorialMG.TutorialState.DrawLine);
        }
        if (driverType == -1) return;
        writing = !writing;

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
        deliveryItems[driverID] = tmpDeliveryItem;
        deliveryProcess[driverID] = tmpDeliveryProcess;
        if (driverType != -1)
        {
            if (routeObjectsID[driverType].Count > 0)
            {
                if (routeObjectsID[driverType][routeObjectsID[driverType].Count - 1] < (int)MapObjectID.HOUSE_1 && (int)MapObjectID.HOUSE_4 < routeObjectsID[driverType][routeObjectsID[driverType].Count - 1])
                {
                    Debug.Log("通過している");
                    routes[driverType] = new List<int[]>();
                    routesPosition[driverType] = new List<Vector3>();
                    line[driverType].positionCount = 0;
                    routeObjectsID[driverType] = new List<int>();
                    isItemSetting[driverType] = false;
                    isProcessSetting[driverType] = false;
                    memoring = false;
                    isDriving[driverType] = false;
                    driver[driverType].SetActive(false);
                }
            }
        }

        driverType = driverID;
        driverSet = true;
        tmpDeliveryProcess = -1;
        tmpDeliveryItem = -1;




    }

    public void DeliveryItemSetting(int deliveryItem)
    {
        tmpDeliveryItem = deliveryItem;

        tmpItemSetting = true;
    }

    public void DeliveryProcessSetting(int deliveryProcessID)
    {
        tmpDeliveryProcess = deliveryProcessID;
        for (int i = 0; i < driverCount; i++)
        {
            driverSetButtonColliders[i].enabled = true;
        }
        tmpProcessSetting = true;
    }

    private void DeliveryCompleted(GameObject obj, int driverType)
    {
        obj.GetComponent<Sinner>().ReceiptDeliveryInformation(deliveryItems[driverType], deliveryProcess[driverType], driverType);
    }
    public void DestinationSetting(GameObject obj)
    {
        tmpDestination = obj;
        tmpDestinationSetting = true;
    }

    [SerializeField] private float rotateSpeed;
    [SerializeField] private GameObject writeButtonBackGround;
    private IEnumerator WriteButtonMover()
    {
        //    if (driverType == -1)
        //    {
        //        StartCoroutine(WriteButtonMover());
        //        yield break;
        
        //    }
        while (true)
        {
            if (driverType != -1 && !writing)
            {
                writeButtonBackGround.transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
            }
            // フレームレートに依存しない回転

            yield return null; // 次のフレームまで待機
        }

        yield return null;
    }

    [SerializeField] float scaleMultiplier; // どれだけ大きくなるか
    [SerializeField] float scaleDuration;   // 大きくなる／戻る時間
    [SerializeField] float scaleInterval;   // 拡大処理の周期

    private Vector3 originalScale;
    IEnumerator ScaleCoroutine()
    {
        while (true)
        {
            if (driverType != -1 && !writing)
            {
                yield return new WaitForSeconds(scaleInterval);

                // 拡大開始地点を必ず originalScale にする
                writeButtonBackGround.transform.localScale = originalScale;

                // 拡大
                yield return StartCoroutine(ScaleFromTo(
                    originalScale,
                    originalScale * scaleMultiplier
                ));

                // 縮小（元に戻る）
                yield return StartCoroutine(ScaleFromTo(
                    originalScale * scaleMultiplier,
                    originalScale
                ));
            }
            else
            {
                yield return null;
            }
        }
    }

    IEnumerator ScaleFromTo(Vector3 startScale, Vector3 targetScale)
    {
        float elapsed = 0f;

        while (elapsed < scaleDuration)
        {
            writeButtonBackGround.transform.localScale = Vector3.Lerp(
                startScale,
                targetScale,
                elapsed / scaleDuration
            );

            elapsed += Time.deltaTime;
            yield return null;
        }

        writeButtonBackGround.transform.localScale = targetScale;
    }

    void DriverStateControler()
    {
        for (int i = 0; i < driverCount; i++)
        {
            if (breaking[i])
            {
                driverStateRenderer[i].sprite = driverStateSprite[3];
                waitText[i].SetActive(false);
                driveText[i].SetActive(false);
                confisonText[i].SetActive(false);
                breakText[i].SetActive(true);
            }
            else if (isConfison[i])
            {
                driverStateRenderer[i].sprite = driverStateSprite[2];
                waitText[i].SetActive(false);
                driveText[i].SetActive(false);
                confisonText[i].SetActive(true);
                breakText[i].SetActive(false);
            }
            else if (isDriving[i])
            {
                driverStateRenderer[i].sprite = driverStateSprite[1];
                waitText[i].SetActive(false);
                driveText[i].SetActive(true);
                confisonText[i].SetActive(false);
                breakText[i].SetActive(false);
            }
            else
            {
                driverStateRenderer[i].sprite = driverStateSprite[0];
                waitText[i].SetActive(true);
                driveText[i].SetActive(false);
                confisonText[i].SetActive(false);
                breakText[i].SetActive(false);
            }
        }
    }

  [SerializeField]  bool[] recovering = new bool[driverCount];
    private void Recovery(int driverID)
    {
        
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Arrow");
        foreach (GameObject o in objs)
        {
            Destroy(o);
        }
        driver[driverID].transform.localPosition = driverFirstPosition[driverID];
        recovering[driverID]=true;
        routes[driverID] = new List<int[]>();
        tmpRoutePosition[driverID] = new List<Vector3>();
        tmpRoutes[driverID] = new List<int[]>();
        routesPosition[driverID] = new List<Vector3>();
        line[driverID].positionCount = 0;
        routeObjectsID[driverID] = new List<int>();
        isItemSetting[driverID] = false;
        isProcessSetting[driverID] = false;
        isDriving[driverID] = false;
        breaking[driverID] = false;
        driver[driverID].SetActive(false);
    }
}
