using UnityEngine;
public class ItemID_014 : Sinner
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Nulla;
        liskClass = LiskClass.Zerath;
        probabilitys = new float[8] { 85.0f, 45.0f, 100.0f, 100.0f, 20.0f, 70.0f, 100.0f, 2.0f };
        sinnerID = "ItemID_014";
        sinnerName = "パンツァー - TSD型 - 車掌";
        LoadSprite("ID014");
        //effect = GameObject.Find("Effect").transform.Find("Fog_001_VFX").gameObject;
        //GetEffectObject(true);
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
