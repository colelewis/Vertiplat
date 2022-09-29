using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool IsPaused;

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
        IsPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) {
            pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
        }
        if (pauseMenu.activeInHierarchy) {
            IsPaused = true;
            Time.timeScale = 0f;
            audio.Pause();

        } else {
            if(IsPaused == true && !pauseMenu.activeInHierarchy)
            {
                Time.timeScale = 1f;
                audio.UnPause();
                IsPaused = false;
            }
        }
    }
}
