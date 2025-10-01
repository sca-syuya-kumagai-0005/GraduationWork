using UnityEngine;
using UnityEngine.EventSystems;
namespace KumagaiLibrary
{
    public class String
    {
        /// <summary>
        /// ����1�ɓn���������������2�œn�����F�ɕύX�����������Ԃ��܂�
        /// </summary>
        /// <param name="text">�F��ς�����������</param>
        /// <param name="color">�ς������F</param>
        /// <returns></returns>
        public static string ColorChanger(string text,string color)
        {
            string output = null;
            for (int i = 0; i < text.Length; i++)
            {
                output += $"<color={color}>{text[i]}</color>";
            }
            return output;
        }
    }
    namespace Unity
    {
        public class EventSet
        {
            public delegate void SetEvent();
            public const string enter = "Enter";
            public const string exit = "Exit";
            public const string down = "Down";
            public const string up = "Up";
            public const string underbar = "_";
           
            public static void PointerEnter()
            {
                Debug.Log("���N���X���̊֐����Ă΂�Ă��܂�\n�p����̊֐����I�[�o�[���C�h���Ă�������");//���N���X���̊֐����Ăяo����Ă��邱�Ƃ�m�点��
            }

            public static void PointerDown()
            {
                Debug.Log("���N���X���̊֐����Ă΂�Ă��܂�\n�p����̊֐����I�[�o�[���C�h���Ă�������");
            }

            public static void PointerExit()
            {
                Debug.Log("���N���X���̊֐����Ă΂�Ă��܂�\\n�p����̊֐����I�[�o�[���C�h���Ă�������\"");
            }

            public static void SetEventType(string eventID, SetEvent e,GameObject obj)
            {
                if (obj.gameObject.GetComponent<EventTrigger>() == null)
                {
                    obj.gameObject.AddComponent<EventTrigger>();
                }
                EventTrigger trigger = obj.gameObject.GetComponent<EventTrigger>();
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
    }

    
}

