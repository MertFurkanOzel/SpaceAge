using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="mission-",menuName ="Create Mission")]
public class Missionscobj : ScriptableObject
{
    public MissionEnum questtype;
    public int id_mission;
    public bool is_active_mission;
    public string title_mission;
    public string explain_mission;
    public int garyum_reward;
    public int nabilium_reward;
    public int xerat_reward;
    public int space_credit_reward;
    public int rank_point_reward;
    public float paradium_reward;
}
