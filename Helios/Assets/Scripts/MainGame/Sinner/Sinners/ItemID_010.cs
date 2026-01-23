using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
public class ItemID_010 : Sinner
{
    const string onatherName = "オオマガトキ";
    private SinnerDistribute distribute;
    private TimeLine timeLine;
    bool isTimeChecked;
    int housedPlotNumber;
    List<int>[] plotContainSinnerID;
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

        distribute = GameObject.Find("Map").gameObject.GetComponent<SinnerDistribute>();
        plotContainSinnerID = distribute.GetPlotContainedSinnerID;
        for (int i = 0; i < plotContainSinnerID.Count(); i++)
        {
            if (plotContainSinnerID[i].Contains(10))
            {
                housedPlotNumber = i;
            }
        }
        timeLine = GameObject.Find("ClockObject").GetComponent<TimeLine>();
        isTimeChecked = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!isTimeChecked)
        {
            if (timeLine.TimeStateAccess == TimeLine.TimeState.Night && deliveryCount == 0)
            {
                isTimeChecked = true;
                AbnormalPhenomenon();
            }
        }
        if (timeLine.AbnormalityList.Count > 1)
        {
            timeLine.RemoveAbnormalityList(sinnerName);
        }
    }
    public override void AbnormalPhenomenon()
    {
        //全ての異常において共通で起きる事があれば↓を変更
        sinnerName = onatherName;
        timeLine.AddAbnormalityList(sinnerName);
        string text = sinnerName + ":異常発生。\n平和は覆された。";
        announceManager.MakeAnnounce(text);

        List<GameObject> samePlotSinners = distribute.GetSinnerHousedObjects[housedPlotNumber];
        for (int i = 0; i < samePlotSinners.Count; i++)
        {
            if (plotContainSinnerID[housedPlotNumber][i] == 10) continue;
            samePlotSinners[i].GetComponent<Sinner>().AbnormalPhenomenon();
        }
        //それぞれの処理はここに書く
        timeLine.TimeStateAccess = TimeLine.TimeState.Abnormal;
    }
}