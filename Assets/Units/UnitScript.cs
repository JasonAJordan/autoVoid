using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class UnitScript : MonoBehaviour
{
    // general 
    public string title;

    public UnitSO unitInit;

    // stats
    public int hp;
    public int speed;
    public int attackTimes; 

    public int statAttack;
    public bool enemy; 

    public int slot; 

    // Attacking related 
    public List<MoveSO> moves;

    public int actionNumber; 

    // UI related

    public TextMeshProUGUI hpText;

    // Start is called before the first frame update
    void Start()
    {   
        title = unitInit.title;
        hp = unitInit.hp;
        speed = unitInit.speed;
        attackTimes = unitInit.attackTimes;
        statAttack = unitInit.statAttack;
        enemy = unitInit.enemy; 
        moves = unitInit.moves; 
        GetComponent<SpriteRenderer>().sprite = unitInit.baseArtwork;
    }

    // Update is called once per frame
    void Update()
    {
        hpText.text = "" + hp;
    }

    // public int BasicAttack(){
    //     // return UnityEngine.Random.Range(Convert.ToInt32(attack - 2), attack);
    //     return statAttack;
    // }


    // This will get the attack damage done and slot(s) hit 
    // for now [0,1,2,3], [4 4 - -], [- 5 5 -], [- - 6 6], [7 7 7 -], [- 8 8 8], [ 9 9 9 9]
    // toDo learn C# or equilvant 
    // Also toDo skip move if can't hit anything enemy in a slot; 
    public MoveSO GetAttack(){
        MoveSO currentMove = moves[actionNumber % moves.Count];
        // if CheckIfAttackIsValid(){
            // this if will check to see if the unit should skip the move 
        //}
        actionNumber++;

        // int moveDmg = Convert.ToInt32(Math.Floor(statAttack* currentMove.damageMod));
        return currentMove;
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
