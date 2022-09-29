using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreenHandler : MonoBehaviour
{

    public GameObject player;
    public GameObject gameOverMenu;
    

    public void Quit() {
        gameOverMenu.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void PlayAgain() {
        gameOverMenu.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    // Start is called before the first frame update
    void Start()
    {
        if (gameOverMenu.activeInHierarchy) {
            Time.timeScale = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
