using UnityEngine;

public class BulletOfEnemy : MonoBehaviour
{
    public GameObject Blood;

    void OnTriggerEnter(Collider player)
    {
        PlayerHelth takeDamage = player.GetComponent<PlayerHelth>();
        takeDamage.TakeDamage(5);

        Instantiate(Blood, transform.position, Quaternion.identity);
    }
}
