using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Manager;
    public bool Hardmode = false;
    public int HighScore;
    public Toggle HardModeButton;

    public void Awake()
    {
        if(Manager != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Manager = this;
    }

    public void HardModeToggle(bool state)
    {
        Hardmode = state;
    }    

    // Start is called before the first frame update
    void Start()
    {
        Hardmode = false;
        HighScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
