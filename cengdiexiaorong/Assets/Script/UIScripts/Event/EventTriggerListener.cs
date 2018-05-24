using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class EventTriggerListener : UnityEngine.EventSystems.EventTrigger
{
    public delegate void VoidDelegate(GameObject go);
    public delegate void EventDelegate(GameObject go, PointerEventData ev);
    public VoidDelegate onClick;
    public EventDelegate onDown;
    public EventDelegate onExit;
    public EventDelegate onUp;
    public VoidDelegate onSelect;
    public VoidDelegate onUpdateSelect;
    public EventDelegate onDragBegin;
    public EventDelegate onDrag;
    public EventDelegate onDragEnd;
    public EventDelegate onEnter;
    public EventDelegate onDrop;

    private float m_fLastClickTime = 0;
    public bool ClickLimit = true;

    static public EventTriggerListener Get(GameObject go)
    {
        EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
        if (listener == null) listener = go.AddComponent<EventTriggerListener>();
        return listener;
    }

    private bool IsValidTrigger()
    {

        return true;
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!IsValidTrigger())
        {
            return;
        }
        if (onClick != null)
        {
            onClick(gameObject);
        }
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!IsValidTrigger())
        {
            return;
        }
        if (onDown != null) onDown(gameObject, eventData);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsValidTrigger())
        {
            return;
        }
        if (onEnter != null) onEnter(gameObject, eventData);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (!IsValidTrigger())
        {
            return;
        }
        if (onExit != null) onExit(gameObject, eventData);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (!IsValidTrigger())
        {
            return;
        }
        if (onUp != null) onUp(gameObject, eventData);
    }
    public override void OnSelect(BaseEventData eventData)
    {
        if (!IsValidTrigger())
        {
            return;
        }
        if (onSelect != null) onSelect(gameObject);
    }
    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if (!IsValidTrigger())
        {
            return;
        }
        if (onUpdateSelect != null) onUpdateSelect(gameObject);
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsValidTrigger())
        {
            return;
        }
        if (onDragBegin != null) onDragBegin(gameObject, eventData);
    }
    public override void OnDrag(PointerEventData eventData)
    {
        if (!IsValidTrigger())
        {
            return;
        }
        if (onDrag != null) onDrag(gameObject, eventData);
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!IsValidTrigger())
        {
            return;
        }
        if (onDragEnd != null) onDragEnd(gameObject, eventData);
    }
    public override void OnDrop(PointerEventData eventData)
    {
        if (!IsValidTrigger())
        {
            return;
        }
        if (onDrop != null) onDrop(gameObject, eventData);
    }
}

