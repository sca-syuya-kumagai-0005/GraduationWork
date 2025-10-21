using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class SinnerDistribute : MonoBehaviour
{
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
    private List<int>[] sinnerPools = new List<int>[poolSize];
    private const int maxSinners = 31;
    private List<GameObject>[] houseList = new List<GameObject>[poolSize];
    [SerializeField]
    private bool[] stayed = new bool[maxSinners];
    private int standbySinners;
    private SaveDataManager saveDataManager;
    private void Start()
    {
        sinnerPools[0] = new List<int>
        {
            1,3,6,7,


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
        int gamePhase = 0;
        if (0 <= saveDataManager.Days && saveDataManager.Days < 10)
        {
            gamePhase = 1;
        }
        else if (10 <= saveDataManager.Days && saveDataManager.Days < 20)
        {
            gamePhase = 2;
        }
        else if (20 <= saveDataManager.Days && saveDataManager.Days < 30)
        {
            gamePhase = 3;
        }
        else gamePhase = 4;

        Distribute(gamePhase);
        saveDataManager.StayedSinner = stayed;
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
        List<GameObject>[] houseList = new List<GameObject>[gamePhase];
        for(int gp = 0; gp < gamePhase; gp++)
        {
            for (int i = plotIDs[gp][0]; i < plotIDs[gp][1]; i++)
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
           //各建物リストにそれぞれの区画の建物を入れた
           //リストから建物に振り分ける
        }



        for (int i = 0; i < stayed.Length; i++)
        {
            if (stayed[i])
            {
                int rand = Random.Range(0, houseList[gamePhase].Count);
                GameObject go = houseList[gamePhase][rand];
                houseList[gamePhase].RemoveAt(rand);

                go.AddComponent(component[i].GetType());
                go.GetComponent<MapObjectRequest>().HaveSinner = true;
                Debug.Log(component[i].GetType() + "出現：" + go.name);
            }
        }
    }
}
