using UnityEngine;


public class ItemEffect : ScriptableObject
{
    [TextArea]
    public string effectDescription;
    public virtual void ExecuteEffect(Transform _EnemyPosition)
    {
        Debug.Log("Effect execute!");
    }
}
