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
        string sceneName = "SceneSpritesWithAudio";

        try
        {
            Debug.Log($"Attempting to load scene: {sceneName}");
            SceneManager.LoadScene(sceneName);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error loading scene '{sceneName}': {e.Message}");
            Debug.Log($"Trying to load alternative scene: GameScene");

            try
            {
                SceneManager.LoadScene("GameScene");
            }
            catch (System.Exception e2)
            {
                Debug.LogError($"Critical error: Cannot load any scene. {e2.Message}");
            }
        }
    }

    public void Exit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
