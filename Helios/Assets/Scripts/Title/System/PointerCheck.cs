using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// 参考サイト
/// https://bluebirdofoz.hatenablog.com/entry/2022/02/15/234402
/// </summary>
public class PointerCheck : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    /// <summary>
    /// タップ開始イベント
    /// </summary>
    public UnityEvent EventPointStart;

    /// <summary>
    /// タップ終了イベント
    /// </summary>
    public UnityEvent EventPointEnd;

    public void OnPointerDown(PointerEventData eventData)
    {
        EventPointStart.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        EventPointEnd.Invoke();
    }
}
