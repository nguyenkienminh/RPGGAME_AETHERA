using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UI_Guide : MonoBehaviour
{
    [SerializeField] private string sceneName;
   public void LoadSceneNextGuide()
    {
        SceneManager.LoadScene(sceneName);
    }
    public void BackToSceneMenu()
    {
        if (!SaveManager.instance.HasSavedData())
        {
            SceneManager.LoadScene("MainMenu1");
        }
        else if (SaveManager.instance.HasSavedData())
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
