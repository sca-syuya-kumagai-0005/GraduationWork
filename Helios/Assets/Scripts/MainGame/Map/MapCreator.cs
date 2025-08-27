using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
public class MapCreator : CsvReader
{
    [SerializeField] TextAsset mapCsv;
    [SerializeField] GameObject objectSpace;
    [SerializeField] GameObject[] mapObjects;
    List<List<MapData>> mapDatas = new List<List<MapData>>();
    public struct MapData//自身と隣接するブロックの情報を格納するstruct　自身と隣接するブロックに対応するフラグがtrue
    {
        public int objectID;
        public int positionID;
        public List<int> aroundTrashCansID;
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
            mapDatas.Add(new List<MapData>());
            for (int j = 0; j < data[i].Length; j++)
            {
                MapData md = new MapData();
                md.objectID = short.Parse(data[i][j]);
                md.positionID = i*data[i].Length+j;
                mapDatas[i].Add(md);
                GameObject obj = Instantiate(objectSpace,new Vector3(0+j,0-i,0),Quaternion.identity,transform);
                GameObject instObj = mapObjects[(int.Parse(data[i][j]))];
                Instantiate(instObj,obj.transform.position,Quaternion.identity,obj.transform);

            }
        }

    }
}