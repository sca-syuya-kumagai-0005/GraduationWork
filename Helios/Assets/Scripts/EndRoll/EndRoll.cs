using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using KumagaiLibrary.Unity;
using UnityEngine.UI;
using DG.Tweening;

public class EndRoll : MonoBehaviour
{
    [SerializeField] AudioClip endRollClip;
    [SerializeField] GameObject ContentsObj;
    [SerializeField] GameObject endRollTextObj;
    [SerializeField] TextAsset endRollCsv;

    private void OnEnable()
    {
        Locator<EndRoll>.Bind(this);
    }

    void Start()
    {
        StartCoroutine(EndRollAnim());
    }

    IEnumerator EndRollAnim()
    {
        float time = endRollClip.length;
        const float dis = 900f;
        Debug.Log(time);
        List<string[]> csv = CsvManager.Read(endRollCsv);
        float y = 150f;
        for(int i = 1;i < csv.Count; i++)
        {
            GameObject obj = Instantiate(endRollTextObj, ContentsObj.transform);
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, y, 0);
            y -= dis;
            Text t = obj.GetComponent<Text>();
            t.text = csv[i][0] + "\n";
            for (int names = 1; names < csv[i].Length; names++)
            {
                t.text += "\n" + csv[i][names];
            }
        }
        AudioManager audioManager = Locator<AudioManager>.Instance;
        audioManager.PlayBGM(endRollClip);
        RectTransform rect = ContentsObj.GetComponent<RectTransform>();
        Debug.Log(y);
        rect.DOAnchorPosY(-y,time).SetEase(Ease.Linear);
        yield return null;
    }
}
