using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class slot_click : MonoBehaviour,IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject.Find("Weapon_shop_panel").GetComponent<weapon_shop>().click_square(gameObject);
    }
}
