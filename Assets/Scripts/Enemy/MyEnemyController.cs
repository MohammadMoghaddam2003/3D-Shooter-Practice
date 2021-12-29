using UnityEngine;
using UnityEngine.AI;

public class MyEnemyController : MonoBehaviour
{
    public Transform Player;

    private Animator _enemyAnimator;
    private Rigidbody _enemyRigidbody;
    private NavMeshAgent _nav;

    void Start()
    {
        _enemyAnimator = GetComponent<Animator>();
        _enemyRigidbody = GetComponent<Rigidbody>();
        _nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        _nav.SetDestination(Player.position);

        _enemyAnimator.SetFloat("VelocityZ", _nav.speed);
        _enemyAnimator.SetFloat("VelocityX", _nav.speed);

        _enemyAnimator.SetFloat("AimingTilt", Mathf.Abs((transform.position - Player.position).normalized.y));
    }
}
