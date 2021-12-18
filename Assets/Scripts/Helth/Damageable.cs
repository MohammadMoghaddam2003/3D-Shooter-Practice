using UnityEngine;

public class Damageable : MonoBehaviour
{
    public GameObject Explotion;
    public float ExplotionWaveRadius = 5f, ExplotionWaveForce = 500f;
    public bool IsDestroyed = false;

    private float Helth = 100f;

    public void Damage(float buletDamage)
    {
        Helth -= buletDamage;

        if (Helth <= 0f)
        {
            Instantiate(Explotion, transform.position, Quaternion.identity);

            Collider[] colliders = Physics.OverlapSphere(transform.position, ExplotionWaveRadius);

            foreach (var item in colliders)
            {
                Rigidbody ExplotionForce = item.transform.GetComponent<Rigidbody>();
                if (ExplotionForce != null)
                    ExplotionForce.AddExplosionForce(ExplotionWaveForce, transform.position, ExplotionWaveRadius);
            }

            Destroy(gameObject);
            IsDestroyed = true;
        }
        else if (Helth < 100 && gameObject.CompareTag("Cylinder"))
        {
            Invoke("Destroyer", 5);
        }
    }

    void Destroyer()
    {
        Instantiate(Explotion, transform.position, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplotionWaveRadius);

        foreach (var item in colliders)
        {
            Rigidbody ExplotionForce = item.transform.GetComponent<Rigidbody>();
            if (ExplotionForce != null)
                ExplotionForce.AddExplosionForce(ExplotionWaveForce, transform.position, ExplotionWaveRadius);
        }

        Destroy(gameObject);
        IsDestroyed = true;
    }
}
