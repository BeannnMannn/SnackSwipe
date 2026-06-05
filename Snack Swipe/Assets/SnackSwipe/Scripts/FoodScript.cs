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

    [Header("Happy Sprites")]
    [SerializeField] private Sprite happySpriteKoala;
    [SerializeField] private Sprite happySpriteKangaroo;
    [SerializeField] private Sprite happySpritePlatapus;

    [Header("Sad Sprites")]
    [SerializeField] private Sprite sadSpriteKoala;
    [SerializeField] private Sprite sadSpriteKangaroo;
    [SerializeField] private Sprite sadSpritePlatapus;

    [SerializeField] private ParticleSystem feedParticlePrefab;
    private Vector3 originalFoodScale;

    void Start()
    {
        originalFoodScale = transform.localScale;
    }

    void OnMouseDown()
    {
        offset = transform.position - GetMousePos();
        isDragging = true;
        AudioSource.PlayClipAtPoint(clickSound, transform.position);
        StartCoroutine(FoodClickPop());
    }

    void OnMouseDrag()
    {
        transform.position = GetMousePos() + offset;
    }

    void OnMouseUp()
    {
        isDragging = false;
        StartCoroutine(FoodShrinkPop());

        if (currentAnimal != null)
        {
            if (currentAnimal.CompareTag(likedAnimalTag))
            {
                SpriteRenderer animalRenderer = currentAnimal.GetComponent<SpriteRenderer>();

                if (currentAnimal.CompareTag("Kangaroo")) animalRenderer.sprite = happySpriteKangaroo;
                else if (currentAnimal.CompareTag("Koala")) animalRenderer.sprite = happySpriteKoala;
                else if (currentAnimal.CompareTag("Platapus")) animalRenderer.sprite = happySpritePlatapus;

                // Start the pop animation safely on the FoodSpawner so it doesn't get cut off by Destroy()
                FoodSpawner.Instance.StartCoroutine(PopAnimation(currentAnimal.transform));

                Instantiate(feedParticlePrefab, currentAnimal.transform.position, Quaternion.Euler(90, 0, 0));
                Debug.Log(currentAnimal.name + " ate the food and is happy :)");

                Destroy(gameObject);
                FoodSpawner.Instance.SpawnNextFood();
            }
            else
            {
                // FIX: We pass the specific animal target to the FoodSpawner instance to process. 
                // This means even if THIS food object is moved or destroyed, the timer still runs!
                FoodSpawner.Instance.StartCoroutine(HandleSadAnimalRoutine(currentAnimal));
            }
        }
    }

    // FIX: Changed to accept a specific Collider2D target parameter so dragging away doesn't break it
    IEnumerator HandleSadAnimalRoutine(Collider2D animal)
    {
        if (animal == null) yield break;

        Debug.Log(animal.name + " DIDNT eat the food and is sad :(");
        FoodSpawner.Instance.StartCoroutine(ShakeNoAnimation(animal.transform));

        SpriteRenderer animalRenderer = animal.GetComponent<SpriteRenderer>();
        Sprite originalSprite = animalRenderer.sprite;

        if (animal.CompareTag("Kangaroo")) animalRenderer.sprite = sadSpriteKangaroo;
        else if (animal.CompareTag("Koala")) animalRenderer.sprite = sadSpriteKoala;
        else if (animal.CompareTag("Platapus")) animalRenderer.sprite = sadSpritePlatapus;

        yield return new WaitForSeconds(3f);

        // Safely check if the animal still exists before resetting its sprite
        if (animalRenderer != null)
        {
            animalRenderer.sprite = originalSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        currentAnimal = other;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // FIX: Only clear currentAnimal if it's the one we are actually leaving
        if (currentAnimal == other)
        {
            currentAnimal = null;
        }
    }

    IEnumerator FoodClickPop()
    {
        Vector3 targetScale = originalFoodScale * 1.3f;
        float time = 0.1f;
        float elapsed = 0;

        while (elapsed < time)
        {
            transform.localScale = Vector3.Lerp(originalFoodScale, targetScale, elapsed / time);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;
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
        if (target == null) yield break;
        Vector3 startScale = target.localScale;
        Vector3 endScale = startScale * 1.5f;
        float time = 0.15f;

        float elapsed = 0;
        while (elapsed < time)
        {
            if (target == null) yield break;
            target.localScale = Vector3.Lerp(startScale, endScale, elapsed / time);
            elapsed += Time.deltaTime;
            yield return null;
        }

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
        if (target == null) yield break;
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

        if (target != null) target.localPosition = startPosition;
    }

    Vector3 GetMousePos()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }
}