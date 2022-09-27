using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

    public AudioSource audio;

    public void Resume() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        audio.UnPause();
    }

    public void transitionHome() {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) {
            pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
        }
        if (pauseMenu.activeInHierarchy) {
            Time.timeScale = 0f;
            audio.Pause();

        } else {
            Time.timeScale = 1f;
            audio.UnPause();
        }
    }
}
