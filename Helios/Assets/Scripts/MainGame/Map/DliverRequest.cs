using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static KumagaiLibrary.Unity.EventSet;
public class DliverRequest : MonoBehaviour
{
    [SerializeField]int driverID;
    [SerializeField]GameObject Drivers;
    SpecifyingDeliveryRoutes sDR;
    TutorialMG tutorialMG;
    private const string SINNER_007 = "ñ`åØÊùÇÕçÇÇÁÇ©Ç…";
    private const string SINNER_008 = "ó÷è•ÇÃéP";
   
    bool isConfison = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tutorialMG = GameObject.Find("TutorialMG").GetComponent<TutorialMG>();
        sDR=Drivers.GetComponent<SpecifyingDeliveryRoutes>();
        SetEventType(down,PointerDown,this.gameObject);
    }

    private void Update()
    {
        int confisonCount = 0;
        if (sDR.SinnerDebuff[driverID].ContainsKey(SINNER_007)) confisonCount += sDR.SinnerDebuff[driverID][SINNER_007] ? 1 : 0;Å@//ÉtÉâÉOÇ™TrueÇ»ÇÁç¨óêèÛë‘Çé¶Ç∑ïœêîÇâ¡éZ
        if (sDR.SinnerDebuff[driverID].ContainsKey(SINNER_008)) confisonCount += sDR.SinnerDebuff[driverID][SINNER_008] ? 1 : 0;  //è„Ç∆ìØÇ∂
        isConfison = confisonCount > 0;
        sDR.IsConfison[driverID] = isConfison;
    }
    public void PointerDown()
    {
        if (tutorialMG.IsTutorial && tutorialMG.CurrentState == TutorialMG.TutorialState.TypeSelection)
        {
            tutorialMG.ChangeState(TutorialMG.TutorialState.PushButton);
        }
        sDR.DriverSetting(driverID);
    }

    public void ConfisonClick()
    {
        sDR.ConfisonClickCount[driverID]--;
        if (sDR.ConfisonClickCount[driverID] <=0)
        {
            if(sDR.SinnerDebuff[driverID].ContainsKey(SINNER_007))sDR.SinnerDebuff[driverID][SINNER_007] = false;
            if (sDR.SinnerDebuff[driverID].ContainsKey(SINNER_007)) sDR.SinnerDebuff[driverID][SINNER_008] = false;
            isConfison = false;
        }
    }

}
