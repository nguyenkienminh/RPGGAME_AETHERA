using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour, ISaveManager
{
    [Header("End screeen")]
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject restartButton;
    [Space]


    [SerializeField] private GameObject characterUI;
    [SerializeField] public GameObject skillTreeUI;
    [SerializeField] private GameObject stashUI;
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject InGameUI;


    public UI_SkillToolTip skillToolTip;
    public UI_ItemTooltip itemToolTip;
    public UI_StatToolTip statToolTip;
    public UI_CraftWindow craftWindow;

    


    [SerializeField] private UI_VolumeSlider[] volumeSettings;
    private void Awake()
    {
        if (skillTreeUI == null)
        {
            Debug.LogError("skillTreeUI chưa được gán trong Inspector!");
        }
        if (fadeScreen == null)
        {
            Debug.LogError("fadeScreen chưa được gán trong Inspector!");
        }
        fadeScreen.gameObject.SetActive(true);
        SwitchToMenu(skillTreeUI);
        //DontDestroyOnLoad(gameObject); // Giữ SaveManager khi chuyển scene

    }

    void Start()
    {
        SwitchToMenu(InGameUI);

        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchWithKeyTo(characterUI);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SwitchWithKeyTo(stashUI);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SwitchWithKeyTo(skillTreeUI);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchWithKeyTo(optionUI);
        }
    }

    public void SwitchToMenu(GameObject _menu)
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null;

            if (fadeScreen == false)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        if (_menu != null)
        {
            if(AudioManager.instance != null)
            {
            AudioManager.instance.PlaySFX(7, null);

            }
            else
            {
                Debug.LogWarning("AudioManager.instance đang null! Hãy kiểm tra AudioManager trong scene.");
            }
            _menu.SetActive(true);
        }

        if(GameManager.instance != null)
        {
            if(_menu == InGameUI)
            {
                GameManager.instance.PauseGame(false);
            }
            else
            {
                GameManager.instance.PauseGame(true);
            }
        }
    }

    public void SwitchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }
        SwitchToMenu(_menu);
    }

    private void CheckForInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScreen>() == null)
            {
                return;
            }
        }

        SwitchToMenu(InGameUI);
    }

    public void SwitchOnEndScreen()
    {
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCaroutine());

    }

    IEnumerator EndScreenCaroutine()
    {
        yield return new WaitForSeconds(1);
        endText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        restartButton.SetActive(true);
    }

    public void RestartGameButton()
    {
        // Gọi hàm SaveGame trước khi restart scene
        SaveManager.instance.SaveGame();
        GameManager.instance.RestartScene();
    }

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string,float> pair in _data.volumeSettings)
        {
            foreach (UI_VolumeSlider item in volumeSettings)
            {
                if(item.parameter == pair.Key)
                {
                    item.LoadSlider(pair.Value);
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.volumeSettings.Clear();

        foreach (UI_VolumeSlider item in volumeSettings)
        {
            _data.volumeSettings.Add(item.parameter, item.slider.value);
        }
    }
}
