using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// May be deprecated soon 
public class MoveScript : MonoBehaviour
{
    public string title;

    // kind of considering renaming this into just modifier for heals, however a skill maybe able to damage and heal... 
    public float damageMod; 

    public float critMod; 

    // There will be two types of 
    public int slot; 

    public int numOfTargets; 


    // chartType & type of move it is like just damaging, or healing, buffs/debuff
    // these are honestly placeholders for now. I still thinking of maybe having like a AttackScript, HealScript, Buff/Debuff script as childern to MoveScript. 
    public int chartType; 

    public int MoveType; 

    // Worry about these later

    public int status; 

    public float statusRate; 

}
