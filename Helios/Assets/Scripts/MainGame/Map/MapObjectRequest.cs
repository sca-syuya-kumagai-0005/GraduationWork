using UnityEngine;
using static KumagaiLibrary.Unity.EventSet;
using static KumagaiLibrary.String;
using UnityEngine.InputSystem;
public class MapObjectReturnName : MonoBehaviour 
{
    string[] objectInfo;
    [SerializeField]int objectID;
    int widthPositionID;
    int heightPositionID;
    [SerializeField]SpecifyingDeliveryRoutes sDR;
    bool haveSinner = false;//���̃I�u�W�F�N�g�ɃV�i�[���z�u����Ă��邩�̃t���O
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
    }

    void PointerDown()
    {
        Debug.Log(ColorChanger("�֐�PointerDown���Ă΂�Ă��܂��B�I�u�W�F�N�gID��"+objectID+"�ł��B","red"));
        
        switch(objectID)
        { 
            case 0:
                {
                    if(sDR.DriverSet&&sDR.Writing)
                    {
                        Debug.Log(ColorChanger("�z�B���[�g�̎w����J�n���܂�", "red"));
                        sDR.MemoryStart();
                        sDR.MemoryRoute(widthPositionID, heightPositionID, objectID,this.gameObject, this.gameObject.transform.localPosition);
                    }
                   
                }
                break;
            case 3:
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
            case 0:
                {
                   
                }
                break;
            case 1:
                {
                    sDR.MemoryRoute(widthPositionID, heightPositionID, objectID, this.gameObject,this.gameObject.transform.localPosition);
                }
                break;
            case 3:
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
