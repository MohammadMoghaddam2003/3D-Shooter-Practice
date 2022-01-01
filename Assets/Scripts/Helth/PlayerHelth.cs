using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHelth : MonoBehaviour
{
    public Camera DyingCamera;
    public Camera[] OtherCamera;
    public Text HelthPlayerText;
    public Image Crosshair;
    public GameObject Plane;
    public Aggro EnemyAggro;


    private Animator DyingAnim;
    private float _helth = 100;

    void Start()
    {
        DyingAnim = transform.root.GetComponent<Animator>();
        HelthPlayerText.text = _helth.ToString();
        HelthPlayerText.color = Color.blue;

    }

    void Update()
    {
        HelthPlayerText.text = _helth.ToString();

        switch (_helth)
        {
            case 70:
                {
                    HelthPlayerText.color = Color.magenta;
                    break;
                }

            case 50:
                {
                    HelthPlayerText.color = Color.yellow;
                    break;
                }

            case 20:
                {
                    HelthPlayerText.color = Color.red;
                    break;
                }
        }
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

            gameObject.tag = "Dead";


            EnemyAggro.state = Aggro.engagementArea.CLEAR;
            EnemyAggro.target = null;

            HelthPlayerText.enabled = false;
            Crosshair.enabled = false;

            Plane.SetActive(true);

            StartCoroutine(MoveCamera());
        }
    }


    IEnumerator MoveCamera()
    {
        DyingCamera.transform.position = Vector3.Lerp
        (DyingCamera.transform.position, new Vector3(DyingCamera.transform.position.x,
         DyingCamera.transform.position.y + 3, DyingCamera.transform.position.z - 3),
          2 * Time.deltaTime);
        yield return null;
    }
}
