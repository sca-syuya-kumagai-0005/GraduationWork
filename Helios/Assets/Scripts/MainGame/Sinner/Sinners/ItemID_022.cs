using UnityEngine;
public class ItemID_022 : Sinner
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Nulla;
        liskClass = LiskClass.Oblivara;
        probabilitys = new float[8] { 350.0f, 350.0f, 350.0f, 350.0f, 350.0f, 350.0f, 350.0f, 350.0f };
        sinnerID = "ItemID_022";
        sinnerName = "後奏のベル";
        LoadSprite("ID022");
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
