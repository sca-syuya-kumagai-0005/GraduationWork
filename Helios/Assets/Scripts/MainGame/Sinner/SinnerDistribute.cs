using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class SinnerDistribute : MonoBehaviour
{
    private const int poolSize = 3;
    private List<Object> sinnerComponents = new List<Object>()
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
    private List<int>[] sinnerPools = new List<int>[poolSize]
    {
        new List<int> { 2,4,5 },
        new List<int> { 3,6,7 },
        new List<int> { 1,8 },
    };
    private const int maxSinners = 31;
    private List<GameObject>[] houseList = new List<GameObject>[poolSize]
    {
        new List<GameObject>(),
        new List<GameObject>(),
        new List<GameObject>(),
    };
    [SerializeField]
    private bool[] housed = new bool[maxSinners];
    private int standbySinners;
    private SaveDataManager saveDataManager;
    private void Start()
    {
        saveDataManager = GameObject.Find("SaveManager").GetComponent<SaveDataManager>();
        housed = saveDataManager.HousedSinner;
        standbySinners = saveDataManager.Days + 1;
        int gamePhase = 0;
        if (0 <= saveDataManager.Days && saveDataManager.Days < 10)
        {
            gamePhase = 0;
        }
        else if (10 <= saveDataManager.Days && saveDataManager.Days < 20)
        {
            gamePhase = 1;
        }
        else if (20 <= saveDataManager.Days && saveDataManager.Days < 30)
        {
            gamePhase = 2;
        }
        else gamePhase = 3;

        Distribute(gamePhase);
        saveDataManager.HousedSinner = housed;
    }

    private void Distribute(int gamePhase)
    {
        //ここに各マップ
        const string mapName = "Address_";
        int[][] plotIDs = new int[4][]
        {
            new int[2]{0,0},
            new int[2]{1,4},
            new int[2]{5,8},
            new int[2]{9,9},
        };
        for (int gp = 0; gp <= gamePhase; gp++)
        {
            for (int i = plotIDs[gp][0]; i <= plotIDs[gp][1]; i++)
            {
                GameObject go = GameObject.Find(mapName + i);
                for (int j = 0; j < go.transform.childCount; j++)
                {
                    const string tileID_House = "9";
                    const string underBar = "_";
                    if (go.transform.GetChild(j).name.Split(underBar)[0] == tileID_House)
                        houseList[gp].Add(go.transform.GetChild(j).gameObject);
                }
            }

            for (int i = 0; i < housed.Length; i++)
            {
                if (sinnerPools[gp].Count == 0) break;
                if (housed[i])
                {
                    HousedNewSinner(gp, i, mapName + i);

                }
            }
        }

        for (int i = 0; i < standbySinners; i++)
        {
            HousedNewSinner(gamePhase, mapName + i);

        }
    }

    private void HousedNewSinner(int phase, string map)
    {
        if (sinnerPools[phase].Count == 0) return;
        if (houseList[phase].Count == 0) return;
        int rand_sinner = Random.Range(0, sinnerPools[phase].Count);
        int rand_house = Random.Range(0, houseList[phase].Count);
        int componentID = sinnerPools[phase][rand_sinner];
        houseList[phase][rand_house].AddComponent(sinnerComponents[componentID - 1].GetType());
        houseList[phase].RemoveAt(rand_house);
        sinnerPools[phase].RemoveAt(rand_sinner);
        housed[rand_sinner] = true;
        Debug.Log(map + "に" + componentID + "出現");
    }
    private void HousedNewSinner(int phase, int stayedSinnerID, string map)
    {
        if (sinnerPools[phase].Count <= stayedSinnerID) return;
        if (houseList[phase].Count == 0) return;
        int rand = Random.Range(0, houseList[phase].Count);
        int componentID = sinnerPools[phase][stayedSinnerID];
        houseList[phase][rand].AddComponent(sinnerComponents[componentID - 1].GetType());
        houseList[phase].RemoveAt(rand);
        standbySinners--;
        Debug.Log(map + "に" + componentID + "出現");
    }
}
