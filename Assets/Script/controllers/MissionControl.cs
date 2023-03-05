using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MissionControl : MonoBehaviour
{
    public List<Missionscobj> missions;
    public  Missionscobj  active_missions;
    [SerializeField] TextMeshProUGUI mission_panel_text;
    [SerializeField] Image filled_image;
    [SerializeField] TextMeshProUGUI current_text,title;
    private playerdata pd;
    
    private void Start()
    {
        pd = GameObject.Find("GC").GetComponent<playerdata>();
        mission_set_active();
    }
    public void show_mission()
    {
        mission_panel_text.text = active_missions.explain_mission;
        current_text.text = active_missions.questtype.current.ToString();
        title.text = active_missions.title_mission;
        filled_image.fillAmount = (float)active_missions.questtype.current / active_missions.questtype.target;
    }
    public void mission_set_active()
    {
        active_missions = missions[pd.Active_mission_id];
        missions[pd.Active_mission_id].is_active_mission = true;
        show_mission();
        if (active_missions.id_mission == 4)
            active_missions.questtype.current = pd.Rank_points;
    }
    public void make_progress(int step)
    {
        active_missions.questtype.func(step);
        filled_image.fillAmount = (float)active_missions.questtype.current / active_missions.questtype.target;
        current_text.text = active_missions.questtype.current.ToString();
        if(active_missions.questtype.is_reached())
        {
            Debug.LogError("mission complete");
            finish_quest();
        }
        GameObject.Find("GC").GetComponent<gamecontrol>().veri_guncelle_ve_kaydet();
    }
    public void finish_quest()
    {
        if (active_missions.space_credit_reward != 0)
        {
            int val= active_missions.space_credit_reward;
            pd.Space_credits += val;
            GetComponent<gamecontrol>().create_log(active_missions.title_mission + " görevinden " + val+" space credit");
        }
        if (active_missions.paradium_reward != 0)
        {
            float val = active_missions.paradium_reward;
            pd.Paradium_credits += val;
            GetComponent<gamecontrol>().create_log(active_missions.title_mission + " görevinden " + val+" paradium");

        }
        if (active_missions.garyum_reward != 0)
        {
            int val= active_missions.garyum_reward;
            pd.Stones_count[0] += val;
            GetComponent<gamecontrol>().create_log(active_missions.title_mission + " görevinden " + val+" garyum");
        }
        if (active_missions.nabilium_reward!= 0)
        {
            int val= active_missions.nabilium_reward;
            pd.Stones_count[1] += val;
            GetComponent<gamecontrol>().create_log(active_missions.title_mission + " görevinden " + val+" nabilium");

        }
        if (active_missions.xerat_reward != 0)
        {
            int val= active_missions.xerat_reward;
            pd.Stones_count[2] += val;
            GetComponent<gamecontrol>().create_log(active_missions.title_mission + " görevinden " + val+" xerat");

        }
        pd.Rank_points += active_missions.rank_point_reward;
        GetComponent<gamecontrol>().create_log(active_missions.title_mission + " görevinden " + active_missions.rank_point_reward+" rank point");
        active_missions.is_active_mission = false;
        pd.Active_mission_id = active_missions.id_mission + 1;
        mission_set_active();


    }
    
}
