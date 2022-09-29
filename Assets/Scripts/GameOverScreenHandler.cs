using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreenHandler : MonoBehaviour
{

    public GameObject player;
    public GameObject gameOverMenu;
    public GameObject KnockbackOverlay;
    public AudioSource audio;
    

    public void Quit() {
        Application.Quit();
    }

    public void PlayAgain() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerInCameraPosition = UnityEngine.Camera.main.WorldToViewportPoint(player.transform.position);
        if (playerInCameraPosition.y < -0.08f) {
            Time.timeScale = 0f;
            gameOverMenu.SetActive(true);
            KnockbackOverlay.SetActive(false);
            audio.Pause();

        } else {
            Time.timeScale = 1f;
            gameOverMenu.SetActive(false);
        }
        
    }
}
