using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
[System.Serializable]
public class playerdata : MonoBehaviour
{
    //[CREDITS]---------------------------------
    private decimal paradium_credits = 0;
    private int rank_points = 0;
    private int space_credits = 0;

    private int[] stones_count = new int[3];
    private int[] lasers_count = new int[5];
    private int[] missiles_count = new int[4];

    private int[] inventory_drone = new int[12];
    private int[] inventory_laser_weapon = new int[24];

    private int active_mission_id = 0;
    private int active_map = 0;
    private int player_active_ship = 0;

    private float player_posx = 20;
    private float player_posy = -20;

    public void loaddata()
    {
        if (File.Exists(Application.persistentDataPath + "/player.data"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/player.data";
            FileStream fs = new FileStream(path, FileMode.Open);
            Savesystem ss = formatter.Deserialize(fs) as Savesystem;
            stones_count[0] = ss.garyum_amount;
            stones_count[1] = ss.nabilium_amount;
            stones_count[2] = ss.xerat_amount;
            rank_points = ss.rank_point;
            space_credits = ss.space_credits;
            paradium_credits = (decimal)ss.paradium_credits;
            active_mission_id = ss.active_mission_id;
            player_active_ship = ss.active_player_ship;
            player_posx = 20;
            player_posy = -20;
            active_map = ss.active_map;
            lasers_count[0] = ss.laser_green;
            lasers_count[1] = ss.laser_blue;
            lasers_count[2] = ss.laser_yellow;
            lasers_count[3] = ss.laser_orange;
            lasers_count[4] = ss.laser_aqua;
            inventory_drone = ss.inventory_drone;
            inventory_laser_weapon = ss.inventory_laser_weapon;
            missiles_count[0] = 10;
            missiles_count[1] = 10;
            missiles_count[2] = 10;
            missiles_count[3] = 10;
            fs.Close();
        }
    }
    public int Space_credits
    {
        get { return space_credits; }
        set 
        {
            if(value<space_credits&&GetComponent<MissionControl>().active_missions.id_mission == 2)
            {
                GetComponent<MissionControl>().make_progress(space_credits-value);
            }
            space_credits = value;
        }
    }
    public int Rank_points
    {
        get { return rank_points; }
        set
        {
            if (GetComponent<MissionControl>().active_missions.id_mission == 4)
                GetComponent<MissionControl>().make_progress(value - rank_points);
            rank_points = value;
        }
    }

    public int[] Inventory_drone
    {
        get { return inventory_drone; }
        set { inventory_drone = value; }
    }
    public int[] Inventory_laser_weapon
    {
        get { return inventory_laser_weapon; }
        set { inventory_laser_weapon = value; }
    }
    public int[] Lasers_count
    {
        get { return lasers_count; }
        set { lasers_count = value; }
    }
    public int[] Stones_count
    {
        get { return stones_count; }
        set { stones_count = value; }
    }
    public int[] Missiles_count
    {
        get { return missiles_count; }
        set { missiles_count = value; }
    }
    public int Active_map
    {
        get { return active_map; }
        set { active_map = value; }
    }
    public float Player_posy
    {
        get { return player_posy; }
        set { player_posy = value; }
    }
    public float Player_posx
    {
        get { return player_posx; }
        set { player_posx = value; }
    }
    public int Player_active_ship
    {
        get { return player_active_ship; }
        set { player_active_ship = value; }
    }
    public int Active_mission_id
    {
        get { return active_mission_id; }
        set { active_mission_id = value; }
    }
    public float Paradium_credits
    {
        get { return (float)paradium_credits; }
        set { paradium_credits = (decimal)value; }
    }

}
