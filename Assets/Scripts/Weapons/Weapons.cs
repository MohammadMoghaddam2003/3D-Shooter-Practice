using UnityEngine;

public class Weapons : MonoBehaviour
{
   // public WeaponState CurrentWeapon = WeaponState.M4;
    public float Power = 100, MaxDistance = 50, FireRate = 25, Damage = 10;
}


public enum WeaponState
{
    M4A4,
    M4MB
}