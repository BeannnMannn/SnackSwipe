using UnityEngine;




// Note: script needs to be adjusted to be played on tablets


public class UniversalFood : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string likedAnimalTag;

    private bool isDragging = false;
    private Vector3 offset;
    private Collider2D currentAnimal; // Tracks if we are hovering over an animal


    //Offset exists so the foods center dosent snap to mouse
    void OnMouseDown()
    {
        offset = transform.position - GetMousePos();
        isDragging = true;
    }

    // Have the food follow the mouse when dragging 
    void OnMouseDrag()
    {
        transform.position = GetMousePos() + offset;
    }

    void OnMouseUp()
    {
        isDragging = false;

        // If there is an animal within the food
        if (currentAnimal != null)
        {
            // if the animal has the same tag that we declare
            if (currentAnimal.CompareTag(likedAnimalTag))
            {
                // Print that the animal is happy destroy the food and spawn anotherfood
                Debug.Log(currentAnimal.name + " ate the food and is happy :)");
                Destroy(gameObject);
                FoodSpawner.Instance.SpawnRandomFood();
            }
            else
            {
                //Print the animal is sad and return the food to the middle of the screen 
                Debug.Log(currentAnimal.name + " DIDNT ate the food and is sad :(");

            }
        }
    }

    // When the foods Box collider enters the animals box collider, set "currentAnimal" to other
    private void OnTriggerEnter2D(Collider2D other)
    {
        currentAnimal = other;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        currentAnimal = null;
    }

    //Get mouse pos in the 2D space
    Vector3 GetMousePos()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }
}