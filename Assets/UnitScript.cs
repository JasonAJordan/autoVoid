using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UnitScript : MonoBehaviour
{

    public string title;
    public int hp;
    public int speed;
    public int attackTimes; 

    public int attack;
    public bool enemy; 

    public TextMeshProUGUI hpText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hpText.text = "" + hp;
    }

    public int BasicAttack(){
        // return UnityEngine.Random.Range(Convert.ToInt32(attack - 2), attack);
        return attack;
    }

    public void TakeDamage(int damage){
        hp = hp - damage; 
        // hpText.text = "" + hp;
    }
}
