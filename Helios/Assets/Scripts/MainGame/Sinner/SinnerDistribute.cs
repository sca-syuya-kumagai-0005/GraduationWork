using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

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
        new ItemID_009(),
        new ItemID_010(),
        new ItemID_011(),
        new ItemID_012(),
        new ItemID_013(),
        new ItemID_014(),
        new ItemID_015(),
        new ItemID_016(),
        new ItemID_017(),
        new ItemID_018(),
        new ItemID_019(),
        new ItemID_020(),
        new ItemID_021(),
        new ItemID_022(),
        new ItemID_023(),
    };
    private List<int>[] sinnerPools = new List<int>[poolSize]
    {
        //new List<int> { 2,4,5,12,13,16,17,18,2,2 },
        //new List<int> { 3,6,7,11,19,20,3,3,3,3 },
        //new List<int> { 1,8,9,10,14 },
        new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, },
        new List<int> { 11,12,13,14,15,16,17,18,19,20 },
        new List<int> { 21,22,23,24,25,26,27,28,29,30 }
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
    [SerializeField]private int standbySinners;
    private SaveDataManager saveDataManager;
    private void Start()
    {
        saveDataManager = GameObject.Find("SaveManager").GetComponent<SaveDataManager>();
        housed = saveDataManager.HousedSinner;
        standbySinners = saveDataManager.Days + 1;
        int gamePhase = 0;
        if (0 <= saveDataManager.Days && saveDataManager.Days < 9)
        {
            gamePhase = 0;
        }
        else if (10 <= saveDataManager.Days && saveDataManager.Days < 19)
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
                    const int num = (int)Map.MapObjectID.HOUSE_1;
                    string tileID_House = num.ToString();
                    const string underBar = "_";
                    if (go.transform.GetChild(j).name.Split(underBar)[0] == tileID_House)
                    {
                        houseList[gp].Add(go.transform.GetChild(j).gameObject);
                    }
                }
            }

            //これBoolの配列に記録するだけにして最後にまとめて住ませればいいのでは？
            for (int i = 0; i < housed.Length; i++)
            {
                if (sinnerPools[gp].Count == 0) break;
                if (housed[i])
                {
                    HousedNewSinner(gp, i);
                }
            }
        }

        for (int i = 0; i < standbySinners; i++)
        {
            HousedNewSinner(gamePhase);
        }
    }
    private void HousedNewSinner(int phase, int stayedSinnerID)
    {
        if (sinnerPools[phase].Count <= stayedSinnerID) return;
        if (houseList[phase].Count == 0) return;
        int rand = Random.Range(0, houseList[phase].Count);
        int componentID = sinnerPools[phase][stayedSinnerID];
        houseList[phase][rand].AddComponent(sinnerComponents[componentID - 1].GetType());
        Debug.Log(houseList[phase][rand] + "に" + componentID + "出現");
        houseList[phase].RemoveAt(rand);
        sinnerPools[phase].Remove(stayedSinnerID);
        standbySinners--;
    }

    private void HousedNewSinner(int phase)
    {
        if (sinnerPools[phase].Count == 0) return;
        if (houseList[phase].Count == 0) return;
        int rand_sinner = Random.Range(0, sinnerPools[phase].Count);
        int rand_house = Random.Range(0, houseList[phase].Count);
        int componentID = sinnerPools[phase][rand_sinner];
        houseList[phase][rand_house].AddComponent(sinnerComponents[componentID - 1].GetType());
        Debug.Log(houseList[phase][rand_house] + "に" + componentID + "出現");
        houseList[phase].RemoveAt(rand_house);
        sinnerPools[phase].RemoveAt(rand_sinner);
        housed[rand_sinner] = true;
    }
}
