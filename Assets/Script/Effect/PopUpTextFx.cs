using TMPro;
using UnityEngine;

public class PopUpTextFx : MonoBehaviour
{
    private TextMeshPro myText;

    [SerializeField] private float speed;
    [SerializeField] private float disappearanceSpeed;
    [SerializeField] private float colorDisappearanceSpeed;
    [SerializeField] private float lifeTime;
    [SerializeField] private float randomRange = 1f; 

    private float textTimer;

    private void Start()
    {
        myText = GetComponent<TextMeshPro>();
        textTimer = lifeTime;

        RandomizePosition();
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + 1), speed * Time.deltaTime);

        textTimer -= Time.deltaTime;

        if (textTimer < 0)
        {
            float alpha = myText.color.a - colorDisappearanceSpeed * Time.deltaTime;
            myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, alpha);

            if (myText.color.a < 50)
            {
                speed = disappearanceSpeed;
            }

            if (myText.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void RandomizePosition()
    {
        float randomX = Random.Range(-randomRange, randomRange);
        float randomY = Random.Range(-randomRange, randomRange);
        transform.position += new Vector3(randomX, randomY, 0);
    }
}
