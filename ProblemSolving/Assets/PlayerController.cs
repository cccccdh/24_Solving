using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Camera mainCamera;

    public float rotationAmount = 90f; // ȸ�� ����
    public float rotationDuration = 1f; // ȸ�� �ð�

    private bool isRotating; // ȸ�� �� ����
    private float targetAngle;
    private float rotationTimer; // ȸ�� Ÿ�̸�
    private Quaternion startRotation; // ���� ȸ�� ����
    private Quaternion targetRotation; // ��ǥ ȸ�� ����
    private Vector3 rotationCenter; // ȸ�� �߽� ��ġ


    void Start()
    {
        rotationCenter = Vector3.zero; // ������ ȸ�� �߽����� ����

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isRotating)
        {
            RotatePlayer(90);
        }
        else if (Input.GetKeyDown(KeyCode.E) && !isRotating)
        {
            RotatePlayer(-90);
        }

        if (isRotating)
        {
            // ȸ�� Ÿ�̸� ����
            rotationTimer += Time.deltaTime;

            float t = Mathf.Clamp01(rotationTimer / rotationDuration);
            mainCamera.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            // ȸ���� ������ ��
            if (rotationTimer >= rotationDuration)
            {
                isRotating = false;
            }
        }


        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    void RotatePlayer(float angle)
    {
        // ������ �߽����� ȸ���ϱ� ���� ī�޶��� ���� ��ġ�� ȸ�� �߽����� ����
        rotationCenter = mainCamera.transform.position;

        // ���� ȸ�� ������ identity�� �����Ͽ� ������ �߽����� ȸ��
        startRotation = Quaternion.identity;

        // ��ǥ ȸ�� ���� ���� (y�� �������� ȸ��)
        targetRotation = Quaternion.Euler(0, angle, 0);

        isRotating = true;
        rotationTimer = 0f;
    }

}
