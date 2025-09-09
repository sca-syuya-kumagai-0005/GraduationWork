using UnityEngine;
using UnityEngine.UI;

public class ResidenceCertificate : EventSet
{
    private GameObject information;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetEventType(down, OnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnClick()
    {
        Debug.Log(gameObject.name+":ƒNƒŠƒbƒN‚³‚ê‚½");
    }
}
