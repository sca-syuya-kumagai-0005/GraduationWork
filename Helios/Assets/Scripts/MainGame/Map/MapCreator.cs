using UnityEngine;
using System.Collections.Generic;

/*memo=========================================================================================
 * �z�B���鑤��Script�ŁA�ʂ������̗אڂ���I�u�W�F�N�g�̎擾�ƃJ�E���g������
 ==============================================================================================*/

public class MapCreator : CsvReader
{
    [SerializeField] TextAsset mapCsv;
    [SerializeField] GameObject objectSpace;
    [SerializeField] GameObject[] mapObjects;
    List<List<MapData>> mapDatas = new List<List<MapData>>();
    public List<List<MapData>> MapDatas { get { return mapDatas; } }
    public struct MapData//���g�Ɨאڂ���u���b�N�̏����i�[����struct�@���g�Ɨאڂ���u���b�N�ɑΉ�����t���O��true
    {
        public int objectID;
        public int positionID;
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

    void MapCreate(List<string[]> data)//�}�b�v�̐����BmapDatas��objectID��positionID�������Őݒ�
    {
      
        for (int i = 0; i < data.Count; i++)
        {
            mapDatas.Add(new List<MapData>());
            for (int j = 0; j < data[i].Length; j++)
            {
                MapData md = new MapData();
                md.objectID = int.Parse(data[i][j]);
                md.positionID = i*data[i].Length+j;
                mapDatas[i].Add(md);
                GameObject obj = Instantiate(objectSpace,new Vector3(0+j,0-i,0),Quaternion.identity,transform);
                GameObject instObj = mapObjects[(int.Parse(data[i][j]))];
                Instantiate(instObj,obj.transform.position,Quaternion.identity,obj.transform);
            }
        }
    }
}