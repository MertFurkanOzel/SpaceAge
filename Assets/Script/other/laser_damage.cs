using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser_damage : MonoBehaviour
{
    public float damage;
    [SerializeField] float coeff;

    public void dmg(float value)
    {
        damage = value * coeff;
    }
}
