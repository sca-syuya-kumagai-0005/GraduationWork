using UnityEngine;
using UnityEngine.UIElements;
public class ItemID_010 : Sinner
{

    const string onatherName = "オオマガトキ";
    private TimeLine timeline;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secureClass = SecureClass.Catra;
        liskClass = LiskClass.Dravex;
        probabilitys = new float[8] { 5.0f, 5.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 5.0f };
        sinnerID = "ItemID_010";
        sinnerName = "平和の眠り鳥";
        LoadSprite("ID010");
        effect = effectObjectParent.transform.GetChild(9).gameObject;
        effectTimer = 6.5f;
        timeline = GameObject.Find("Clock").GetComponent<TimeLine>();
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
        timeline.TimeState = TimeLine.TimeStates.Abnormal;
    }
}
