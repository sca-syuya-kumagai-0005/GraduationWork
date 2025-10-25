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
    private const int ADDRES_MAX = 10;
    private const int OUTER = 2;
    private const int MAPWIDTH_MIN=21;
    private const int MAPHEIGHT_MIN=21;
    private const int MAPWIDTH_MAX = MAPWIDTH_MIN * 3;
    private const int MAPHEIGHT_MAX = MAPHEIGHT_MIN * 4;
    private int[] plotNumber = new int[ADDRES_MAX + OUTER] { 10, 9, 11, 8, 1, 5, 4, 0, 2, 7, 3, 6 };
    private GameObject[] plot = new GameObject[ADDRES_MAX];
   

    private MapData[][] mapDatas = new MapData[MAPHEIGHT_MAX][];
    public MapData[][] MapDatas { get { return mapDatas;} }
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < MAPHEIGHT_MAX;i++)
        {
            mapDatas[i]=new MapData[MAPWIDTH_MAX];
        }
     

   
        List<string[]> data = Read(mapCsv[0]);
        for(int i=0; i<ADDRES_MAX+OUTER; i++)
        {
            GameObject address = new GameObject();
            address.transform.parent = transform;
            address.name = "Address" + underbar + plotNumber[i];
            if(plotNumber[i]!=0)data = Read(mapCsv[1]);//後で変更　今は中心のマップ以外を0で埋められたCSVで代用
            else data = Read(mapCsv[0]);
            MapCreate(data, plotNumber[i], address);
        }

      
        for(int i=0;i<MAPHEIGHT_MAX;i++)
        {
            string str = "";
            for (int j = 0; j<MAPWIDTH_MAX; j++)
            {
                str += mapDatas[i][j].objectID.ToString();
            }

        }
        this.gameObject.transform.position = new Vector3(-30, 50, 0) ;//カメラ(マップ)の初期位置
    }
    void MapCreate(List<string[]> data,int mapNumber,GameObject address)//マップの生成。mapDatasのobjectIDとpositionIDもここで設定
    {
        int width=0;
        int height=0;
        
        switch (mapNumber)
        {
            case 0://真ん中
                {
                    width = MAPWIDTH_MIN;
                    height = MAPHEIGHT_MIN*2; 
                    break;
                }
            case 1://上
                {
                    width = MAPWIDTH_MIN;
                    height = MAPHEIGHT_MIN;
                    break;
                }

            case 2://右
                {
                    width = MAPWIDTH_MIN*2;
                    height = MAPHEIGHT_MIN*2;
                    break;
                }
            case 3://下
                {
                    width = MAPWIDTH_MIN;
                    height = MAPHEIGHT_MIN*3;
                    break;
                }
            case 4://左
                {
                    width = 0;
                    height = MAPHEIGHT_MIN * 2;
                    break;
                }
            case 5://右上
                {
                    width = MAPWIDTH_MIN*2;
                    height = MAPHEIGHT_MIN; 
                    break;
                }
            case 6://右下
                {
                    width = MAPWIDTH_MIN * 2;
                    height = MAPHEIGHT_MIN*3;
                    break;
                }
            case 7://左下
                {
                    width = 0;
                    height =MAPHEIGHT_MIN * 3;
                    break;
                }
            case 8://左上
                {
                    width = 0;
                    height = MAPHEIGHT_MIN;
                    break;
                }
            case 9://左上
                {
                    width = MAPWIDTH_MIN;
                    height = 0;
                    break;
                }
            case 10:
                {
                    width = 0;
                    height = 0;
                    break;
                }
            case 11:
                {
                    width = MAPWIDTH_MIN * 2;
                    height = 0;
                    break;
                }

            default:
                {
                    break;
                }
        }
     
        for (int i = 0; i < data.Count; i++)
        {
            for (int j = 0; j < data[i].Length; j++)
            {
                string[] strs = data[i][j].Split(underbar);
                MapData md = new MapData();
                md.objectID = int.Parse(strs[0]);
                md.widthPositionID = j+width;
                md.heightPositionID =i+height;
                mapDatas[i+height][j+width]=md;
                GameObject obj = Instantiate(objectSpace,new Vector3(j+width,-(i+height),0),Quaternion.identity,address.transform);
                GameObject instObj = mapObjects[(int.Parse(strs[0]))];
                instObj = Instantiate(instObj, obj.transform.position, Quaternion.identity, obj.transform);
                if(strs.Length>1) instObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, float.Parse(strs[1])));
                instObj.transform.parent.name = md.objectID.ToString()+underbar+md.widthPositionID.ToString()+underbar+md.heightPositionID.ToString();
            }
        }

    }

}


