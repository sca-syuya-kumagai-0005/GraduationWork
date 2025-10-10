using UnityEngine;
using System.Collections.Generic;
using static KumagaiLibrary.Unity.CsvManager;

/*memo=========================================================================================
 * 配達する側のScriptで、通った道の隣接するオブジェクトの取得とカウントをする
 ==============================================================================================*/

public class Map : CsvReader
{
    [SerializeField] TextAsset mapCsv;
    [SerializeField] GameObject objectSpace;
    [SerializeField] GameObject[] mapObjects;
    private char underbar = '_';
    private List<List<MapData>> mapDatas = new List<List<MapData>>();
    public List<List<MapData>> MapDatas { get { return mapDatas;} }
    public struct MapData//自身と隣接するブロックの情報を格納するstruct　自身と隣接するブロックに対応するフラグがtrue
    {
        public int objectID;
        public int widthPositionID;
        public int heightPositionID;    
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        List<string[]> data=Read(mapCsv);
        MapCreate(data);
        this.gameObject.transform.position = new Vector3(-10, 10, 0);
    }
    void MapCreate(List<string[]> data)//マップの生成。mapDatasのobjectIDとpositionIDもここで設定
    {
        for (int i = 0; i < data.Count; i++)
        {
            mapDatas.Add(new List<MapData>());
            for (int j = 0; j < data[i].Length; j++)
            {
                string[] strs = data[i][j].Split(underbar);
                MapData md = new MapData();
                md.objectID = int.Parse(strs[0]);
                md.widthPositionID = j;
                md.heightPositionID = i;
                mapDatas[i].Add(md);
                GameObject obj = Instantiate(objectSpace,new Vector3(0+j,0-i,0),Quaternion.identity,transform);
                GameObject instObj = mapObjects[(int.Parse(strs[0]))];
                instObj = Instantiate(instObj,obj.transform.position,Quaternion.identity,obj.transform);
                if(strs.Length>1) instObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, float.Parse(strs[1])));
                instObj.transform.parent.name = md.objectID.ToString()+underbar+md.widthPositionID.ToString()+underbar+md.heightPositionID.ToString();
            }
        }
    }

}