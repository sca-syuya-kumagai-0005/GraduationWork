using System.Collections.Generic;
using System.IO;
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
        public static string ColorChanger(string text, string color)
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
            /// <summary>
            /// Unity�̃C�x���g��ݒ肷��֐��ł�
            /// </summary>
            /// <param name="eventID">�ǂ̃C�x���g��ǉ���������(Enter,Down,UP�Ȃ�)</param>
            /// <param name="e">�C�x���g�̎��ɌĂт����֐�(void�̂�)</param>
            /// <param name="obj">�C�x���g��ǉ��������Q�[���I�u�W�F�N�g</param>
            public static void SetEventType(string eventID, SetEvent e, GameObject obj)
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
        public class CsvManager
        {
            /// <summary>
            /// Csv�̓ǂݎ����s���֐�
            /// </summary>
            /// <param name="csvData">�ǂ݂���CSV�t�@�C��</param>
            /// <returns>List<string[]>�Œl���Ԃ���܂�</returns>
            public static List<string[]> Read(TextAsset csvData) //�����ɓ��͂���CSV�t�@�C�������X�g�ɕϊ�����֐�
            {
                List<string[]> datas = new List<string[]>();
                StringReader reader = new StringReader(csvData.text);
                while (reader.Peek() != -1)
                {
                    string line = reader.ReadLine();
                    datas.Add(line.Split(','));
                }
                return datas;
            }
        }
    }

    namespace Dictionary
    {
        public class Support
        {

            /// <summary>
            /// Dictionary�̔z���Key,value��v�f�Ƃ��Ēǉ����܂�
            /// </summary>
            /// <param name="dictionary">�ǉ�������Dictionary</param>
            /// <param name="key">�ǉ��������v�f��Key</param>
            /// <param name="value">�ǉ��������v�f��value</param>
            /// <returns>Dictionary<string,bool>[] �Œl���Ԃ���܂�</returns>
            public static Dictionary<string, bool>[] AddArray(Dictionary<string, bool>[] dictionary, string key, bool value)
            {
                bool duplication = false;
                for (int i = 0; i < dictionary.Length; i++)
                {
                    foreach (KeyValuePair<string, bool> kvp in dictionary[i])
                    {
                        if (kvp.Key == key)
                        {
                            duplication = true;
                            break;
                        }
                    }
                    if (!duplication)
                    {
                        dictionary[i].Add(key, value);
                    }
                    duplication = false;

                }

                return dictionary;
            }

            public static Dictionary<string, bool>[] AddArray(Dictionary<string, bool>[] dictionary, string[] keys, bool value)
            {
                bool duplication = false;
                for (int i = 0; i < keys.Length; i++)
                {
                    for (int j = 0; j < dictionary.Length; j++)
                    {
                        foreach (KeyValuePair<string, bool> kvp in dictionary[j])
                        {
                            if (kvp.Key == keys[i])
                            {
                                duplication = true;
                                break;
                            }
                        }
                        if (!duplication)
                        {
                            dictionary[j].Add(keys[i], value);
                        }
                        duplication = false;

                    }
                }
                

                return dictionary;
            }
            public bool CheckDuplication(Dictionary<string, bool> dictionary, string key)
            {
                return true;
            }

        }
    }
}
