using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Move")]
public class MoveSO : ScriptableObject
{

    public string title;

    // kind of considering renaming this into just modifier for heals, however a skill maybe able to damage and heal... 
    public bool isDamage; 
    public float damageMod; 

    public bool isHeal;

    public float healMod;

    public float critMod; 

    public bool isDot;

    public StatusSO dotStatusScriptableObject; 

    // This will get the attack damage done and slot(s) hit 
    // for now [0,1,2,3], [4 4 - -], [- 5 5 -], [- - 6 6], [7 7 7 -], [- 8 8 8], [ 9 9 9 9]
    public int slot; 

    public int numOfTargets; 


    // priority and other things that will make this game diffrent from DD

    public int priorityLevel;

    public bool limitedPP; 
    public int powerPoint;


    // chartType & type of move it is like just damaging, or healing, buffs/debuff
    // these are honestly placeholders for now. I still thinking of maybe having like a AttackScript, HealScript, Buff/Debuff script as childern to MoveScript. 
    public int chartType; 

    // enum MoveType {damage, heal, dot}
    // public int MoveType; 

    // Worry about these later

    public int status; 

    public float statusRate; 

}
