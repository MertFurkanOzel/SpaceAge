using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MissionEnum
{
    public questtype _questtype;
    public int target, current;

    public bool is_reached()
    {
        return (target <= current);
    }
    public void stone_collected()
    {
        
    }
    public void enemy_killed()
    {
        
    }
    public void func(int step)
    {
        current += step;
        switch (_questtype)
        {
            case questtype.stone_pickup:
                stone_collected();
                break;
            case questtype.stone_sell:
                break;
            case questtype.Reach:
                break;
            case questtype.Jackpot_pickup:
                break;
            case questtype.enemyjackpot_pickup:
                break;
            case questtype.takedamage:
                break;
            case questtype.givedamage:
                break;
            case questtype.healthregen:
                break;
            case questtype.enemykilled:
                enemy_killed();
                break;
            default:
                break;
        }
    }
    
}
public enum questtype
{
    stone_pickup,
    stone_sell,
    Jackpot_pickup,
    Reach,
    enemyjackpot_pickup,
    takedamage,
    givedamage,
    healthregen,
    enemykilled,
}