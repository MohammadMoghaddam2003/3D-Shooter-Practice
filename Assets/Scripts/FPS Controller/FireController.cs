using UnityEngine;

public class FireController : MonoBehaviour
{
    public GameObject Impact;
    public ParticleSystem[] GunFire;
    public float ShootPower = 50f, FireRate = 15f;

    private RaycastHit _hit;
    private Camera _camera;
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
                Instantiate(Impact, _hit.point, Quaternion.identity);
                if (_hit.rigidbody != null)
                    _hit.rigidbody.AddForce(-_hit.normal * ShootPower);
            }
        }
        
        if (Input.GetButtonDown("Fire1"))
        {
            foreach (var item in GunFire)
            {
                item.Play();
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            foreach (var item in GunFire)
            {
                item.Stop();
            }
        }
    }
}
