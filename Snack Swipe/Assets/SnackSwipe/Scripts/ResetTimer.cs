using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetTimer : MonoBehaviour
{
    public float idleDuration = 30f;
    private float timer;

    void Start()
    {
        timer = idleDuration; 
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            timer = idleDuration;
        }  
        
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SceneManager.LoadScene("StartScreen");
        }
    }
    
}
