using UnityEngine;

public class MapObjectRequest : EventSet
{
    SpecifyingDeliveryRoutes sDR;
    string[] objectInfo;
    private void Start()
    {
        objectInfo = this.gameObject.name.Split(underbar);
        sDR = GameObject.Find("Map").GetComponent<SpecifyingDeliveryRoutes>();
        Debug.Log(sDR);
        //switch ()
        SetEventType(enter,PointerDown);
    }

    protected override void PointerDown()
    { 
       
        int objectID = int.Parse(objectInfo[0]);
        switch(objectID)
        { 
            case 0:
                {
                    Debug.Log("配達ルートの指定を開始します");
                }
                break;

                default:
                {

                }
                break;
        }
    }
}
