using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panel_button:MonoBehaviour
{
    public Vector2 button_1stpos;
    public GameObject child_panel;
    [SerializeField] int val;
    public bool changed = false;
    private void Awake()
    {
        button_1stpos = transform.parent.GetComponent<RectTransform>().anchoredPosition;
    }
    public void _click()
    {
        if(!changed)
        {
            child_panel.SetActive(true);
            transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(button_1stpos.x - val, button_1stpos.y);
            changed = true;
        }
        else
        {
            child_panel.SetActive(false);
            transform.parent.GetComponent <RectTransform>().anchoredPosition = button_1stpos;
            changed = false;
        }
    }
}
