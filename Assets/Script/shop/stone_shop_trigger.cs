using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stone_shop_trigger : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject panel2;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            panel.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            panel.SetActive(false);
            panel2.SetActive(false);
        }
    }
}
