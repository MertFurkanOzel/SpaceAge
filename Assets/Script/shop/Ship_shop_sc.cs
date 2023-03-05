using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;
using UnityEngine.UI;
public class Ship_shop_sc : MonoBehaviour
{
    [SerializeField] spaceship_variables[] Playerships_variable;
    [SerializeField] TextMeshProUGUI basehp, attackpower, basehealtregen, greenlasercp, bluelasercp, yellowlasercp, orangelasercp, aqualasercp;
    [SerializeField] TextMeshProUGUI[] ship_name, ship_cost;
    [SerializeField] GameObject use_button, buy_button,purchase_panel;
    [SerializeField] Image purchase_panel_image;
    [SerializeField] TextMeshProUGUI selected_ship_cost_tm;
    [SerializeField] Sprite[] ship_sprites;
    private NumberFormatInfo nfi;
    private int selected_ship;
    gamecontrol gc;
    playerdata pd;
    private void Awake()
    {
        nfi = new NumberFormatInfo()
        {
            NumberDecimalDigits = 0,
            NumberGroupSeparator = "."
        };
        gc = GameObject.Find("GC").GetComponent<gamecontrol>();
        pd = gc.GetComponent<playerdata>();
    }
    private void OnEnable()
    {
        show_ship_name_and_cost();
        show_ship_stat(0);
    }
    private void show_ship_name_and_cost()
    {     
        for (int i = 0; i < 15; i++)
        {
            string c = (Playerships_variable[i].Cost_type) switch
            {
                0=>"C",
                _=>"P"
            };
            ship_name[i].text = Playerships_variable[i].ship_name;
            ship_cost[i].text = Playerships_variable[i].Cost.ToString("N",nfi) +" "+c;
        }
    }
    public void show_ship_stat(int val)
    {
        selected_ship = val;
        basehp.text = Playerships_variable[val].base_hp.ToString();
        attackpower.text = Playerships_variable[val].base_attack_damage.ToString();
        basehealtregen.text = Playerships_variable[val].base_hp_regen.ToString();
        greenlasercp.text = Playerships_variable[val].max_laser_green.ToString();
        bluelasercp.text = Playerships_variable[val].max_laser_blue.ToString();
        yellowlasercp.text = Playerships_variable[val].max_laser_yellow.ToString();
        orangelasercp.text = Playerships_variable[val].max_laser_orange.ToString();
        aqualasercp.text = Playerships_variable[val].max_laser_aqua.ToString();
        show_button();     
    }
    private void show_button()
    {
        if (Playerships_variable[selected_ship].player_has_ship)
        {
            use_button.SetActive(true);
            buy_button.SetActive(false);
        }
        else
        {
            buy_button.SetActive(true);
            use_button.SetActive(false);

        }
    }
    public void purchase_click()
    {
        if (Playerships_variable[selected_ship].Cost_type == 0 && Playerships_variable[selected_ship].Cost <= pd.Space_credits)
        {
            pd.Space_credits -= Playerships_variable[selected_ship].Cost;          
            gc.create_log(string.Format("{0} gemisi {1} Paradium kredisiyle satýn alýndý.", Playerships_variable[selected_ship].ship_name, Playerships_variable[selected_ship].Cost));
        }
        else if (Playerships_variable[selected_ship].Cost_type == 1 && Playerships_variable[selected_ship].Cost <= pd.Paradium_credits)
        {
            pd.Paradium_credits -= Playerships_variable[selected_ship].Cost;
            gc.create_log(string.Format("{0} gemisi {1} Space kredisiyle satýn alýndý.", Playerships_variable[selected_ship].ship_name, Playerships_variable[selected_ship].Cost));
        }
        else
        {
            Debug.Log("Yeterli kaynaðýnýz yok");
            return;
        }
        Playerships_variable[selected_ship].player_has_ship = true;
        purchase_panel.SetActive(false);
        show_button();
        gc.veri_guncelle_ve_kaydet();
    }
    public void buy_click()
    {
        purchase_panel.SetActive(true);
        purchase_panel_image.sprite = ship_sprites[selected_ship];
        selected_ship_cost_tm.text = Playerships_variable[selected_ship].Cost.ToString("N",nfi);
    }
    public void purchase_panel_exit()
    {
        purchase_panel.SetActive(false);
    }
    public void use_click()
    {
        pd.Player_active_ship = selected_ship;
        Debug.LogError(pd.Player_active_ship);
        gc.veri_guncelle_ve_kaydet();
        gc.change_ship();
    }
}
