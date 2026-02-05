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
    [SerializeField]GameObject sinnerReference;
    Collider col;
    bool haveSinner = false;//このオブジェクトにシナーが配置されているかのフラグ
    public bool HaveSinner { set{haveSinner=value; } }

    MapObjectID id;
    private TutorialMG tutorialMG;
    private GameObject residenceCertificate;//配達表
    private void Awake()
    {
        residenceCertificate = GameObject.Find("ResidenceCertificate");
    }
    private void Start()
    {
        objectInfo = this.gameObject.name.Split(underbar);
        id = (MapObjectID)int.Parse(objectInfo[0]);
        widthPositionID = int.Parse(objectInfo[1]);
        heightPositionID = int.Parse(objectInfo[2]);
        tutorialMG = GameObject.Find("TutorialMG").GetComponent<TutorialMG>();
        sDR = GameObject.Find("Drivers").GetComponent<SpecifyingDeliveryRoutes>();
        sinnerReference = GameObject.Find("ReferenceData").gameObject.transform.GetChild(0).gameObject;
        col=this.gameObject.GetComponent<Collider>();
        SetEventType(down,PointerDown,this.gameObject);
        SetEventType(enter,PointerEnter, this.gameObject);
    }

    private void Update()
    {
        col.enabled=!sinnerReference.activeSelf;
    }

    void PointerDown()
    {   
        switch(id)
        { 
            case MapObjectID.COMPANY:
                {
                    if(sDR.DriverSet&&sDR.Writing)
                    {
                        Debug.Log(ColorChanger("配達ルートの指定を開始します", "red"));
                        sDR.MemoryStart();
                        sDR.MemoryRoute(widthPositionID, heightPositionID, (int)id,this.gameObject, this.gameObject.transform.localPosition);
                    }
                   
                }
                break;
            case MapObjectID.HOUSE_1:
                {
                    if (tutorialMG.IsTutorial && haveSinner && tutorialMG.CurrentState == TutorialMG.TutorialState.Click)
                    {
                        tutorialMG.ChangeState(TutorialMG.TutorialState.DeliveryExplanation);
                    }
                    else if (tutorialMG.IsTutorial && haveSinner && tutorialMG.CurrentState == TutorialMG.TutorialState.OneMoreHouseClick)
                    {
                        tutorialMG.ChangeState(TutorialMG.TutorialState.TypeSelection);
                    }
                    sDR.DestinationSetting(this.gameObject);

                }
                break;
            case MapObjectID.HOUSE_2:
                {
                    sDR.DestinationSetting(this.gameObject);

                }
                break;
            case MapObjectID.HOUSE_3:
                {
                    sDR.DestinationSetting(this.gameObject);

                }
                break;
            case MapObjectID.HOUSE_4:
                {
                    sDR.DestinationSetting(this.gameObject);

                }
                break;

            default:
                {

                }
                break;
        }
        if (!haveSinner)
            residenceCertificate.SetActive(false);
    }

    void PointerEnter()
    {
        switch(id)
        {
            case MapObjectID.NULL:
                {
                    //sDR.MemoryRoute(widthPositionID, heightPositionID, (int)id, this.gameObject, this.gameObject.transform.localPosition);
                }
                break;
            case MapObjectID.COMPANY:
                {
                    sDR.MemoryRoute(widthPositionID, heightPositionID, (int)id, this.gameObject, this.gameObject.transform.localPosition);
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
            case MapObjectID.HOUSE_2:
                {
                    if (haveSinner)
                    {
                        sDR.MemoryRoute(widthPositionID, heightPositionID, (int)id, this.gameObject, this.gameObject.transform.localPosition);
                    }

                }
                break;
            case MapObjectID.HOUSE_3:
                {
                    if (haveSinner)
                    {
                        sDR.MemoryRoute(widthPositionID, heightPositionID, (int)id, this.gameObject, this.gameObject.transform.localPosition);
                    }

                }
                break;
            case MapObjectID.HOUSE_4:
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
