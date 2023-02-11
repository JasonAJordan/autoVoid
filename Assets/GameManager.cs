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

    public GameObject hero1;
    public GameObject hero2;
    public GameObject hero3;
    public GameObject hero4;
    
    public GameObject enemy1; 

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
        createAttackOrder(hero1, enemy1);
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
            
            // I would proby just replace this whole thing with passing the move script down so
            // I can get other information like crit, status, etc. 
            (int slot, int damage, string moveName) = currentUnit.GetAttack(); 

            if (currentUnit.enemy){
                damageUnit(hero1, damage, currentUnit, moveName);
            } else {
                damageUnit(enemy1, damage, currentUnit, moveName);
            }

            counter++; 
            actionCounter.text = "Action Number " + counter;
        }
        

        }
    }
        

    public void toggleCombat(){
        battleLive = !battleLive; 
    }   

    // checks to see if either all hero or enemy unit hp total is less than 0
    public void checkAllUnitHPs(){
        UnitScript h1Script = hero1.gameObject.GetComponent<UnitScript>();
        UnitScript e1Script = enemy1.gameObject.GetComponent<UnitScript>();

        if (e1Script.hp <=0){
            toggleCombat();
            actionDisplayText.text = "First enemy defeated";
        }
        // This would house all the hp pool
        if ((h1Script.hp <= 0) ){
            toggleCombat();
            actionDisplayText.text = "Party all K.O. Combat ended.";
        }
    }

    // This might be a huge fail by me, I'm going to google unity turn base rpg attack speed
    
    public void createAttackOrder(GameObject hero1, GameObject enemy1){
        //  GameObject hero2

        UnitScript h1Script = hero1.gameObject.GetComponent<UnitScript>();
        // UnitScript h2Script = hero2.gameObject.GetComponent<UnitScript>();
        UnitScript e1Script = enemy1.gameObject.GetComponent<UnitScript>();
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

    public void damageUnit(GameObject unit, int damage, UnitScript attackingUnit, string moveName){
        UnitScript unitScript = unit.gameObject.GetComponent<UnitScript>();
        // unitScript.hp = unitScript.hp - damage;
        unitScript.TakeDamage(damage);

        Debug.Log( attackingUnit.title + " has damaged " + unitScript.title + " for " + damage);
        actionDisplayText.text = 
            attackingUnit.title + " has damaged " + unitScript.title + " for " + damage 
            + " with " + moveName;
                                ///+ ". HP "+  unitScript.hp;
    }



    // I may not ever need this if each type of attack hit X space
    public void selectAttackTarget(){

    }


}
