using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject[] foodPrefabs; // array to drag the three foods into
    public Transform spawnPoint;    // Where the food should appear

    public static FoodSpawner Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SpawnRandomFood();
    }

    public void SpawnRandomFood()
    {
        // Pick a random Food from the array
        int randomIndex = Random.Range(0, foodPrefabs.Length);

        // Create the food at the spawn points position
        Instantiate(foodPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);
    }
}