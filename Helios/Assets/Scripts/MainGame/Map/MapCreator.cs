using UnityEngine;
using System.Collections.Generic;
public class MapCreator : CsvReader
{
    [SerializeField] TextAsset mapCsv;
    [SerializeField] GameObject objectSpace;
    [SerializeField] GameObject[] mapObjects;
    List<int[]> mapDatas = new List<int[]>();   

    struct MapData//自身と隣接するブロックの情報を格納するstruct　自身と隣接するブロックに対応するフラグがtrue
    {
        bool dust;
        bool light;
        bool home;
        
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        List<string[]> data=Read(mapCsv);
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
                GameObject obj = Instantiate(objectSpace,new Vector3(0+j,0-i,0),Quaternion.identity,transform);
                GameObject instObj = mapObjects[(int.Parse(data[i][j]))];
                Instantiate(instObj,obj.transform.position,Quaternion.identity,obj.transform);

            }
        }

    }
}