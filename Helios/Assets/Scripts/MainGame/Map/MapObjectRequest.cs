using UnityEngine;

public class MapObjectReturnName : EventSet
{
    string[] objectInfo;
    int objectID;
    int objectPositionID;
    [SerializeField]SpecifyingDeliveryRoutes sDR;
    private void Start()
    {
        objectInfo = this.gameObject.name.Split(underbar);
        objectID = int.Parse(objectInfo[0]);
        objectPositionID = int.Parse(objectInfo[1]);
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
                        Debug.Log("�z�B���[�g�̎w����J�n���܂�");
                        sDR.MemoryStart();
                    }
                   
                }
                break;
            case 3:
                {
                    if(sDR.Memorying)
                    {
                        Debug.Log("�z�B���[�g�̎w����I�����܂���");
                        sDR.MemoryEnd();
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
        if(objectID == 1)
        {
            sDR.MemoryRoute(objectPositionID,this.gameObject.transform.position);
        }
    }
}
