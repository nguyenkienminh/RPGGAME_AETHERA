using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Intro_UI : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip backgroundMusic;

    public GameObject[] introImages;
    public float[] displayDurations;
    [SerializeField] private float delayBeforeStart = 5f;

    [SerializeField] private GameObject startButton;
    [SerializeField] private float fadeDuration = 1f;
    private bool isSkipped = false;
    void Start()
    {
        if (introImages.Length != displayDurations.Length)
        {
            Debug.LogError("Số lượng hình ảnh và thời gian hiển thị không khớp!");
            return;
        }

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = backgroundMusic;
        audioSource.loop = false;
        audioSource.playOnAwake = false;
        audioSource.volume = 1f;
        audioSource.Play();

        foreach (GameObject img in introImages)
        {
            img.SetActive(false);
        }




        StartCoroutine(ShowIntroImages(delayBeforeStart));
    }

    IEnumerator ShowIntroImages(float second)
    {
        yield return new WaitForSeconds(second);

        for (int i = 0; i < introImages.Length; i++)
        {
            GameObject currentImage = introImages[i];

            yield return StartCoroutine(FadeImage(currentImage, 0f, 1f, fadeDuration));

            yield return new WaitForSeconds(displayDurations[i]);

            if (i < introImages.Length - 1)
            {
                yield return StartCoroutine(FadeImage(currentImage, 1f, 0f, fadeDuration));
                currentImage.SetActive(false);
            }
            else
            {
                startButton.SetActive(true);
            }


        }

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    IEnumerator FadeImage(GameObject imageObj, float startAlpha, float endAlpha, float duration)
    {
        imageObj.SetActive(true);
        Image img = imageObj.GetComponent<Image>();

        if (img == null)
        {
            yield break;
        }

        float elapsedTime = 0f;
        float speedFactor = 5f;
        Color color = img.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime * speedFactor;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            img.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        img.color = new Color(color.r, color.g, color.b, endAlpha);
    }

    public void SkipAudio()
    {
        if (isSkipped) return;

        isSkipped = true;
        StopAllCoroutines(); 

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        foreach (GameObject img in introImages)
        {
            img.SetActive(false);
        }

        GameObject lastImage = introImages[introImages.Length - 1];
        lastImage.SetActive(true);

        startButton.SetActive(true);
    }
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
