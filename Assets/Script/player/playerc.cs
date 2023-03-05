using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class playerc : MonoBehaviour //WILL BE FIXED
{
    [SerializeField] public spaceship_variables player_spaceship;
    private float hiz;
    [SerializeField] Transform point1, point2, missile_point;
    Image player_health_bar;
    TextMeshProUGUI ship_panel_current_player_hp_text;
    [SerializeField] GameObject repair_robot;
    GameObject warning_area;
    Vector2 mousepos;
    public static float hp_current;
    private float shoot_time;
    bool player_shoot;
    private Coroutine base_coroutine, robot_coroutine;
    public static Coroutine player_move_coroutine;
    public float base_dmg;
    [SerializeField] Transform[] drone_points;
    private bool in_play_area = true;
    public bool missile_bool = false;
    private bool missile_rate = true;
    public bool target_enemy_log = true;
    private GameObject prev_target_enemy = null;
    playerdata pd;
    gamecontrol Gameco;
    [SerializeField]Transform outcircle,incircle;
    Vector2 PointA, PointB;
    private void Start()
    {
        Gameco = GameObject.Find("GC").GetComponent<gamecontrol>();
        hiz = player_spaceship.speed / 120;
        player_health_bar = Gameco.ship_panel_hp_filled_bar;
        ship_panel_current_player_hp_text = Gameco.ship_panel_hp_text;
        //ship_panel_current_player_hp_text = GameObject.FindWithTag("panel_crr_hp").GetComponent<TextMeshProUGUI>();
        //drone_instantiate();
        pd = Gameco.GetComponent<playerdata>();
        shoot_time = 0;
        hp_current = player_spaceship.base_hp;
        base_coroutine = StartCoroutine(health_regen(player_spaceship.base_hp_regen));
        //base_dmg_drone_update();
        //outcircle = GameObject.FindWithTag("out").transform;
        //incircle = GameObject.FindWithTag("in").transform;
    }
    //void deneme()
    //{
    //    for (int i = 0; i < player_spaceship.drones.Length; i++)
    //    {
    //        if(i<denemedrone.Length)
    //        player_spaceship.drones[i] = denemedrone[i];
    //        else
    //        player_spaceship.drones[i] = null;

    //    }

    //    drone_instantiate();
    //}

    //void drone_instantiate()
    //{
    //    int i = 0;

    //    foreach (GameObject item in player_spaceship.drones)
    //    {
    //        if (drone_points[i].childCount > 0)
    //            Destroy(drone_points[i].GetChild(0).gameObject);
    //        if (item != null)
    //        {
    //            Instantiate(item, drone_points[i]);
    //            //points[i].name = "asd" + i;
    //            //points[i] = item;
    //            i++;
    //        }

    //    }
    //    base_dmg_drone_update();
    //}
    void Update()
    {
        if (shoot_time > 0)
        {
            shoot_time -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            repair_robot_active();
        }
        if(Input.touchCount>0)
        {
            
            //Touch t1 = Input.GetTouch(0);
            //Vector2 touchpos = Camera.main.WorldToScreenPoint(t1.position);
            //if(t1.phase==TouchPhase.Began)
            //{
            //    incircle.position = touchpos;
            //    outcircle.position = touchpos;
            //    PointA = touchpos;
            //}
            //incircle.position = touchpos;
            //Vector2 offset = touchpos - PointA;
            //Vector2 direction = Vector2.ClampMagnitude(offset, 1);
            //asd(direction);
            //incircle.position = new Vector2(PointA.x + direction.x, PointA.y + direction.y);
            //if(t1.phase==TouchPhase.Ended)
            //{
            //    //incircle.GetComponent<SpriteRenderer>().enabled = false;
            //    //outcircle.GetComponent<SpriteRenderer>().enabled = false;

            //}
        }
        if (Input.GetMouseButtonDown(0) && !(EventSystem.current.IsPointerOverGameObject()))
        {

            mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.up = (mousepos - (Vector2)transform.position).normalized;
            asd(mousepos);
        }
        //if (Input.touchCount > 0)
        //{
        //    if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        //    {
        //        Debug.LogError("ui týklandý");

        //    }
        //    else
        //    {
        //        Vector2 touchpos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        //        transform.up = (touchpos - (Vector2)transform.position).normalized;
        //        asd(touchpos);
        //    }
        //}

        if (gamecontrol.attackable_enemy_ships.Count > 0)
        {
            if (Input.GetKey(KeyCode.Space) && shoot_time <= 0 && !player_shoot && Gameco.ammo_count(gamecontrol.selected_laser_int) > 0)
            {
                StartCoroutine("lock_target");
                StartCoroutine("shoot");
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (shoot_time <= 0 && player_shoot)
                    shoot_time = player_spaceship.fire_rate;
                if (player_shoot)
                {
                    Gameco.create_log("Saldýrý iptal edildi");
                    target_enemy_log = true;
                }
                StopCoroutine("shoot");
                player_shoot = false;
                StopCoroutine("lock_target");
            }
        }
        else
        {
            StopCoroutine("shoot");
            StopCoroutine("lock_target");
            if (player_shoot)
                shoot_time = player_spaceship.fire_rate;
            player_shoot = false;

        }
    }
    private void target_enemy_changed()
    {
        if (prev_target_enemy == null || prev_target_enemy != selected_target())
            target_enemy_log = true;
        prev_target_enemy = selected_target();
    }

    public void repair_robot_active()
    {
        repair_robot.SetActive(!repair_robot.activeInHierarchy);
    }
    public void base_regen()
    {
        StopCoroutine(robot_coroutine);
        base_coroutine = StartCoroutine(health_regen(player_spaceship.base_hp_regen));
    }
    public void repair_robot_regen(float regen)
    {
        StopCoroutine(base_coroutine);
        robot_coroutine = StartCoroutine(health_regen(regen));
    }
    public IEnumerator health_regen(float hp_regen)
    {
        while (true)
        {

            if (hp_current < player_spaceship.base_hp)
            {
                if (hp_current + hp_regen > player_spaceship.base_hp)
                {
                    hp_current = player_spaceship.base_hp;
                }
                else
                {
                    hp_current += hp_regen;
                }
            }
            player_health_bar.fillAmount = hp_current / player_spaceship.base_hp;
            yield return new WaitForSeconds(1);
        }

    }

    public GameObject selected_target()
    {
        GameObject target = null;
        float distance = 1000;
        List<GameObject> targets = gamecontrol.attackable_enemy_ships;
        foreach (GameObject ship in targets)
        {
            float dis = Vector2.Distance(ship.transform.position, gameObject.transform.position);
            if (dis < distance)
            {
                target = ship;
                distance = dis;
            }
        }
        return target;
    }
    public GameObject selected_laser()
    {
        return gamecontrol.selected_laser;
    }
    public void asd(Vector2 touchpos)
    {
        repair_robot.SetActive(false);
        if (player_move_coroutine != null)
            StopCoroutine(player_move_coroutine);
        player_move_coroutine = StartCoroutine("hareket", touchpos);
    }
    public IEnumerator lock_target()
    {
        while (true)
        {
            transform.rotation = gamecontrol.lookat(transform, selected_target().transform);
            yield return 1;
        }

    }
    public void stop_move()
    {
        if (player_move_coroutine != null)
            StopCoroutine(player_move_coroutine);
    }
    public IEnumerator hareket(Vector2 mousepos)
    {
        while ((Vector2)transform.position != mousepos)
        {
            if (!player_shoot)
                transform.up = (mousepos - (Vector2)transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, mousepos, hiz * Time.deltaTime);
            yield return 1;

        }
        //StopCoroutine("hareket");
    }
    IEnumerator shoot()
    {
        player_shoot = true;
        while ((Gameco.ammo_count(gamecontrol.selected_laser_int) > 0))
        {
            GameObject target = selected_target();
            target_enemy_changed();
            if (target_enemy_log)
                Gameco.create_log(target.name + "rakibine saldýrýya baþlandý");
            target_enemy_log = false;
            Gameco.ammo(0, 1, gamecontrol.selected_laser_int);
            Gameco.laser_info_panel_update();
            Gameco.veri_guncelle_ve_kaydet();
            float damage = (Random.Range(-3, 4f) + base_dmg);
            GameObject l1 = Instantiate(selected_laser(), point1.position, gamecontrol.lookat(point1, target.transform));
            GameObject l2 = Instantiate(selected_laser(), point2.position, gamecontrol.lookat(point2, target.transform));
            l1.GetComponent<Rigidbody2D>().velocity = (target.transform.position - l1.transform.position).normalized * 20;
            l1.GetComponent<laser_damage>().dmg(damage);
            l2.GetComponent<Rigidbody2D>().velocity = (target.transform.position - l2.transform.position).normalized * 20;
            l2.GetComponent<laser_damage>().dmg(damage);
            if (missile_bool && missile_rate)
                Invoke("throw_missile", player_spaceship.fire_rate / 2);
            else
                missile_rate = true;
            yield return new WaitForSeconds(player_spaceship.fire_rate);
        }
        StopCoroutine("lock_target");

    }
    void throw_missile()
    {
        int selected = gamecontrol.selected_missiles_int;
        if (player_shoot && pd.Missiles_count[selected] > 0)
        {
            GameObject miss = Instantiate(Gameco.missiles[selected], missile_point.position, gamecontrol.lookat(missile_point, selected_target().transform));
            miss.GetComponent<Rigidbody2D>().velocity = (selected_target().transform.position - miss.transform.position).normalized * 20;
            missile_rate = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "stone":
                Gameco.stone_pickup(collision.gameObject);
                break;
            case "jackpot":
                Gameco.jackpoticerik(Random.Range(0, 4), collision.gameObject);
                break;
            case "enemy_laser":
                take_damage(collision.GetComponent<laser_damage>().damage);
                Destroy(collision.gameObject);
                break;
            //case "oyun_alani_uyari":
            //    Gameco().play_area_warning_start_coroutine();
            //    break;
            case "oyun_alani_destroy":
                Destroy(gameObject);
                break;
            case "enemy_jackpot":
                if (gamecontrol.enemy_jackpot_is_pickable)
                    Gameco.enemy_jackpot_pickup(collision.gameObject);
                break;
            default:
                break;
        }
    }
    public void take_damage(float damage_value)
    {
        if (Gameco.GetComponent<MissionControl>().active_missions.id_mission == 3)
            Gameco.GetComponent<MissionControl>().make_progress((int)damage_value);
        hp_current -= damage_value;
        info_update();
        if (hp_current < 0)
        {
            Destroy(gameObject);
        }
    }

    public void info_update()
    {
        player_health_bar.fillAmount = hp_current / player_spaceship.base_hp;
        ship_panel_current_player_hp_text.text = ((int)hp_current).ToString();
    }

    //public void drone_instantiate()
    //{
    //    if (playerdata.Drones == null)
    //    {
    //        return;

    //    }
    //    int i = 0;
    //    foreach (var item in playerdata.Drones)
    //    {
    //        //player_spaceship.drones[i] = Instantiate(item, drone_points[i]);
    //        if(item!=null)
    //        {
    //            Instantiate(item, drone_points[i]);
    //            i++;
    //        }
    //    }

    //}
}
