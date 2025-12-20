using UnityEngine;
public class ItemID_009 : Sinner
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Catra;
        liskClass = LiskClass.Zerath;
        probabilitys = new float[8] { 10.0f, 10.0f, 20.0f, 100.0f, 150.0f, 50.0f, 250.0f, 0.0f };
        sinnerID = "ItemID_009";
        sinnerName = "朽ちた天馬";
        LoadSprite("ID009");
        effect = effectObjectParent.transform.GetChild(8).gameObject;
        effectTimer = 6.5f;
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
