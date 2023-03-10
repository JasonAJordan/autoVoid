using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class UnitScript : MonoBehaviour
{
    // general 
    public string title;

    public UnitSO unitInit;

    // stats
    public int hp;
    public int maxHp;
    public int speed;
    public int attackTimes; 

    public int statAttack;
    public bool enemy; 

    public int slot; 

    // Attacking related 
    public List<MoveSO> moves;

    public int actionNumber; 

    // Buffs, Debuffs, DOTS

    public List<StatusSO> statues; 


    // UI related
    public GameObject ThisGameObject;
    public GameObject UnitStatus;
    public TextMeshProUGUI hpText;

    // Start is called before the first frame update
    void Start()
    {   
        title = unitInit.title;
        hp = unitInit.hp;
        maxHp = unitInit.hp;
        speed = unitInit.speed;
        attackTimes = unitInit.attackTimes;
        statAttack = unitInit.statAttack;
        enemy = unitInit.enemy; 
        moves = unitInit.moves; 
        statues = unitInit.statuses;
        GetComponent<SpriteRenderer>().sprite = unitInit.baseArtwork;



    }

    // Update is called once per frame
    void Update()
    {
        hpText.text = "" + hp;


    }



    // This will get the attack damage done and slot(s) hit 
    // for now [0,1,2,3], [4 4 - -], [- 5 5 -], [- - 6 6], [7 7 7 -], [- 8 8 8], [ 9 9 9 9]
    // toDo learn C# or equilvant 
    // Also toDo make sure this skips if the attack has priority 

    public MoveSO GetAttack(List<GameObject> opposition, bool fromCreateAttackOrder ){
        // testing

        bool isValidAttack = false; 
        int[] twoUnits = {0,1,4,5,7,8,9};
        List<int> twoUnitValid = new List<int>(twoUnits);
        int[] oneUnits = {0,4,7,9};
        List<int> oneUnitValid = new List<int>(oneUnits);
        
        // Debug.Log(title + moves.Count);
        MoveSO currentMove = moves[actionNumber % moves.Count];
        // Only attacks moves can "fail" due to not having a target, 
        // buffs will always go through 
        if (currentMove.isDamage){ 
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
                // This will cause the priority move NOT to go off and NOT select the next move. 
                // This is so if a priority moves fails, the unit will not act at a higher speed tier with a "stronger" move. 
                // Also it adds a draw back to priority moves. 
                if (currentMove.priorityLevel == 1 && !fromCreateAttackOrder){
                    break; 
                }
                actionNumber++;
                currentMove = moves[actionNumber % moves.Count];
            }
        } else {
            isValidAttack = true;
        }

        // moving the actionNumber++ to damageUnit() so I can run this check in multi locations; 
        // actionNumber++;
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

    public void changeHP(int hpChange){
        int newHp = hp + hpChange;
        hp = newHp < 0 ? 0 : newHp;
        hp = hp > maxHp ? maxHp : hp; 
    }

    public void addStatus(StatusSO status){
        List <StatusSO> newStatues = (from s in statues
            where s.title != status.title
            select s).ToList();
        StatusSO newStatus = ScriptableObject.Instantiate<StatusSO>(status);

        // 
        if (statues.Any( x=> x.title == status.title)){
            // For now we will just refresh the status
            StatusSO oldStatus = statues.Find(x => x.title == status.title);
            oldStatus = status;
            Debug.Log("Debuff reapplied!");
        } else {
            // This way might be overkill... 
            // Also needs to be a function to check for status that are already applied 
            GameObject newStatusGameObject = ScriptableObject.Instantiate<GameObject>(UnitStatus, transform.position, Quaternion.identity);
            newStatusGameObject.name = status.title;
            
            // Buff / debuffs stat changes will be here
            executeStatusStatChange(true, status);


            // I forgot why we would need the tag when we use the title...
            // newStatusGameObject.tag = status.title;


            newStatusGameObject.GetComponent<SpriteRenderer>().sprite = status.baseArtwork;
            float translateSpriteHeight = (newStatues.Count() + 1f)  * .9f ; 
            newStatusGameObject.transform.Translate(0, translateSpriteHeight, 0);
            newStatusGameObject.transform.localScale = new Vector3(.08f,.08f,0);
            
            newStatusGameObject.transform.parent = ThisGameObject.transform;
            reTranslateStatusSprites();
        }


        newStatues.Add(newStatus);
        statues = newStatues;
    }

    public string executeStatus(){
        String returnString = "";


        for (int i  = 0; i< statues.Count; i++){
            StatusSO status = statues[i];
            changeHP(status.hpChange);
            int hpChangeAbs = Math.Abs(status.hpChange);
            status.actionsTurnsRemaining = status.actionsTurnsRemaining - 1;
            if (status.statusType == 0){
                returnString = title + " took " + hpChangeAbs + " from " + status.title;
            } 
            // else if (status.statusType == 1){
            //     returnString = status.title + " has been applied to " + title; 
            // }

            if (status.actionsTurnsRemaining == 0 ){

                executeStatusStatChange(false, status);

                statues.RemoveAt(i);
                GameObject statusRenderGO = transform.Find(status.title).gameObject;
                if (statusRenderGO){
                    Destroy(statusRenderGO);
                    reTranslateStatusSprites();
                }
                i--;
            }
        }
        
        return returnString;
    }

    public void executeStatusStatChange(bool adding, StatusSO status){
        if (adding){
            statAttack += status.tempStatAttackChange;
        } else {
            statAttack -= status.tempStatAttackChange;
        }
    }

    public void reTranslateStatusSprites(){
        float translateSpriteHeight = 0.9f;
        Vector3 UnitGOPos = transform.position; 
        foreach (StatusSO status in statues){
            GameObject statusRenderGO = transform.Find(status.title).gameObject;
            statusRenderGO.transform.position = UnitGOPos;
            statusRenderGO.transform.Translate(0, translateSpriteHeight, 0);
            translateSpriteHeight += 0.9f;
        }
    }
}
