using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warning_area : MonoBehaviour
{
    public bool is_player_area = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            is_player_area = true;
            GameObject.Find("GC").GetComponent<gamecontrol>().play_area_warning_start_coroutine();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            is_player_area = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            is_player_area = false;
            Invoke("func", 0.05f);
        }
    }
    void func()
    {
        if(!is_player_area)
        GameObject.Find("GC").GetComponent<gamecontrol>().play_area_warning_stop_coroutine();

    }
}
