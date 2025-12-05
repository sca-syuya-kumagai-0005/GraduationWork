using UnityEngine;

public class Sinner_019VS : MonoBehaviour
{
    public enum VSState
    {
        Win,
        Lose,
        Battle
    }

    [Header("現在の状態")]
    public VSState currentState;

    [Header("Animator（Win/Lose Boolを持つ）")]
    public Animator animator;

    [Header("表示/非表示対象オブジェクト")]
    public GameObject winObject;
    public GameObject loseObject;

    [Header("表示時に消したいTarget ×2")]
    public GameObject targetObj01;
    public GameObject targetObj02;


    private void OnEnable()
    {
        ApplyStateAnimBool();
        ApplyStateObjectActive();
        HideTargetObjectsIfVisible();
    }

    private void ApplyStateAnimBool()
    {
        if (animator == null) return;

        bool win = (currentState == VSState.Win);
        bool lose = (currentState == VSState.Lose);

        animator.SetBool("Win", win);
        animator.SetBool("Lose", lose);

        // Battleは全てFalse
        if (currentState == VSState.Battle)
        {
            animator.SetBool("Win", false);
            animator.SetBool("Lose", false);
        }
    }

    private void ApplyStateObjectActive()
    {
        switch (currentState)
        {
            case VSState.Win:
                SetActive(winObject, true);
                SetActive(loseObject, false);
                break;

            case VSState.Lose:
                SetActive(winObject, false);
                SetActive(loseObject, true);
                break;

            case VSState.Battle:
                SetActive(winObject, false);
                SetActive(loseObject, false);
                break;
        }
    }

    private void HideTargetObjectsIfVisible()
    {
        if (targetObj01 != null && targetObj01.activeSelf) targetObj01.SetActive(false);
        if (targetObj02 != null && targetObj02.activeSelf) targetObj02.SetActive(false);
    }


    private void SetActive(GameObject obj, bool value)
    {
        obj.SetActive(value);
    }

    void ActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
