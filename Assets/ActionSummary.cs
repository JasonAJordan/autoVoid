using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSummary : MonoBehaviour
{
    public UnitScript unit; 

    public enum MoveType {damage, heal, buff, debuff, dot}
    public MoveType movetype;
    public int hpChange; 


    // not sure how to handle buffs or debuffs just yet. 
}
