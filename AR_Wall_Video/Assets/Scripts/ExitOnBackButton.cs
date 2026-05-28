using UnityEngine;

public class ExitOnBackButton : MonoBehaviour
{
    void Update()
    {
        // Runs every frame and checks for the Android back button
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Quit the application
            Application.Quit();

            // For safety in the Editor (optional): stop play mode
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}