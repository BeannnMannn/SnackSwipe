using UnityEngine;
using System.Collections;



// Note: script needs to be adjusted to be played on tablets


public class UniversalFood : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string likedAnimalTag;

    private bool isDragging = false;
    private Vector3 offset;
    private Collider2D currentAnimal; // Tracks if hovering over an animal
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private Sprite happySpriteKoala;
    [SerializeField] private Sprite happySpriteKangaroo;
    [SerializeField] private Sprite happySpritePlatapus;

    [SerializeField] private Sprite sadSpriteKoala;
    [SerializeField] private Sprite sadSpriteKangaroo;
    [SerializeField] private Sprite sadSpritePlatapus;

    [SerializeField] private ParticleSystem feedParticlePrefab;
    private Vector3 originalFoodScale;

    void Start()
    {
        originalFoodScale = transform.localScale;
    }

    //Offset exists so the foods center dosent snap to mouse
    void OnMouseDown()
    {
        offset = transform.position - GetMousePos();
        isDragging = true;
        AudioSource.PlayClipAtPoint(clickSound,transform.position);
        StartCoroutine(FoodClickPop());
    }

    // Have the food follow the mouse when dragging 
    void OnMouseDrag()
    {
        transform.position = GetMousePos() + offset;
    }



    void OnMouseUp()
    {
        isDragging = false;
        StartCoroutine(FoodShrinkPop());

        // If there is an animal within the food
        if (currentAnimal != null)
        {
            // if the animal has the same tag that we declare
            if (currentAnimal.CompareTag(likedAnimalTag))
            {
                SpriteRenderer animalRenderer = currentAnimal.GetComponent<SpriteRenderer>();

                if (currentAnimal.CompareTag("Kangaroo"))
                {
                    animalRenderer.sprite = happySpriteKangaroo;
                }
                else if (currentAnimal.CompareTag("Koala"))
                {
                    animalRenderer.sprite = happySpriteKoala;
                }
                else if (currentAnimal.CompareTag("Platapus"))
                {
                    animalRenderer.sprite = happySpritePlatapus;
                }

                currentAnimal.GetComponent<MonoBehaviour>().StartCoroutine(PopAnimation(currentAnimal.transform));

               
                ParticleSystem pfx = Instantiate(feedParticlePrefab, currentAnimal.transform.position, Quaternion.Euler(90, 0, 0));
                Debug.Log(currentAnimal.name + " ate the food and is happy :)");
                Destroy(gameObject);
                FoodSpawner.Instance.SpawnNextFood();
            }
            else
            {
              
                StartCoroutine(HandleSadAnimalRoutine());
            }
        }
    }

   
    IEnumerator HandleSadAnimalRoutine()
    {
        
        Debug.Log(currentAnimal.name + " DIDNT eat the food and is sad :(");
        StartCoroutine(ShakeNoAnimation(currentAnimal.transform));

        SpriteRenderer animalRenderer = currentAnimal.GetComponent<SpriteRenderer>();
        Sprite originalSprite = animalRenderer.sprite; 

        // Change to sad sprite
        if (currentAnimal.CompareTag("Kangaroo"))
        {
            animalRenderer.sprite = sadSpriteKangaroo;
        }
        else if (currentAnimal.CompareTag("Koala"))
        {
            animalRenderer.sprite = sadSpriteKoala;
        }
        else if (currentAnimal.CompareTag("Platapus"))
        {
            animalRenderer.sprite = sadSpritePlatapus;
        }

        
        yield return new WaitForSeconds(3f);

       
        animalRenderer.sprite = originalSprite;
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

    IEnumerator FoodClickPop()
    {
        Vector3 targetScale = originalFoodScale * 1.3f;
        float time = 0.1f; 
        float elapsed = 0;

        // Grow to 1.3x
        while (elapsed < time)
        {
            transform.localScale = Vector3.Lerp(originalFoodScale, targetScale, elapsed / time);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;
    }

    IEnumerator Wait(float x)
    {
       
        yield return new WaitForSeconds(x); 
        
     
    }

    IEnumerator FoodShrinkPop()
    {
        Vector3 currentScale = transform.localScale;
        float time = 0.1f;
        float elapsed = 0;

        while (elapsed < time)
        {
            transform.localScale = Vector3.Lerp(currentScale, originalFoodScale, elapsed / time);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = originalFoodScale;
    }
    IEnumerator PopAnimation(Transform target)
    {
        Vector3 startScale = target.localScale;
        Vector3 endScale = startScale * 1.5f;
        float time = 0.15f; // Duration of grow/shrink

        // Grow
        float elapsed = 0;
        while (elapsed < time)
        {
            if (target == null) yield break; // Safety check
            target.localScale = Vector3.Lerp(startScale, endScale, elapsed / time);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Shrink
        elapsed = 0;
        while (elapsed < time)
        {
            if (target == null) yield break;
            target.localScale = Vector3.Lerp(endScale, startScale, elapsed / time);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (target != null) target.localScale = startScale;
    }


    IEnumerator ShakeNoAnimation(Transform target)
    {

        
        
        Vector3 startPosition = target.localPosition;

        float duration = 0.35f; 
        float elapsed = 0f;

       
        float intensity = target.GetComponent<RectTransform>() != null ? 15f : 0.15f;
        float speed = 45f; 

        while (elapsed < duration)
        {
            if (target == null) yield break;

            float normalizedTime = elapsed / duration;

      
            float shakeFactor = Mathf.Sin(elapsed * speed);

          
            float fade = 1f - normalizedTime;

      
            float currentXOffset = shakeFactor * intensity * fade;
            target.localPosition = startPosition + new Vector3(currentXOffset, 0f, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

       
        if (target != null)
        {
            target.localPosition = startPosition;
        }
    }

    //Get mouse pos in the 2D space
    Vector3 GetMousePos()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }



}


