using UnityEngine;

public class ManagerBackground : MonoBehaviour
{
    [SerializeField] private GameObject background1;
    [SerializeField] private GameObject background2;
    [SerializeField] private GameObject background3;
    [SerializeField] private GameObject background4;
    [SerializeField] private GameObject background5;
    [SerializeField] private GameObject background6;

    private bool isBackgroundActive = false; 

    public void ToggleBackground()
    {
        isBackgroundActive = !isBackgroundActive;

        background1.SetActive(isBackgroundActive);
        background2.SetActive(isBackgroundActive);
        background3.SetActive(!isBackgroundActive);
        background4.SetActive(!isBackgroundActive);
        background5.SetActive(!isBackgroundActive);
        background6.SetActive(!isBackgroundActive);
    }
}
