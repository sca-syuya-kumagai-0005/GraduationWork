using UnityEngine;

public class MapObjectReturnName : EventSet
{
    string[] objectInfo;
    int objectID;
    int widthPositionID;
    int heightPositionID;
    [SerializeField]SpecifyingDeliveryRoutes sDR;
    private void Start()
    {
        objectInfo = this.gameObject.name.Split(underbar);
        objectID = int.Parse(objectInfo[0]);
        widthPositionID = int.Parse(objectInfo[1]);
        heightPositionID = int.Parse(objectInfo[2]);
        sDR = GameObject.Find("Map").GetComponent<SpecifyingDeliveryRoutes>();
        //switch ()
        SetEventType(down,PointerDown);
        SetEventType(enter,PointerEnter);
    }

    protected override void PointerDown()
    { 
       
        
        switch(objectID)
        { 
            case 0:
                {
                    if(!sDR.Memorying)
                    {
                        Debug.Log("配達ルートの指定を開始します");
                        sDR.MemoryStart();
                        sDR.MemoryRoute(widthPositionID, heightPositionID, objectID,this.gameObject, this.gameObject.transform.position);
                    }
                   
                }
                break;
            case 3:
                {
                    if(sDR.Memorying)
                    {
                        sDR.MemoryEnd(widthPositionID, heightPositionID, objectID);
                    }
                   
                }
                break;

                default:
                {

                }
                break;
        }
    }

    protected override void PointerEnter()
    {
        switch(objectID)
        { 
            case 0:
                {
                   
                }
                break;
            case 1:
                {
                    sDR.MemoryRoute(widthPositionID, heightPositionID, objectID, this.gameObject,this.gameObject.transform.position);
                }
                break;
          
        }
    }
}
