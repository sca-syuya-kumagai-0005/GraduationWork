using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class SinnerDistribute : MonoBehaviour
{
    [SerializeField,Range(1,31)]
    private const string tileID_House = "3";
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
    private bool[] stayed = new bool[maxSinners];
    private int standbySinners;
    private SaveDataManager saveDataManager;
    private void Start()
    {
        saveDataManager = GameObject.Find("SaveManager").GetComponent<SaveDataManager>();
        standbySinners = saveDataManager.Days;
        stayed = saveDataManager.StayedSinner;
        houseList = GetHouse();
        Distribute(stayed);
        for (int i = 0; i < standbySinners; i++)
        {
            int rand = Random.Range(0, components.Count);
            Distribute(rand);
        }
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
    private void Distribute(bool[] stayed)
    {
        List<int> sinnerIDList = new List<int>();
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
                standbySinners--;
                sinnerIDList.Add(i);
                this.stayed[i] = true;
            }
        }
        for(int i = 0; i < sinnerIDList.Count; i++) components.RemoveAt(i);
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
        stayed[rand] = true;
    }
}
