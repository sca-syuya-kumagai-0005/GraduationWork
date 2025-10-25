using System.Collections;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] TitleAnimationManager animationManager;
    private void Awake()
    {
        
    }
    void Start()
    {
        StartCoroutine(TitleStart());
    }

    IEnumerator TitleStart()
    {
        yield return StartCoroutine(animationManager.CautionaryAnim(CautionaryNum.WARNING));
        yield return StartCoroutine(animationManager.CautionaryAnim(CautionaryNum.SOUND));
        yield return StartCoroutine(animationManager.TitleAnim());
    }
}
