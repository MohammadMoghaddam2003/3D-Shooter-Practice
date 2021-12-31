using System.Collections;
using UnityEngine;

public class PlayerHelth : MonoBehaviour
{
    public Camera DyingCamera;
    public Camera[] OtherCamera;

    private Animator DyingAnim;
    private float _helth = 100;

    void Start()
    {
        DyingAnim = transform.root.GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        _helth -= damage;

        if (_helth <= 0)
        {
            DyingAnim.SetTrigger("Dead");

            DyingCamera.enabled = true;

            foreach (var item in OtherCamera)
            {
                item.enabled = false;
            }

            StartCoroutine(MoveCamera());
        }
    }


    IEnumerator MoveCamera()
    {
        DyingCamera.transform.position = Vector3.Lerp
        (DyingCamera.transform.position, new Vector3(DyingCamera.transform.position.x,
         DyingCamera.transform.position.y +3, DyingCamera.transform.position.z - 3),
          2 * Time.deltaTime);
        yield return null;
    }
}
