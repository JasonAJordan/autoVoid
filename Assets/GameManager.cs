using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class GameManager : MonoBehaviour
{
    // Battle Actives
    public bool battleLive; 
    public float turnSpeed = 1.0f;
    public float period = 1.0f;
    public int counter = 0;
    public int turn; 

    // Battle Settings 

    public int turnLength; 
    
    public List<GameObject> heroes;

    public List<GameObject> enemies;

    public List<UnitScript> attackOrder; 

    // Game UI and related
    public TextMeshProUGUI actionCounter;

    public TextMeshProUGUI turnCounter; 

    public GameObject startCombatButton;

    public TextMeshProUGUI actionDisplayText; 
    // temp UI
    public TextMeshProUGUI etcDisplayText; 

    // Start is called before the first frame update
    void Start()
    {
        turn = 1;
        findTurnLength(); 
        createAttackOrder(heroes, enemies);

    }

    // Update is called once per frame
    void Update()
    { 
    if (battleLive){
        startCombatButton.SetActive (false);

        checkAllUnitHPs();


        period += Time.deltaTime;
 
        // An action step 
        if (period >= turnSpeed) {
            // clear any old text
            etcDisplayText.text = "";
            period = period - turnSpeed;

            // run any test that are need for a new turn
            // NEW no longer needed as status are now applied after the attack!!
            // checkAllUnitStatus();


            UnitScript currentUnit = attackOrder[counter % attackOrder.Count];
            // int damage = currentUnit.BasicAttack();



            List<GameObject> opposition = currentUnit.enemy ? heroes : enemies; 
            
            // I will need to update GetAttack, executeMove, & executeMoveSummaries each time I update moves for new functions 
            // GetAttack will get the next possible attack. 
            MoveSO moveScript = currentUnit.GetAttack(opposition, false); 
            executeMove(currentUnit, moveScript);
            executeUnitStatus(currentUnit); 

            counter++; 
            actionCounter.text = "Action Number " + counter;
            if (counter % turnLength == 0){
                turn++; 
                createAttackOrder(heroes, enemies);
                turnCounter.text = "Turn: " + turn;
            }
            // Attack order will need to update for priority moves
           
        }
        

        }
    }
        

    public void toggleCombat(){
        battleLive = !battleLive; 
    }   

    // checks to see if either all hero or enemy unit hp total is less than 0
    // This would need a update 
    public void checkAllUnitHPs(){

        bool enemiesHPsCheck = enemies.Select(enemy => enemy.gameObject.GetComponent<UnitScript>().hp ).ToList().All( hp => hp <= 0);

        bool herosHPsCheck = heroes.Select(hero => hero.gameObject.GetComponent<UnitScript>().hp ).ToList().All( hp => hp <= 0);

        if (enemiesHPsCheck){
            toggleCombat();
            actionDisplayText.text = "First enemy defeated";
        }
        // This would house all the hp pool
        if (herosHPsCheck){
            toggleCombat();
            actionDisplayText.text = "Party all K.O. Combat ended.";
        }
    }

    // public void checkAllUnitStatus(){
    //     foreach (GameObject enemy in enemies){
    //         enemy.gameObject.GetComponent<UnitScript>().UpdateUnitStatus();
    //     }

    //     foreach (GameObject hero in heroes){
    //         hero.gameObject.GetComponent<UnitScript>().UpdateUnitStatus();
    //     }

    // }

    public void findTurnLength(){
        int enemiesAttacks = enemies.Select(enemy => enemy.gameObject.GetComponent<UnitScript>().attackTimes ).ToList().Aggregate(0, (total, next) => total + next);

        int herosAttacks= heroes.Select(hero => hero.gameObject.GetComponent<UnitScript>().attackTimes ).ToList().Aggregate(0, (total, next) => total + next);

        turnLength = enemiesAttacks + herosAttacks;

    }

    // This might be a huge fail by me, I'm going to google unity turn base rpg attack speed
    
    public void createAttackOrder(List<GameObject> heroes, List<GameObject> enemies){
        // Not sure if I should do this in one or two lines
        attackOrder = new List<UnitScript>();
        attackOrder = enemies.Select(enemy => enemy.gameObject.GetComponent<UnitScript>()).ToList();
        List<UnitScript> attacksHeros = heroes.Select(heroes => heroes.gameObject.GetComponent<UnitScript>()).ToList();
        attackOrder = attackOrder.Concat(attacksHeros).ToList();

        attackOrder = attackOrder.OrderBy( w=> w.speed).ToList();
        attackOrder.Reverse();

        List<UnitScript> extraAttacks = new List<UnitScript>();
        for (int i = 0; i < attackOrder.Count; i++){
            UnitScript currentUnit = attackOrder[i];
            if (currentUnit.attackTimes > 1){
                for (int j = 1; j < currentUnit.attackTimes; j++)
                {
                    int maxIdx = extraAttacks.Count;
                    int idx = Random.Range(0, maxIdx);
                    extraAttacks.Insert(idx, currentUnit);
                }
            }
        }
        attackOrder = attackOrder.Concat(extraAttacks).ToList(); 

        List<UnitScript> prior1Attacks = new List<UnitScript>();
        List<UnitScript> speed0Attacks = new List<UnitScript>();
        foreach( var unit in attackOrder){
            List<GameObject> opposition = unit.enemy ? heroes : enemies; 
            // I believe this will help the "priorty" bug that may happen
            // next move is on priorty but if it can't attack it will select the next move 
            // which will be sorted in the right speed. 
            MoveSO moveScript = unit.GetAttack(opposition, true); 
            if (moveScript.priorityLevel > 0){
                prior1Attacks.Add(unit);
            } else {
                speed0Attacks.Add(unit);
            }
        }

        attackOrder = prior1Attacks.Concat(speed0Attacks).ToList();

    }

    public void executeMove(UnitScript currentUnit, MoveSO moveScript){
        // Debug.Log("Running Attack for" + attackingUnit.title); 
        List<ActionSummary> effectedUnits = new List<ActionSummary>(); 
        List<int> enemySlots = GetSlots(moveScript.slot, moveScript.numOfTargets);
        List<int> heroSlots = GetSlots(moveScript.slot, moveScript.numOfTargets);

        if (moveScript.isDamage){ // Checking if the move is an attack; 
            if (!currentUnit.enemy){ // Hero attacking
                // List<int> enemySlots = GetSlots(moveScript.slot, moveScript.numOfTargets);
                // I would need a check to see if there is a unit to be hit so I don't get a out of range bug. 
                for (int i = 0; i < enemySlots.Count; i++){ 
                    UnitScript eScript = enemies[enemySlots[i]].gameObject.GetComponent<UnitScript>();
                    effectedUnits.Add(unitActionSummary(eScript, currentUnit, moveScript));
                }
                
            } else {
                for (int i = 0; i < heroSlots.Count; i++){
                    UnitScript eScript = heroes[heroSlots[i]].gameObject.GetComponent<UnitScript>();
                    effectedUnits.Add(unitActionSummary(eScript, currentUnit, moveScript));
                }
            }
        } else if (moveScript.isSelfBuff){
            // For self buffs I still need to add the current unit
            effectedUnits.Add(unitActionSummary(currentUnit, currentUnit, moveScript));
        } else { 

            // I would need to test None self buffs later but I'm writing the code now
            if (moveScript.partySingleOrAll){ // True = single, False = All
                if (!currentUnit.enemy){ // Hero action 
                    int randomUnitIdx = Random.Range(0,enemies.Count);
                    UnitScript eScript = enemies[heroSlots[randomUnitIdx]].gameObject.GetComponent<UnitScript>();
                    effectedUnits.Add(unitActionSummary(eScript, currentUnit, moveScript));
                } else {
                    int randomUnitIdx = Random.Range(0,heroes.Count);
                    UnitScript eScript = enemies[heroSlots[randomUnitIdx]].gameObject.GetComponent<UnitScript>();
                    effectedUnits.Add(unitActionSummary(eScript, currentUnit, moveScript));
                }
            } else {
                if (!currentUnit.enemy){ // Hero action 
                    for (int i = 0; i < heroes.Count; i++){ 
                        UnitScript eScript = heroes[i].gameObject.GetComponent<UnitScript>();
                        effectedUnits.Add(unitActionSummary(eScript, currentUnit, moveScript));
                    }
                } else {
                    for (int i = 0; i < enemies.Count; i++){ 
                        UnitScript eScript = enemies[i].gameObject.GetComponent<UnitScript>();
                        effectedUnits.Add(unitActionSummary(eScript, currentUnit, moveScript));
                    }
                }
            }
        }
        
        executeMoveSummaries( currentUnit,  effectedUnits, moveScript);
    }

    public void executeMoveSummaries(UnitScript currentUnit, List<ActionSummary> effectedUnitsSummary, MoveSO moveScript){

        actionDisplayText.text = "";
        string tempActionDisplayText = currentUnit.title + " used " + moveScript.title;;
        for (int i = 0; i< effectedUnitsSummary.Count; i++){
            ActionSummary currentUnitSum = effectedUnitsSummary[i];
            // Here I will need to add the "switch statemts for heals, buffs, debuffs etc
            // self buffs will also be seprate from buffs 
            if (currentUnitSum.isDamage){
                currentUnitSum.unit.changeHP(currentUnitSum.hpChange);
                tempActionDisplayText += actionDisplayText.text + ". Damaged " + currentUnitSum.unit.title + " for " + (Mathf.Abs(currentUnitSum.hpChange));
            }
            if (currentUnitSum.isDot){
                currentUnitSum.unit.addStatus(currentUnitSum.dotStatusScriptableObject);
                
            }
            if (currentUnitSum.isBuff){
                currentUnitSum.unit.addStatus(currentUnitSum.BuffScriptableObject);
                tempActionDisplayText +=  currentUnitSum.unit.title + "has applied " + actionDisplayText.text;
            }
        }



        actionDisplayText.text = tempActionDisplayText;
        currentUnit.actionNumber++;

    }

    public void executeUnitStatus (UnitScript unit){
        // Not sure if I should have a function do "2"ish things... 
        etcDisplayText.text = unit.executeStatus();


    }
    

    // I might/will have to update this for each slot + target combo.... 
    private List<int> GetSlots(int slot, int targets){
        List<int> returnList = new List<int>();
        if (slot <= 3 && targets == 1){
            returnList.Add(slot);
            return returnList;
        } else if (slot == 4 && targets == 1){
            returnList.Add(Random.Range(0,2)); // 1 or 2
            return returnList;
        } else if (slot == 4){
            returnList.Add(0);
            returnList.Add(1);
            return returnList;
        }
        return returnList;
    }


    
    public ActionSummary unitActionSummary(UnitScript unitBeingHit, UnitScript attackingUnit, MoveSO moveScript){
        
        GameObject gameObject = new GameObject("ActionSummary");
        ActionSummary summary = gameObject.AddComponent<ActionSummary>();
        summary.unit = unitBeingHit;


        if (moveScript.isDamage){
            int damage =  System.Convert.ToInt32(System.Math.Floor(attackingUnit.statAttack* moveScript.damageMod));
            summary.hpChange = -damage;
            summary.isDamage = true; 
        }

        if (moveScript.isDot){
            summary.isDot = true; 
            summary.dotStatusScriptableObject = moveScript.dotStatusScriptableObject;
        }

        if (moveScript.isSelfBuff){
            summary.isBuff = true;
            summary.BuffScriptableObject = moveScript.selfBuff;
        }


        // The actionSummary shouldn't need more than 3 secs 
        Destroy(gameObject, 3);
        return summary;
    }

 


    // I may not ever need this if each type of attack hit X space
    public void selectAttackTarget(){

    }


}
