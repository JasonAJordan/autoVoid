using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UnitScript : MonoBehaviour
{
    // general 
    public string title;

    // stats
    public int hp;
    public int speed;
    public int attackTimes; 

    public int statAttack;
    public bool enemy; 

    public int slot; 

    // Attacking related 
    public List<MoveScript> moves;

    public int actionNumber; 

    // UI related

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
        return statAttack;
    }


    // This will get the attack damage done and slot(s) hit 
    // for now [0,1,2,3], [4 4 - -], [- 5 5 -], [- - 6 6], [7 7 7 -], [- 8 8 8], [ 9 9 9 9]
    // toDo learn C# or equilvant 
    // Also toDo skip move if can't hit anything enemy in a slot; 
    public (int, int, string ) GetAttack(){
        MoveScript currentMove = moves[actionNumber % moves.Count];
        actionNumber++;
        int moveDmg = Convert.ToInt32(Math.Floor(statAttack* currentMove.damageMod));
        return (0, statAttack, currentMove.title);
    }
    // see GetAttack() comment. 
    public bool CheckIfAttackIsValid(){
        return false; 
    }

    public void TakeDamage(int damage){
        int newHp = hp - damage;
        hp = newHp < 0 ? 0 : newHp;

    }
}
