using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GMCommunicator : MonoBehaviour
{
    public GameObject HighScoreLabel;

    private GameManager GM;
    private int HighScore;
    private void Awake()
    {
        GM = FindObjectOfType<GameManager>();
        FindObjectOfType<Toggle>().isOn = GM.Hardmode;
        HighScore = GM.HighScore;
        if(HighScore>0)
        {

            HighScoreLabel.GetComponent<TextMeshProUGUI>().text = "High Score: " + HighScore;
            HighScoreLabel.SetActive(true);
        }
    }

    public void HardModeToggle(bool state)
    {
        GM.Hardmode = state;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
