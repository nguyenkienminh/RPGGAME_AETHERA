using UnityEngine;
using UnityEngine.EventSystems;

public class NPC_Controller : MonoBehaviour
{
    [SerializeField] private GameObject boardPrefabs;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject gameUI;
    private void OnMouseDown()
    {
        boardPrefabs.SetActive(true);
        gameUI.SetActive(false);
        craftUI.SetActive(false);
        Time.timeScale = 0;
    }

    public void closeButton()
    {
        Time.timeScale = 1;
        gameUI.SetActive(true);
        boardPrefabs.SetActive(false);
    }
    public void OpenShop()
    {
        Time.timeScale = 0;
        boardPrefabs.SetActive(false);
        gameUI.SetActive(false);
        craftUI.SetActive(true);
    }

    public void CloseShop()
    {
        Time.timeScale = 1;
        gameUI.SetActive(true);
        craftUI.SetActive(false);
    }
}
