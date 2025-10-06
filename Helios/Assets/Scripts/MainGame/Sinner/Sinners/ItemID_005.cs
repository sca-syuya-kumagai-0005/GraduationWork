using UnityEngine;
public class ItemID_005 : Sinner
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Secra;
        liskClass = LiskClass.Lumenis;
        probabilitys = new float[8] { 0.0f, 100.0f, 125.0f, 100.0f, 75.0f, 0.0f, 25.0f, 100.0f };
        sinnerID = "ItemID_005";
        sinnerName = "あなたの為の天象儀";
        LoadSprite("atokaraireru");
        effect = GameObject.Find("Effect").transform.Find("").gameObject;
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
