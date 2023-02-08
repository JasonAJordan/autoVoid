using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{
    public bool battleLive; 
    public float turnSpeed = 100.0f;
    public float period = 0.1f;

    public int counter = 0;

    public TextMeshProUGUI ActionCounter;

    public GameObject startCombatButton;

    // Start is called before the first frame update
    void Start()
    {
        ///ActionCounter = FindObjectOfType<TextMeshProUGUI>();
        // <"Action Number " + counter;
        // ActionCounter.text = "test";
    }

    // Update is called once per frame
    void Update()
    { if (battleLive){
        startCombatButton.SetActive (false);

        if (Time.time > turnSpeed ) {
            turnSpeed += period;
            // execute block of code here
            counter++; 
            ActionCounter.text = "Action Number " + counter;
        }
        }
    }
        

    public void toggleCombat(){
        battleLive = !battleLive; 
    }   

}
