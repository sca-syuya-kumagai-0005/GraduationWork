using UnityEngine;
using static KumagaiLibrary.Unity.EventSet;
public class DliverRequest : MonoBehaviour
{
    [SerializeField]int driverID;
    [SerializeField]GameObject Drivers;
    SpecifyingDeliveryRoutes sDR;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sDR=Drivers.GetComponent<SpecifyingDeliveryRoutes>();
        SetEventType(down,PointerDown,this.gameObject);
    }

    public void PointerDown()
    {
        sDR.DriverSetting(driverID);
    }

}
