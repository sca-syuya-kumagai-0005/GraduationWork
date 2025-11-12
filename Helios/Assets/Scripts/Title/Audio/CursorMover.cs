using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CursorMover : MonoBehaviour
{
    enum Direction
    {
        HORIZONTAL,
        VERTICAL,
    }

    [SerializeField] Direction direction;
    //KeyCode addKey;
    //KeyCode subKey;
    string dir;
    string dPad;

    //[SerializeField] GameObject cursor; カーソルにアタッチするため不要
    [SerializeField] GameObject[] buttons;
    RectTransform cursorRect;
    RectTransform[] buttonsRect;
    public Image cursorImg {  get; private set; }
    public int nowButton {  get; private set; }
    public bool move {  get; private set; }

    float time;
    const float moveTime = 0.75f;
    [SerializeField] float alpha;

    void Awake()
    {
        switch (direction) { 
            case Direction.HORIZONTAL:
                dir = "Horizontal";
                dPad = "Debug Horizontal";
                break;
            case Direction.VERTICAL:
                dir = "Vertical";
                dPad = "Debug Vertical";
                break;
        }

        cursorImg = GetComponent<Image>();
        cursorRect = GetComponent<RectTransform>();
        Reset();
    }

    private void OnEnable()
    {
        move = false;
        cursorRect.anchoredPosition = buttonsRect[nowButton].anchoredPosition;
    }

    void Update()
    {
        if (move) return;

        float inputDir = Input.GetAxisRaw(dir);
        if(inputDir == 0) inputDir = Input.GetAxisRaw(dPad);
        if (inputDir != 0)
        {
            int stockNum = nowButton;
            if (direction == Direction.VERTICAL) inputDir *= -1;
            nowButton += (int)inputDir;
            if (nowButton < 0) nowButton = 0;
            if (nowButton > buttonsRect.Length - 1) nowButton = buttonsRect.Length - 1;
            if(nowButton != stockNum) StartCoroutine(CursorMove());
        }

        //if (Input.GetKeyDown(addKey))
        //{
        //    if (nowButton < buttons.Length - 1)
        //    {
        //        nowButton++;
        //        StartCoroutine(CursorMove());
        //    }
        //}
        //if (Input.GetKeyDown(subKey))
        //{
        //    if (nowButton > 0)
        //    {
        //        nowButton--;
        //        StartCoroutine(CursorMove());
        //    }
        //}
    }

    public void Reset()
    {
        buttonsRect = new RectTransform[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            buttonsRect[i] = buttons[i].GetComponent<RectTransform>();
        }
        move = false;
        nowButton = 0;
        cursorRect.anchoredPosition = buttonsRect[nowButton].anchoredPosition;
    }

    public void SetImage(Sprite _sprite)
    {
        cursorImg.sprite = _sprite;
    }

    IEnumerator CursorMove()
    {
        move = true;
        cursorImg.DOFade(0f, 0.05f);
        yield return cursorRect.DOAnchorPos(buttonsRect[nowButton].anchoredPosition, 0.075f).WaitForCompletion();
        cursorImg.DOFade(alpha, 0.5f);
        yield return new WaitForSeconds(0.05f);
        move = false;
    }
}
