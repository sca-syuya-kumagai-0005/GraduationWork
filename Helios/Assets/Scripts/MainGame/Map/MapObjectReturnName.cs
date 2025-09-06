using UnityEngine;

public class MapObjectReturnName : EventSet
{
    private void Start()
    {
        SetEventType(down,DebugName);
        SetEventType(enter,DebugName);
    }

    void DebugName()
    {
        Debug.Log(this.gameObject.name);
    }
}
