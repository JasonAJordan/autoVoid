using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
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

    public List<GameObject> attackOrder; 

    // Game UI and related
    public TextMeshProUGUI ActionCounter;

    public GameObject startCombatButton;

    // Start is called before the first frame update
    void Start()
    {
        round = 1;
        createAttackOrder(Hero1, Enemy1);
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
    public void createAttackOrder(GameObject Hero1, GameObject Enemy1){
        // I would need to figure out a way how not to hard code "e1Script" 
        e1script e1Script = Enemy1.gameObject.GetComponent<e1script>();
        if (e1Script.attackTimes > 1){
            for (int i = 1; i < e1Script.attackTimes; i++)
            {
                // need to figure out how to change attack speed for instances of more than 1
                attackOrder.Add(Enemy1);
            }
        }
        attackOrder.Add(Enemy1);
        attackOrder.Add(Hero1);

    }

}
