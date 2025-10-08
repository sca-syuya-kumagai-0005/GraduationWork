using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class SinnerDistribute : MonoBehaviour
{
    [SerializeField,Range(1,31)]
    private int days;
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
    private void Start()
    {
        int standbySinner = days;
        houseList = GetHouse();
        for (int i = 0; i < stayed.Length; i++)
        {
            if (stayed[i])
            {
                standbySinner--;
                Distribute(i);
            }
        }
        for (int i = 0; i < standbySinner; i++)
        {
            int rand = Random.Range(0, components.Count);
            Distribute(rand);
        }
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
    private void Distribute(int SinnerID)
    {
        int rand = Random.Range(0, houseList.Count);
        GameObject go = houseList[rand];
        houseList.RemoveAt(rand);

        go.AddComponent(components[SinnerID].GetType());
        go.GetComponent<MapObjectReturnName>().HaveSinner = true;
        Debug.Log(components[SinnerID].GetType() + "出現：" + go.name);
        components.RemoveAt(SinnerID);
        stayed[rand] = true;
    }
}
