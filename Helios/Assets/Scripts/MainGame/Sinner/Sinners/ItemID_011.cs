using UnityEngine;
public class ItemID_011 : Sinner
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Secra;
        liskClass = LiskClass.Lumenis;
        probabilitys = new float[8] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
        sinnerID = "ItemID_011";
        sinnerName = "メモリースリンガー";
        LoadSprite("ID011");
        effect = effectObjectParent.transform.GetChild(10).gameObject.transform.GetChild(0).gameObject;
    }
    // Update is called once per frame
    void Update()
    {

    }
    protected override void AbnormalPhenomenon()
    {
        //全ての異常において共通で起きる事があれば↓を変更
        base.AbnormalPhenomenon();

        //それぞれの処理はここに書く
    }
}
