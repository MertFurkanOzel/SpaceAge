using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "enemy_ship-", menuName = "enemy_CreateShip")]
public class enemy_spaceship_variables : ScriptableObject
{
    public string enemy_name;
    public float base_attack_damage;
    public float base_hp;
    public float base_hp_regen;
    public float fire_rate;
    public float fire_range;
    public int speed;
    public int reward_space_credits;
    public int reward_rank_points;
    [Range(.5f, 250f)]
    public float reward_min_paradium_credits, reward_max_paradium_credits;
}