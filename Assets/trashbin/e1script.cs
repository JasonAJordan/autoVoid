using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class e1script : MonoBehaviour
{

    public int hp = 60;
    
    public int speed = 6;

    public int attackTimes = 2; 

    public TextMeshProUGUI hpText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hpText.text = "" + hp;
    }

    public int combat(){
        return 1;
    }
}
