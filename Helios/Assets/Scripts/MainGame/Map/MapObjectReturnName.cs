using UnityEngine;

public class MapObjectReturnName : EventSet
{
    private void Start()
    {
        SetEventType(down,DebugName);
    }

    void DebugName()
    {
        Debug.Log(this.gameObject.name);
    }
}
