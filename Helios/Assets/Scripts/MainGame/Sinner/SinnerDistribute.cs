using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class SinnerDistribute : MonoBehaviour
{
    [SerializeField]
    private GameObject sinnerObject;
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
    private List<GameObject>[] sinnerHousedObjects = new List<GameObject>[10];
    public List<GameObject>[] GetSinnerHousedObjects { get { return sinnerHousedObjects; } }
    private List<int>[] sinnerPools = new List<int>[poolSize]
    {
        new List<int> { 2,4, 5, 6, 7, 8, 9,12,13,23},
        new List<int> { 1,3,16,10,11,14,19,20,17,18},
        new List<int> { 21},
        //new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
        //new List<int> { 11,12,13,14,15,16,17,18,19,20 },
        //new List<int> { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 },
        //new List<int> { 21,22,23,24,25,26,27,28,29,30 }
    };
    private const int maxSinners = 31;
    private List<GameObject>[] houseList = new List<GameObject>[poolSize]
    {
        new List<GameObject>(),
        new List<GameObject>(),
        new List<GameObject>(),
    };
    private List<int> totalSinnerPool = new List<int>();
    private bool[] housed = new bool[maxSinners];
    private int standbySinners;
    private SaveDataManager saveDataManager;
    private TutorialMG tutorialMG;

    //各区画に住むシナーを入れるリスト
    private List<int>[] plotContainedSinner = new List<int>[10];
    public List<int>[] GetPlotContainedSinnerID { get { return plotContainedSinner; } }
    const string mapName = "Address_";

    [Header("Tutorial Fixed House Settings")]
    [SerializeField] private string tutorialHouseA = "9_26_52";

    [SerializeField] private int tutorialSinnerA = 4;

    private void Start()
    {
        tutorialMG = GameObject.Find("TutorialMG").GetComponent<TutorialMG>();
        saveDataManager = GameObject.Find("SaveManager").GetComponent<SaveDataManager>();　
       
       
       
        for (int i = 0; i < plotContainedSinner.Length; i++)
        {
            plotContainedSinner[i] = new List<int>();
            sinnerHousedObjects[i] = new List<GameObject>();
        }
        if (tutorialMG.IsTutorial)//名前間違てるねごめん
        {
            // セーブ使いたくないんで
            housed = new bool[maxSinners];
            DistributeTutorial();
        }
        else
        {
            housed = saveDataManager.HousedSinner;
            standbySinners = saveDataManager.Days + 1;
            int gamePhase = GetGamePhase(saveDataManager.Days);
            Distribute(gamePhase);
            saveDataManager.HousedSinner = housed;
        }

       
    }

    private void Distribute(int gamePhase)
    {
        for (int i = 0; i < sinnerPools.Length; i++)
        {
            totalSinnerPool.AddRange(sinnerPools[i]);
        }
        //ここに各マップ
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
                    List<string> mapTile_House = new List<string>
                    {
                    ((int)Map.MapObjectID.HOUSE_1).ToString(),
                    ((int)Map.MapObjectID.HOUSE_2).ToString(),
                    ((int)Map.MapObjectID.HOUSE_3).ToString(),
                    ((int)Map.MapObjectID.HOUSE_4).ToString(),
                    };
                    const string underBar = "_";
                    if (mapTile_House.Contains(go.transform.GetChild(j).name.Split(underBar)[0]))
                    {
                        houseList[gp].Add(go.transform.GetChild(j).gameObject);
                    }
                }
            }
        }

        for (int i = 0; i < housed.Length; i++)
        {
            if (housed[i])
            {
                //合算プールの何番目か
                int sinnerID = totalSinnerPool.IndexOf(i + 1);
                int gp = GetGamePhase(sinnerID);
                HousedExistingSinner(gp, sinnerID);
            }
        }

        for (int i = 0; i < standbySinners; i++)
        {
            HousedNewSinner(gamePhase);
        }
    }
    private void HousedExistingSinner(int phase, int housedSinnerID)
    {
        if (houseList[phase].Count == 0) return;
        int rand = Random.Range(0, houseList[phase].Count);
        houseList[phase][rand].AddComponent(sinnerComponents[totalSinnerPool[housedSinnerID] - 1].GetType());
        GameObject sinnerHousedObject = houseList[phase][rand];

        string plot = houseList[phase][rand].transform.parent.name;
        Debug.Log(plot + "に" + sinnerComponents[totalSinnerPool[housedSinnerID] - 1].GetType() + "続けて出現");
        standbySinners--;

        int plotNumber = int.Parse(plot.Split(mapName)[1]);
        plotContainedSinner[plotNumber].Add(totalSinnerPool[housedSinnerID]);
        sinnerHousedObjects[plotNumber].Add(sinnerHousedObject);

        sinnerPools[phase].Remove(totalSinnerPool[housedSinnerID]);
        houseList[phase].RemoveAt(rand);
    }

    private void HousedNewSinner(int phase)
    {
        if (sinnerPools[phase].Count == 0) return;
        if (houseList[phase].Count == 0) return;
        int rand_sinner = Random.Range(0, sinnerPools[phase].Count);
        int rand_house = Random.Range(0, houseList[phase].Count);
        int componentID = sinnerPools[phase][rand_sinner];
        houseList[phase][rand_house].AddComponent(sinnerComponents[componentID - 1].GetType());
        GameObject sinnerHousedObject = houseList[phase][rand_house];

        string plot = houseList[phase][rand_house].transform.parent.name;
        Debug.Log(plot + "に" + sinnerComponents[componentID-1].GetType() + "初めて出現");
        housed[rand_sinner] = true;

        int plotNumber = int.Parse(plot.Split(mapName)[1]);
        plotContainedSinner[plotNumber].Add(sinnerPools[phase][rand_sinner]);
        sinnerHousedObjects[plotNumber].Add(sinnerHousedObject);

        sinnerPools[phase].Remove(componentID);
        houseList[phase].RemoveAt(rand_house);
    }

    private int GetGamePhase(int number)
    {
        int gamePhase = 0;
        if (0 <= number && number <= 9)
        {
            gamePhase = 0;
        }
        else if (10 <= number && number <= 19)
        {
            gamePhase = 1;
        }
        else if (20 <= number && number < 30)
        {
            gamePhase = 2;
        }
        else gamePhase = 3;

        return gamePhase;
    }

    private void DistributeTutorial()
    {
        Debug.Log("チュートリアル用のシナーを固定配置します");

        PlaceTutorialSinnerToHouseName(tutorialSinnerA, tutorialHouseA);
    }


    private void PlaceTutorialSinnerToHouseName(int sinnerID, string houseName)
    {
        GameObject house = GameObject.Find(houseName);

        if (house == null)
        {
            Debug.LogError($"[Tutorial] House '{houseName}' が見つかりません");
            return;
        }

        // すでにシナーがいるかチェック
        foreach (var comp in sinnerComponents)
        {
            if (house.GetComponent(comp.GetType()) != null)
            {
                Debug.LogWarning($"[Tutorial] {houseName} にはすでにシナーがいます");
                return;
            }
        }

        // シナー配置
        house.AddComponent(
            sinnerComponents[sinnerID - 1].GetType()
        );

        housed[sinnerID - 1] = true;

        // Plot番号取得（親が Address_x）
        string plotName = house.transform.parent.name;
        int plotNumber = int.Parse(plotName.Replace(mapName, ""));

        plotContainedSinner[plotNumber].Add(sinnerID);
        sinnerHousedObjects[plotNumber].Add(house);

        Debug.Log($"[Tutorial] {houseName} に Sinner {sinnerID} を固定配置");
    }

}
