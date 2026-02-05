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
    [SerializeField] AudioClip selectSE;
    [SerializeField] AudioClip slingerSE;

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
        Locator<AudioManager>.Instance.PlaySE(slingerSE);
        select = ((int)select + _dir < 0) ? Select.END : (Select)(((int)select + (int)_dir) % (int)Select.MAX);
        StartCoroutine(slingerMove.Move(_dir));
    }

    IEnumerator TitleStart()
    {
        yield return StartCoroutine(animationManager.CautionaryAnim(CautionaryNum.WARNING));
        yield return StartCoroutine(animationManager.CautionaryAnim(CautionaryNum.SOUND));
        yield return StartCoroutine(animationManager.TitleStartAnim(selectSE));
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
        Locator<AudioManager>.Instance.PlaySE(selectSE);
        Instantiate(optionCanvasObj);
    }

    public void CloseMyRoom()
    {
        Locator<AudioManager>.Instance.PlaySE(selectSE);
        StartCoroutine(animationManager.BackTitleSelect());
    }

    public void OpenMemorySlinger()
    {
        Locator<AudioManager>.Instance.PlaySE(selectSE);
        StartCoroutine(animationManager.MemorySlingerAnim());
    }

    public void CloseMemorySlinger()
    {
        Locator<AudioManager>.Instance.PlaySE(selectSE);
        StartCoroutine(animationManager.MyRoomAnim());
    }

    IEnumerator GoMain(bool _isPlay)
    {
        yield return StartCoroutine(animationManager.FadeAnimation(1f,0.5f));
        if(_isPlay)
        {
            SaveDataManager saveDataManager = GameObject.Find("SaveManager").GetComponent<SaveDataManager>();
            saveDataManager.Load();
        }
        SceneManager.LoadScene("Adventure");
    }

    public void SelectButton()
    {
        isSelect = false;
        Locator<AudioManager>.Instance.PlaySE(selectSE);
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
