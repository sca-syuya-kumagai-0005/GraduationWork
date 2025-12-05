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
        LoadSprite("atokaraireru");
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
