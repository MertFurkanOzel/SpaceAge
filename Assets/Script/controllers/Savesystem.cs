using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
[System.Serializable]
public class Savesystem
{
    public int space_credits;
    public int rank_point;
    public float paradium_credits;   
    //--------------------------
    public int garyum_amount;
    public int nabilium_amount;
    public int xerat_amount;
    //----------------------
    public int laser_green;
    public int laser_blue;
    public int laser_yellow;
    public int laser_orange;
    public int laser_aqua;
    public GameObject[] drones;
    public int active_mission_id;
    public int active_player_ship;
    public int active_map;
    public float player_posx;
    public float player_posy;
    public int[] missiles_count;
    public int[] inventory_drone = new int[12];
    public int[] inventory_laser_weapon = new int[24];
    public void savedata()
    {
        playerdata pd = GameObject.Find("GC").GetComponent<playerdata>();

        space_credits = pd.Space_credits;
        rank_point = pd.Rank_points;
        paradium_credits = pd.Paradium_credits;
        //-----------------------------------------------
        garyum_amount = pd.Stones_count[0];
        nabilium_amount = pd.Stones_count[1];
        xerat_amount = pd.Stones_count[2];
        //-----------------------------------------------
        laser_green = pd.Lasers_count[0];
        laser_blue = pd.Lasers_count[1];
        laser_yellow = pd.Lasers_count[2];
        laser_orange = pd.Lasers_count[3];
        laser_aqua = pd.Lasers_count[4];
        //-----------------------------------------------
        active_mission_id = pd.Active_mission_id;
        active_player_ship = pd.Player_active_ship;
        active_map = pd.Active_map;

        player_posx = pd.Player_posx;
        player_posy = pd.Player_posy;

        inventory_drone = pd.Inventory_drone;
        inventory_laser_weapon = pd.Inventory_laser_weapon;

        missiles_count = pd.Missiles_count;


        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.data";
        FileStream fs = new FileStream(path,FileMode.Create);
        formatter.Serialize(fs, this);
        fs.Close();
    }
  
}
