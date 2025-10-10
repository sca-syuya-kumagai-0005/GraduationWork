using UnityEngine;
using System.Collections.Generic;
using static KumagaiLibrary.Unity.CsvManager;

/*memo=========================================================================================
 * �z�B���鑤��Script�ŁA�ʂ������̗אڂ���I�u�W�F�N�g�̎擾�ƃJ�E���g������
 ==============================================================================================*/

public class Map : CsvReader
{
    [SerializeField] TextAsset mapCsv;
    [SerializeField] GameObject objectSpace;
    [SerializeField] GameObject[] mapObjects;
    private char underbar = '_';
    private List<List<MapData>> mapDatas = new List<List<MapData>>();
    public List<List<MapData>> MapDatas { get { return mapDatas;} }
    public struct MapData//���g�Ɨאڂ���u���b�N�̏����i�[����struct�@���g�Ɨאڂ���u���b�N�ɑΉ�����t���O��true
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
    void MapCreate(List<string[]> data)//�}�b�v�̐����BmapDatas��objectID��positionID�������Őݒ�
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