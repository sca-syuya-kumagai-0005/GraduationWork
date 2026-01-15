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
    Select select;
    [SerializeField] GameObject optionCanvasObj;
    bool isSelect;
    [SerializeField] AudioClip titleBGM;
    private void Awake()
    {
        Locator<TitleManager>.Bind(this);
        select = Select.BEGINNING;
        isSelect = false;
    }
    void Start()
    {
        Locator<AudioManager>.Instance.PlayBGM(titleBGM);
        StartCoroutine(TitleStart());
    }

    void Update()
    {
        InputSlingerMove();
    }

    void InputSlingerMove()
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
        StartCoroutine(animationManager.TitleSelectDisplayAnim());
    }
    public void SelectStart()
    {
        isSelect = true;
    }

    [Button]
    public void OptionOpen()
    {
        Instantiate(optionCanvasObj);
    }

    public void CloseMyRoom()
    {
        StartCoroutine(animationManager.TitleSelectDisplayAnim());
    }

    public void OpenMemorySlinger()
    {
        StartCoroutine(animationManager.MemorySlingerAnim());
    }

    public void CloseMemorySlinger()
    {
        StartCoroutine(animationManager.MyRoomAnim());
    }

    IEnumerator GoMain(bool _isPlay)
    {
        yield return StartCoroutine(animationManager.FadeAnimation(1f,0.5f));
        SceneManager.LoadScene("Adventure");
    }

    public void SelectButton()
    {
        isSelect = false;
        switch (select)
        {
            case Select.BEGINNING:
                StartCoroutine(GoMain(false));
                break;
            case Select.CONTINUATION:
                StartCoroutine(GoMain(true));
                break;
            case Select.MYROOM:
                StartCoroutine(animationManager.MyRoomAnim());
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

    public void ending()
    {
        SceneManager.LoadScene("EndRollScene");
    }
}
