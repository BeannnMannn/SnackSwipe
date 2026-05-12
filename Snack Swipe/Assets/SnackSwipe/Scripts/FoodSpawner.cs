using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject[] foodPrefabs; // array to drag the three foods into
    public Transform spawnPoint;    // Where the food should appear

    public static FoodSpawner Instance;

    private int Index = 0;

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
        // Pick a random Food from the array


        // Create the food at the spawn points position
        Instantiate(foodPrefabs[Index], spawnPoint.position, Quaternion.identity);

        Index++;
    }
}