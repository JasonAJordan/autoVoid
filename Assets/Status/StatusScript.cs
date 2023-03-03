using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Let's not use this anymore 
public class StatusScript : MonoBehaviour
{
    // common
    public string title;

    public int actionsTurnsRemaining;

    public Sprite baseArtwork; 

    // dot related
    public int damage;

    // buffs and debuffs related
    

    void Start()
    {
        // GetComponent<SpriteRenderer>().sprite = baseArtwork;
    }
}
