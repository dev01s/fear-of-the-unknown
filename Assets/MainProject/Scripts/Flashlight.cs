using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting; // Не забудь подключить URP

public class Flashlight : MonoBehaviour
{
    public float rotationSpeed = 360f; // Скорость вращения фонарика
    [SerializeField] CharacterController characterController;
    private Light2D flashlight;
    private float angle;

    void Start()
    {
        flashlight = GetComponent<Light2D>();
        if (flashlight == null)
        {
            Debug.LogError("На объекте должен быть Light2D компонент");
        }
    }

    void Update()
    {
        RotateFlashlight();
    }

    void RotateFlashlight()
    {
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void ToggleFlashlight()
    {
        flashlight.enabled = !flashlight.enabled;
    }

    public void SetAimDirection (Vector3 aimDirection)
    {
        angle = UtilsClass.GetAngleFromVectorFloat(aimDirection) - flashlight.pointLightOuterAngle;
    }
}