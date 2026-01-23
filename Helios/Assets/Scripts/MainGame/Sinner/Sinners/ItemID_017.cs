using UnityEngine;
public class ItemID_017 : Sinner
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Vigil;
        liskClass = LiskClass.Lumenis;
        probabilitys = new float[8] { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 35.0f, 0.0f };
        sinnerID = "ItemID_017";
        sinnerName = "マッハピエロ";
        LoadSprite("ID017");
        effect = effectObjectParent.transform.GetChild(16).gameObject;
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
    }
}
