using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class portal : MonoBehaviour
{
    public int required_rank;
    public int tp_map;
    private Vector2 outpos = new Vector2(15, -15);
    [SerializeField] GameObject button;

    public void teleport()
    {
        GameObject gc = GameObject.Find("GC");
        gc.GetComponent<gamecontrol>().map_on_changed();
        SceneManager.LoadScene(tp_map);
        GameObject.FindWithTag("Player").transform.position = outpos;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (GameObject.Find("GC").GetComponent<playerdata>().Rank_points >= required_rank)
                button.SetActive(true);
            else
                GameObject.Find("GC").GetComponent<gamecontrol>().create_log("Rank Point Bu Portal Ýçin yetersiz [" + required_rank + "]");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            button.SetActive(false);
    }
}
