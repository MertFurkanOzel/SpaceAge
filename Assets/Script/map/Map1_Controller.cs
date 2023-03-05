using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map1_Controller : MonoBehaviour
{  
    void Start()
    {
        GameObject gc = GameObject.Find("GC");
        gc.GetComponent<gamecontrol>().garyum_instantiate();
        gc.GetComponent<gamecontrol>().jackpot_1_instantiate();
        gc.GetComponent<gamecontrol>().celarid_instantiate();
    }
}
