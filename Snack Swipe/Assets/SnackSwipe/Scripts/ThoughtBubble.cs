using UnityEngine;
using System.Collections; 

public class ThoughtBubble : MonoBehaviour
{
    public SpriteRenderer bubbleSprite;
    public float delay = 2f;
    public float fadeDuration = 1f;

    void Start()
    {
        StartCoroutine(ShowBubble());
    }

    IEnumerator ShowBubble()
    {
        yield return new WaitForSeconds(delay);
        bubbleSprite.gameObject.SetActive(true);
        StartCoroutine(Fade(bubbleSprite, 0f, 1f, fadeDuration));
    }

    IEnumerator Fade(SpriteRenderer sr, float start, float end, float duration)
    {
        float t = 0f;
        Color c = sr.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(start, end, t / duration);
            sr.color = c;
            yield return null;
        }
    }
}




