using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class SinnerDistribute : MonoBehaviour
{
    private int days;
    private const string tileID_House = "3";
    private const string underBar = "_";
    private List<Object> components = new List<Object>() 
    { 
        //new ItemID_001(),
        new ItemID_002(),
        new ItemID_004(),
        new ItemID_005(),
    };
    
    private void Start()
    {
        days = 1;
        Distribute();
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
            if (tileName[0] == tileID_House) houseList.Add(go);
        }
        return houseList;
    }
    private void Distribute()
    {
        List<GameObject> houseList = GetHouse();
        for (int i = 0; i < days; i++)
        {
            int rand = Random.Range(0, houseList.Count);
            GameObject go = houseList[rand];

            rand = Random.Range(0, components.Count);
            go.AddComponent(components[rand].GetType());
            go.GetComponent<MapObjectReturnName>().HaveSinner = true;
            Debug.Log(components[rand].GetType() + "出現：" + go.name);
            components.Remove(components[rand]);
        }
    }
}
