using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSummary : MonoBehaviour
{
    public UnitScript unit; 
    // Thinking about getting rid of this for damage + dot + etc attacks 
    public enum MoveType {damage, heal, buff, debuff, dot}
    public bool isDamage;
    public bool isDot;

    public StatusSO dotStatusScriptableObject; 

    public MoveType movetype;
    public int hpChange; 


    // not sure how to handle buffs or debuffs just yet. 
}
