using UnityEngine;
using static KumagaiLibrary.Unity.EventSet;
using static KumagaiLibrary.String;
using UnityEngine.InputSystem;
using static Map;

[DefaultExecutionOrder(2)] 
public class MapObjectRequest : MonoBehaviour
{
    string[] objectInfo;
    int widthPositionID;
    int heightPositionID;
    [SerializeField]SpecifyingDeliveryRoutes sDR;
    bool haveSinner = false;//���̃I�u�W�F�N�g�ɃV�i�[���z�u����Ă��邩�̃t���O
    public bool HaveSinner { set{haveSinner=value; } }

    MapObjectID id;
    private void Start()
    {
        objectInfo = this.gameObject.name.Split(underbar);
        id = (MapObjectID)int.Parse(objectInfo[0]);
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
        switch(id)
        { 
            case MapObjectID.COMPANY:
                {
                    if(sDR.DriverSet&&sDR.Writing)
                    {
                        Debug.Log(ColorChanger("�z�B���[�g�̎w����J�n���܂�", "red"));
                        sDR.MemoryStart();
                        sDR.MemoryRoute(widthPositionID, heightPositionID, (int)id,this.gameObject, this.gameObject.transform.localPosition);
                    }
                   
                }
                break;
            case MapObjectID.HOUSE_1:
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
        switch(id)
        { 
            case MapObjectID.COMPANY:
                {
                   
                }
                break;
            case MapObjectID.STRAIGHT:
                {
                    sDR.MemoryRoute(widthPositionID, heightPositionID,(int)id, this.gameObject,this.gameObject.transform.localPosition);
                }
                break;
            case MapObjectID.CORNER:
                {
                    sDR.MemoryRoute(widthPositionID, heightPositionID, (int)id, this.gameObject, this.gameObject.transform.localPosition);
                }
                break;
            case MapObjectID.TJUNCTION:
                {
                    sDR.MemoryRoute(widthPositionID, heightPositionID, (int)id, this.gameObject, this.gameObject.transform.localPosition);
                }
                break;
            case MapObjectID.CROSS:
                {
                    sDR.MemoryRoute(widthPositionID, heightPositionID, (int)id, this.gameObject, this.gameObject.transform.localPosition);
                }
                break;
            case MapObjectID.HOUSE_1:
                {
                    if (haveSinner)
                    {
                        
                        sDR.MemoryRoute(widthPositionID, heightPositionID, (int)id, this.gameObject, this.gameObject.transform.localPosition);
                    }
                    
                }
                break;
          
        }
    }
}
