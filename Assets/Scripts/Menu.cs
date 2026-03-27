using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject UIGame;
    // Start is called before the first frame update
    public void StartGame()
    {
        mainMenu.SetActive(false);
        UIGame.SetActive(true);
    }

    // Update is called once per frame
    void Update(){
    }
}
