using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Out : MonoBehaviour
{

    public GameObject ovrtUI;
    public GameObject goUI;

    private void Start()
    {
        ovrtUI.SetActive(false);
        Time.timeScale = 0;
    }
    public void GameOver()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Time.timeScale = 0;
            ovrtUI.SetActive(true);
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        goUI.SetActive(false);   
    }
}
