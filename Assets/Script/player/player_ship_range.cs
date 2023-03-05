using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_ship_range : MonoBehaviour
{
    GameObject player;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        GetComponent<CircleCollider2D>().radius = player.GetComponent<playerc>().player_spaceship.fire_range;
    }

    private void Update()
    {
        if(player)
        transform.position = player.transform.position;
        else
            player = GameObject.FindWithTag("Player");
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "enemy" && !gamecontrol.attackable_enemy_ships.Contains(collision.gameObject))
            gamecontrol.attackable_enemy_ships.Add(collision.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "enemy" && gamecontrol.attackable_enemy_ships.Contains(collision.gameObject))
            gamecontrol.attackable_enemy_ships.Remove(collision.gameObject);
    }

}
