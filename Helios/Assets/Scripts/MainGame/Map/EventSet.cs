using UnityEngine;
using UnityEngine.EventSystems;

public class EventSet : MonoBehaviour//このクラスはマウス対応の際に必要な基底クラスです
{
    public delegate void SetEvent();
    private string corectEventID;
    public const string enter = "Enter";
    public const string exit = "Exit";
    public const string down = "Down";
    public const string up = "Up";
    public virtual void PointerEnter()
    {
        Debug.Log("基底クラス側の関数が呼ばれています\n継承先の関数をオーバーライドしてください");//基底クラス側の関数が呼び出されていることを知らせる
    }

    public virtual void PointerDown()
    {
        Debug.Log("基底クラス側の関数が呼ばれています\\n継承先の関数をオーバーライドしてください\"");
    }

    public virtual void PointerExit()
    {
        Debug.Log("基底クラス側の関数が呼ばれています\\n継承先の関数をオーバーライドしてください\"");
    }

    public virtual void SetEventType(string eventID, SetEvent e)
    {
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();

        switch (eventID)
        {
            case (enter):
                {
                    entry.eventID = EventTriggerType.PointerEnter;
                }
                break;
            case (down):
                {
                    entry.eventID = EventTriggerType.PointerDown;
                }
                break;
            case (exit):
                {
                    entry.eventID = EventTriggerType.PointerExit;
                }
                break;
            case (up):
                {
                    entry.eventID = EventTriggerType.PointerUp;
                }
                break;
        }

        entry.callback.AddListener((data) => { e(); });
        trigger.triggers.Add(entry);
    }
}
