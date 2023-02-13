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

    public int round; 

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
        round = 1;
        // Maybe I can feed in only the unitScripts here
        // createAttackOrder(hero1, hero2, Enemy1);
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
        }
        

        }
    }
        

    public void toggleCombat(){
        battleLive = !battleLive; 
    }   

    // checks to see if either all hero or enemy unit hp total is less than 0
    // This would need a update 
    public void checkAllUnitHPs(){

        bool enemiesHPsCheck = enemies.Select(hero => hero.gameObject.GetComponent<UnitScript>().hp ).ToList().All( hp => hp <= 0);

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

    // This might be a huge fail by me, I'm going to google unity turn base rpg attack speed
    
    public void createAttackOrder(List<GameObject> heroes, List<GameObject> enemies){
        
        // I will need to learn how to make a proper unity c# loop 
        UnitScript h1Script = heroes[0].gameObject.GetComponent<UnitScript>();
        // UnitScript h2Script = hero2.gameObject.GetComponent<UnitScript>();
        UnitScript e1Script = enemies[0].gameObject.GetComponent<UnitScript>();
        // Debug.Log(h1Script, h2Script);
        attackOrder.Add(h1Script);
        // attackOrder.Add(h2Script);
        attackOrder.Add(e1Script);

        attackOrder = attackOrder.OrderBy( w=> w.speed).ToList();

        if (e1Script.attackTimes > 1){
            for (int i = 1; i < e1Script.attackTimes; i++)
            {
                // I need to figure out how to make an object copy and not a ref... or maybe I don't need to because I can use .Insert
                // e1Script.speed = Random.Range(0,e1Script.speed );
                // UnitScript e1ScriptX = (UnitScript)e1Script.MemberwiseClone();
                int maxIdx = attackOrder.IndexOf(e1Script);
                int idx = Random.Range(0, maxIdx);
                attackOrder.Insert(idx, e1Script);
            }
        }
        attackOrder.Reverse();

        for (int i =0; i< attackOrder.Count; i++){
            Debug.Log(attackOrder[i].title +" " + attackOrder[i].speed.ToString());
        }

        // Debug.Log(attackOrder);
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
