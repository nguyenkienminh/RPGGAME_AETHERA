using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.IO;


[System.Serializable]
public class VideoData
{
    public bool hasPlayed;

    public VideoData()
    {
        hasPlayed = false;
    }
}

public class Intro1_UI : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public RawImage rawImage;
    [SerializeField] private GameObject pauseImage;

    private static string savePath;
    private static VideoData videoData;
    void Start()
    {
        savePath = Application.persistentDataPath + "/VideoData.json";
        LoadVideoData();

        if (videoData.hasPlayed)
        {
            LoadNextScene(null);
            return;
        }

        if (videoPlayer.targetTexture == null)
        {

            videoPlayer.targetTexture = new RenderTexture(1920, 1080, 0);
        }

        pauseImage.SetActive(false);
        rawImage.texture = videoPlayer.targetTexture;


        videoPlayer.Play();

        videoPlayer.loopPointReached += LoadNextScene;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (videoPlayer.isPlaying)
            {
                pauseImage.SetActive(true);
                videoPlayer.Pause();
            }
            else
            {
                pauseImage.SetActive(false);
                videoPlayer.Play();
            }
        }
    }

    public void LoadNextScene(VideoPlayer vp)
    {
        videoData.hasPlayed = true;
        SaveVideoData();
        if (!SaveManager.instance.HasSavedData())
        {
            SceneManager.LoadScene("MainMenu1");
        }else if (SaveManager.instance.HasSavedData())
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private static void LoadVideoData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            videoData = JsonUtility.FromJson<VideoData>(json);
        }
        else
        {
            videoData = new VideoData();
        }
    }
    private static void SaveVideoData()
    {
        string json = JsonUtility.ToJson(videoData, true);
        File.WriteAllText(savePath, json);
    }
}
