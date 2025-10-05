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

        // Reset game state before starting a new game
        Debug.Log("Starting new game - Destroying old GameManager for fresh start...");

        // Destroy existing GameManager to ensure completely fresh start
        if (GameManager.Instance != null)
        {
            Debug.Log("Destroying existing GameManager instance");
            Destroy(GameManager.Instance.gameObject);
            GameManager.Instance = null;
        }

        // Reset Paddle static variables
        Paddle.ResetStaticVariables();

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
