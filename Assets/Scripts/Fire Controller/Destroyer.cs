using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public float TimeForDestroy = 5f;
    public bool isDestroyed;
    public ImpactFlame impactFlame = ImpactFlame.Flame;

    void Update()
    {
        if (impactFlame == ImpactFlame.Flame && isDestroyed)
            Destroy(transform.gameObject);
        else if (isDestroyed)
            Destroy(transform.gameObject, TimeForDestroy);
    }
}
public enum ImpactFlame
{
    Impact,
    Flame
}

