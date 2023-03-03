using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Status")]
public class StatusSO : ScriptableObject
{
    // common
    public string title;

    public int actionsTurnsRemaining;

    public Sprite baseArtwork; 

    // dot/heal related
    public int hpChange;

    // buffs and debuffs related
    

}
