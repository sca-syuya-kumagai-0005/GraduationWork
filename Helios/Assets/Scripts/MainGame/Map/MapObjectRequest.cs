using UnityEngine;
using static KumagaiLibrary.Unity.EventSet;
using static KumagaiLibrary.String;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(2)] 
public class MapObjectRequest : MonoBehaviour 
{
    string[] objectInfo;
    [SerializeField]int objectID;
    int widthPositionID;
    int heightPositionID;
    [SerializeField]SpecifyingDeliveryRoutes sDR;
    bool haveSinner = false;//このオブジェクトにシナーが配置されているかのフラグ
    public bool HaveSinner { set{haveSinner=value; } }
    private void Start()
    {
        
        objectInfo = this.gameObject.name.Split(underbar);
        objectID = int.Parse(objectInfo[0]);
        widthPositionID = int.Parse(objectInfo[1]);
        heightPositionID = int.Parse(objectInfo[2]);
        sDR = GameObject.Find("Drivers").GetComponent<SpecifyingDeliveryRoutes>();
        SetEventType(down,PointerDown,this.gameObject);
        SetEventType(enter,PointerEnter, this.gameObject);
        if(haveSinner)
        {
            SpriteRenderer sr = this.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
            sr.color = new Color(1.0f, 0.4f, 0.0f);
        }
    }

    void PointerDown()
    {
        Debug.Log(ColorChanger("関数PointerDownが呼ばれています。オブジェクトIDは"+objectID+"です。","red"));
        
        switch(objectID)
        { 
            case 1:
                {
                    if(sDR.DriverSet&&sDR.Writing)
                    {
                        Debug.Log(ColorChanger("配達ルートの指定を開始します", "red"));
                        sDR.MemoryStart();
                        sDR.MemoryRoute(widthPositionID, heightPositionID, objectID,this.gameObject, this.gameObject.transform.localPosition);
                    }
                   
                }
                break;
            case 9:
                {
                        sDR.DestinationSetting(this.gameObject);
                   
                }
                break;

                default:
                {

                }
                break;
        }
    }

    void PointerEnter()
    {
        switch(objectID)
        { 
            case 1:
                {
                   
                }
                break;
            case 2:
                {
                    sDR.MemoryRoute(widthPositionID, heightPositionID, objectID, this.gameObject,this.gameObject.transform.localPosition);
                }
                break;
            case 3:
                {
                    sDR.MemoryRoute(widthPositionID, heightPositionID, objectID, this.gameObject, this.gameObject.transform.localPosition);
                }
                break;
            case 4:
                {
                    sDR.MemoryRoute(widthPositionID, heightPositionID, objectID, this.gameObject, this.gameObject.transform.localPosition);
                }
                break;
            case 5:
                {
                    sDR.MemoryRoute(widthPositionID, heightPositionID, objectID, this.gameObject, this.gameObject.transform.localPosition);
                }
                break;
            case 9:
                {
                    if (haveSinner)
                    {
                        
                        sDR.MemoryRoute(widthPositionID, heightPositionID, objectID, this.gameObject, this.gameObject.transform.localPosition);
                    }
                    
                }
                break;
          
        }
    }
}
