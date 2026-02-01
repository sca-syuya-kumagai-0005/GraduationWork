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
            SlingerButton((int)f);
            Debug.Log(select);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SelectButton();
        }
    }

    public void SlingerButton(int _dir)
    {
        if (!isSelect || slingerMove.isMove) return;
        select = ((int)select + _dir < 0) ? Select.END : (Select)(((int)select + (int)_dir) % (int)Select.MAX);
        StartCoroutine(slingerMove.Move(_dir));
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
        select = Select.BEGINNING;
    }

    [Button]
    public void OptionOpen()
    {
        Instantiate(optionCanvasObj);
    }

    public void CloseMyRoom()
    {
        StartCoroutine(animationManager.BackTitleSelect());
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
