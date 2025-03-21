using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;

    private Transform player;

    [SerializeField] public CheckPoint[] checkPoints;
    [SerializeField] private string closesCheckpointId;

    [Header("lost currency")]
    [SerializeField] private GameObject lostCurrencyPrefab;
    public int lostCurrencyAmount;
    [SerializeField] private float lostCurrencyX;
    [SerializeField] private float lostCurrencyY;
    private bool isMapOpen = false;


    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;


        //DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        checkPoints = FindObjectsByType<CheckPoint>(FindObjectsSortMode.None);

        player = PlayerManager.instance.player.transform;
    }
    private void Update()
    {
      
        if (Input.GetKeyDown(KeyCode.M))
        {
            isMapOpen = !isMapOpen;
            CheckPointMap.instance.ToggleMap(isMapOpen);
        }

       
    }
    public void RestartScene()
    {

        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data) => StartCoroutine(LoadWithDelay(_data));

    public void LoadCheckpoints(GameData _data)
    {
        foreach (KeyValuePair<string, bool> pair in _data.checkPoints)
        {

            foreach (CheckPoint checkpoint in checkPoints)
            {

                if (checkpoint.checkPointId == pair.Key && pair.Value == true)
                    checkpoint.ActivateCheckpoint();



            }
        }
    }
    public void SetMapOpen(bool value)
    {
        isMapOpen = value;
    }



    private void LoadLostCurrency(GameData _data)
    {


        lostCurrencyAmount = _data.lostCurrencyAmount;
        lostCurrencyX = _data.lostCurrencyX;
        lostCurrencyY = _data.lostCurrencyY;

        if (lostCurrencyAmount > 0)
        {
            GameObject newLostCurrency = Instantiate(lostCurrencyPrefab, new Vector3(lostCurrencyX, lostCurrencyY), Quaternion.identity);
            newLostCurrency.GetComponent<LostCurrencyController>().currency = lostCurrencyAmount;

        }


        lostCurrencyAmount = 0;
    }

    private IEnumerator LoadWithDelay(GameData _data)
    {
        yield return new WaitForSeconds(.1f);

        LoadCheckpoints(_data);
        LoadClosestCheckpoint(_data);
        LoadLostCurrency(_data);
    }

    public void SaveData(ref GameData _data)
    {
        _data.lostCurrencyAmount = lostCurrencyAmount;
        _data.lostCurrencyX = player.position.x;
        _data.lostCurrencyY = player.position.y;



        if (FindClosestCheckpoint() != null)
            _data.closesCheckpointId = FindClosestCheckpoint().checkPointId;

        _data.checkPoints.Clear();

        foreach (CheckPoint checkpoint in checkPoints)
        {
            _data.checkPoints.Add(checkpoint.checkPointId, checkpoint.activitionStatus);
        }
    }
    private void LoadClosestCheckpoint(GameData _data)
    {
        if (_data.closesCheckpointId == null)
            return;


        closesCheckpointId = _data.closesCheckpointId;

        foreach (CheckPoint checkpoint in checkPoints)
        {
            if (closesCheckpointId == checkpoint.checkPointId)
                player.position = checkpoint.transform.position;
        }
    }

    private CheckPoint FindClosestCheckpoint()
    {
        float closestDistance = Mathf.Infinity;
        CheckPoint closestCheckpoint = null;

        foreach (var checkpoint in checkPoints)
        {
            float distanceToCheckpoint = Vector2.Distance(player.position, checkpoint.transform.position);

            if (distanceToCheckpoint < closestDistance && checkpoint.activitionStatus == true)
            {
                closestDistance = distanceToCheckpoint;
                closestCheckpoint = checkpoint;
            }
        }

        return closestCheckpoint;
    }

    public void PauseGame(bool _pause)
    {
        if (_pause)
        {
            Time.timeScale = 0.0f;
        }
        else
            Time.timeScale = 1.0f;
    }
}
