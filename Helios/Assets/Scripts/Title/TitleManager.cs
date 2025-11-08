using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Select
{
    BEGINNING = 0,
    CONTINUATION,
    OPTION,
    END,
    MAX,
}

public class TitleManager : MonoBehaviour
{
    [SerializeField] TitleAnimationManager animationManager;
    [SerializeField] MoveTest moveTest;
    KeyCode[] inputKey;
    Select select;
    private void Awake()
    {
        Locator<TitleManager>.Bind(this);
        select = Select.BEGINNING;
        inputKey = new KeyCode[2];
        inputKey[0] = KeyCode.A;
        inputKey[1] = KeyCode.D;
    }
    void Start()
    {
        StartCoroutine(TitleStart());
    }

    void Update()
    {
        float mouse = Input.mouseScrollDelta.y;
        for (int i = 0; i < inputKey.Length; i++)
        {
            if (Input.GetKeyDown(inputKey[i]) || mouse != 0f)
            {
                if(moveTest.isMove) return;
                float f = (mouse != 0f) ? mouse : -Input.GetAxisRaw("Horizontal");
                select = ((int)select + (int)f < 0) ? Select.END : (Select)(((int)select + (int)f) % (int)Select.MAX);
                StartCoroutine(moveTest.Move(f));
            }
        }
    }

    IEnumerator TitleStart()
    {
        yield return StartCoroutine(animationManager.CautionaryAnim(CautionaryNum.WARNING));
        yield return StartCoroutine(animationManager.CautionaryAnim(CautionaryNum.SOUND));
        yield return StartCoroutine(animationManager.TitleStartAnim());
    }

    public void ChangeSelect(int _add)
    {

    }
}
