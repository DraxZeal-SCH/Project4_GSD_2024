using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public void RestartButton()
    {
        SceneManager.LoadScene("MainScene"); // Replace "MainScene" with the name of your main game scene
    }
}
