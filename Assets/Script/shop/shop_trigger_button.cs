using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shop_trigger_button : MonoBehaviour
{
    [SerializeField] GameObject button_to_open;
    [SerializeField] GameObject button_to_close;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            button_to_open.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            button_to_open.SetActive(false);
            //button_to_close.SetActive(false);
        }
    }
}
