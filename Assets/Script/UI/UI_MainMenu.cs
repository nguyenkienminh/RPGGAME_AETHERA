using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene1";
    [SerializeField] private GameObject continueButton;
    [SerializeField] UI_FadeScreen fadeScreen;
    [SerializeField] private AudioSource bgMusic; // Thêm AudioSource cho nhạc nền

    private void Start()
    {
        //if (!SaveManager.instance.HasSavedData())
        //{
        //    continueButton.SetActive(false);
        //}
        if (bgMusic != null && !bgMusic.isPlaying)
        {
            bgMusic.Play();
        }
    }


    public void ContinueGame()
    {
        StartCoroutine(LoadSceneWithFadeEffect(1.75f));
   }

    public void NewGame()
    {
        SaveManager.instance.DeleteSaveData();
        StartCoroutine(LoadSceneWithFadeEffect(1.75f));
    } 
    public void ExitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
    public void GuideGameButton()
    {
        StartCoroutine(GuideGame());
    }
    public IEnumerator GuideGame()
    {
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(1.75f);

        SceneManager.LoadScene("GuideGame");
    }
    IEnumerator LoadSceneWithFadeEffect(float _delay)
    {
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(_delay);

        SceneManager.LoadScene(sceneName);
    }
}
