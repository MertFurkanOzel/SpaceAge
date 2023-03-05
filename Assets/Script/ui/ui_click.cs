using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ui_click : MonoBehaviour, IPointerDownHandler
{
   [SerializeField] GameObject set_gameobject;
    private bool changed = false;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] private bool health;
    public void OnPointerDown(PointerEventData eventData)
    {
        changed = !changed;
        if (changed)
        {
            if (health)
                text.text = ((int)playerc.hp_current).ToString();//("0.##");
            else
                text.text = GameObject.Find("GC").GetComponent<gamecontrol>().stone_storage().ToString();
        }
        set_gameobject.SetActive(true);
        gameObject.SetActive(false);
    }
}
