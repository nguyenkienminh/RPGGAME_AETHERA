using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Animator animator;
    public string checkPointId;
    public string checkPointName;
    public bool activitionStatus;
    void Start()
    {
        animator = GetComponent<Animator>();    
    }

    [ContextMenu("Generate checkpoint id")]
    private void GenerateId()
    {
        checkPointName = gameObject.name;
        checkPointId = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            if(activitionStatus == false)
            {
                AudioManager.instance.PlaySFX(5, transform);

            }
            ActivateCheckpoint();
        }
    }

    public void ActivateCheckpoint()
    {
        activitionStatus = true;
        animator.SetBool("active", true);
    }
}
