using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject ScoreText;
    public GameObject gameOverMenu;

    private int score;

    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        int currScore = Mathf.RoundToInt(transform.position.y*10);
        if(currScore>score)
        {
            score = currScore;
            ScoreText.GetComponent<TextMeshProUGUI>().text = score.ToString();
            gameOverMenu.transform.Find("FinalScoreText").GetComponent<TextMeshProUGUI>().text = "Score: " + score.ToString();
        }
    }
}
