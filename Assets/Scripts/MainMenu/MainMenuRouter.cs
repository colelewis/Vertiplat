using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuRouter : MonoBehaviour
{
    public GameObject MainMenu;

    // Start is called before the first frame update
    void Start()
    {
        mainMenu();
    }

    public void PlayButton() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void mainMenu() {
        MainMenu.SetActive(true);
    }

    public void QuitButton() {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
