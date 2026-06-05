using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FoodSpawner : MonoBehaviour
{
    public GameObject[] foodPrefabs; // Array to drag the three foods into
    public Transform spawnPoint;    // Where the food should appear

    public static FoodSpawner Instance;

    public Animator animator;

    private int index = 0; 

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SpawnNextFood();
    }

    public void SpawnNextFood()
    {

        if (index >= 3)
        {

            StartCoroutine(LoadSceneSequence());
        }


        Instantiate(foodPrefabs[index], spawnPoint.position, Quaternion.identity);

        index++;
        Debug.Log(index);

       
    }


    private IEnumerator LoadSceneSequence()
    {
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("EndScene");
    }
}