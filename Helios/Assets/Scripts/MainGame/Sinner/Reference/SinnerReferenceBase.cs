using UnityEngine;
using UnityEngine.UI;
using System;

public class SinnerReferenceBase : MonoBehaviour
{
     
    protected struct SinnerRisk
    {
        public const string LUMENIS = "Lumenis";
        public const string VELGRA = "Velgra";
        public const string DRAVEX = "Dravex";
        public const string ZERATH = "Zerath";
        public const string OBLIVARA = "Oblivara";
    }

    protected struct SinnerType
    {
        public const string SECRA = "Secra";
        public const string VIGIL = "Vigil";
        public const string HAZRA = "Hazra";
        public const string CATRA = "Catra";
        public const string NULLA = "Nulla";
    }

    [System.Serializable]
    public struct SinnerInfomation
    {
        public string name;//名前
        public int id;//番号
        public string risk;//リスククラス
        public string type;//収容クラス
        public string abnormal;//異常の内容
        public string[] condition;//異常の条件
        public string apperance;//外観説明
        public string exeplanation;//説明
        public string[] overView;//インタビューやそのほか
        public Sprite icon;//アイコン
        public GameObject thisObject;//この情報を与えるオブジェクト
    }
    [System.Serializable]
    public struct Risk
    {
        public bool lumenis;
        public bool velgra;
        public bool dravex;
        public bool zerath;
        public bool oblivara;
    }
    [System.Serializable]
    public struct Type
    {
        public bool secra;
        public bool vigil;
        public bool hazra;
        public bool catra;
        public bool nulla;
    }
       
    
}
