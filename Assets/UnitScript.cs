using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UnitScript : MonoBehaviour
{

    public string title;
    public int hp;
    public int speed;
    public int attackTimes; 

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
