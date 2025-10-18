using UnityEngine;
using System.Collections.Generic;
using static KumagaiLibrary.Unity.CsvManager;
using Unity.VisualScripting;

/*memo=========================================================================================
 * 配達する側のScriptで、通った道の隣接するオブジェクトの取得とカウントをする
 ==============================================================================================*/

public class Map : MonoBehaviour
{
    [SerializeField] TextAsset[] mapCsv;
    [SerializeField] GameObject objectSpace;
    [SerializeField] GameObject[] mapObjects;
    private char underbar = '_';
    private int mapNumber;
    private const int ADDRES_MAX = 9;
    private int mapWidth;
    private int mapHeight;
    private GameObject[] plot = new GameObject[ADDRES_MAX];
  

    private List<List<MapData>> mapDatas = new List<List<MapData>>();
    public List<List<MapData>> MapDatas { get { return mapDatas;} }
    public struct MapData//自身と隣接するブロックの情報を格納するstruct　自身と隣接するブロックに対応するフラグがtrue
    {
        public int objectID;
        public int widthPositionID;
        public int heightPositionID;    
    }

    public  enum MapObjectID
    {
        NULL = 0,
        COMPANY = 1,
        STRAIGHT = 2,
        CORNER = 3,
        TJUNCTION = 4,
        CROSS = 5,
        TREE_1 = 6,
        TREE_2S = 7,
        TREE_3 = 8,
        HOUSE_1 = 9,
        HOUSE_2 = 10,
        HOUSE_3 = 11,
        HOUSE_4 = 12,
        APARTMENT_1 = 13,
        APARTMENT_2 = 14,
        APARTMENT_3 = 15,
        APAETMENT_4 = 16,
        SHIRINE = 17,
        SCHOOL = 18,
        AQUARIUM = 19,
    }

    private MapObjectID id;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        List<string[]> data = Read(mapCsv[0]);
        mapWidth = data.Count;
        mapHeight = data[0].Length;


        
        for(int i=0; i<ADDRES_MAX; i++)
        {
            GameObject address = new GameObject();
            address.transform.parent = transform;
            address.name = "Address" + underbar + i;
            if(i!=0)data = Read(mapCsv[1]);//後で変更　今は中心のマップ以外を0で埋められたCSVで代用
            MapCreate(data, i, address);
        }
            
        this.gameObject.transform.position = new Vector3(-31, -11, 0);//カメラ(マップ)の初期位置
    }
    void MapCreate(List<string[]> data,int mapNumber,GameObject address)//マップの生成。mapDatasのobjectIDとpositionIDもここで設定
    {
        int width=0;
        int height=0; 
        switch(mapNumber)
        {
            case 0://真ん中
                {
                    width = mapWidth;
                    height = mapHeight; 
                    break;
                }
            case 1://上
                {
                    width = mapWidth;
                    height = 0;
                    break;
                }

            case 2://右
                {
                    width = mapWidth*2;
                    height = mapHeight;
                    break;
                }
            case 3://下
                {
                    width = mapWidth;
                    height = mapHeight*2;
                    break;
                }
            case 4://左
                {
                    width = 0;
                    height = mapHeight;
                    break;
                }
            case 5://右上
                {
                    width = mapWidth*2;
                    height = 0; 
                    break;
                }
            case 6://右下
                {
                    width = mapWidth * 2;
                    height = mapHeight*2;
                    break;
                }
            case 7://左下
                {
                    width = 0;
                    height =mapHeight * 2;
                    break;
                }
            case 8://左上
                {
                    width = 0;
                    height = 0;
                    break;
                }
            //case 9://左上
            //    {
            //        width = -mapWidth;
            //        height = mapHeight;
            //        break;
            //    }

            default:
                {
                    break;
                }
        }
     
        for (int i = 0; i < data.Count; i++)
        {
            mapDatas.Add(new List<MapData>());
            for (int j = 0; j < data[i].Length; j++)
            {
                string[] strs = data[i][j].Split(underbar);
                MapData md = new MapData();
                md.objectID = int.Parse(strs[0]);
                md.widthPositionID = j+width;
                md.heightPositionID = i+height;
                mapDatas[i].Add(md);
                GameObject obj = Instantiate(objectSpace,new Vector3(j+width,-i+height,0),Quaternion.identity,address.transform);
                GameObject instObj = mapObjects[(int.Parse(strs[0]))];
                instObj = Instantiate(instObj,obj.transform.position,Quaternion.identity,obj.transform);
                if(strs.Length>1) instObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, float.Parse(strs[1])));
                instObj.transform.parent.name = md.objectID.ToString()+underbar+md.widthPositionID.ToString()+underbar+md.heightPositionID.ToString();
            }
        }

    }

}