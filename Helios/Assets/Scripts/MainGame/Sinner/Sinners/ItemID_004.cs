using System.Linq;
using UnityEngine;
public class ItemID_004 : Sinner
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Secra;
        liskClass = LiskClass.Lumenis;
        probabilitys = new float[8] { 0.0f, 0.0f, 100.0f, 100.0f, 75.0f, 0.0f, 50.0f, 0.0f };
        sinnerID = "ItemID_004";
        sinnerName = "水面と明鏡";
        LoadSprite("ID004");
        effect = effectObjectParent.transform.GetChild(3).gameObject;
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
        if (specifyingDeliveryRoutes.SinnerDebuff[0].ContainsKey("君と歩んだクラゲ"))
        {
            specifyingDeliveryRoutes.SinnerDebuff[0]["君と歩んだクラゲ"] = false;
        }
    }
}
