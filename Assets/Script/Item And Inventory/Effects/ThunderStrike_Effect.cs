using UnityEngine;


[CreateAssetMenu(fileName = "Thunder strike effect", menuName = "Data/Item Effect/Thunder strike")]

public class ThunderStrike_Effect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefab;

    public override void ExecuteEffect(Transform _EnemyPosition)
    {
        GameObject mewThunderStrike = Instantiate(thunderStrikePrefab,_EnemyPosition.position,Quaternion.identity);

        Destroy(mewThunderStrike, 1);
    }
}
