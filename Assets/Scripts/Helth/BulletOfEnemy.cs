using UnityEngine;

public class BulletOfEnemy : MonoBehaviour
{
    public GameObject Blood;

    void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            PlayerHelth takeDamage = player.GetComponent<PlayerHelth>();
            takeDamage.TakeDamage(50);

            Instantiate(Blood, transform.position, Quaternion.identity);
        }
    }
}
