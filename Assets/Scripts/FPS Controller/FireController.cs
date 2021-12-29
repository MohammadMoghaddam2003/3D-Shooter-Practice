using UnityEngine;

public class FireController : MonoBehaviour
{
    public GameObject Impact, Flame, M4MBMazerFlash_TPS, M4A4MazerFlash_TPS, M4MBMazerFlash_FPS, M4A4MazerFlash_FPS;
    public Weapons M4A4_FPS, M4MB_FPS, M4MB_TPS, M4A4_TPS;

    private RaycastHit _hit;
    private Camera _camera;
    private WeaponState _currentWeapon = WeaponState.M4A4;
    private float _nextTimeToFire;


    void Start()
    {
        _camera = GetComponent<Camera>();
        M4MBMazerFlash_TPS.SetActive(false);
        M4MBMazerFlash_FPS.SetActive(false);

        M4A4MazerFlash_TPS.SetActive(false);
        M4A4MazerFlash_FPS.SetActive(false);
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
                        _nextTimeToFire = Time.time + 1 / M4A4_FPS.FireRate;

                        if (Physics.Raycast(transform.position, transform.forward, out _hit, M4A4_FPS.MaxDistance))
                        {
                            if (_hit.rigidbody != null)
                                _hit.rigidbody.AddForce(-_hit.point * M4A4_FPS.Power);

                            if (_hit.transform.CompareTag("Cylinder"))
                            {
                                GameObject cloneFlame = Instantiate(Flame, _hit.point, Quaternion.identity);
                                cloneFlame.transform.SetParent(GameObject.Find("Cylinder").transform);

                                Damageable TakeDamage = _hit.transform.GetComponent<Damageable>();
                                TakeDamage.Damage(M4A4_FPS.Damage * 3);
                            }
                            else
                            {
                                GameObject clonedImpact = Instantiate(Impact, _hit.point, Quaternion.identity);
                                clonedImpact.transform.SetParent(_hit.transform);

                                if (!_hit.transform.CompareTag("Ground"))
                                {
                                    Damageable TakeDamage = _hit.transform.GetComponent<Damageable>();
                                    TakeDamage.Damage(M4A4_FPS.Damage);
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
                        _nextTimeToFire = Time.time + 1 / M4MB_FPS.FireRate;

                        if (Physics.Raycast(transform.position, transform.forward, out _hit, M4MB_FPS.MaxDistance))
                        {
                            if (_hit.rigidbody != null)
                                _hit.rigidbody.AddForce(-_hit.point * M4MB_FPS.Power);

                            if (_hit.transform.CompareTag("Cylinder"))
                            {
                                GameObject cloneFlame = Instantiate(Flame, _hit.point, Quaternion.identity);
                                cloneFlame.transform.SetParent(GameObject.Find("Cylinder").transform);

                                Damageable TakeDamage = _hit.transform.GetComponent<Damageable>();
                                TakeDamage.Damage(M4MB_FPS.Damage * 3);
                            }
                            else
                            {
                                GameObject clonedImpact = Instantiate(Impact, _hit.point, Quaternion.identity);
                                clonedImpact.transform.SetParent(_hit.transform);

                                if (!_hit.transform.CompareTag("Ground"))
                                {
                                    Damageable TakeDamage = _hit.transform.GetComponent<Damageable>();
                                    TakeDamage.Damage(M4MB_FPS.Damage);
                                }
                            }
                        }
                    }

                    break;
                }

        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (_currentWeapon == WeaponState.M4MB)
            {
                M4MBMazerFlash_TPS.SetActive(true);
                M4MBMazerFlash_FPS.SetActive(true);
            }

            else
            {
                M4A4MazerFlash_TPS.SetActive(true);
                M4A4MazerFlash_FPS.SetActive(true);
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            if (_currentWeapon == WeaponState.M4MB)
            {
                M4MBMazerFlash_TPS.SetActive(false);
                M4MBMazerFlash_FPS.SetActive(false);
            }
            else
            {
                M4A4MazerFlash_TPS.SetActive(false);
                M4A4MazerFlash_FPS.SetActive(false);
            }
        }
    }

    void ChangeWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _currentWeapon = WeaponState.M4MB;
            M4A4_FPS.gameObject.SetActive(false);
            M4A4_TPS.gameObject.SetActive(false);

            M4MB_FPS.gameObject.SetActive(true);
            M4MB_TPS.gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _currentWeapon = WeaponState.M4A4;
            M4MB_FPS.gameObject.SetActive(false);
            M4MB_TPS.gameObject.SetActive(false);

            M4A4_FPS.gameObject.SetActive(true);
            M4A4_TPS.gameObject.SetActive(true);
        }
    }
}
