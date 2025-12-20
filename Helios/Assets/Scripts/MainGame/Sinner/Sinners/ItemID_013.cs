using UnityEngine;
public class ItemID_013 : Sinner
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Secra;
        liskClass = LiskClass.Lumenis;
        probabilitys = new float[8] { 5.0f, 5.0f, 100.0f, 100.0f, 40.0f, 10.0f, 50.0f, 5.0f };
        sinnerID = "ItemID_013";
        sinnerName = "自覚の道は己が夢";
        LoadSprite("ID013");
        effect = effectObjectParent.transform.GetChild(12).gameObject;
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
