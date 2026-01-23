using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static KumagaiLibrary.Unity.EventSet;
public class DliverRequest : MonoBehaviour
{
    [SerializeField]int driverID;
    [SerializeField]GameObject Drivers;
    SpecifyingDeliveryRoutes sDR;
    private const string SINNER_007 = "ñ`åØÊùÇÕçÇÇÁÇ©Ç…";
    private const string SINNER_008 = "ó÷è•ÇÃéP";
    bool isConfison = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sDR=Drivers.GetComponent<SpecifyingDeliveryRoutes>();
        SetEventType(down,PointerDown,this.gameObject);
    }

    private void Update()
    {
        //int confisonCount = 0;
        //if (sDR.SinnerDebuff[driverID].ContainsKey(SINNER_007)) confisonCount += sDR.SinnerDebuff[driverID][SINNER_007] ? 1 : 0;Å@//ÉtÉâÉOÇ™TrueÇ»ÇÁç¨óêèÛë‘Çé¶Ç∑ïœêîÇâ¡éZ
        //if (sDR.SinnerDebuff[driverID].ContainsKey(SINNER_008)) confisonCount += sDR.SinnerDebuff[driverID][SINNER_008] ? 1 : 0;  //è„Ç∆ìØÇ∂
        //isConfison = confisonCount > 0;
        //sDR.IsConfison[driverID] = isConfison;
    }
    public void PointerDown()
    {
        sDR.DriverSetting(driverID);
       


    }

}
