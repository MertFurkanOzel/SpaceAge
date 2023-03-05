using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class panel_drag : MonoBehaviour, IDragHandler
{
    RectTransform rt;
    [SerializeField] float dragspeed;
    Vector2 no_need;
    private void Start()
    {
        rt = transform.parent as RectTransform;
    }
    public void OnDrag(PointerEventData eventData)
    {
        rt.position =Vector2.SmoothDamp(rt.position, eventData.position + new Vector2(-100, 20),ref no_need,dragspeed);
    }
}
