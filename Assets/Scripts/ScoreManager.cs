using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameManager GM;
    public GameObject ScoreText;
    public GameObject gameOverMenu;

    private int score;
    private bool HardMode;

    void Start()
    {
        score = 0;
        GM = FindObjectOfType<GameManager>();
        HardMode = GM.Hardmode;
        gameOverMenu.transform.Find("FinalScoreText").GetComponent<TextMeshProUGUI>().text = "Score: " + score.ToString();
        gameOverMenu.transform.Find("HighScoreText").GetComponent<TextMeshProUGUI>().text = "High Score: " + GM.HighScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        int HardModeMult = 1;
        if (HardMode)
            HardModeMult = 2;
        int currScore = Mathf.RoundToInt(transform.position.y*10) * HardModeMult;
        if(currScore>score)
        {
            score = currScore;
            ScoreText.GetComponent<TextMeshProUGUI>().text = score.ToString();
            gameOverMenu.transform.Find("FinalScoreText").GetComponent<TextMeshProUGUI>().text = "Score: " + score.ToString();
            if(GM != null && score>GM.HighScore)
            {
                GM.HighScore = score;
            }

            gameOverMenu.transform.Find("HighScoreText").GetComponent<TextMeshProUGUI>().text = "High Score: " + GM.HighScore.ToString();
        }
    }
}
