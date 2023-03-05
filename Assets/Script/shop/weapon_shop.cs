using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class weapon_shop : MonoBehaviour
{
    enum page_tab
    {
        inventory,
        drones,
        laser_weapons,
    }
    enum inventory_slot_tab
    {
        drones_slot,
        laser_weapons_slot,
    }
    [SerializeField] laser_weapon[] lasers;
    [SerializeField] drone_variables[] drones;
    [SerializeField] GameObject[] drones_gameobject;
    [SerializeField] GameObject[] Tabs;
    [SerializeField] TextMeshProUGUI ship_name_text, ship_drone_slot_text, ship_laser_slot_text;
    [SerializeField] Image ship_image;
    [SerializeField] Image[] Top_button_image,slot_button_image;
    [SerializeField] GameObject slot,slot_parent,slot_parent_inventory;
    [SerializeField] Sprite[] drone_sprite, laser_weapon_sprite;
    [SerializeField] GameObject[] player_equipment_slot;
    [SerializeField] GameObject info_panel;
    [SerializeField] TextMeshProUGUI[] mevcut_laser, kullanilabilir_laser, mevcut_drone, kullanilabilir_drone;
    [SerializeField] TextMeshProUGUI info_Text;
    private page_tab current_page_Tab;
    private inventory_slot_tab current_slot_tab = inventory_slot_tab.drones_slot;
    private Color32 button_select_color = new (140, 231, 145, 255);
    private playerc player;
    private playerdata pd;
    private gamecontrol gc;
    private GameObject[] save_drone_gameobject;
    private laser_weapon[] save_laser_weapon;
    private int[] save_drone_inventory;
    private int[] save_laser_inventory;
    private void OnEnable()
    {
        player = GameObject.FindWithTag("Player").GetComponent<playerc>();
        pd = GameObject.Find("GC").GetComponent<playerdata>();
        gc= GameObject.Find("GC").GetComponent<gamecontrol>();
        info_panel.SetActive(false);
        save_drone_gameobject = new GameObject[player.player_spaceship.drones.Length];
        save_drone_inventory= new int[pd.Inventory_drone.Length];
        save_laser_weapon = new laser_weapon[player.player_spaceship.lasers.Length];
        save_laser_inventory = new int[pd.Inventory_laser_weapon.Length];
        select_tab(0);      
    }
    public void select_tab(int val)
    {
        select_button_unselect_others(Top_button_image, val);
        open_tab_close_others(val);
        current_page_Tab = (page_tab)val;
        switch (val)
        {
            case 0:
                ship_image.sprite = GameObject.FindWithTag("Player").GetComponent<SpriteRenderer>().sprite;
                ship_name_text.text = player.player_spaceship.ship_name;
                ship_laser_slot_text.text = player.player_spaceship.lasers.Length.ToString();
                ship_drone_slot_text.text = player.player_spaceship.drones.Length.ToString();
                select_slot_tab(0);
                break;
            case 1:
                tab_drone_on_enable();
                break;
            case 2:
                tab_laser_on_enable();
                break;
            default:
                break;
        }
    }
    public void select_slot_tab(int val)
    {
        //if ((int)(current_slot_tab) == val&&is_generated_slot)
        //    return;
        current_slot_tab = (inventory_slot_tab)val;
        select_button_unselect_others(slot_button_image, val);
        generate_ship_slot();
        generate_slot_player_equipment();
        switch (val)
        {
            case 0:
                break;
            case 1:
                break;
            default:
                break;
        }
    }
    private void open_tab_close_others(int val)
    {
        for (int i = 0; i < Tabs.Length; i++)
        {
            if (i != val)
                Tabs[i].SetActive(false);
            else
                Tabs[i].SetActive(true);
        }
    }
    private void select_button_unselect_others(Image[] imgs,int val)
    {
        int i = 0;
        foreach (var item in imgs)
        {
            if (i == val)
                item.color = button_select_color;
            else
                item.color = Color.white;
            i++;
        }
    }
    private void generate_ship_slot()
    {
        //if (is_generated_slot)
        //    return;
        if(slot_parent.transform.childCount>0)
        {
            for (int i = 0; i < slot_parent.transform.childCount; i++)
            {
                Destroy(slot_parent.transform.GetChild(i).gameObject);
            }
        }
        switch (current_slot_tab)
        {
            case inventory_slot_tab.drones_slot:
                foreach (GameObject item in player.player_spaceship.drones)
                {
                    GameObject slot = Instantiate(this.slot, slot_parent.transform);
                    if(item!=null)
                    {
                        int which = (int)(item.GetComponent<drone>().variables.drone_Type);
                        slot.transform.GetChild(0).GetComponent<Image>().sprite = drone_sprite[which];
                        //slot.transform.GetChild(0).GetComponent<Image>().preserveAspect = true;
                    }
                }
                break;
            case inventory_slot_tab.laser_weapons_slot:
                foreach (laser_weapon item in player.player_spaceship.lasers)
                {
                    GameObject slot = Instantiate(this.slot, slot_parent.transform);
                    if(item!=null)
                    {
                        int which = (int)(item.laser_Type);
                        slot.transform.GetChild(0).GetComponent<Image>().sprite = laser_weapon_sprite[which];
                    }
                }
                break;
            default:
                break;
        }
    }
    void generate_slot_player_equipment()
    {
        int i = 0;
        switch (current_slot_tab)
        {
            case inventory_slot_tab.drones_slot:
                foreach (var item in pd.Inventory_drone)
                {
                    if (item != 0)
                    {
                        player_equipment_slot[i].transform.GetChild(0).GetComponent<Image>().sprite = drone_sprite[item-1];
                    }
                    else
                        player_equipment_slot[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                    i++;
                }
                break;
            case inventory_slot_tab.laser_weapons_slot:
                foreach (var item in pd.Inventory_laser_weapon)
                {
                    if (item != 0)
                        player_equipment_slot[i].transform.GetChild(0).GetComponent<Image>().sprite = laser_weapon_sprite[item-1];
                    else
                        player_equipment_slot[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                    i++;
                }
                break;
            default:
                break;
        }
    }
    public void save()
    {
        switch (current_slot_tab)
        {
            case inventory_slot_tab.drones_slot:
                #region Ship_drone_slot
                for (int i = 0; i < slot_parent.transform.childCount; i++)
                {
                    if (slot_parent.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite == drone_sprite[0])
                    {
                        save_drone_gameobject[i] = drones_gameobject[0];
                    }
                    else if (slot_parent.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite == drone_sprite[1])
                    {
                        save_drone_gameobject[i] = drones_gameobject[1];
                    }
                    else if (slot_parent.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite == drone_sprite[2])
                    {
                        save_drone_gameobject[i] = drones_gameobject[2];
                    }
                    else
                    {
                        save_drone_gameobject[i] = null;
                    }
                }
                #endregion
                #region Player_drone_equipment
                for (int i = 0; i < pd.Inventory_drone.Length; i++)
                {
                    if(slot_parent_inventory.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite==drone_sprite[0])
                    {
                        save_drone_inventory[i] = 1;
                    }
                    else if (slot_parent_inventory.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite == drone_sprite[1])
                    {
                        save_drone_inventory[i] = 2;
                    }
                    else if (slot_parent_inventory.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite == drone_sprite[2])
                    {
                        save_drone_inventory[i] = 3;
                    }
                    else
                    {
                        save_drone_inventory[i] = 0;
                    }
                }
                #endregion
                //System.Array.Sort(save_laser_inventory);
                //System.Array.Reverse(save_drone_inventory);
                pd.Inventory_drone = save_drone_inventory;
                //System.Array.Sort(save_drone_gameobject);
                player.player_spaceship.drones = save_drone_gameobject;
                //player.drone_instantiate();
                break;
            case inventory_slot_tab.laser_weapons_slot:
                #region Ship_laser_weapon_slot
                for (int i = 0; i < slot_parent.transform.childCount; i++)
                {
                    if (slot_parent.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite == laser_weapon_sprite[0])
                    {
                        save_laser_weapon[i] = lasers[0];
                    }
                    else if (slot_parent.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite == laser_weapon_sprite[1])
                    {
                        save_laser_weapon[i] = lasers[1];
                    }
                    else if (slot_parent.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite == laser_weapon_sprite[2])
                    {
                        save_laser_weapon[i] = lasers[2];
                    }
                    else
                    {
                        save_laser_weapon[i] = null;
                    }
                }
                #endregion
                #region Player_laser_weapon_equipment
                for (int i = 0; i < pd.Inventory_laser_weapon.Length; i++)
                {
                    if (slot_parent_inventory.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite == laser_weapon_sprite[0])
                    {
                        save_laser_inventory[i] = 1;
                    }
                    else if (slot_parent_inventory.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite == laser_weapon_sprite[1])
                    {
                        save_laser_inventory[i] = 2;
                    }
                    else if (slot_parent_inventory.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite == laser_weapon_sprite[2])
                    {
                        save_laser_inventory[i] = 3;
                    }
                    else
                    {
                        save_laser_inventory[i] = 0;
                    }
                }
                #endregion
                //System.Array.Sort(save_laser_inventory);
                //System.Array.Reverse(save_laser_inventory);
                pd.Inventory_laser_weapon = save_laser_inventory;
                //System.Array.Sort(save_laser_weapon);
                player.player_spaceship.lasers = save_laser_weapon;
                break;
            default:
                break;
        }
        GameObject.Find("GC").GetComponent<gamecontrol>().veri_guncelle_ve_kaydet();
    }
    public void exit()
    {
        gameObject.SetActive(false);
    }
    public void click_square(GameObject square)
    {
        Transform trans_parent = (square.transform.parent.name) switch
        {
            "Panel" =>slot_parent.transform,
            _ =>slot_parent_inventory.transform,
        };
        for (int i = 0; i < trans_parent.childCount; i++)
        {
            if(trans_parent.GetChild(i).GetChild(0).GetComponent<Image>().sprite==null)
            {
                trans_parent.GetChild(i).GetChild(0).GetComponent<Image>().sprite = square.transform.GetChild(0).GetComponent<Image>().sprite;
                square.transform.GetChild(0).GetComponent<Image>().sprite = null;
                break;
            }
        }


    }

    public void drone_buy_click(int val)
    {
        drone drone = drones_gameobject[val].GetComponent<drone>();
        int cost = drone.variables.cost;
        bool is_afford = false;
        if(drone.variables.cost_type==true&&pd.Paradium_credits>=cost)
        {
            is_afford = true;
            pd.Paradium_credits -= cost;
        }
        else if(drone.variables.cost_type == false && pd.Space_credits >= cost)
        {
            is_afford = true;
            pd.Space_credits -= cost;
        }
        if(is_afford)
        {
            for (int i = 0; i < pd.Inventory_drone.Length; i++)
            {
                if (pd.Inventory_drone[i] == 0)
                {
                    pd.Inventory_drone[i] = val + 1;
                    GameObject.Find("GC").GetComponent<gamecontrol>().veri_guncelle_ve_kaydet();
                    break;
                }
            }
        }
        info_panel_on_enable(is_afford);
    }
    public void laser_weapon_buy_click(int val)
    {
        laser_weapon lw = lasers[val];
        int cost = lw.cost;
        bool is_afford = false;
        if(cost<=pd.Space_credits)
        {
            is_afford = true;
            pd.Space_credits -= cost;
        }
        if(is_afford)
        {
            for (int i = 0; i < pd.Inventory_laser_weapon.Length; i++)
            {
                if (pd.Inventory_laser_weapon[i] == 0)
                {
                    pd.Inventory_laser_weapon[i] = val + 1;
                    GameObject.Find("GC").GetComponent<gamecontrol>().veri_guncelle_ve_kaydet();
                    break;
                }
            }
        }
        info_panel_on_enable(is_afford);
    }
    private void tab_drone_on_enable()
    {
        for (int i = 0; i < drones.Length; i++)
        {
            mevcut_drone[i].text = (ships_used_count(i,true)+drone_inventory_count(i)).ToString();
            kullanilabilir_drone[i].text = drone_inventory_count(i).ToString();
        }
    }
    private void tab_laser_on_enable()
    {
        for (int i = 0; i < lasers.Length; i++)
        {
            mevcut_laser[i].text = (ships_used_count(i,false) + laser_weapon_inventory_count(i)).ToString();
            kullanilabilir_laser[i].text = laser_weapon_inventory_count(i).ToString();
        }
    }
    private int ships_used_count(int val,bool drone)
    {
        int x = 0;
        foreach (var item in gc.Playerships_variable)
        {
            if (item.player_has_ship ==true)
            {
                if(drone&&item.drones.Length>0)
                {
                    foreach (var drones in item.drones)
                    {
                        if(drones!=null)
                        {
                            if (drones.GetComponent<drone>().variables.drone_Type == (Drone_type)val)
                                x++;
                        }                      
                    }
                }
                else if(!drone&&item.lasers.Length>0)
                {
                    foreach (var laser in item.lasers)
                    {
                        if(laser!=null)
                        if (laser.laser_Type == (Laser_type)val)
                            x++;
                    }
                }
            }                
        }
        return x;
    }

    private int drone_inventory_count(int val)
    {
        int x = 0;
        foreach (var item in pd.Inventory_drone)
        {
            if (item==val+1)
                x++;
        }
        return x;
    }
    private int laser_weapon_inventory_count(int val)
    {
        int x = 0;
        foreach (var item in pd.Inventory_laser_weapon)
        {
            if (item ==val+1)
                x++;
        }
        return x;
    }
    private void info_panel_on_enable(bool status)
    {
        info_panel.SetActive(true);
        info_Text.text = (status) switch
        {
            true=>"Satýn alma baþarýyla gerçekleþti.",
            _ =>"Yeterli kaynaða sahip deðilsiniz."
        };
    }
}
