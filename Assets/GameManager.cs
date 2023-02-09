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

    public GameObject Hero1;
    public GameObject Hero2;
    public GameObject Hero3;
    public GameObject Hero4;
    
    public GameObject Enemy1; 

    public List<UnitScript> attackOrder; 

    // Game UI and related
    public TextMeshProUGUI ActionCounter;

    public GameObject startCombatButton;

    // Start is called before the first frame update
    void Start()
    {
        round = 1;
        // Maybe I can feed in only the unitScripts here
        createAttackOrder(Hero1, Hero2, Enemy1);
    }

    // Update is called once per frame
    void Update()
    { if (battleLive){
        startCombatButton.SetActive (false);

        period += Time.deltaTime;
 
        if (period >= turnSpeed) {
            period = period - turnSpeed;

            // execute block of code here
            counter++; 
            ActionCounter.text = "Action Number " + counter;
        }

        }
    }
        

    public void toggleCombat(){
        battleLive = !battleLive; 
    }   


    // This might be a huge fail by me, I'm going to google unity turn base rpg attack speed
    
    public void createAttackOrder(GameObject Hero1, GameObject Hero2, GameObject Enemy1){

        UnitScript h1Script = Hero1.gameObject.GetComponent<UnitScript>();
        UnitScript h2Script = Hero2.gameObject.GetComponent<UnitScript>();
        UnitScript e1Script = Enemy1.gameObject.GetComponent<UnitScript>();
        // Debug.Log(h1Script, h2Script);
        attackOrder.Add(h1Script);
        attackOrder.Add(h2Script);
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

}
