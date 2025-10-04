using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
namespace KumagaiLibrary
{
    public class String
    {
        /// <summary>
        /// 引数1に渡した文字列を引数2で渡した色に変更した文字列を返します
        /// </summary>
        /// <param name="text">色を変えたい文字列</param>
        /// <param name="color">変えたい色</param>
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
                Debug.Log("基底クラス側の関数が呼ばれています\n継承先の関数をオーバーライドしてください");//基底クラス側の関数が呼び出されていることを知らせる
            }

            public static void PointerDown()
            {
                Debug.Log("基底クラス側の関数が呼ばれています\n継承先の関数をオーバーライドしてください");
            }

            public static void PointerExit()
            {
                Debug.Log("基底クラス側の関数が呼ばれています\\n継承先の関数をオーバーライドしてください\"");
            }

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
            public static List<string[]> Read(TextAsset csvData) //引数に入力したCSVファイルをリストに変換する関数
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



}

