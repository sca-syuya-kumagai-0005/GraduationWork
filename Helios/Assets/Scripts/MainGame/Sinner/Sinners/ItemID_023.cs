using UnityEngine;
public class ItemID_023 : Sinner
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Vigil;
        liskClass = LiskClass.Velgra;
        probabilitys = new float[8] { 20.0f, 200.0f, 50.0f, 70.0f, 40.0f, 0.0f, 30.0f, 0.0f };
        sinnerID = "ItemID_023";
        sinnerName = "ひとりぼっちのブランコ";
        LoadSprite("ID023");
        effect = effectObjectParent.transform.GetChild(22).gameObject;
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
