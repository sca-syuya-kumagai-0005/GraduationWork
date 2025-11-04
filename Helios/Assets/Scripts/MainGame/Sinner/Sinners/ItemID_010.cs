using UnityEngine;
public class ItemID_010 : Sinner
{
    const string onatherName = "オオマガトキ";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Catra;
        liskClass = LiskClass.Dravex;
        probabilitys = new float[8] { 5.0f, 5.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 5.0f };
        sinnerID = "ItemID_010";
        sinnerName = "平和の眠り鳥";
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
        //神社が実装されたら書く
    }
}
