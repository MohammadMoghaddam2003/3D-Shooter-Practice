using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform Player;

    private Animator _enemyAnimator;
    private CharacterController _enemyCharController;
    private NavMeshAgent _nav;

    void Start()
    {
        _enemyAnimator = GetComponent<Animator>();
        _enemyCharController = GetComponent<CharacterController>();
        _nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        _nav.SetDestination(Player.position);

        _enemyAnimator.SetFloat("VelocityZ", transform.TransformDirection(_enemyCharController.velocity).z);
        _enemyAnimator.SetFloat("VelocityX", transform.TransformDirection(_enemyCharController.velocity).x);
    }
}
