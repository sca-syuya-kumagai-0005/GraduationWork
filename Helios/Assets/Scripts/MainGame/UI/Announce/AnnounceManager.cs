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
        DELETE,
        END
    }
    [SerializeField]
    private List<float>times;
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
    private float viewDuration;
    [SerializeField]
    private int viewLimit;
    private struct AnnounceTimer
    {
        public float timer { get; set; }
        public bool requested { get; set; }
        public AnnounceTimer(float _timer) 
        {
            timer = _timer;
            requested = false;
        }
    }
    [SerializeField]
    private List<AnnounceTimer> timerList = new List<AnnounceTimer>();

    private const int listTop = 0;
    // Update is called once per frame
    void Update()
    {
        times = new List<float>();
        for (int i = 0; i < timerList.Count; i++)
        {
            times.Add(timerList[i].timer);
            float late = 1.0f - times[i] / viewDuration;
            announceList[i].GetComponent<Announce>()
                .SetTimerFrame = late;
        }
            if (Input.GetMouseButtonDown(0)) MakeAnnounce();

        for (int i = 0; i < timerList.Count; i++)
        {
            AnnounceTimer announceTimer = timerList[i];
            announceTimer.timer += Time.deltaTime;
            timerList[i] = announceTimer;
            if (announceList.Count > viewLimit)
            {
                AnnounceTimer at = timerList[listTop];
                at.timer += viewDuration;
                timerList[listTop] = at;
            }
            if(timerList[i].timer >= viewDuration && !timerList[i].requested)
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

            case State.WAIT:
                if (requestList[listTop] == RequestType.END) state = State.CHACK;
                break;
        }
    }

    private void LateUpdate()
    {
        switch (state)
        {
            case State.PUSH:
                if (announceList.Count != 0)
                    for (int i = 0; i < announceList.Count; i++)
                        StartCoroutine(PushUp(announceList[i]));
                else
                {
                    requestList[listTop] = RequestType.END;
                    state = State.CHACK;
                }
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
        AnnounceTimer annouceTimer = new AnnounceTimer(0.0f);
        timerList.Add(annouceTimer);
        announceList.Add(announce);
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
    private IEnumerator PushUp(GameObject annouceObject)
    {
        state = State.WAIT;
        bool isEnd = false;
        float t = 0.0f;
        const float motionLate = 0.5f;
        float addPosY = 0.0f;
        Vector3 defaultPosition = annouceObject.transform.localPosition;

        while (!isEnd)
        {
            defaultPosition.x = annouceObject.transform.position.x;
            addPosY = announceSizeY * EaseOutCirc(t);
            annouceObject.transform.localPosition = defaultPosition + new Vector3(0, addPosY, 0);
            if (t >= 1.0f) isEnd = true;
            t += Time.deltaTime / motionLate;
            yield return null;
        }
        requestList[listTop] = RequestType.END;
    }
}

