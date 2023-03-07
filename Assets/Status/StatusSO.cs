using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Status")]
public class StatusSO : ScriptableObject
{
    // fake enum  0 -> dot, 1 -> buff, also not being used currently
    public int statusType;

    // common
    public string title;

    public int actionsTurnsRemaining;

    public Sprite baseArtwork; 

    // dot/heal related
    public int hpChange;

    // buffs and debuffs related
    
    public int tempStatAttackChange;

}
