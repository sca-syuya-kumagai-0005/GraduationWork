using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
public class ItemID_014 : Sinner
{
    private GameObject mapObject;
    private List<GameObject> houseList = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Nulla;
        liskClass = LiskClass.Zerath;
        probabilitys = new float[8] { 85.0f, 45.0f, 100.0f, 100.0f, 20.0f, 70.0f, 100.0f, 2.0f };
        sinnerID = "ItemID_014";
        sinnerName = "パンツァー - TSD型 - 車掌";
        LoadSprite("ID014");
        LoadSinnerIconObject();
        effect = effectObjectParent.transform.GetChild(13).gameObject;
        const string underBar = "_";
        string myPositionText = gameObject.name;
        int[] myPosition = new int[2]
        {
        int.Parse(myPositionText.Split(underBar)[1]),
        int.Parse(myPositionText.Split(underBar)[2]),
        };
        int[][] correction = new int[4][]
        {
            new int[2]{ 1,0},
            new int[2]{ -1,0},
            new int[2]{ 0,1},
            new int[2]{ 0,-1},
        };
        for (int plotNumber = (int)Map.MapObjectID.STRAIGHT; plotNumber <= (int)Map.MapObjectID.CROSS; plotNumber++)
        {
            for(int i = 0; i < correction.Length; i++)
            {
                GameObject go = GameObject.Find(plotNumber
                    + underBar + (myPosition[0] + correction[i][0])
                    + underBar + (myPosition[1] + correction[i][1]));

                if (go != null)
                {
                    houseList.Add(go);
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    public override void AbnormalPhenomenon()
    {
        //全ての異常において共通で起きる事があれば↓を変更
        base.AbnormalPhenomenon();

        //それぞれの処理はここに書く
        int rand = Random.Range(0, houseList.Count);
        int generationCount = 2;
        for (int i = 0; i < generationCount; i++)
        {
            mapObject = Instantiate(sinnerIconObject, houseList[rand].transform.localPosition, Quaternion.identity, transform.parent.parent);
            Tank tank = mapObject.AddComponent<Tank>();
            tank.SetSprite = sinnerSprite;
            mapObject.name = "戦車オルオーン";
            tank.StartPosition = houseList[rand].name;
        }
    }
}
