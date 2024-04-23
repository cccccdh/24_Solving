using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Camera mainCamera;

    public float rotationAmount = 90f; // 회전 각도
    public float rotationDuration = 1f; // 회전 시간

    private bool isRotating; // 회전 중 여부
    private float targetAngle;
    private float rotationTimer; // 회전 타이머
    private Quaternion startRotation; // 시작 회전 각도
    private Quaternion targetRotation; // 목표 회전 각도
    private Vector3 rotationCenter; // 회전 중심 위치


    void Start()
    {
        rotationCenter = Vector3.zero; // 원점을 회전 중심으로 설정

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
            // 회전 타이머 갱신
            rotationTimer += Time.deltaTime;

            float t = Mathf.Clamp01(rotationTimer / rotationDuration);
            mainCamera.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            // 회전이 끝났을 때
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
        // 원점을 중심으로 회전하기 위해 카메라의 현재 위치를 회전 중심으로 설정
        rotationCenter = mainCamera.transform.position;

        // 시작 회전 각도를 identity로 설정하여 원점을 중심으로 회전
        startRotation = Quaternion.identity;

        // 목표 회전 각도 설정 (y축 기준으로 회전)
        targetRotation = Quaternion.Euler(0, angle, 0);

        isRotating = true;
        rotationTimer = 0f;
    }

}
