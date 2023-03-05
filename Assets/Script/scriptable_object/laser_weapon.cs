using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "laserweapon-", menuName = "CreateLaserWeapon")]
public class laser_weapon : ScriptableObject
{
    public Laser_type laser_Type;
    public int attack_damage;
    public float fire_rate_coeff;
    public int cost;
}
