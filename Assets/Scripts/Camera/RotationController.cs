using UnityEngine;

public class RotationController : MonoBehaviour
{
    public Axis SetAxis = Axis.AxisX;
    public float Sensivity = 10f, X_Min = -360f, X_Max = 360f, Y_Min = -60f, Y_Max = 60f;

    private float _rotationValue;

    void LateUpdate()
    {
        RotationCamera();
    }

    void RotationCamera()
    {

        switch (SetAxis)
        {
            case Axis.AxisX:
                {
                    _rotationValue += Input.GetAxis("Mouse X") * Sensivity * Time.deltaTime;
                    _rotationValue = RotationClamping(_rotationValue, X_Min, X_Max);
                    transform.rotation = Quaternion.Euler(0f, _rotationValue, 0f);
                    break;
                }
            case Axis.AxisY:
                {
                    _rotationValue -= Input.GetAxis("Mouse Y") * Sensivity * Time.deltaTime;
                    _rotationValue = RotationClamping(_rotationValue, Y_Min, Y_Max);
                    transform.localRotation = Quaternion.Euler(_rotationValue, 0f, 0f);
                    break;
                }
        }
    }


    float RotationClamping(float value, float min, float max)
    {
        if (value > 360f)
        {
            value -= 360f;
        }
        else if (value < -360f)
        {
            value += 360f;
        }


        float Result = Mathf.Clamp(value, min, max);
        return Result;
    }
}

public enum Axis
{
    AxisX,
    AxisY
}
