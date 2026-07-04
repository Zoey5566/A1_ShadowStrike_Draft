using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [Header("移动设置")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("视角设置")]
    public float mouseSensitivity = 2f;
    public float verticalLookLimit = 80f;   // 上下视角限制（度）

    [Header("相机引用")]
    public Transform cameraTransform;       // 若不赋值，会自动找 Camera.main

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float verticalRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null)
        {
            if (Camera.main != null)
                cameraTransform = Camera.main.transform;
            else
                Debug.LogError("未找到主相机，请在 Inspector 中手动指定 cameraTransform！");
        }
    }

    void Update()
    {
        // -------------------- 鼠标锁定/解锁 --------------------
        if (Input.GetMouseButtonDown(1))    // 右键按下
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (Input.GetMouseButtonUp(1))      // 右键松开
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // -------------------- 视角旋转（仅锁定状态） --------------------
        if (Cursor.lockState == CursorLockMode.Locked && cameraTransform != null)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            // 水平旋转 → 旋转玩家自身
            transform.Rotate(Vector3.up * mouseX);

            // 垂直旋转 → 旋转相机，并限制角度
            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);
            cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        }

        // -------------------- 移动输入 --------------------
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = (transform.right * horizontal + transform.forward * vertical).normalized;

        // Shift 加速
        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        // -------------------- 重力与跳跃（可选） --------------------
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;   // 保持贴地

       
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 应用重力
        velocity.y += gravity * Time.deltaTime;

        // 组合最终移动向量（水平 + 垂直）
        Vector3 movement = moveDirection * speed;
        movement.y = velocity.y;

        // 通过 CharacterController 移动
        controller.Move(movement * Time.deltaTime);
    }
}
