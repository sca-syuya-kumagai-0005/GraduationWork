using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class SinnerDistribute : MonoBehaviour
{
    private const string tileID_House = "9";
    private const string underBar = "_";
    private List<Object> components = new List<Object>() 
    { 
        new ItemID_001(),
        new ItemID_002(),
        new ItemID_004(),
        new ItemID_005(),
    };
    private const int maxSinners = 31;
    private List<GameObject> houseList = new List<GameObject>();
    [SerializeField]
    private bool[] stayed = new bool[maxSinners];
    private int standbySinners;
    private SaveDataManager saveDataManager;
    private void Start()
    {
        saveDataManager = GameObject.Find("SaveManager").GetComponent<SaveDataManager>();
        standbySinners = saveDataManager.Days;
        stayed = saveDataManager.StayedSinner;
        houseList = GetHouse();
        for (int i = 0; i < standbySinners;)
        {
            if (standbySinners > components.Count) break;
            int rand = Random.Range(0, components.Count);
            if (!stayed[rand])i++;
            stayed[rand] = true;
            standbySinners--;
        }
        Distribute();
        saveDataManager.StayedSinner = stayed;
    }
    private List<GameObject> GetHouse()
    {
        //ここに各マップ
        const string map = "Map";
        GameObject mapObject = GameObject.Find(map);
        List<GameObject> houseList = new List<GameObject>();
        for (int i = 0; i < mapObject.transform.childCount; i++)
        {
            GameObject go = mapObject.transform.GetChild(i).gameObject;
            string[] tileName = go.name.Split(underBar);
            const int arrayTop = 0;
            if (tileName[arrayTop] == tileID_House) houseList.Add(go);
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

                go.AddComponent(components[i].GetType());
                go.GetComponent<MapObjectReturnName>().HaveSinner = true;
                Debug.Log(components[i].GetType() + "出現：" + go.name);
            }
        }
    }
    private void Distribute(int sinnerID)
    {
        int rand = Random.Range(0, houseList.Count);
        GameObject go = houseList[rand];
        houseList.RemoveAt(rand);

        go.AddComponent(components[sinnerID].GetType());
        go.GetComponent<MapObjectReturnName>().HaveSinner = true;
        Debug.Log(components[sinnerID].GetType() + "出現：" + go.name);
        components.RemoveAt(sinnerID);
        stayed[sinnerID] = true;
    }
}
