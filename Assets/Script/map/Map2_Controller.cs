using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map2_Controller : MonoBehaviour
{
    GameObject gc;
    void Start()
    {
        gc=GameObject.Find("GC");
        stone_clear();
        enemy_clear();
        map_insts();
    }
    public void stone_clear()
    {
        gc.GetComponent<gamecontrol>().stone_area.Clear();
        GameObject stoneparent = GameObject.Find("Stones");
        for (int i = 0; i < stoneparent.transform.childCount; i++)
        {
            if (stoneparent.transform.GetChild(i).name == "Nabiliums" || stoneparent.transform.GetChild(i).name == "Xerats")
                continue;
            Destroy(stoneparent.transform.GetChild(i).gameObject);
        }
    }
    public void enemy_clear()
    {
        gc.GetComponent<gamecontrol>().enemy_area.Clear();
        GameObject enemy_parent = GameObject.Find("Enemies");
        for (int i = 0; i < enemy_parent.transform.childCount; i++)
        {
            if (enemy_parent.transform.GetChild(i).name == "dilbians")
                continue;
            Destroy(enemy_parent.transform.GetChild(i).gameObject);
        }
    }
    private void map_insts()
    {
        gc.GetComponent<gamecontrol>().dilbian_instantiate();
        gc.GetComponent<gamecontrol>().boss_dilbian_instantiate();
        gc.GetComponent<gamecontrol>().nabilium_instantiate();
        gc.GetComponent<gamecontrol>().xerat_instantiate();
    }

}
