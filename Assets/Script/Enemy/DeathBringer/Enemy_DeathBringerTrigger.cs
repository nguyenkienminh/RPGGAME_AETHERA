using UnityEngine;

public class Enemy_DeathBringerTrigger : Enemy_AnimationTrigger
{
    private Enemy_DeathBringer enemyDeathBringer => GetComponentInParent<Enemy_DeathBringer>();

    private void Relocate()
    {
        enemyDeathBringer.FindPosition();
    }
    private void MakeInvisible() => enemyDeathBringer.fx.MakeTransparent(true);
    private void Makevisible() => enemyDeathBringer.fx.MakeTransparent(false);


}
