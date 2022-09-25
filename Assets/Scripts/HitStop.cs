using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    private bool Paused = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause(float duration)
    {
        if(!Paused)
        {
            Time.timeScale = 0;
            StartCoroutine(Wait(duration));
        }
        

    }

    IEnumerator Wait(float duration)
    {
        Paused = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
        Paused = false;
    }
}
