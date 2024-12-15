using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    void Awake()
    {
        Time.timeScale = 1f;
    }
    
    public void StartNextGame()
    {
        
        int gameStartIndex = 1;

        
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(gameStartIndex);
        }
        else
        {
        
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;

            if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
            {
                nextSceneIndex = 0; 
            }

            SceneManager.LoadScene(nextSceneIndex);
        }
    }

  
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // Derlenmiş oyunu kapat
#endif
    }
}
