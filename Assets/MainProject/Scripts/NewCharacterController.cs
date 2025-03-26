using System.IO;
using UnityEngine;

public class NewCharacterController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer; // Для флипа по X

    public float distance;

    private Vector3 m_CameraPosition;

    [SerializeField] DynamicJoystick joys;
    private Transform _cameraTransform;
    private Vector3 movement;
    private Vector3 lastDirection = Vector3.down; // Начальное idle-направление
    [SerializeField] FieldOfView fieldOfView;
    private Vector3 targetDir;

    void Start()
    {
        _cameraTransform = Camera.main.transform;
    }
    void Update()
    {
        movement.x = joys.Horizontal;
        movement.y = joys.Vertical;

        if (movement.sqrMagnitude > 1)
            movement = movement.normalized;

        UpdateAnimation();
        m_CameraPosition = transform.position - _cameraTransform.forward * distance;
        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, m_CameraPosition, Time.deltaTime * 3f);
        Vector3 moveDirection = GetMovementDirection();
        fieldOfView.SetAimDirection(moveDirection);
        fieldOfView.SetOrigin(transform.position);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
    }   

    private void UpdateAnimation()
    {
        if (movement != Vector3.zero)
        {
            animator.SetBool("isMoving", true);
            lastDirection = movement;
            // Флип спрайта при движении влево
            spriteRenderer.flipX = movement.x > 0;
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        // Передаем параметры в Animator для Blend Tree
        animator.SetFloat("MoveX", Mathf.Abs(movement.x)); // Берем модуль, чтобы работал Blend Tree
        animator.SetFloat("MoveY", movement.y);
        animator.SetFloat("LastX", lastDirection.x);
        animator.SetFloat("LastY", lastDirection.y);
        animator.SetBool("isMoving", movement != Vector3.zero);
    }

    private Vector3 GetMovementDirection()
{
    if (movement.sqrMagnitude > 0) // Если персонаж движется
    {
        lastDirection = movement.normalized; // Запоминаем направление
    }

    return lastDirection;
}
}
