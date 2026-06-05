using Unity.VisualScripting;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetTimer : MonoBehaviour
{
    public float idleDuration = 300f;
    private float timer;
    public string currentScene = SceneManager.GetActiveScene().name;
    public Animator animator;

    void Start()
    {

        timer = idleDuration;

        if (currentScene == "EndScene")
        {

            StartCoroutine(LoadScene());

        }

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
            StartCoroutine(LoadScene2());
        }



    }
    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(10f);

        animator.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("StartScreen");
    }

    private IEnumerator LoadScene2()
    {
        

        animator.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("StartScreen");
    }



}
