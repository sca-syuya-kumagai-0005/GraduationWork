using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class SinnerDistribute : MonoBehaviour
{
    private const string tileID_House = "9";
    private const string underBar = "_";
    private const int poolSize = 3;
    private List<Object> component = new List<Object>()
    {
        new ItemID_001(),
        new ItemID_002(),
        new ItemID_003(),
        new ItemID_004(),
        new ItemID_005(),
        new ItemID_006(),
        new ItemID_007(),
        new ItemID_008(),

    };
    private List<int>[] sinnerPools=new List<int>[poolSize];
    private const int maxSinners = 31;
    private List<GameObject> houseList = new List<GameObject>();
    [SerializeField]
    private bool[] stayed = new bool[maxSinners];
    private int standbySinners;
    private SaveDataManager saveDataManager;
    private void Start()
    {
        sinnerPools[0] = new List<int>
        {
            2,
            4,
            5
        };
        sinnerPools[1] = new List<int>
        {
            3,
            6,
            7
        };
        sinnerPools[2] = new List<int>
        {
            1,
            8
        };

        saveDataManager = GameObject.Find("SaveManager").GetComponent<SaveDataManager>();
        stayed = saveDataManager.StayedSinner;
        standbySinners = saveDataManager.Days + 1;
        standbySinners -= stayed.Count(b => b);
        standbySinners = 1;
        int gamePhase = 0;
        if (0 <= saveDataManager.Days && saveDataManager.Days < 10)
        {
            gamePhase = 1;
        }
        else if(10<=saveDataManager.Days && saveDataManager.Days < 20)
        {
            gamePhase = 2;
        }
        else if(20<=saveDataManager.Days&& saveDataManager.Days < 30)
        {
            gamePhase = 3;
        }
        else gamePhase = 4;

        for (int i = 0; i < gamePhase; i++)
        {
            houseList = GetHouse(i);

            for (int j = 0; j < standbySinners; j++)
            {
                int rand = Random.Range(0, sinnerPools[i].Count);
                int selectedSinnerID = sinnerPools[i][rand] - 1;
                stayed[selectedSinnerID] = true;
                sinnerPools[i].RemoveAt(rand);
            }
            Distribute();
        }

        saveDataManager.StayedSinner = stayed;
    }
    private List<GameObject> GetHouse(int gamePhase)
    {
        //ここに各マップ
        const string mapName = "Address_0";
        int[] mapIDs = new int[2];
        switch (gamePhase)
        {
            case 0: mapIDs = new int[] { 0, 0 }; break;
            case 1: mapIDs = new int[] { 1, 4 }; break;
            case 2: mapIDs = new int[] { 5, 8 }; break;
            case 4: mapIDs = new int[] { 9, 9 }; break;
        }
        
        GameObject mapObject = null;
        List<GameObject> houseList = new List<GameObject>();
        int iMax = mapIDs[1] - mapIDs[0];
        for (int i = 0; i <= iMax; i++)
        {
            int id = mapIDs[i];
            mapObject = GameObject.Find(mapName + id);
            Debug.Log(mapObject.name);
            for (int j = 0; j < mapObject.transform.childCount; j++)
            {
                GameObject go = mapObject.transform.GetChild(j).gameObject;
                string[] tileName = go.name.Split(underBar);
                const int arrayTop = 0;
                if (tileName[arrayTop] == tileID_House) houseList.Add(go);
            }
        }

        return houseList;
    }
    private void Distribute()
    {
        for(int i = 0; i < stayed.Length; i++)
        {
            if (stayed[i])
            {
                int rand = Random.Range(0, houseList.Count);
                GameObject go = houseList[rand];
                houseList.RemoveAt(rand);

                go.AddComponent(component[i].GetType());
                go.GetComponent<MapObjectRequest>().HaveSinner = true;
                Debug.Log(component[i].GetType() + "出現：" + go.name);
            }
        }
    }
}
