using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class enemyc : MonoBehaviour
{
    [SerializeField] public enemy_spaceship_variables enemy_variables;
    [SerializeField] GameObject explosion_effect;
    [SerializeField] GameObject enemy_jackpot;
    [SerializeField] Image healthbar;
    [SerializeField] TextMeshProUGUI damage_text;
    [SerializeField] float damage_text_minx, damage_text_maxX, damage_text_minY, damage_text_maxY;
    [SerializeField] GameObject enemy_laser;
    [SerializeField] GameObject[] laser_points;
    float hp_current;
    private bool agressive;
    private bool prev_agressive;
    GameObject player;
    private int speed;
    public int play_area_x;
    public int play_area_y;
    private int max_x, max_y, min_x, min_y, random_x, random_y;
    private int takedmg;
    private float change_pos_time =0.5f;
    GameObject gameco;
    playerdata pd;
    private void Start()
    {      
        hp_current = enemy_variables.base_hp;
        speed = enemy_variables.speed / 140;
        prev_agressive = false;
        max_x = (int)transform.position.x + play_area_x;
        min_x = (int)transform.position.x - play_area_x;
        max_y = (int)transform.position.y + play_area_y;
        min_y = (int)transform.position.y - play_area_y;
        random_x = UnityEngine.Random.Range(min_x, max_x + 1);
        random_y = UnityEngine.Random.Range(min_y, max_y + 1);
        player = GameObject.FindGameObjectWithTag("Player");
        takedmg = 0;
        gameco = GameObject.Find("GC");
        pd = gameco.GetComponent<playerdata>();
        //in_attackable_area = true;

    }
    IEnumerator health_regen()
    {
        while (true)
        {        
            if (hp_current < enemy_variables.base_hp)
            {
                if (hp_current + enemy_variables.base_hp_regen > enemy_variables.base_hp)
                {
                    hp_current = enemy_variables.base_hp;
                }
                else
                {
                    hp_current += enemy_variables.base_hp_regen;
                }
            }
            healthbar.fillAmount = hp_current / enemy_variables.base_hp;
            yield return new WaitForSeconds(1);
        }
    }
    IEnumerator updte()
    {     
        while (true)
        {
            if (agressive_on_changed())
            {
                StartCoroutine("enemy_shoot");
            }
            if (agressive)
            {
                enemy_agressive_move();
                healthbar.transform.parent.gameObject.SetActive(true);
            }
            else
            {
                StopCoroutine("enemy_shoot");
                enemy_move();
                healthbar.transform.parent.gameObject.SetActive(false);
            }
            yield return 0;

        }
    }
    private void OnBecameVisible()
    {
        StartCoroutine("updte");
        StartCoroutine("health_regen");
    }
    private void OnBecameInvisible()
    {
        StopCoroutine("updte");
        StopCoroutine("health_regen");
        StopCoroutine("enemy_shoot");
    }
    private bool agressive_on_changed()
    {
        if (prev_agressive == false && agressive)
        {
            prev_agressive = agressive;
            return true;
        }
        else if (prev_agressive == true && !agressive)
        {
            prev_agressive = agressive;
            return false;
        }
        return false;
    }
    private void enemy_move()
    {
        
        Vector2 random_pos = new(random_x, random_y);
        if (Vector2.Distance(transform.position, random_pos) < .4f)
        {
            if (change_pos_time>0)
            {
                change_pos_time -= Time.deltaTime;
            }
            else
            {
                random_x = UnityEngine.Random.Range(min_x, max_x + 1);
                random_y = UnityEngine.Random.Range(min_y, max_y + 1);
                change_pos_time = .6f;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, random_pos, Time.deltaTime * speed);
            transform.up = (random_pos - (Vector2)transform.position).normalized;
        }

    }

    private void enemy_agressive_move()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        float angle;
        if (agressive)
        {
            if (distance > 5)
            {
                agressive = false;
                return;
            }
            Vector2 direction = player.transform.position - transform.position;
            direction.Normalize();
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
            if (distance < 3)
            {
                return;
            }
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            //transform.SetPositionAndRotation(Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime),
            // Quaternion.Euler(Vector3.forward * angle));
        }
    }
    IEnumerator enemy_shoot()
    {
        while (true)
        {
            foreach (var item in laser_points)
            {
                GameObject l1= Instantiate(enemy_laser, item.transform.position, gamecontrol.lookat(item.transform, player.transform));
                l1.GetComponent<Rigidbody2D>().velocity = (player.transform.position - l1.transform.position).normalized * 25;
                l1.GetComponent<laser_damage>().dmg(enemy_variables.base_attack_damage + UnityEngine.Random.Range(-2, +3f));
            }
            yield return new WaitForSeconds(enemy_variables.fire_rate);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "player_laser")
        {
            agressive = true;
            take_damage(collision.GetComponent<laser_damage>().damage);
            Destroy(collision.gameObject);
        }
        else if(collision.tag=="missile")
        {
            take_damage(collision.GetComponent<missile>().damage);
            Destroy(collision.gameObject);
        }
        //if (collision.tag == "station")
        //{
        //    in_attackable_area = false;
        //}


    }
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.tag == "station")
    //    {
    //        in_attackable_area = true;
    //    }
    //}
    public void take_damage(float damage_value)
    {   
        takedmg++;
        hp_current -= damage_value;
        healthbar.fillAmount = hp_current / enemy_variables.base_hp;
        if (takedmg % 2 == 1)
            return;
        Vector2 randompos = new(UnityEngine.Random.Range(damage_text_minx, damage_text_maxY), UnityEngine.Random.Range(damage_text_minY, damage_text_maxY));
        damage_text.rectTransform.anchoredPosition = randompos;
        damage_text.text = (damage_value*2).ToString("0.#");
        StartCoroutine("damage_text_rotation");
        Invoke("text_reset", player.GetComponent<playerc>().player_spaceship.fire_rate-.25f);
        if (hp_current < 0)
        {
            switch (enemy_variables.enemy_name)
            {
                case "Celarid":
                    gamecontrol.mapteki_e1_sayisi--;
                    break;
                case "Dilbian":
                    gamecontrol.mapteki_dilbian_sayisi--;
                    break;
                case "Boss-Dilbian":gamecontrol.mapteki_bossdilbian_sayisi--;
                    break;
                default:
                    break;
            }
            damage_text.transform.parent = null;
            damage_text.transform.position = transform.position;
            Destroy(damage_text.gameObject,.5f);
            Instantiate(explosion_effect, transform.position, Quaternion.identity);
            GameObject ej= Instantiate(enemy_jackpot, transform.position, Quaternion.identity);
            ej.GetComponent<enemy_jackpot>().space_credits = enemy_variables.reward_space_credits;
            ej.GetComponent<enemy_jackpot>().rank_points = enemy_variables.reward_rank_points;
            ej.GetComponent<enemy_jackpot>().paradium = MathF.Round(UnityEngine.Random.Range(enemy_variables.reward_min_paradium_credits
                , enemy_variables.reward_max_paradium_credits),1);
            ej.GetComponent<enemy_jackpot>().owner =enemy_variables.enemy_name;
            if (pd.Active_mission_id== 1&&enemy_variables.enemy_name=="Celarid")
                gameco.GetComponent<MissionControl>().make_progress(1);
            player.GetComponent<playerc>().target_enemy_log = true;
            Destroy(ej,20f);
            Destroy(gameObject);
        }
    }
    void text_reset()
    {
        damage_text.text = "";
        StopCoroutine("damage_text_rotation");
    }
    IEnumerator damage_text_rotation()
    {
        while(true)
        {
            damage_text.rectTransform.rotation = Quaternion.Euler(0, 0, -transform.rotation.z);
            yield return 0;
        }
       
    }

}
