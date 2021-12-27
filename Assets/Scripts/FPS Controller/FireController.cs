using System.Net.Http.Headers;
using UnityEngine;

public class FireController : MonoBehaviour
{
    public GameObject Impact, Flame, GunFire;
    public Weapons M4A4, M4MB;

    private RaycastHit _hit;
    private Camera _camera;
    private WeaponState _currentWeapon = WeaponState.M4MB;
    private float _nextTimeToFire;


    void Start()
    {
        _camera = GetComponent<Camera>();
        GunFire.SetActive(false);
    }

    void Update()
    {
        ChangeWeapon();
        if (Input.GetButton("Fire1") && _camera.enabled)
        {
            Fire();
        }
    }

    void Fire()
    {
        switch (_currentWeapon)
        {
            case WeaponState.M4A4:
                {
                    if (Time.time >= _nextTimeToFire)
                    {
                        _nextTimeToFire = Time.time + 1 / M4A4.FireRate;

                        if (Physics.Raycast(transform.position, transform.forward, out _hit, M4A4.MaxDistance))
                        {
                            if (_hit.rigidbody != null)
                                _hit.rigidbody.AddForce(-_hit.point * M4A4.Power);

                            if (_hit.transform.CompareTag("Cylinder"))
                            {
                                GameObject cloneFlame = Instantiate(Flame, _hit.point, Quaternion.identity);
                                cloneFlame.transform.SetParent(GameObject.Find("Cylinder").transform);

                                Damageable TakeDamage = _hit.transform.GetComponent<Damageable>();
                                TakeDamage.Damage(M4A4.Damage * 3);
                            }
                            else
                            {
                                GameObject clonedImpact = Instantiate(Impact, _hit.point, Quaternion.identity);
                                clonedImpact.transform.SetParent(_hit.transform);

                                if (!_hit.transform.CompareTag("Ground"))
                                {
                                    Damageable TakeDamage = _hit.transform.GetComponent<Damageable>();
                                    TakeDamage.Damage(M4A4.Damage);
                                }
                            }
                        }
                    }

                    break;
                }

            case WeaponState.M4MB:
                {
                    if (Time.time >= _nextTimeToFire)
                    {
                        _nextTimeToFire = Time.time + 1 / M4MB.FireRate;

                        if (Physics.Raycast(transform.position, transform.forward, out _hit, M4MB.MaxDistance))
                        {
                            if (_hit.rigidbody != null)
                                _hit.rigidbody.AddForce(-_hit.point * M4MB.Power);

                            if (_hit.transform.CompareTag("Cylinder"))
                            {
                                GameObject cloneFlame = Instantiate(Flame, _hit.point, Quaternion.identity);
                                cloneFlame.transform.SetParent(GameObject.Find("Cylinder").transform);

                                Damageable TakeDamage = _hit.transform.GetComponent<Damageable>();
                                TakeDamage.Damage(M4MB.Damage * 3);
                            }
                            else
                            {
                                GameObject clonedImpact = Instantiate(Impact, _hit.point, Quaternion.identity);
                                clonedImpact.transform.SetParent(_hit.transform);

                                if (!_hit.transform.CompareTag("Ground"))
                                {
                                    Damageable TakeDamage = _hit.transform.GetComponent<Damageable>();
                                    TakeDamage.Damage(M4MB.Damage);
                                }
                            }
                        }
                    }

                    break;
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

    void ChangeWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _currentWeapon = WeaponState.M4MB;
            M4A4.gameObject.SetActive(false);
            M4MB.gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _currentWeapon = WeaponState.M4A4;
            M4MB.gameObject.SetActive(false);
            M4A4.gameObject.SetActive(true);
        }
    }
}
