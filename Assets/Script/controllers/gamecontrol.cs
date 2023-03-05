using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using UnityEngine.SceneManagement;
public class gamecontrol : MonoBehaviour
{
    #region Definition
    //-----------------GENERAL-----------------
    [SerializeField] public spaceship_variables[] Playerships_variable;
    [SerializeField] Camera main_camera;
    [SerializeField] Camera play_area_warning_camera;
    [SerializeField] float warningcam;
    public stone[] _stones;
    private NumberFormatInfo nfi;
    //public List<Vector2> stone_area = new List<Vector2>();
    //public List<Vector2> enemy_area = new List<Vector2>();
    public HashSet<Vector2> enemy_area = new HashSet<Vector2>();
    public HashSet<Vector2> stone_area = new HashSet<Vector2>();
    public static List<GameObject> attackable_enemy_ships = new List<GameObject>();
    public List<string> logs = new();
    Savesystem svs;
    //-----------------GAMEOBJECT-----------------
    [SerializeField] GameObject[] Playerships;
    [SerializeField] GameObject[] enemies;
    [SerializeField] GameObject[] enemies_parent;
    [SerializeField] GameObject[] lasers;
    [SerializeField] GameObject[] drones;
    [SerializeField] GameObject minimap_panel;
    [SerializeField] GameObject stones_shop;
    [SerializeField] GameObject weapon_shop;
    [SerializeField] GameObject garyum;
    [SerializeField] GameObject garyums_parent;
    [SerializeField] GameObject jackpot;
    [SerializeField] GameObject jackpots_parent;
    [SerializeField] GameObject nabilium;
    [SerializeField] GameObject nabilium_parent;
    [SerializeField] GameObject xerat;
    [SerializeField] GameObject xerat_parent;
    [SerializeField] GameObject under_panel;
    [SerializeField] GameObject credits_panel;
    [SerializeField] GameObject play_area_arrow;
    [SerializeField] GameObject log_text_gameobject, log_content;
    [SerializeField] GameObject laser_shop_info_panel;
    [SerializeField] GameObject missiles_panel;
    [SerializeField] GameObject missiles_up_button;
    [SerializeField] GameObject missiles_down_button;
    [SerializeField] public GameObject[] missiles;
    public static GameObject selected_laser;
    public static GameObject selected_missile;
    public static GameObject selected_target;
    public static GameObject Player;
    //-----------------UI-----------------
    [SerializeField] TextMeshProUGUI[] kazanilan_item_tmpro;
    [SerializeField] TextMeshProUGUI[] enemy_jackpot_rewards_tmpro;
    [SerializeField] TextMeshProUGUI uzay_kredisi_tmpro;
    [SerializeField] TextMeshProUGUI kidem_puani_tmpro;
    [SerializeField] TextMeshProUGUI paradium_kredisi_tmpro;
    [SerializeField] TextMeshProUGUI laser_shop_cost_text;
    [SerializeField] TextMeshProUGUI play_area_warning_text;
    [SerializeField] TextMeshProUGUI t1, t2, t3;
    [SerializeField] TextMeshProUGUI Laser_shop_info_text;
    [SerializeField] TMP_InputField laser_shop_if;
    [SerializeField] Sprite selected_buton, normal_buton, selected_green;
    [SerializeField] Sprite[] lasers_image;
    [SerializeField] Sprite[] backgrounds_back;
    [SerializeField] Sprite[] backgrounds_right;
    [SerializeField] Button[] button_under_panel;
    [SerializeField] Button[] button_under_panel_missiles;
    [SerializeField] Button map_button;
    [SerializeField] Image selected_laser_image;
    [SerializeField] Image[] laser_bar_filledimages;
    [SerializeField] Image stone_storage_bar_filled_image;
    [SerializeField] SpriteRenderer background_back;
    [SerializeField] SpriteRenderer background_right;
    public TextMeshProUGUI ship_panel_hp_text;
    public Image ship_panel_hp_filled_bar;
    Button prev;
    Button prev_missiles;
    //--------------------------------------------------------
    [SerializeField] int log_capacity;
    public static int mapteki_dilbian_sayisi = 0;
    public static int mapteki_e1_sayisi = 0;
    public static int mapteki_bossdilbian_sayisi = 0;
    public static int selected_laser_int = 0;
    public static int selected_missiles_int = 0;
    private static int mapteki_garyum_sayisi = 0;
    private static int mapteki_jackpot_sayisi;
    private static int mapteki_nabilium_sayisi;
    private static int mapteki_xerat_sayisi;
    private int selected_laser_laser_shop;
    public float playership_max_ammo_count;
    private int log_int = 0;
    private int laser_shop_cost;
    private int laser_shop_amount;
    private string laser_shop_laser_name = "LCB -10";
    private playerdata pd;
    public static bool enemy_jackpot_is_pickable = true;
    private bool warningcr;
    private Coroutine warning;
    #endregion
    private void Awake()
    {
        GetComponent<playerdata>().loaddata();
        pd = GetComponent<playerdata>();
        Player = Instantiate(Playerships[pd.Player_active_ship], new Vector2(pd.Player_posx, pd.Player_posy), Quaternion.identity);
        svs = new Savesystem();
        veri_guncelle_ve_kaydet();
        selected_laser = lasers[0];
        prev = button_under_panel[0];
        prev_missiles = button_under_panel_missiles[0];

        nfi = new NumberFormatInfo()
        {
            NumberDecimalDigits = 0,
            NumberGroupSeparator = "."
        };
        reset_static_variables();
    }
    private void Start()
    {
        stone_storage_filled_image();
        button_under_panel_missiles[0].GetComponent<Image>().sprite = selected_buton;
        button_under_panel[0].GetComponent<Image>().sprite = selected_buton;
        for (int i = 0; i < laser_bar_filledimages.Length; i++)
        {
            laser_filled_image_update(i);
        }
    }

    public void create_log(string message)
    {
        log_int++;
        if (logs.Count >= log_capacity)
        {
            logs.RemoveAt(0);
            Destroy(log_content.transform.GetChild(0).gameObject);
        }
        GameObject text = Instantiate(log_text_gameobject, log_content.transform);
        text.GetComponent<TextMeshProUGUI>().text = message;
        text.GetComponent<TextMeshProUGUI>().color = (log_int % 2) switch
        {
            0 => new Color32(70, 70, 70, 255),
            _ => new Color32(0, 0, 0, 255),
        };

        logs.Add(message);
    }
    public static Quaternion lookat(Transform obj, Transform target)
    {
        Vector2 direction = target.position - obj.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        return Quaternion.Euler(Vector3.forward * angle);
    }
    private void reset_static_variables()
    {
        mapteki_dilbian_sayisi = 0;
        mapteki_e1_sayisi = 0;
        mapteki_bossdilbian_sayisi = 0;
        selected_laser_int = 0;
        mapteki_garyum_sayisi = 0;
        mapteki_jackpot_sayisi = 0;
        mapteki_nabilium_sayisi = 0;
        mapteki_xerat_sayisi = 0;
    }
    public void laser_info_panel_update()
    {
        float laser_count = ammo_count(selected_laser_int);
        if (selected_laser_int == 0)
        {
            t2.text = "%100";
            t3.text = "Infinity";
            selected_laser_image.sprite = lasers_image[0];
            return;
        }
        selected_laser_image.sprite = lasers_image[selected_laser_int];
        if (playership_max_ammo_count == 0)
        { t2.text = "%0"; }
        else
        { t2.text = "%" + (100 * laser_count / playership_max_ammo_count).ToString("0.#"); }

        t3.text = laser_count.ToString() + " / " + playership_max_ammo_count;
    }
    public void change_ship()
    {
        //Player= Instantiate(Playerships[playerdata.Player_active_ship], new Vector2(playerdata.Player_posx, playerdata.Player_posy), Quaternion.identity);
        //Player.transform.parent = GameObject.Find("Dontdestroyy").transform;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void missile_change_status()
    {
        Player.GetComponent<playerc>().missile_bool = !Player.GetComponent<playerc>().missile_bool;
    }
    #region coroutine_method
    public void garyum_instantiate()
    {
        StartCoroutine(stone_instantiate(0, garyum, GameObject.FindWithTag("Garyums_Parent"), "garyum", 60));
    }
    public void jackpot_1_instantiate()
    {
        StartCoroutine(stone_instantiate(100, jackpot, GameObject.FindWithTag("Jackpots_Parent"), "jackpot", 15));
    }
    public void celarid_instantiate()
    {
        StartCoroutine(enemy_spawn(0, "Celarid", 30));
    }
    public void dilbian_instantiate()
    {
        StartCoroutine(enemy_spawn(1, "Dilbian", 30));
    }
    public void boss_dilbian_instantiate()
    {
        StartCoroutine(enemy_spawn(2, "Boss-Dilbian", 1));
    }
    public void nabilium_instantiate()
    {
        StartCoroutine(stone_instantiate(1, nabilium, GameObject.FindWithTag("Nabiliums_Parent"), "nabilium", 60));
    }
    public void xerat_instantiate()
    {
        StartCoroutine(stone_instantiate(2, xerat, GameObject.FindWithTag("Xerats_Parent"), "xerat", 30));
    }
    #endregion

    #region ---------------UI----------------
    public void minimapackapa()
    {
        Vector3 vec = map_button.GetComponent<RectTransform>().anchoredPosition;
        minimap_panel.SetActive(!minimap_panel.activeInHierarchy);
        if (vec.y == -1000)
            vec.y = -700;
        else
            vec.y = -1000;
        map_button.GetComponent<RectTransform>().anchoredPosition = vec;
    }

    public void laser_filled_image_update(int laser_to_update_int)
    {
        laser_bar_filledimages[laser_to_update_int].fillAmount = (float)ammo_count(laser_to_update_int) / playership_max_ammo_count;
    }
    public void kazanilan_item_text_goster(float adet, string itemadi, TextMeshProUGUI[] tmp, float duration)
    {
        TextMeshProUGUI tmpro = null;
        foreach (var item in tmp)
        {
            if (item.text == "")
                tmpro = item;
        }
        tmpro.text = "[" + adet + "]   " + itemadi;
        veri_guncelle_ve_kaydet();
        StartCoroutine(kazan(tmpro, duration));
        //Invoke("kazanilan_item_text_goster_kaybet", 1.5f);
    }
    IEnumerator kazan(TextMeshProUGUI tmpro, float duration)
    {
        enemy_jackpot_is_pickable = false;
        yield return new WaitForSeconds(duration);
        tmpro.text = "";
        enemy_jackpot_is_pickable = true;

    }
    public void stone_storage_filled_image()
    {
        stone_storage_bar_filled_image.fillAmount = (float)stone_storage() / Player.GetComponent<playerc>().player_spaceship.max_stone;
    }
    public int stone_storage()
    {
        int storage = 0;
        foreach (var item in pd.Stones_count)
        {
            storage += item;
        }
        return storage;
    }
    public void stones_shopac()
    {
        stones_shop.SetActive(true);
        RectTransform rt = credits_panel.GetComponent<RectTransform>();
        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, rt.rect.width);
        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, rt.rect.height);
        under_panel.SetActive(false);
        minimapackapa();
    }

    public void Shop_open(GameObject panel)
    {
        panel.SetActive(true);
    }
    public void Shop_close(GameObject panel)
    {
        panel.SetActive(false);
    }
    public void Laser_shop_inputfield_changed()
    {
        if (laser_shop_if.text == "")
            return;
        int cost;
        int amount = int.Parse(laser_shop_if.text);
        switch (selected_laser_laser_shop)
        {
            case 1:
                if (amount > Player.GetComponent<playerc>().player_spaceship.max_laser_green - pd.Lasers_count[0])
                {
                    amount = Player.GetComponent<playerc>().player_spaceship.max_laser_green - pd.Lasers_count[0];
                    laser_shop_if.text = amount.ToString();
                }
                cost = int.Parse(laser_shop_if.text);
                laser_shop_cost = cost;
                laser_shop_cost_text.text = cost + " Credit";
                laser_shop_amount = amount;
                break;
            case 2:
                if (amount > Player.GetComponent<playerc>().player_spaceship.max_laser_blue - pd.Lasers_count[1])
                {
                    amount = Player.GetComponent<playerc>().player_spaceship.max_laser_blue - pd.Lasers_count[1];
                    laser_shop_if.text = amount.ToString();
                }
                cost = int.Parse(laser_shop_if.text) * 2;
                laser_shop_cost = cost;
                laser_shop_cost_text.text = cost + " Credit";
                laser_shop_amount = amount;
                break;
            case 3:
                if (amount > Player.GetComponent<playerc>().player_spaceship.max_laser_yellow - pd.Lasers_count[2])
                {
                    amount = Player.GetComponent<playerc>().player_spaceship.max_laser_yellow - pd.Lasers_count[2];
                    laser_shop_if.text = amount.ToString();
                }
                cost = int.Parse(laser_shop_if.text) * 3;
                laser_shop_cost = cost;
                laser_shop_cost_text.text = cost + " Credit";
                laser_shop_amount = amount;
                break;
            case 4:
                if (amount > Player.GetComponent<playerc>().player_spaceship.max_laser_orange - pd.Lasers_count[3])
                {
                    amount = Player.GetComponent<playerc>().player_spaceship.max_laser_orange - pd.Lasers_count[3];
                    laser_shop_if.text = amount.ToString();
                }
                cost = int.Parse(laser_shop_if.text);
                laser_shop_cost = cost;
                laser_shop_cost_text.text = cost + " Paradium";
                laser_shop_amount = amount;
                break;
            case 5:
                if (amount > Player.GetComponent<playerc>().player_spaceship.max_laser_aqua - pd.Lasers_count[4])
                {
                    amount = Player.GetComponent<playerc>().player_spaceship.max_laser_aqua - pd.Lasers_count[4];
                    laser_shop_if.text = amount.ToString();
                }
                cost = int.Parse(laser_shop_if.text) * 2;
                laser_shop_cost = cost;
                laser_shop_cost_text.text = cost + " Paradium";
                laser_shop_amount = amount;
                break;
            default:
                break;
        }

    }
    public void laser_shop_buy()
    {
        laser_shop_info_panel.SetActive(true);
        if ((selected_laser_laser_shop == 4 || selected_laser_laser_shop == 5) && laser_shop_cost <= pd.Paradium_credits)
        {
            pd.Paradium_credits -= laser_shop_cost;
            ammo(1, int.Parse(laser_shop_if.text), selected_laser_laser_shop);
            veri_guncelle_ve_kaydet();
            for (int i = 1; i < 6; i++)
                laser_filled_image_update(i);
            laser_info_panel_update();
            Laser_shop_info_text.text = string.Format("{0} Adet {1} türü cephane satýn alýndý.", laser_shop_amount, laser_shop_laser_name);
        }
        else if ((laser_shop_cost <= pd.Space_credits) && (selected_laser_laser_shop != 4 && selected_laser_laser_shop != 5))
        {
            pd.Space_credits -= laser_shop_cost;
            ammo(1, int.Parse(laser_shop_if.text), selected_laser_laser_shop);
            veri_guncelle_ve_kaydet();
            for (int i = 1; i < 6; i++)
                laser_filled_image_update(i);
            laser_info_panel_update();
            Laser_shop_info_text.text = string.Format("{0} Adet {1} türü cephane satýn alýndý.", laser_shop_amount, laser_shop_laser_name);
        }
        else
        {
            Laser_shop_info_text.text = "Satýn alma baþarýsýz oldu";
        }

    }
    public void Laser_shop_select_laser(int Laser)
    {
        selected_laser_laser_shop = Laser;
        Laser_shop_inputfield_changed();
    }
    public void stones_shopkapa()
    {
        stones_shop.SetActive(false);
        RectTransform rt = credits_panel.GetComponent<RectTransform>();
        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, rt.rect.width);
        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, rt.rect.height);
        under_panel.SetActive(true);
        minimapackapa();
    }
    #endregion
    #region --------------ENEMY--------------
    public int enemy_count(int enemy_id)
    {
        switch (enemy_id)
        {
            case 0: return mapteki_e1_sayisi;
            case 1: return mapteki_dilbian_sayisi;
            case 2: return mapteki_bossdilbian_sayisi;
            default:
                return 0;
        }
    }
    private Transform enemy_parent(int val)
    {
        switch (val)
        {
            case 0:
                return GameObject.FindWithTag("Celarids_Parent").transform;
            case 1:
                return GameObject.FindWithTag("Dilbians_Parent").transform;
            case 2:
                return GameObject.FindWithTag("Dilbians_Parent").transform;
            default:
                return null;
        }
    }
    private IEnumerator enemy_spawn(int enemy_id, string name, int how_many)
    {
        int play_area_x = enemies[enemy_id].GetComponent<enemyc>().play_area_x;
        int play_area_y = enemies[enemy_id].GetComponent<enemyc>().play_area_y;

        bool enemy_is_spawned = false;
        while (true)
        {
            int Randomx = Random.Range(15, 150);
            int Randomy = Random.Range(-65, -15);
            Vector2 randompos = new Vector2(Randomx, Randomy);
            if (!(((Randomx > 0 && Randomx < 42) && (Randomy > -42 && Randomy < 0)) || enemy_area.Contains(randompos)))
            {
                if (enemy_count(enemy_id) < how_many)
                {
                    enemy_area.Add(randompos);
                    //WÝLL BE FÝXED
                    for (int i = Randomx - play_area_x; i < Randomx + play_area_x; i++)
                    {
                        for (int j = Randomy - play_area_y; j < Randomy + play_area_y; j++)
                        {
                            enemy_area.Add(new Vector2(i, j));
                        }
                    }
                    //Debug.LogError(randompos);
                    GameObject temp = Instantiate(enemies[enemy_id], randompos, Quaternion.identity);
                    temp.transform.SetParent(enemy_parent(enemy_id));
                    temp.transform.name = name;
                    switch (enemy_id)
                    {
                        case 0:
                            mapteki_e1_sayisi++;
                            break;
                        case 1:
                            mapteki_dilbian_sayisi++;
                            break;
                        case 2:
                            mapteki_bossdilbian_sayisi++;
                            break;
                        default:
                            break;
                    }
                }
                else
                    enemy_is_spawned = true;
                if (enemy_is_spawned)
                    yield return new WaitForSeconds(5);
                else
                    yield return 0;
            }
        }
    }
    public void enemy_jackpot_pickup(GameObject jackpot)
    {
        float val;
        if (jackpot.GetComponent<enemy_jackpot>().space_credits != 0)
        {
            val = jackpot.GetComponent<enemy_jackpot>().space_credits;
            pd.Space_credits += (int)val;
            kazanilan_item_text_goster(val, "Space Credit", enemy_jackpot_rewards_tmpro, 2.3f);
            create_log(jackpot.GetComponent<enemy_jackpot>().owner + " rakibinden " + val.ToString() + " Space Credit");

        }
        if (jackpot.GetComponent<enemy_jackpot>().rank_points != 0)
        {
            val = jackpot.GetComponent<enemy_jackpot>().rank_points;
            pd.Rank_points += (int)val;
            kazanilan_item_text_goster(val, "Rank Point", enemy_jackpot_rewards_tmpro, 2.3f);
            create_log(jackpot.GetComponent<enemy_jackpot>().owner + " rakibinden " + val.ToString() + " Rank Point");
        }
        if (jackpot.GetComponent<enemy_jackpot>().paradium != 0)
        {
            val = jackpot.GetComponent<enemy_jackpot>().paradium;
            pd.Paradium_credits += val;
            kazanilan_item_text_goster(val, "Paradium", enemy_jackpot_rewards_tmpro, 2.3f);
            create_log(jackpot.GetComponent<enemy_jackpot>().owner + " rakibinden " + val.ToString() + " Paradium");
        }
        if (jackpot.GetComponent<enemy_jackpot>().garyum != 0)
        {
            val = jackpot.GetComponent<enemy_jackpot>().garyum;
            pd.Stones_count[0] += (int)val;
            kazanilan_item_text_goster(val, "Garyum", enemy_jackpot_rewards_tmpro, 2.3f);
            stone_storage_filled_image();
            create_log(jackpot.GetComponent<enemy_jackpot>().owner + " rakibinden " + val.ToString() + " Garyum");
        }
        if (jackpot.GetComponent<enemy_jackpot>().xerat != 0)
        {
            val = jackpot.GetComponent<enemy_jackpot>().xerat;
            pd.Stones_count[1] += (int)val;
            kazanilan_item_text_goster(val, "Xerat", enemy_jackpot_rewards_tmpro, 2.3f);
            stone_storage_filled_image();
            create_log(jackpot.GetComponent<enemy_jackpot>().owner + " rakibinden " + val.ToString() + " Xerat");
        }
        if (jackpot.GetComponent<enemy_jackpot>().nabilium != 0)
        {
            val = jackpot.GetComponent<enemy_jackpot>().nabilium;
            pd.Stones_count[2] += (int)val;
            kazanilan_item_text_goster(val, "Nabilium", enemy_jackpot_rewards_tmpro, 2.3f);
            stone_storage_filled_image();
            create_log(jackpot.GetComponent<enemy_jackpot>().owner + " rakibinden " + val.ToString() + " Nabilium");
        }
        //playerdata.Space_credits += jackpot.GetComponent<enemy_jackpot>().space_credits;
        //playerdata.Rank_point += jackpot.GetComponent<enemy_jackpot>().rank_points;
        //playerdata.Paradium_credits +=jackpot.GetComponent<enemy_jackpot>().paradium;
        //playerdata.Garyum_amount += jackpot.GetComponent<enemy_jackpot>().garyum;
        //playerdata.Xerat_amount += jackpot.GetComponent<enemy_jackpot>().xerat;
        //playerdata.Nabilium_amount += jackpot.GetComponent<enemy_jackpot>().nabilium;
        veri_guncelle_ve_kaydet();
        Destroy(jackpot);

    }
    #endregion



    public virtual void stone_pickup(GameObject stone_go)
    {
        switch (stone_go.name)
        {
            case "garyum":
                if (stone_storage() + _stones[0].hacim > Player.GetComponent<playerc>().player_spaceship.max_stone)
                    return;
                if (GetComponent<MissionControl>().active_missions.id_mission == 0)
                    GetComponent<MissionControl>().make_progress(1);
                pd.Stones_count[0]++;
                pd.Rank_points += _stones[0].stone_kidem_value;
                stone_area.Remove(stone_go.transform.position);
                stone_storage_filled_image();
                Destroy(stone_go);
                mapteki_garyum_sayisi--;
                kazanilan_item_text_goster(1, "Garyum", kazanilan_item_tmpro, 1.5f);
                create_log("1 Adet Garyum Topladýn");
                veri_guncelle_ve_kaydet();
                break;
            case "nabilium":
                if (stone_storage() + _stones[1].hacim > Player.GetComponent<playerc>().player_spaceship.max_stone)
                    return;
                if (GetComponent<MissionControl>().active_missions.id_mission == 3)
                    GetComponent<MissionControl>().make_progress(1);
                pd.Stones_count[1]++;
                pd.Rank_points += _stones[1].stone_kidem_value;
                stone_area.Remove(stone_go.transform.position);
                stone_storage_filled_image();
                Destroy(stone_go);
                mapteki_nabilium_sayisi--;
                kazanilan_item_text_goster(1, "Nabilium", kazanilan_item_tmpro, 1.5f);
                create_log("1 Adet Nabilium Topladýn");
                veri_guncelle_ve_kaydet();

                break;
            case "xerat":
                if (stone_storage() + _stones[2].hacim > Player.GetComponent<playerc>().player_spaceship.max_stone)
                    return;
                pd.Stones_count[2]++;
                pd.Rank_points += _stones[2].stone_kidem_value;
                stone_area.Remove(stone_go.transform.position);
                stone_storage_filled_image();
                Destroy(stone_go);
                mapteki_xerat_sayisi--;
                kazanilan_item_text_goster(1, "Xerat", kazanilan_item_tmpro, 1.5f);
                create_log("1 Adet Xerat Topladýn");
                veri_guncelle_ve_kaydet();

                break;
            default:
                break;
        }

    }
    public void laser_select(int val)
    {
        prev.GetComponent<Image>().sprite = normal_buton;
        selected_laser = lasers[val];
        selected_laser_int = val;
        button_under_panel[val].GetComponent<Image>().sprite = selected_buton;
        prev = button_under_panel[val];
        laser_info_panel_update();
    }
    public void missiles_select(int val)
    {
        prev_missiles.GetComponent<Image>().sprite = normal_buton;
        selected_missile = missiles[val];
        selected_missiles_int = val;
        button_under_panel_missiles[val].GetComponent<Image>().sprite = selected_buton;
        prev_missiles = button_under_panel_missiles[val];
        laser_info_panel_update();
    }
    public void missiles_panel_open()
    {
        missiles_up_button.SetActive(false);
        missiles_panel.SetActive(true);
        missiles_down_button.SetActive(true);
    }
    public void missiles_panel_close()
    {
        missiles_down_button.SetActive(false);
        missiles_panel.SetActive(false);
        missiles_up_button.SetActive(true);
    }
    public void veri_guncelle_ve_kaydet()
    {
        uzay_kredisi_tmpro.text = pd.Space_credits.ToString("N0");
        kidem_puani_tmpro.text = pd.Rank_points.ToString("N0");
        paradium_kredisi_tmpro.text = pd.Paradium_credits.ToString();
        svs.savedata();
    }
    private IEnumerator stone_instantiate(int stone_id, GameObject inst_stone, GameObject stone_parent, string stone_name, int how_many)
    {
        bool is_stone_full = false;
        while (true)
        {
            int Randomx = Random.Range(15, 145);
            int Randomy = Random.Range(-62, -15);
            Vector2 randompos = new(Randomx, Randomy);
            if (!(((Randomx > 0 && Randomx < 42) && (Randomy > -42 && Randomy < 0)) || (stone_area.Contains(randompos))))
            {

                if (stone_count(stone_id) < how_many)
                {
                    stone_area.Add(new Vector2(Randomx, Randomy));
                    GameObject temp = Instantiate(inst_stone, randompos, Quaternion.identity);
                    temp.transform.SetParent(stone_parent.transform);
                    temp.transform.name = stone_name;
                    stone_increase(stone_id);
                }
                else
                    is_stone_full = true;
                if (is_stone_full)
                    yield return new WaitForSeconds(1);
                else
                    yield return 0;
            }
        }
    }
    private void stone_increase(int val)
    {
        switch (val)
        {
            case 0:
                mapteki_garyum_sayisi++;
                break;
            case 1:
                mapteki_nabilium_sayisi++;
                break;
            case 2:
                mapteki_xerat_sayisi++;
                break;
            case 100:
                mapteki_jackpot_sayisi++;
                break;
            default:
                break;
        }
    }
    private int stone_count(int val)
    {
        switch (val)
        {
            case 0:
                return mapteki_garyum_sayisi;
            case 1:
                return mapteki_nabilium_sayisi;
            case 2:
                return mapteki_xerat_sayisi;
            case 100:
                return mapteki_jackpot_sayisi;
            default:
                return 0;
        }
    }

    public IEnumerator jackpot_olustur()
    {
        bool jackpot_olustu_mu = false;
        while (true)
        {
            int Randomx = Random.Range(20, 145);
            int Randomy = Random.Range(-62, -16);
            Vector2 randompos = new(Randomx, Randomy);

            if (!((Randomx > 0 && Randomx < 35) && (Randomy > -35 && Randomy < 0)) && !(stone_area.Contains(randompos)))
            {
                if (mapteki_jackpot_sayisi < 20)
                {
                    stone_area.Add(new Vector2(Randomx, Randomy));
                    GameObject temp = Instantiate(jackpot, randompos, Quaternion.identity);
                    temp.transform.SetParent(jackpots_parent.transform);
                    temp.transform.name = "jackpot";
                    mapteki_jackpot_sayisi++;
                }
                else
                    jackpot_olustu_mu = true;
                if (jackpot_olustu_mu)
                    yield return new WaitForSeconds(1);
                else
                    yield return 0;
            }
        }
    }

    public void jackpoticerik(int val, GameObject jckpt)
    {
        float x;
        mapteki_jackpot_sayisi--;
        stone_area.Remove(jckpt.transform.position);
        Destroy(jckpt);
        switch (val)
        {
            case 0:
                x = Random.Range(1, 5) * 5;
                pd.Space_credits += (int)x;
                kazanilan_item_text_goster(x, "Space Credits", kazanilan_item_tmpro, 1.5f);
                create_log("Jackpottan " + x.ToString() + " Space Credits");
                break;
            case 1:
                x = Random.Range(5, 11);
                pd.Rank_points += (int)x;
                kazanilan_item_text_goster(x, "Rank Point", kazanilan_item_tmpro, 1.5f);
                create_log("Jackpottan " + x.ToString() + " Rank Point");
                break;
            case 2:
                x = Random.Range(1, 5);
                pd.Stones_count[0] += (int)x;
                kazanilan_item_text_goster(x, "Garyum", kazanilan_item_tmpro, 1.5f);
                stone_storage_filled_image();
                create_log("Jackpottan " + x.ToString() + " Garyum");
                break;
            case 3:
                x = Random.Range(1, 7) * 0.25f;
                pd.Paradium_credits += x;
                kazanilan_item_text_goster(x, "Paradium", kazanilan_item_tmpro, 1.5f);
                create_log("Jackpottan " + x.ToString() + " Paradium");
                break;
            default:
                break;
        }

    }
    public void repair_robot_active()
    {
        Player.GetComponent<playerc>().repair_robot_active();
    }

    public void map_on_changed()
    {
        StopCoroutine("enemy_spawn");
        StopCoroutine("stone_instantiate");
        StopCoroutine("jackpot_olustur");
    }

    public void ammo(int islem, int val, int selected_laser)//0->azalt,, 1->arttir,, 2->oku 
    {
        if (selected_laser == 0)
            return;
        if (islem == 0)
        {

            pd.Lasers_count[selected_laser - 1] -= val;
        }
        else if (islem == 1)
        {
            pd.Lasers_count[selected_laser - 1] += val;
        }
        else
        {

        }
    }
    public float ammo_count(int selected_laser)
    {
        GameObject player = GameObject.FindWithTag("Player");
        switch (selected_laser)
        {
            //case 0:
            //    playership_max_ammo_count = 1;
            //    return Mathf.Infinity;
            case 1:
                playership_max_ammo_count = player.GetComponent<playerc>().player_spaceship.max_laser_green;
                return pd.Lasers_count[0];
            case 2:
                playership_max_ammo_count = player.GetComponent<playerc>().player_spaceship.max_laser_blue;
                return pd.Lasers_count[1];
            case 3:
                playership_max_ammo_count = player.GetComponent<playerc>().player_spaceship.max_laser_yellow;
                return pd.Lasers_count[2];
            case 4:
                playership_max_ammo_count = player.GetComponent<playerc>().player_spaceship.max_laser_orange;
                return pd.Lasers_count[3];
            case 5:
                playership_max_ammo_count = player.GetComponent<playerc>().player_spaceship.max_laser_aqua;
                return pd.Lasers_count[4];
            default:
                return 1;
        }
    }

    public void play_area_warning_start_coroutine()
    {
        if (!warningcr)
            warning = StartCoroutine("play_area_warning");
    }
    public void play_area_warning_stop_coroutine()
    {
        StopCoroutine(warning);
        warningcr = false;
        main_camera.targetDisplay = 0;
        play_area_warning_camera.targetDisplay = 3;
        play_area_warning_text.gameObject.SetActive(false);
        play_area_arrow.SetActive(false);

    }
    public IEnumerator play_area_warning()
    {
        warningcr = true;
        play_area_warning_camera.targetDisplay = 0;
        play_area_warning_text.gameObject.SetActive(true);
        play_area_arrow.SetActive(true);
        while (true)
        {
            float t = Mathf.PingPong(Time.time, warningcam) / warningcam;
            play_area_warning_camera.backgroundColor = Color.Lerp(new Color32(9, 2, 1, 255), Color.black, t);
            play_area_arrow.transform.up = (new Vector2(40.96f, -40.96f) - (Vector2)Player.transform.position).normalized;
            yield return null;
        }
    }


    //public void loadscene(int val)
    //{
    //    background_back.sprite = backgrounds_back[val];
    //    background_right.sprite = backgrounds_right[val];

    //    switch (val)
    //    {
    //        case 0:
    //            enemy_clear(new string[] { "celarids" });
    //            stone_clear(new string[] { "Garyums" });
    //            break;

    //        case 1:
    //            enemy_clear(new string[] { "dilbians" });
    //            stone_clear(new string[] { "Nabiliums", "Xerats" });
    //            dilbian_instantiate();
    //            boss_dilbian_instantiate();
    //            nabilium_instantiate();
    //            xerat_instantiate();
    //            break;

    //        case 2:
    //            enemy_clear(new string[] { "" });
    //            stone_clear(new string[] { "" });
    //            break;

    //        case 3:

    //            break;
    //        default:
    //            break;
    //    }
    //}
    //public void enemy_clear(string[] enemy_names)
    //{
    //    string destroy_go_name;
    //    string destroy_go_tag;
    //    enemy_area.Clear(); 
    //    GameObject enemyparent = GameObject.Find("Enemies");
    //    for (int i = 0; i < enemyparent.transform.childCount; i++)
    //    {
    //        foreach (string item in enemy_names)
    //        {
    //            if (item == enemyparent.transform.GetChild(i).name)
    //                continue;
    //        }
    //        destroy_go_name = enemyparent.transform.GetChild(i).name;
    //        destroy_go_tag = enemyparent.transform.GetChild(i).tag;
    //        Destroy(enemyparent.transform.GetChild(i).gameObject);
    //        GameObject go= new GameObject(destroy_go_name);
    //        go.tag = destroy_go_tag;
    //    }

    //}

    //public void stone_clear(string[] stone_names)
    //{
    //    string destroy_go_name;
    //    string destroy_go_tag;
    //    stone_area.Clear();
    //    GameObject stoneparent = GameObject.Find("Stones");
    //    for (int i = 0; i < stoneparent.transform.childCount; i++)
    //    {
    //        foreach (string item in stone_names)
    //        {
    //            if (item == stoneparent.transform.GetChild(i).name)
    //                continue;
    //        }
    //        destroy_go_name = stoneparent.transform.GetChild(i).name;
    //        destroy_go_tag = stoneparent.transform.GetChild(i).tag;
    //        Destroy(stoneparent.transform.GetChild(i).gameObject);
    //        GameObject go = new GameObject(destroy_go_name);
    //        go.tag = destroy_go_tag;
    //    }

    //}

}
public enum Laser_type
{
    NB_30,
    CFZ_250,
    CFZ_270,
}
public enum Drone_type
{
    dart,
    vio,
    aquila,
}
