using UnityEngine;
public class ItemID_018 : Sinner
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Vigil;
        liskClass = LiskClass.Lumenis;
        probabilitys = new float[8] { 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 5.0f, 0.0f };
        sinnerID = "ItemID_018";
        sinnerName = "そのまさか、逆さまの祖";
        LoadSprite("ID018");
        effect = effectObjectParent.transform.GetChild(17).gameObject;
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
