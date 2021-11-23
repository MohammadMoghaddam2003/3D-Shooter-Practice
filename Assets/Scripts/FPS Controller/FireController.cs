using UnityEngine;

public class FireController : MonoBehaviour
{
    public GameObject Prefab;

    private RaycastHit _hit;


    void Start()
    {

    }

    void Update()
    {
        Fire();
    }

    void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (Physics.Raycast(transform.position, transform.forward, out _hit))
            {
                Instantiate(Prefab, _hit.point, Quaternion.identity);
            }
        }
    }
}
