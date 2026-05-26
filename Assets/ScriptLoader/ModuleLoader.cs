using UnityEngine;
using UnityEngine.SceneManagement; // This is required for loading scenes

public class ModuleLoader : MonoBehaviour
{
    // This function will be called when a button is clicked
    public void LoadModuleScene(string sceneName)
    {
        // Checks if the scene name is not empty before trying to load
        if (!string.IsNullOrEmpty(sceneName))
        {
            Debug.Log("Loading Scene: " + sceneName);
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Scene name is empty! Please assign a name in the button inspector.");
        }
    }

    // Optional: A function to easily go back to the Main Menu
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Make sure this matches your home scene name exactly
    }
}