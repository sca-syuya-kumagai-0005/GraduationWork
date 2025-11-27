using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Select
{
    BEGINNING = 0,
    CONTINUATION,
    MYROOM,
    END,
    MAX,
}

public class TitleManager : MonoBehaviour
{
    [SerializeField] TitleAnimationManager animationManager;
    [SerializeField] SlingerMove slingerMove;
    KeyCode[] inputKey;
    Select select;
    [SerializeField] GameObject optionCanvasObj;
    bool isSelect;
    private void Awake()
    {
        Locator<TitleManager>.Bind(this);
        select = Select.BEGINNING;
        inputKey = new KeyCode[2];
        inputKey[0] = KeyCode.A;
        inputKey[1] = KeyCode.D;
        isSelect = false;
    }
    void Start()
    {
        StartCoroutine(TitleStart());
    }

    void Update()
    {
        if (!isSelect || slingerMove.isMove) return;
        float mouse = Input.mouseScrollDelta.y;
        float axis = Input.GetAxisRaw("Horizontal");
        if (axis != 0 || mouse != 0f)
        {
            float f = (mouse != 0f) ? mouse : -axis;
            select = ((int)select + (int)f < 0) ? Select.END : (Select)(((int)select + (int)f) % (int)Select.MAX);
            StartCoroutine(slingerMove.Move(f));
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SelectButton();
        }
    }

    IEnumerator TitleStart()
    {
        yield return StartCoroutine(animationManager.CautionaryAnim(CautionaryNum.WARNING));
        yield return StartCoroutine(animationManager.CautionaryAnim(CautionaryNum.SOUND));
        yield return StartCoroutine(animationManager.TitleStartAnim());
        yield return StartCoroutine(animationManager.TitleSelectDisplayAnim());
        isSelect = true;
    }

    public void OptionOpen()
    {
        Instantiate(optionCanvasObj);
    }

    public void SelectButton()
    {
        switch (select)
        {
            case Select.BEGINNING:
                SceneManager.LoadScene("");
                break;
            case Select.CONTINUATION:
                SceneManager.LoadScene("");
                break;
            case Select.MYROOM:
                break;
            case Select.END:
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
                break;
        }
    }
}
