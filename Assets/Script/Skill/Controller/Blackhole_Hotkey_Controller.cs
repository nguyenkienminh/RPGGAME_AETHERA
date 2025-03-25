using TMPro;
using UnityEngine;

public class Blackhole_Hotkey_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;

    private Transform myEnemy;
    private BlackHole_Skill_Controller blackhole;
    public void SetupHotKey(KeyCode _myNewHotKey,Transform _myEnemy,BlackHole_Skill_Controller _myBlackhole)
    {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();

        myHotKey = _myNewHotKey;
        myText.text = _myNewHotKey.ToString();

        myEnemy = _myEnemy;
        blackhole = _myBlackhole;
    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            blackhole.addEnemyToList(myEnemy);

            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
