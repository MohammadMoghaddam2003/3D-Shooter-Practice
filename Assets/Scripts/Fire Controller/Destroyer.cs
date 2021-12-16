using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public float TimeForDestroy = 5f;
    void Start()
    {
        Destroy(gameObject, TimeForDestroy);
    }
}
