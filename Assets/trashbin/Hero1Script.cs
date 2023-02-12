using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Hero1Script : MonoBehaviour
{

    public int hp = 10;
    public int speed = 4;
    public int attackTimes = 1; 


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
}
