using UnityEngine;

public class FireController : MonoBehaviour
{
    public GameObject Prefab;

    private RaycastHit _hit;
    private Camera _camera;


    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    void Update()
    {
        Fire();
    }

    void Fire()
    {
        if (Input.GetButtonDown("Fire1") && _camera.enabled)
        {
            if (Physics.Raycast(transform.position, transform.forward, out _hit))
            {
                Instantiate(Prefab, _hit.point, Quaternion.identity);
            }
        }
    }
}
