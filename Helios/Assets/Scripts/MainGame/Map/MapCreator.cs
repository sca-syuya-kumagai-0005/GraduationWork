using UnityEngine;
using System.Collections.Generic;
public class MapCreator : CsvReader
{
    [SerializeField] TextAsset mapData;
    [SerializeField] GameObject objectSpace;
    [SerializeField] GameObject[] mapObjects;
    [SerializeField] GameObject[] map;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        List<string[]> data=Read(mapData);
        MapCreate(data);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MapCreate(List<string[]> data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            for (int j = 0; j < data[i].Length; j++)
            {
                GameObject obj = Instantiate(objectSpace,new Vector3(0+j,0+i,0),Quaternion.identity,transform);
                GameObject instObj = mapObjects[(int.Parse(data[i][j]))];
                Instantiate(instObj,obj.transform.position,Quaternion.identity,obj.transform);

            }
        }

    }
}