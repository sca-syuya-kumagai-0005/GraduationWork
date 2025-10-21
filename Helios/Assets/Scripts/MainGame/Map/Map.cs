using UnityEngine;
using System.Collections.Generic;
using static KumagaiLibrary.Unity.CsvManager;

/*memo=========================================================================================
 * �z�B���鑤��Script�ŁA�ʂ������̗אڂ���I�u�W�F�N�g�̎擾�ƃJ�E���g������
 ==============================================================================================*/

public class Map : MonoBehaviour
{
    [SerializeField] TextAsset[] mapCsv;
    [SerializeField] GameObject objectSpace;
    [SerializeField] GameObject[] mapObjects;
    private char underbar = '_';
    private int mapNumber;
    private const int ADDRES_MAX = 9;
    private const int OUTER = 2;
    private const int mapWidth=21;
    private const int mapHeight=21;
    private GameObject[] plot = new GameObject[ADDRES_MAX];
  
   // private MapData[][] mapDatas = new MapDasta[mapWidth * 3][mapHeight*4];
    private List<List<MapData>> mapDatas = new List<List<MapData>>();
    public List<List<MapData>> MapDatas { get { return mapDatas;} }
    public struct MapData//���g�Ɨאڂ���u���b�N�̏����i�[����struct�@���g�Ɨאڂ���u���b�N�ɑΉ�����t���O��true
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
        
        List<string[]> data = Read(mapCsv[0]);
        
        for(int i=0;i<mapHeight*4;i++)
        {
            mapDatas.Add(new List<MapData>());
            for(int j=0;j<mapWidth;j++)
            {
                mapDatas[i] = new List<MapData>();
            }
        }

        
        for(int i=0; i<ADDRES_MAX+20; i++)
        {
            GameObject address = new GameObject();
            address.transform.parent = transform;
            address.name = "Address" + underbar + i;
            if(i!=0)data = Read(mapCsv[1]);//��ŕύX�@���͒��S�̃}�b�v�ȊO��0�Ŗ��߂�ꂽCSV�ő�p
            MapCreate(data, i, address);
        }

      
        for (int i = 0; i < mapDatas.Count; i++)
        {
            string str = "";
            for (int j = 0; j < mapDatas[i].Count; j++)
            {
                str += mapDatas[i][j].objectID.ToString();
            }
            Debug.Log(str);
        }
        this.gameObject.transform.position = new Vector3(-31, -11, 0);//�J����(�}�b�v)�̏����ʒu
    }
    void MapCreate(List<string[]> data,int mapNumber,GameObject address)//�}�b�v�̐����BmapDatas��objectID��positionID�������Őݒ�
    {
        int width=0;
        int height=0; 
        
        switch(mapNumber)
        {
            case 0://�^��
                {
                    width = mapWidth;
                    height = mapHeight; 
                    break;
                }
            case 1://��
                {
                    width = mapWidth;
                    height = 0;
                    break;
                }

            case 2://�E
                {
                    width = mapWidth*2;
                    height = mapHeight;
                    break;
                }
            case 3://��
                {
                    width = mapWidth;
                    height = mapHeight*2;
                    break;
                }
            case 4://��
                {
                    width = 0;
                    height = mapHeight;
                    break;
                }
            case 5://�E��
                {
                    width = mapWidth*2;
                    height = 0; 
                    break;
                }
            case 6://�E��
                {
                    width = mapWidth * 2;
                    height = mapHeight*2;
                    break;
                }
            case 7://����
                {
                    width = 0;
                    height =mapHeight * 2;
                    break;
                }
            case 8://����
                {
                    width = 0;
                    height = 0;
                    break;
                }
            //case 9://����
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
     
        for (int i = 0;i<mapHeight;i++)
        {
            for (int j = 0; j < mapWidth; j++)
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