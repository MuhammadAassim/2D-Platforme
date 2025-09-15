using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneShift : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("Level 01");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
