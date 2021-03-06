using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FireController : MonoBehaviour
{
    public GameObject Impact, Flame, M4MBMazerFlash_TPS, M4A4MazerFlash_TPS, M4MBMazerFlash_FPS,
     M4A4MazerFlash_FPS, Blood, TargetForEnemy, GrenadePrefab;
    public Weapons M4A4_FPS, M4MB_FPS, M4MB_TPS, M4A4_TPS;
    public Animator SoldierAnim;
    public float MaxBulletOfM4A4 = 10f, MaxBulletOfM4MB = 5f, MaxAmmoM4A4 = 3f, MaxAmmoM4MB = 3;

    private RaycastHit _hit;
    private Camera _camera;
    private WeaponState _currentWeapon = WeaponState.M4A4;
    private AudioSource _weaponsSounds;
    private float _nextTimeToFire;
    private static float _shootedBulletM4A4, _shootedBulletM4MB, _usedAmmoM4A4, _usedAmmoM4MB;
    private bool _canFire = true;


    void Start()
    {
        if (!_weaponsSounds)
            _weaponsSounds = GetComponent<AudioSource>();

        _camera = GetComponent<Camera>();
        M4MBMazerFlash_TPS.SetActive(false);
        M4MBMazerFlash_FPS.SetActive(false);

        M4A4MazerFlash_TPS.SetActive(false);
        M4A4MazerFlash_FPS.SetActive(false);
    }

    void Update()
    {
        ChangeWeapon();
        if (_currentWeapon == WeaponState.M4A4)
        {
            if (Input.GetButton("Fire1") && _camera.enabled)
            {
                if (_shootedBulletM4A4 <= MaxBulletOfM4A4 && _canFire)
                {
                    _weaponsSounds.clip = M4A4_FPS.FireSound;
                    _weaponsSounds.Play();
                    Fire();
                }
                else
                {
                    _weaponsSounds.clip = M4A4_FPS.NoAmmoSound;
                    _weaponsSounds.Play();
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && _camera.enabled)
            {
                if (_shootedBulletM4MB <= MaxBulletOfM4MB && _canFire)
                {
                    _weaponsSounds.clip = M4MB_FPS.FireSound;
                    _weaponsSounds.Play();
                    Fire();
                }
                else
                {
                    _weaponsSounds.clip = M4MB_FPS.NoAmmoSound;
                    _weaponsSounds.Play();
                }
            }
        }

        SetActiveParticalSystemOfFire();

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (_currentWeapon == WeaponState.M4A4 && MaxAmmoM4A4 >= _usedAmmoM4A4 && _shootedBulletM4A4 > 0)
            {
                StartCoroutine(Reloading());
                StopCoroutine(Reloading());
            }
            else if (_currentWeapon == WeaponState.M4MB && MaxAmmoM4MB >= _usedAmmoM4MB && _shootedBulletM4MB > 0)
            {
                StartCoroutine(Reloading());
                StopCoroutine(Reloading());
            }
        }

        if (Input.GetKeyDown(KeyCode.G) && _camera.enabled)
        {
            ShootGrenade();
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

                        _shootedBulletM4A4++;

                        SoldierAnim.SetTrigger("IsShooting");

                        if (Physics.Raycast(transform.position, transform.forward, out _hit, M4A4_FPS.MaxDistance))
                        {
                            if (_hit.transform.CompareTag("Enemy"))
                            {
                                Instantiate(Blood, _hit.point, Quaternion.identity);

                                Health TakeDamage = _hit.transform.GetComponent<Health>();
                                TakeDamage.adjustCurrentHealth(-(int)M4A4_FPS.Damage);

                                Aggro enemyAggro = _hit.transform.GetComponent<Aggro>();
                                enemyAggro.state = Aggro.engagementArea.CONE;

                                enemyAggro.target = TargetForEnemy;
                            }
                            else
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

                                    if (!_hit.transform.CompareTag("Ground") && !_hit.transform.CompareTag("Enemy") && !_hit.transform.CompareTag("Wall"))
                                    {
                                        Damageable TakeDamage = _hit.transform.GetComponent<Damageable>();
                                        TakeDamage.Damage(M4A4_FPS.Damage);
                                    }
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

                        _shootedBulletM4MB++;

                        SoldierAnim.SetTrigger("IsShooting");

                        if (Physics.Raycast(transform.position, transform.forward, out _hit, M4MB_FPS.MaxDistance))
                        {
                            if (_hit.transform.CompareTag("Enemy"))
                            {
                                Instantiate(Blood, -_hit.transform.position, Quaternion.identity);

                                Health TakeDamage = _hit.transform.GetComponent<Health>();
                                TakeDamage.adjustCurrentHealth(-(int)M4MB_FPS.Damage);

                                Aggro enemyAggro = _hit.transform.GetComponent<Aggro>();
                                enemyAggro.state = Aggro.engagementArea.CONE;

                                enemyAggro.target = TargetForEnemy;
                            }
                            else
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

                                    if (!_hit.transform.CompareTag("Ground") && !_hit.transform.CompareTag("Enemy"))
                                    {
                                        Damageable TakeDamage = _hit.transform.GetComponent<Damageable>();
                                        TakeDamage.Damage(M4MB_FPS.Damage);
                                    }
                                }
                            }
                        }
                    }

                    break;
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

    void SetActiveParticalSystemOfFire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (_currentWeapon == WeaponState.M4MB)
            {
                M4MBMazerFlash_TPS.SetActive(true);
                M4MBMazerFlash_FPS.SetActive(true);

                Invoke("SetFalseParticalSystemOfFire", 1 * Time.deltaTime);
            }

            else
            {
                M4A4MazerFlash_TPS.SetActive(true);
                M4A4MazerFlash_FPS.SetActive(true);
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            SetFalseParticalSystemOfFire();
        }
    }


    void SetFalseParticalSystemOfFire()
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


    void ShootGrenade()
    {
        GameObject grenade = Instantiate(GrenadePrefab, transform.position, transform.rotation);
        grenade.GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.VelocityChange);
    }


    IEnumerator Reloading()
    {
        SoldierAnim.SetTrigger("Reload");

        if (_currentWeapon == WeaponState.M4A4)
        {
            _weaponsSounds.clip = M4A4_FPS.ReloadSound;
            _weaponsSounds.Play();
        }
        else
        {
            _weaponsSounds.clip = M4MB_FPS.ReloadSound;
            _weaponsSounds.Play();
        }

        yield return new WaitForSeconds(2.2f);

        if (_currentWeapon == WeaponState.M4A4)
        {
            _shootedBulletM4A4 = 0f;
            _usedAmmoM4A4++;
        }
        else
        {
            _shootedBulletM4MB = 0f;
            _usedAmmoM4MB++;
        }

        _canFire = true;
    }
}
