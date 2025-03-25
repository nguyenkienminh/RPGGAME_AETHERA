using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class CheckPointMap : MonoBehaviour
{
    public static CheckPointMap instance;

    [SerializeField] private GameObject mapUI;
    [SerializeField] private Button checkpointButtonPrefab;
    [SerializeField] private Transform checkpointListParent;

    private void Awake()
    {
        instance = this;
        mapUI.SetActive(false);
    }

    public void ToggleMap(bool _show)
    {
        if (_show)
        {
            ShowCheckpoints();
            Time.timeScale = 0;
            mapUI.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            mapUI.SetActive(false);
            ClearCheckpointButtons();
        }
    }

    private void ShowCheckpoints()
    {
        foreach (CheckPoint checkpoint in GameManager.instance.checkPoints)
        {
            if (checkpoint.activitionStatus)
            {
                Button button = Instantiate(checkpointButtonPrefab, checkpointListParent);
                if (checkpoint.checkPointName != null)
                {
                    button.GetComponentInChildren<TextMeshProUGUI>().text = checkpoint.checkPointName;
                }
                button.onClick.AddListener(() => SelectCheckpoint(checkpoint.transform, checkpoint.activitionStatus));
            }

        }
    }

    private void ClearCheckpointButtons()
    {
        foreach (Transform child in checkpointListParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void SelectCheckpoint(Transform checkpointTransform, bool isActive)
    {
        PlayerManager.instance.player.transform.position = checkpointTransform.position;
        GameManager.instance.SetMapOpen(false); 
        ToggleMap(false);
    }

}
