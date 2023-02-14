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


    // Battle Settings 
    public int turn; 
    public int turnLength; 

    // public GameObject hero1;
    // public GameObject hero2;
    // public GameObject hero3;
    // public GameObject hero4;
    
    public List<GameObject> heroes;

    // public GameObject enemy1; 

    public List<GameObject> enemies;

    public List<UnitScript> attackOrder; 

    // Game UI and related
    public TextMeshProUGUI actionCounter;

    public GameObject startCombatButton;

    public TextMeshProUGUI actionDisplayText; 

    // Start is called before the first frame update
    void Start()
    {
        turn = 1;

        findTurnLength(); 

        createAttackOrder(heroes, enemies);
    }

    // Update is called once per frame
    void Update()
    { if (battleLive){
        startCombatButton.SetActive (false);

        checkAllUnitHPs();

        period += Time.deltaTime;
 
        // An action step 
        if (period >= turnSpeed) {
            period = period - turnSpeed;

            UnitScript currentUnit = attackOrder[counter % attackOrder.Count];
            // int damage = currentUnit.BasicAttack();

            List<GameObject> opposition = currentUnit.enemy ? heroes : enemies; 
            
            MoveSO moveScript = currentUnit.GetAttack(opposition); 
            executeMove(currentUnit, moveScript);

            counter++; 
            actionCounter.text = "Action Number " + counter;
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

    }

    public void executeMove(UnitScript attackingUnit, MoveSO moveScript){
        Debug.Log("Running Attack for" + attackingUnit.title); 
        if (!attackingUnit.enemy){ // Hero attacking
            List<int> enemySlots = GetEnemySlots(moveScript.slot, moveScript.numOfTargets);
            // I would need a check to see if there is a unit to be hit so I don't get a out of range bug. 
                for (int i = 0; i < enemySlots.Count; i++){
                    Debug.Log(enemySlots[i]); 
                    UnitScript eScript = enemies[enemySlots[i]].gameObject.GetComponent<UnitScript>();
                    damageUnit(eScript, attackingUnit, moveScript);
                }
            
        } else {
            List<int> heroSlots = GetHeroSlots(moveScript.slot, moveScript.numOfTargets);
                for (int i =0; i < heroSlots.Count; i++){
                    Debug.Log(heroSlots[i]); 
                    UnitScript eScript = heroes[heroSlots[i]].gameObject.GetComponent<UnitScript>();
                    damageUnit(eScript, attackingUnit, moveScript);
                }
        }

    }

    // Not sure if it's the best to keep these two the same (GetEnemySlots && GetHeroSlots) or seperate 
    private List<int> GetEnemySlots(int slot, int targets){
        List<int> returnList = new List<int>();
        if (slot <= 3 && targets == 1){
            returnList.Add(slot);
            return returnList;
        } else if (slot == 4 && targets == 1){
            returnList.Add(Random.Range(0,2)); // 1 or 2
            return returnList;
        }
        return returnList;
    }

    private List<int> GetHeroSlots(int slot, int targets){
        List<int> returnList = new List<int>();
        if (slot <= 3 && targets == 1){
            returnList.Add(slot);
            return returnList;
        } else if (slot == 4 && targets == 1){
            returnList.Add(Random.Range(0,2)); // 1 or 2
            return returnList;
        }
        return returnList;
    }


    public void damageUnit(UnitScript unitBeingHit, UnitScript attackingUnit, MoveSO moveScript){
        //UnitScript unitScript = unit.gameObject.GetComponent<UnitScript>();

        int damage =  System.Convert.ToInt32(System.Math.Floor(attackingUnit.statAttack* moveScript.damageMod));
        unitBeingHit.TakeDamage(damage);

        // action Display will have to move away from here when I get multi target hits runnig. 
        actionDisplayText.text = 
            attackingUnit.title + " has damaged " + unitBeingHit.title + " for " + damage 
            + " with " + moveScript.title;

        Debug.Log( attackingUnit.title + " has damaged " + unitBeingHit.title + " for " + damage);
    }

    public void damageUnitOld(GameObject unit, UnitScript attackingUnit, MoveSO moveScript){
        UnitScript unitBeingHit = unit.gameObject.GetComponent<UnitScript>();

        int damage =  System.Convert.ToInt32(System.Math.Floor(attackingUnit.statAttack* moveScript.damageMod));
        unitBeingHit.TakeDamage(damage);

        // action Display will have to move away from here when I get multi target hits runnig. 
        actionDisplayText.text = 
            attackingUnit.title + " has damaged " + unitBeingHit.title + " for " + damage 
            + " with " + moveScript.title;

        Debug.Log( attackingUnit.title + " has damaged " + unitBeingHit.title + " for " + damage);
    }



    // I may not ever need this if each type of attack hit X space
    public void selectAttackTarget(){

    }


}
