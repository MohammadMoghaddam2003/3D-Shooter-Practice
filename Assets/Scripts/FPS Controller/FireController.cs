using UnityEngine;

public class FireController : MonoBehaviour
{
    public GameObject Impact, Flame;
    public GameObject GunFire;
    public float ShootPower = 50f, FireRate = 15f, BuletDamage = 30f;

    private RaycastHit _hit;
    private Camera _camera;
    private float _nextTimeToFire;


    void Start()
    {
        _camera = GetComponent<Camera>();
        GunFire.SetActive(false);
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
                if (_hit.rigidbody != null)
                    _hit.rigidbody.AddForce(-_hit.point * ShootPower);

                if (_hit.transform.CompareTag("Cylinder"))
                {
                    GameObject cloneFlame = Instantiate(Flame, _hit.point, Quaternion.identity);
                    cloneFlame.transform.SetParent(GameObject.Find("Cylinder").transform);

                    Damageable TakeDamage = _hit.transform.GetComponent<Damageable>();
                    TakeDamage.Damage(BuletDamage * 3);
                }
                else
                {
                    GameObject clonedImpact = Instantiate(Impact, _hit.point, Quaternion.identity);
                    clonedImpact.transform.SetParent(_hit.transform);

                    if (!_hit.transform.CompareTag("Ground"))
                    {
                        Damageable TakeDamage = _hit.transform.GetComponent<Damageable>();
                        TakeDamage.Damage(BuletDamage);
                    }
                }
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            GunFire.SetActive(true);
        }

        if (Input.GetButtonUp("Fire1"))
        {
            GunFire.SetActive(false);
        }
    }
}
