using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile : MonoBehaviour
{
    public int missile_id;
    public float coeff;
    public float damage;
    private void Start()
    {
        float ship_base_attack = GameObject.FindWithTag("Player").GetComponent<playerc>().player_spaceship.base_attack_damage;
        damage = coeff * +Random.Range(-ship_base_attack/10,ship_base_attack/10)+ship_base_attack;
        //playerdata.Missiles_count[missile_id] -= 1;
    }
    
}
