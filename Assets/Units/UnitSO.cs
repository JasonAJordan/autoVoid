using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


[CreateAssetMenu(fileName = "New Unit")]
public class UnitSO : ScriptableObject
{
    public string title;
    public int hp;
    public int speed;
    public int attackTimes; 
    public int statAttack;
    public bool enemy; 
    public List<MoveSO> moves;

    public Sprite baseArtwork; 

}
