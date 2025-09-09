using UnityEngine;
using UnityEngine.EventSystems;

public class EventSet : MonoBehaviour//���̃N���X�̓}�E�X�Ή��̍ۂɕK�v�Ȋ��N���X�ł�
{
    public delegate void SetEvent();
    private string corectEventID;
    public const string enter = "Enter";
    public const string exit = "Exit";
    public const string down = "Down";
    public const string up = "Up";
    public const string underbar = "_";
    protected virtual void PointerEnter()
    {
        Debug.Log("���N���X���̊֐����Ă΂�Ă��܂�\n�p����̊֐����I�[�o�[���C�h���Ă�������");//���N���X���̊֐����Ăяo����Ă��邱�Ƃ�m�点��
    }

    protected virtual void PointerDown()
    {
        Debug.Log("���N���X���̊֐����Ă΂�Ă��܂�\\n�p����̊֐����I�[�o�[���C�h���Ă�������\"");
    }

    protected virtual void PointerExit()
    {
        Debug.Log("���N���X���̊֐����Ă΂�Ă��܂�\\n�p����̊֐����I�[�o�[���C�h���Ă�������\"");
    }

    protected virtual void SetEventType(string eventID, SetEvent e)
    {
        gameObject.AddComponent<EventTrigger>();
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
