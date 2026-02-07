using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TutorialMG : MonoBehaviour
{
    public enum TutorialState
    {
        None,
        Click,
        DeliveryExplanation,
        DocumentCheck,
        OpenDocument,
        OneMoreHouseClick,
        TypeSelection,
        PushButton,
        DrawLine,
        EndDrawLine,
        EndDelivery,
        ProgressGauge,
        FailureExplanation,
        DoItAloneIdiot
    }

    public enum WaitType
    {
        Click,
        External
    }



    public TutorialState CurrentState => currentState;


    //========================
    // テキスト＆State設定データ
    //========================
    [System.Serializable]
    public class TutorialTextData
    {
        public TutorialState state;

        [TextArea(2, 4)]
        public string[] messages;

        [Header("Wait Type (per line)")]
        public WaitType[] waitTypes;

        public SpriteRenderer[] sprites;

        [Header("Show Settings")]
        public bool showUIWindow = true;
        public bool showCharacter = true;
    }

    //[SerializeField] private 
    private bool skipCurrentMessage = false;

    [SerializeField] private UnmaskScale unmaskScale;
    [SerializeField] private Image tutorialcurrent;

    [SerializeField] private string[] targetMessages;


    [Header("Tutorial State")]
    [SerializeField] private TutorialState currentState = TutorialState.None;

    [Header("Objects")]
    [SerializeField] private GameObject uiWindow;
    [SerializeField] private GameObject character;
    [SerializeField] private Text tutorialText;

    [Header("Text Data")]
    [SerializeField] private TutorialTextData[] tutorialTextDatas;

    [Header("Text Settings")]
    [SerializeField] private float charInterval = 0.05f;
    private Coroutine textCoroutine;

    [Header("Mask Objects")]
    [SerializeField] private GameObject pin;
    [SerializeField] private Image unMask;
    [SerializeField] private Image screen;

    private bool isTyping = false;
    private bool isWaitingForClick = false;
    private bool isClicked = false;
    bool actionInvoked = false;
    private TutorialState prevInspectorState;

    private Dictionary<(TutorialState, int), Action> stateMessageActions;

    [SerializeField]private bool isTutorial = true;
    public bool IsTutorial
    {
        get { return isTutorial; } // getterの部分
        set { isTutorial = value; } // setterの部分}
    }

    private bool isWaitingForExternalCall = false;
    private bool externalCalled = false;

    public void ContinueFromOutside()
    {
        externalCalled = true;

        // タイピング中 or 待機前でも次へ進める
        skipCurrentMessage = true;
        isTyping = false;
        isClicked = true;
    }


    void Awake()
    {
        stateMessageActions = new Dictionary<(TutorialState, int), Action>();

        // OpenDocument 開始直後（0）
        stateMessageActions.Add((TutorialState.DeliveryExplanation, 0), () =>
        {
            Debug.Log("OpenDocument : 開始");
            unmaskScale.StartByState(TutorialState.DeliveryExplanation);
            unMask.enabled = true;
            screen.enabled = true;
        });

        // OpenDocument の2つ目
        stateMessageActions.Add((TutorialState.DeliveryExplanation, 3), () =>
        {
            Debug.Log("OpenDocument : 2つ目");
            unMask.enabled = false;
        });



        // OpenDocument の1つ目
        stateMessageActions.Add((TutorialState.OpenDocument, 1), () =>
        {
            Debug.Log("OpenDocument : 1つ目");
            unmaskScale.StartByState(TutorialState.OpenDocument);
            unMask.enabled = true;
            screen.enabled = true;
        });

        // OpenDocument の2つ目
        stateMessageActions.Add((TutorialState.OpenDocument, 2), () =>
        {
            Debug.Log("OpenDocument : 2つ目");
            unMask.enabled = false;
        });

        // OpenDocument の3つ目
        stateMessageActions.Add((TutorialState.OpenDocument, 3), () =>
        {
            unmaskScale.StartByState(TutorialState.OpenDocument);
            unMask.enabled = true;
        });

        // OpenDocument の4つ目
        stateMessageActions.Add((TutorialState.OpenDocument, 4), () =>
        {
            unmaskScale.StartByState(TutorialState.OpenDocument);
        });

        // OpenDocument の5つ目
        stateMessageActions.Add((TutorialState.OpenDocument, 5), () =>
        {
            unmaskScale.StartByState(TutorialState.OpenDocument);
            unMask.enabled = false;
        });
        stateMessageActions.Add((TutorialState.OpenDocument, 6), () =>
        {
            unmaskScale.StartByState(TutorialState.OpenDocument);
            unMask.enabled = true;
        });
        stateMessageActions.Add((TutorialState.OpenDocument, 7), () =>
        {
            unmaskScale.StartByState(TutorialState.OpenDocument);
            Debug.Log("7");
        });
        stateMessageActions.Add((TutorialState.OpenDocument, 8), () =>
        {
            Debug.Log("8");
            unmaskScale.StartByState(TutorialState.OpenDocument);
        });
        stateMessageActions.Add((TutorialState.OpenDocument, 9), () =>
        {
            Debug.Log("9");
            unmaskScale.StartByState(TutorialState.OpenDocument);
        });
        stateMessageActions.Add((TutorialState.OpenDocument, 10), () =>
        {
            Debug.Log("10");
            unmaskScale.StartByState(TutorialState.OpenDocument);
        });

        stateMessageActions.Add((TutorialState.TypeSelection, 1), () =>
        {
            unMask.enabled = true;
            screen.enabled = true;
            unmaskScale.StartByState(TutorialState.TypeSelection);
        });
        stateMessageActions.Add((TutorialState.TypeSelection, 2), () =>
        {
            unmaskScale.StartByState(TutorialState.TypeSelection);
        });
        stateMessageActions.Add((TutorialState.TypeSelection, 3), () =>
        {
            unMask.enabled = false;
            screen.enabled = false;
        });
        stateMessageActions.Add((TutorialState.TypeSelection, 4), () =>
        {
            unmaskScale.StartByState(TutorialState.TypeSelection);
        });
        stateMessageActions.Add((TutorialState.ProgressGauge, 0), () =>
        {
            unMask.enabled = true;
            screen.enabled = true;
            unmaskScale.StartByState(TutorialState.ProgressGauge);
        });
        stateMessageActions.Add((TutorialState.ProgressGauge, 1), () =>
        {
            unmaskScale.StartByState(TutorialState.ProgressGauge);
        });
        stateMessageActions.Add((TutorialState.ProgressGauge, 3), () =>
        {
            unmaskScale.StartByState(TutorialState.ProgressGauge);
        });
    }




    void OnValidate()
    {
        if (!Application.isPlaying) return;

        if (prevInspectorState != currentState)
        {
            ChangeState(currentState, true);
            prevInspectorState = currentState;
        }
    }

    void Start()
    {
        if(currentState == TutorialState.Click)
        {
            tutorialcurrent.enabled = true;

        }
        else
        {
            tutorialcurrent.enabled = false;
        }
        unMask.enabled = false;
        screen.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                isTyping = false;
            }
            else if (isWaitingForClick)
            {
                isClicked = true;
            }
        }
    }

    //========================
    // State Control
    //========================
    public void ChangeState(TutorialState nextState, bool force = false)
    {
        if (!force && currentState == nextState) return;

        TutorialState prevState = currentState;
        currentState = nextState;

        Debug.Log($"TutorialState変更: {prevState} → {currentState}");

        unMask.enabled = false;
        screen.enabled = false; 
        OnStateChanged(prevState, currentState);
        if(currentState == TutorialState.Click)
        {
            tutorialcurrent.enabled = true;
        }
    }

    public void OnStateChanged(TutorialState prev, TutorialState current)
    {
        ShowTutorial(current);
    }

    //========================
    // Tutorial 表示
    //========================
    void ShowTutorial(TutorialState state)
    {
        TutorialTextData data = GetDataByState(state);
        if (data == null || data.messages == null || data.messages.Length == 0)
        {
            uiWindow?.SetActive(false);
            character?.SetActive(false);
            return;
        }

        uiWindow?.SetActive(data.showUIWindow);
        character?.SetActive(data.showCharacter);

        tutorialText.text = "";

        if (textCoroutine != null)
            StopCoroutine(textCoroutine);

        textCoroutine = StartCoroutine(PlayTutorialText(data.messages));
    }

    TutorialTextData GetDataByState(TutorialState state)
    {
        foreach (var data in tutorialTextDatas)
        {
            if (data.state == state)
                return data;
        }
        return null;
    }

    IEnumerator PlayTutorialText(string[] messages)
    {
        TutorialTextData data = GetDataByState(currentState);

        for (int i = 0; i < messages.Length; i++)
        {
            string message = messages[i];
            tutorialText.text = "";

            isTyping = true;
            isClicked = false;
            externalCalled = false;
            skipCurrentMessage = false;
            bool actionInvoked = false;

            foreach (char c in message)
            {
                if (!isTyping || skipCurrentMessage)
                {
                    tutorialText.text = message;
                    break;
                }

                tutorialText.text += c;

                if (tutorialText.text.Length == 1)
                {
                    TryInvokeStateMessageAction(i, ref actionInvoked);
                }

                yield return new WaitForSeconds(charInterval);
            }

            isTyping = false;

            // 待ちタイプ判定
            WaitType waitType = WaitType.Click;
            if (data.waitTypes != null && i < data.waitTypes.Length)
            {
                waitType = data.waitTypes[i];
            }

            // External がもう来てたら待たない
            if (skipCurrentMessage)
                continue;

            if (waitType == WaitType.External)
            {
                isWaitingForExternalCall = true;
                yield return new WaitUntil(() => externalCalled);
                isWaitingForExternalCall = false;
            }
            else
            {
                isWaitingForClick = true;
                yield return new WaitUntil(() => isClicked);
                isWaitingForClick = false;
            }
        }

        EndConversation();
    }



    void TryInvokeStateMessageAction(int messageIndex, ref bool actionInvoked)
    {
        if (actionInvoked) return;
        if (stateMessageActions == null) return;

        if (stateMessageActions.TryGetValue((currentState, messageIndex), out var action))
        {
            action.Invoke();
            actionInvoked = true;
        }
    }





    //========================
    // 会話終了処理（State別）
    //========================
    void EndConversation()
    {
        // 共通終了処理
        uiWindow?.SetActive(false);
        character?.SetActive(false);

        // Stateごとの処理
        switch (currentState)
        {
            case TutorialState.Click:
                // UIWindow と キャラクターを非表示
                uiWindow?.SetActive(false); character?.SetActive(false);
                unMask.enabled = true; screen.enabled = true;
                pin.SetActive(true);
                GameObject parent = GameObject.Find("9_26_52");
                if (parent != null && pin != null)
                {
                    pin.transform.SetParent(parent.transform, false);
                    Debug.Log("pin を 9_26_52 の子オブジェクトに設定しました");
                }
                else { Debug.LogWarning("9_26_52 または pin が見つかりません"); }
                unmaskScale.StartByState(currentState);
                break;

            case TutorialState.DeliveryExplanation:
                ChangeState(TutorialState.DocumentCheck);
                unmaskScale.StartByState(currentState);
                break;

            case TutorialState.DocumentCheck:
                unMask.enabled = true;
                screen.enabled = true;
                break;

            case TutorialState.OpenDocument:
                unmaskScale.StartByState(currentState);
                unMask.enabled = true;
                screen.enabled = true;
                break;

            case TutorialState.OneMoreHouseClick:
                unmaskScale.StartByState(TutorialState.OneMoreHouseClick);
                unMask.enabled = true;
                screen.enabled = true;
                break;

            case TutorialState.TypeSelection:
                unmaskScale.StartByState(TutorialState.TypeSelection);
                unMask.enabled = true;
                screen.enabled = true;
                break;

            case TutorialState.PushButton:
                unmaskScale.StartByState(currentState);
                unMask.enabled = true;
                screen.enabled = true;
                break;
            case TutorialState.DrawLine:
                unmaskScale.StartByState(currentState);
                pin.SetActive(false);
                unMask.enabled = true;
                screen.enabled = true;
                break;

            case TutorialState.EndDrawLine:
                unMask.enabled = true;
                screen.enabled = true;
                unmaskScale.StartByState(TutorialState.EndDrawLine);
                break;
            case TutorialState.EndDelivery:
                ChangeState(TutorialState.ProgressGauge);
                break;
            case TutorialState.ProgressGauge:
                ChangeState(TutorialState.FailureExplanation);
                break;

            case TutorialState.FailureExplanation:
                ChangeState(TutorialState.None);
                SceneManager.LoadScene("TitleScene");
                
                break;

            case TutorialState.None:
            default:
                unMask.enabled = false;
                screen.enabled = false;
                tutorialcurrent.enabled = false;
                break;
        }
    }

}
