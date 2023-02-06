using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{

    [SerializeField] private string startGame = "MainScene";

    // Start is called before the first frame update
    void Start(){
    }

    public void QuitGame(){
        Application.Quit();
        Debug.Log("Quit Button Pushed");
    }

    public void StartGame(){
        SceneManager.LoadScene(startGame); 
    }

    public void ClearData(){
        PlayerPrefs.DeleteAll();
    }
}
