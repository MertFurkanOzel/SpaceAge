using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ship-", menuName = "CreateShip")]
public class spaceship_variables : ScriptableObject
{
    public bool player_has_ship;
    public string ship_name;
    public int Cost;
    public int Cost_type;
    public float base_attack_damage;
    public float base_hp;
    public float base_hp_regen;
    public float fire_rate;
    public float fire_range;
    public GameObject[] drones;
    public int laser_weapon_max_slot;
    public laser_weapon[] lasers;
    public int speed;
    public int max_stone;
    public int max_laser_green;
    public int max_laser_blue;
    public int max_laser_yellow;
    public int max_laser_orange;
    public int max_laser_aqua;

}