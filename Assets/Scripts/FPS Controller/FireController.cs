using UnityEngine;

public class FireController : MonoBehaviour
{
    public GameObject Impact, Flame;
    public ParticleSystem GunFire;
    public float ShootPower = 50f, FireRate = 15f, BuletDamage = 30f;

    private RaycastHit _hit;
    private Camera _camera;
    private Destroyer destroy;
    private float _nextTimeToFire;


    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    void Update()
    {
        Fire();
    }

    void Fire()
    {
        if (Input.GetButton("Fire1") && _camera.enabled && Time.time >= _nextTimeToFire)
        {
            _nextTimeToFire = Time.time + 1 / FireRate;

            if (Physics.Raycast(transform.position, transform.forward, out _hit))
            {
                //if (_hit.rigidbody != null)
                //  _hit.rigidbody.AddForce(-_hit.point * ShootPower);

                if (_hit.transform.CompareTag("Cylinder"))
                {
                    GameObject clonedFlame = Instantiate(Flame, _hit.point, Quaternion.identity);
                    Damageable TakeDamage = _hit.transform.GetComponent<Damageable>();

                    TakeDamage.Damage(BuletDamage * 3);
                    if (TakeDamage.IsDestroyed)
                    {
                        clonedFlame.GetComponent<Destroyer>().isDestroyed = true;
                    }
                }
                else
                {
                    GameObject clonedImpact = Instantiate(Impact, _hit.point, Quaternion.identity);

                    if (!_hit.transform.CompareTag("Ground"))
                    {
                        Damageable TakeDamage = _hit.transform.GetComponent<Damageable>();
                        TakeDamage.Damage(BuletDamage);
                        if (TakeDamage.IsDestroyed)
                        {
                            clonedImpact.GetComponent<Destroyer>().isDestroyed = true;
                        }
                    }
                }
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            GunFire.Play();
        }

        if (Input.GetButtonUp("Fire1"))
        {
            GunFire.Stop();
        }
    }
}
