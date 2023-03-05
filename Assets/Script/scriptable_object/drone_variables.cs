using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Create_drone",fileName ="Drone-")]
public class drone_variables : ScriptableObject
{
    public Drone_type drone_Type;
    public int attack_percent;
    public int repair_cost;
    public float hp;
    public int cost;
    public bool cost_type;
}
