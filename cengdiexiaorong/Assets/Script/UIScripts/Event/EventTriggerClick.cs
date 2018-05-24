using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

//只监听鼠标点击事件~EventTriggerListener会使滚动列表的Drag事件被拦截而导致无法滚动
public class EventTriggerClick : MonoBehaviour, IPointerClickHandler ,IPointerDownHandler
{
    public System.Action<GameObject> onClick;
    public EventDelegate onDown;

    static public EventTriggerClick Get(GameObject go)
    {
        if (go == null)
        {
            Develop.LogWarning("EventTriggerClick.Get, GameObject is null!!");
            return null;
        }

        EventTriggerClick listener = go.GetComponent<EventTriggerClick>();
        if (listener == null) listener = go.AddComponent<EventTriggerClick>();
        return listener;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
		if (onClick != null)
		{
			onClick(gameObject);
		}
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        if (onDown != null) onDown(gameObject, eventData);
    }
}
