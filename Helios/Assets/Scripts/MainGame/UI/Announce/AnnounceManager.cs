using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnnounceManager : EasingMethods
{
    [SerializeField]
    private GameObject announcePrefab;
    private const float announceSizeX = 500.0f;
    private const float announceSizeY = 190.0f;
    protected List<GameObject> announceList = new List<GameObject>();
    public int GetAnnouceCount { get { return announceList.Count; } }
    private Vector3 localPosition= new Vector3(-1920 / 2, 1080 / 2, 925);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) MakeAnnounce();
        if (Input.GetMouseButtonDown(1)) DeleteAnnounce();
    }

    public void MakeAnnounce()
    {
        Vector3 instantPosition = localPosition;
        instantPosition.x -= announceSizeX;
        instantPosition.y -= announceSizeY * announceList.Count;
        GameObject announce = Instantiate(announcePrefab,instantPosition,Quaternion.identity,transform);
        Image iconImage = announce.GetComponent<Image>();
        Text text = announce.GetComponent<Text>();
        announceList.Add(announce);
    }

    private void DeleteAnnounce()
    {
        if (announceList.Count > 0)
        {
            StartCoroutine(announceList[0].GetComponent<Announce>().DeleteThisAnnounce());
            announceList.Remove(announceList[0]);
            for(int i = 0; i < announceList.Count; i++)
            {
                StartCoroutine(announceList[i].GetComponent<Announce>().PushUp());
            }
        }
        else Debug.Log("’Ê’m‚ª‚ ‚è‚Ü‚¹‚ñ");
    }
}
