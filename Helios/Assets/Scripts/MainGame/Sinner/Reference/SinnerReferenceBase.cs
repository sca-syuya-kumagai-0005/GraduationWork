using UnityEngine;

public class SinnerReferenceBase : MonoBehaviour
{
     
    public struct SinnerRisk
    {
        public const string LUMENIS = "Lumenis";
        public const string VELGRA = "Velgra";
        public const string DRAVEX = "Dravex";
        public const string ZERATH = "Zerath";
        public const string OBLIVARA = "Oblivara";
    }

    public struct SinnerType
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
        public string name;
        public int id;
        public string risk;
        public string type;
        public string abnormal;
        public string[] condition;
        public string apperance;
        public string exeplanation;
        public string[] overView;
    }


    public struct SortFlags
    {
        public struct Risk
        {
           public bool lumenis;
           public bool velgra;
           public bool dravex;
           public bool zerath;
           public bool oblivara;
        }

        public struct Type
        {
            public bool secra;
            public bool vigil;
            public bool hazra;
            public bool catra;
            public bool nulla;
        }
       
    }
}
