using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        SceneManager.LoadScene("StartScreen");
    }
}
