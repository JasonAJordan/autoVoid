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
    public MoveSO GetAttack(List<GameObject> opposition ){
        bool isValidAttack = false; 
        //int[] threeUnits = {0,1,2,4,5,7};
        //List<int> threeUnitValid = new List<int>(threeUnits);
        int[] twoUnits = {0,1,4,5,7,8,9};
        List<int> twoUnitValid = new List<int>(twoUnits);
        int[] oneUnits = {0,4,7,9};
        List<int> oneUnitValid = new List<int>(oneUnits);
        // MoveSO dudAttack = CreateDudAttack();

        MoveSO currentMove = moves[actionNumber % moves.Count];
        for (int i = 0; i < 4; i++){
            if (opposition.Count == 4){
                isValidAttack = true;
                break;
            } else if (opposition.Count == 3 && currentMove.slot != 3){
                isValidAttack = true;
                break;
            } else if (opposition.Count == 2){
                if (twoUnitValid.Contains(currentMove.slot)){
                    isValidAttack = true;
                    break;
                }
            } else {
                if (oneUnitValid.Contains(currentMove.slot)){
                    isValidAttack = true;
                    break;
                }
            }
            actionNumber++;
            currentMove = moves[actionNumber % moves.Count];
        }
        actionNumber++;
        return isValidAttack ? currentMove : CreateDudAttack();
    }

    public MoveSO CreateDudAttack(){
        MoveSO dudAttack = new MoveSO();
        dudAttack.title = "Struggle";
        dudAttack.damageMod = 1;
        dudAttack.critMod = 1;
        dudAttack.slot = 0;
        dudAttack.numOfTargets = 1;
        return dudAttack;
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
