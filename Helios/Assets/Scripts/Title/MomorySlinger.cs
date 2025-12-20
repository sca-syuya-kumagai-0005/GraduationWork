using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MomorySlinger : MonoBehaviour
{
    [SerializeField] SlingerMove slingerMove;
    int nowSlinger;
    int nowSinner;
    int maxSinner;//csv‚©‚ç—v‘f”‚ğ‘ã“ü
    [SerializeField] Text[] sinnerNameTexts;
    const int right = 2;
    const int left = 2;
    string[] names;

    private void Awake()
    {
        nowSlinger = 0;
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

    private void LateUpdate()
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
            mouse /= Mathf.Abs(mouse);
            float f = (mouse != 0f) ? mouse : -axis;
            int a = (f < 0) ? right : left;
            int num = (a * (int)f + nowSlinger + sinnerNameTexts.Length) % sinnerNameTexts.Length;
            int nameNum = (a * (int)f + nowSinner + maxSinner) % maxSinner;
            sinnerNameTexts[num].text = names[nameNum];
            nowSlinger = (nowSlinger + (int)f < 0) ? sinnerNameTexts.Length - 1 : (nowSlinger + (int)f) % sinnerNameTexts.Length;
            nowSinner = (nowSinner + (int)f < 0) ? maxSinner - 1 : (nowSinner + (int)f) % maxSinner;
            StartCoroutine(slingerMove.Move(f));
        }
    }
}
