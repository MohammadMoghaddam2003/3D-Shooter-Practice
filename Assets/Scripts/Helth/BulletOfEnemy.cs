using UnityEngine;

public class BulletOfEnemy : MonoBehaviour
{
    void OnTriggerEnter(Collider player)
    {
        PlayerHelth takeDamage = player.GetComponent<PlayerHelth>();
        takeDamage.TakeDamage(5);
    }
}
