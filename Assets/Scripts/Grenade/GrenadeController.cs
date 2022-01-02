using UnityEngine;

public class GrenadeController : MonoBehaviour
{
    public float ExplosionTime = 3, Radius = 10, Force = 80;
    public GameObject ExplosionPrefab;
    private bool IsExploded;

    void Start()
    {
    }

    void Update()
    {

        if (!IsExploded)
        {
            ExplosionTime -= Time.deltaTime;

            if (ExplosionTime <= 0)
                Explosion();
        }
    }

    void Explosion()
    {
        GameObject explotion = Instantiate(ExplosionPrefab, transform.position, transform.rotation);

        Collider[] cols = Physics.OverlapSphere(transform.position, Radius);

        foreach (Collider item in cols)
        {
            if (item.attachedRigidbody)
            {
                if (item.CompareTag("Enemy") && item.GetComponent<Health>())
                {
                    Health TakeDamage = item.GetComponent<Health>();
                    TakeDamage.adjustCurrentHealth(-100);
                }
                else
                    item.GetComponent<Rigidbody>().AddExplosionForce(Force, transform.position, Radius, 1, ForceMode.Acceleration);
            }
        }

        Destroy(gameObject);
        IsExploded = true;
    }
}
