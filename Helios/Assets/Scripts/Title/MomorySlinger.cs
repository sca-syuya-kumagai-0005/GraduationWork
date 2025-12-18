using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MomorySlinger : MonoBehaviour
{
    [SerializeField] SlingerMove slingerMove;
    int nowSinner;
    int maxSinner;//csv‚©‚ç—v‘f”‚ğ‘ã“ü
    [SerializeField] Text[] sinnerNameTexts;
    const int right = 2;
    const int left = 2;
    string[] names;

    private void Awake()
    {
        nowSinner = 0;
        maxSinner = 32;
        names = new string[maxSinner];
        for(int i = 0; i < maxSinner; i++)
        {
            names[i] = (i + 1).ToString();
        }
        for (int i = 0; i < sinnerNameTexts.Length; i++)
        {
            int num = (i < right) ? i : maxSinner - (sinnerNameTexts.Length - i);
            sinnerNameTexts[i].text = names[num];
        }
    }

    private void Update()
    {
        InputSlingerMove();
    }

    void InputSlingerMove()
    {
        if (slingerMove.isMove) return;
        float mouse = Input.mouseScrollDelta.y;
        float axis = Input.GetAxisRaw("Horizontal");
        if (axis != 0 || mouse != 0f)
        {
            float f = (mouse != 0f) ? mouse : -axis;

            int a = (f < 0) ? right : left;
            int num = (a * (int)f + nowSinner + sinnerNameTexts.Length) % sinnerNameTexts.Length;
            int nameNum = ((int)f + nowSinner + maxSinner) % maxSinner;
            Debug.Log(nameNum);
            sinnerNameTexts[num].text = names[nameNum];
            nowSinner = (nowSinner <= 0 && f < 0) ? maxSinner - 1 : (nowSinner + (int)f) % maxSinner;
            StartCoroutine(slingerMove.Move(f));
        }
    }
}
