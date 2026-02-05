using UnityEngine;
using UnityEngine.UI;

public class ConditionText : MonoBehaviour
{
    [SerializeField] Text conditionText;

    public void SetText(string _condition)
    {
        conditionText.text = _condition;
    }
}
