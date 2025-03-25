using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
public class SaveManager : MonoBehaviour
{
    private GameData gameData;

    [SerializeField] private string fileName;
    [SerializeField] private bool encrypteData;

    public static SaveManager instance;

    private List<ISaveManager> saveManagers;

    private FileDataHandle dataHandler;

    [ContextMenu("Delete Save Data and Enemy Data and Chest")]
    public void DeleteSaveData()
    {
        dataHandler = new FileDataHandle(Application.persistentDataPath, fileName, encrypteData);
        dataHandler.Delete();

        string enemyDataPath = Application.persistentDataPath + "/EnemyData.json";

        if (File.Exists(enemyDataPath))
        {
            File.Delete(enemyDataPath);
            Debug.Log("Đã xóa file EnemyData.json thành công!");
        }
        else
        {
            Debug.LogWarning("File EnemyData.json không tồn tại!");
        }

        string Chestpath = Application.persistentDataPath + "/ChestData.json";

        if (File.Exists(Chestpath))
        {
            File.Delete(Chestpath);
            Debug.Log("Đã xóa dữ liệu rương!");
        }
        else
        {
            Debug.LogWarning("File ChestData.json không tồn tại!");
        }

    }

    [ContextMenu("Delete Battle Boss")]
    public void DeleteSaveDataBoss()
    {
        dataHandler = new FileDataHandle(Application.persistentDataPath, fileName, encrypteData);
        dataHandler.Delete();
        string IntroPath = Application.persistentDataPath + "/AreaBattle.json";

        if (File.Exists(IntroPath))
        {
            File.Delete(IntroPath);
            Debug.Log("Đã xóa dữ AreaBattle!");
        }
        else
        {
            Debug.LogWarning("File AreaBattle.json không tồn tại!");
        }
    }

    [ContextMenu("Delete Save Intro")]
    public void DeleteSaveDataIntro()
    {
        dataHandler = new FileDataHandle(Application.persistentDataPath, fileName, encrypteData);
        dataHandler.Delete();
        string IntroPath = Application.persistentDataPath + "/VideoData.json";

        if (File.Exists(IntroPath))
        {
            File.Delete(IntroPath);
            Debug.Log("Đã xóa dữ intro!");
        }
        else
        {
            Debug.LogWarning("File Intro.json không tồn tại!");
        }
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        //DontDestroyOnLoad(this.gameObject); // Giữ SaveManager khi chuyển scene
    }


    private void Start()
    {
        dataHandler = new FileDataHandle(Application.persistentDataPath, fileName, encrypteData);

       

        saveManagers = FindAllSaveManagers();
        LoadGame();

    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if (this.gameData == null)
        {
            NewGame();
        }

        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }

    }
    public void SaveGame()
    {
        foreach (ISaveManager saveManager in saveManagers)
        {
            Debug.Log($"📝 Saving: {saveManager}");
            saveManager.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);


    }
    //private void OnEnable()
    //{
    //    UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    //}

    //private void OnDisable()
    //{
    //    UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    //}

    //private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    //{
    //    saveManagers = FindAllSaveManagers(); // Cập nhật lại danh sách ISaveManager khi chuyển map
    //}

    public void OnApplicationQuit()
    {
        SaveGame();
        Application.Quit(); // Thoát game
    }

    //private List<ISaveManager> FindAllSaveManagers()
    //{
    //    var managers = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISaveManager>().ToList();
    //    Debug.Log("Tìm thấy " + managers.Count + " ISaveManager trong Scene.");
    //    return managers;
    //}

    private List<ISaveManager> FindAllSaveManagers()
    {
        //IEnumerable<ISaveManager> saveManagers = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISaveManager>();

        //return new List<ISaveManager>(saveManagers);

        List<ISaveManager> saveManagers = new List<ISaveManager>();
        MonoBehaviour[] allScripts = FindObjectsOfType<MonoBehaviour>(true);

        foreach (MonoBehaviour script in allScripts)
        {
            if (script is ISaveManager saveManager)
            {
                saveManagers.Add(saveManager);
                Debug.Log($"🛠 Found: {script.GetType().Name} on {script.gameObject.name}");
            }
        }

        Debug.Log($"🔍 Total SaveManagers found: {saveManagers.Count}");
        return saveManagers;
    }

    public bool HasSavedData()
    {
        if(dataHandler.Load() != null)
        {
            return true;
        }
        return false;
    }
}
