using UnityEngine;

public class MapObjectReturnName : EventSet
{
    string[] objectInfo;
    private void Start()
    {
        objectInfo = this.gameObject.name.Split(underbar);

        switch ()
        SetEventType(enter,PointerDown);
    }

    protected override void PointerDown()
    { 
       
        int objectID = int.Parse(objectInfo[0]);
        switch(objectID)
        { 
            case 0:
                {
                    Debug.Log("�z�B���[�g�̎w����J�n���܂�");
                }
                break;

                default:
                {

                }
                break;
        }
    }
}
