using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    
    public Animator animator;

   

    public void OnStartCLick()
    {
        StartCoroutine(LoadScene());
        Debug.Log("Scene changed to main");
    }

    IEnumerator LoadScene()
    {
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("MainScene");

    }

}
