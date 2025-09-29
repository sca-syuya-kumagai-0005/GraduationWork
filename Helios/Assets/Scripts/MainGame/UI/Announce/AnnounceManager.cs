using System.Collections;
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
    private Vector3 localPosition = new Vector3(-1920 / 2, 1080 / 2, 925);
    private enum RequestType
    {
        MAKE, 
        DELETE,
        END
    }
    [SerializeField]
    private List<RequestType> requestList = new List<RequestType>();
    private enum State
    {
        CHACK,
        DO,
        PUSH,
        WAIT,
    }
    private State state;

    [SerializeField]
    private float duration;
    private struct AnnounceTimer
    {
        public float timer { get; set; }
        public bool requested { get; set; }
        public AnnounceTimer(float _timer, bool _requested) 
        {
            timer = _timer;
            requested = _requested;
        }
    }
    [SerializeField]
    private List<AnnounceTimer> timerList = new List<AnnounceTimer>();

    private const int listTop = 0;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) requestList.Add(RequestType.MAKE);

        for (int i = 0; i < timerList.Count; i++)
        {
            AnnounceTimer announceTimer = timerList[i];
            announceTimer.timer += Time.deltaTime;
            timerList[i] = announceTimer;
            if(timerList[i].timer >= duration && !timerList[i].requested)
            {
                announceTimer.requested = true;
                timerList[i] = announceTimer;
                requestList.Add(RequestType.DELETE);
            }
        }

        switch (state)
        {
            case State.CHACK:
                if (requestList.Count > 0) state = State.DO;
            break;

            case State.DO:
                switch (requestList[listTop])
                {
                    case RequestType.MAKE:
                        MakeAnnounce();
                        state = State.CHACK;
                    break;

                    case RequestType.DELETE:
                        DeleteAnnounce();
                        state = State.PUSH;
                    break;

                    case RequestType.END:
                        requestList.RemoveAt(listTop);
                        state = State.CHACK;
                    break;
                }
            break;

            case State.PUSH:
                StartCoroutine(PushUp());
            break;

            case State.WAIT:
                if (requestList[listTop] == RequestType.END) state = State.CHACK;
                break;
        }
    }

    public void MakeAnnounce()
    {
        Vector3 instantPosition = localPosition;
        instantPosition.x -= announceSizeX;
        instantPosition.y -= announceSizeY * announceList.Count;
        GameObject announce = Instantiate(announcePrefab,instantPosition,Quaternion.identity,transform);
        Image iconImage = announce.GetComponent<Image>();
        Text text = announce.GetComponent<Text>();
        AnnounceTimer annouceTimer = new AnnounceTimer(0.0f,false);
        timerList.Add(annouceTimer);
        announceList.Add(announce);
        requestList.RemoveAt(listTop);
    }

    private void DeleteAnnounce()
    {
        if (announceList.Count > 0)//ç≈èIìIÇ…è¡Ç∑
        {
            StartCoroutine(announceList[listTop].GetComponent<Announce>().FadeOut());
            announceList.RemoveAt(listTop);
            timerList.RemoveAt(listTop);
        }
        else Debug.Log("í ímÇ™Ç†ÇËÇ‹ÇπÇÒ");
    }
    private IEnumerator PushUp()
    {
        state = State.WAIT;
        bool isEnd = false;
        float t = 0.0f;
        const float motionLate = 0.5f;
        float addPosY = 0.0f;
        Vector3[] defaultPosition = new Vector3[announceList.Count];
        for (int i = 0; i < defaultPosition.Length; i++) 
            defaultPosition[i] = announceList[i].transform.localPosition;

        while (!isEnd)
        {
            for (int i = 0; i < announceList.Count; i++)
            {
                addPosY = announceSizeY * EaseOutCirc(t);
                announceList[i].transform.localPosition = defaultPosition[i] + new Vector3(0, addPosY, 0);
            }
            if (t >= 1.0f) isEnd = true;
            t += Time.deltaTime / motionLate;
            yield return null;
        }
        //yield return new WaitForSeconds(0.5f);
        requestList[0] = RequestType.END;
    }
}

