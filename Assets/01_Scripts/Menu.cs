using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public static Menu instance;

    private void Awake()
    {
        instance = this;
    }

    public void Play()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Exit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
