using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GMCommunicator : MonoBehaviour
{
    private GameManager GM;
    private void Awake()
    {
        GM = FindObjectOfType<GameManager>();
        FindObjectOfType<Toggle>().isOn = GM.Hardmode;
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
