using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Transform cameraPivot;
    public TextMeshProUGUI UIText;
    public GameObject retryBtn;
    public Transform startPos;
    public float speed;

    Dictionary<Door, bool> doorStates = new Dictionary<Door, bool>(); // �� ���� ���� ���θ� ����

    int hasKeyCount = 0;

    float rotationDuration = 1f; // ȸ�� �ð�
    float rotationTimer; // ȸ�� Ÿ�̸�

    bool isRotating; // ȸ�� �� ����
    bool gameOver = false;

    Quaternion startRotation; // ���� ȸ�� ����
    Quaternion targetRotation; // ��ǥ ȸ�� ����

    private void Awake()
    {
        transform.position = new Vector3(startPos.position.x, 1f, startPos.position.z);
        InitializeDoor();
    }

    void InitializeDoor()
    {
        // Door �±׸� ���� ��� ���� ������Ʈ�� ã���ϴ�.
        GameObject[] foundDoors = GameObject.FindGameObjectsWithTag("Door");

        // �� ���� ������Ʈ�� Door ������Ʈ�� �ִٸ� ����Ʈ�� �߰��մϴ�.
        foreach (GameObject doorObject in foundDoors)
        {
            Door doorComponent = doorObject.GetComponent<Door>();
            if (doorComponent != null)
            {
                doorStates.Add(doorComponent, false); // �� ���� �ʱ� ���¸� false(����)�� ����
            }
        }
    }

    void Update()
    {
        if (!gameOver)
        {
            PlayerMove();
            CameraInput();
            CameraRotateAnimation();
        }
    }
    
    void PlayerMove()
    {
        float hInput = 0; // �¿� �Է�
        float vInput = 0; // ���� �Է�

        if (Input.GetKey(KeyCode.W))
        {
            vInput = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            vInput = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            hInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            hInput = 1f;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteractWithDoor();
        }

        // ī�޶��� ���� ���͸� �������� �̵� ���� ����
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f; // ���� �̵��̹Ƿ� y���� 0���� ����
        cameraForward.Normalize(); // ����ȭ

        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0f; // ���� �̵��̹Ƿ� y���� 0���� ����
        cameraRight.Normalize(); // ����ȭ

        // �̵� ���� ���� ���
        Vector3 moveDir = cameraForward * vInput + cameraRight * hInput;
        moveDir.Normalize(); // ����ȭ

        // �÷��̾� �̵�
        transform.Translate(moveDir * speed * Time.deltaTime, Space.World);

        // �̵� �������� ȸ��
        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
    void TryInteractWithDoor()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1f))
        {
            if (hit.collider.CompareTag("Door"))
            {
                Door doorComponent = hit.collider.GetComponent<Door>();
                if (doorComponent != null && doorStates.ContainsKey(doorComponent))
                {
                    if (doorStates[doorComponent] || hasKeyCount > 0) // ���� �����ְų� ���踦 ������ �ִ� ���
                    {
                        doorComponent.ChangeDoorState();
                        doorStates[doorComponent] = !doorStates[doorComponent]; // �� ���� ������Ʈ

                        if (doorStates[doorComponent] && hasKeyCount > 0)
                        {
                            hasKeyCount--; // ���� ���Ȱ� ���踦 ����� ���, ���� ���� ����
                        }
                    }
                }
            }
        }
    }
    
    void CameraInput()
    {
        if (Input.GetKeyDown(KeyCode.O) && !isRotating)
        {
            RotateCamera(90);
        }
        else if (Input.GetKeyDown(KeyCode.P) && !isRotating)
        {
            RotateCamera(-90);
        }
    }

    void CameraRotateAnimation()
    {
        // ȸ�� ���� ���� ȸ�� �ִϸ��̼� ����
        if (isRotating)
        {
            rotationTimer += Time.deltaTime;
            float t = Mathf.Clamp01(rotationTimer / rotationDuration);
            cameraPivot.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            // ȸ�� �߿� �÷��̾ �Բ� ȸ��
            transform.rotation = cameraPivot.transform.rotation;

            if (rotationTimer >= rotationDuration)
            {
                isRotating = false;
            }
        }
    }

    void RotateCamera(float angle)
    {
        startRotation = cameraPivot.transform.rotation;
        targetRotation = Quaternion.Euler(0, angle, 0) * startRotation;
        isRotating = true;
        rotationTimer = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EndPoint"))
        {
            UIText.gameObject.SetActive(true);
            UIText.text = "Game Clear !";
            retryBtn.gameObject.SetActive(true);
            gameOver = true;
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            UIText.gameObject.SetActive(true);
            UIText.text = "Game Over !";
            retryBtn.gameObject.SetActive(true);
            gameOver = true;
        }
        if (other.gameObject.CompareTag("Key"))
        {
            hasKeyCount++;
            other.gameObject.SetActive(false);
        }
    }

    public void RetryGame()
    {
        SceneManager.LoadScene("JailBreak");
    }
    public bool IsGameOver()
    {
        return gameOver;
    }
}
