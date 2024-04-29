using UnityEngine;

public class Enemy : MonoBehaviour
{
    PatrolArea Area; // ���ƴٴ� ������ ������ �ִ� �� ������Ʈ
    Transform player; // �÷��̾�
    Camera enemyCamera;
    PlayerController playerCtrol;
    Rigidbody rb;
    LayerMask Wall;

    public Vector3 targetPosition; // ��ǥ ��ġ

    float moveSpeed = 3f;
    float elapsedTime = 0f;
    float turnDuration = 3f;
    bool IsTurning = false;

    void Start()
    {
        Area = GetComponentInParent<PatrolArea>();
        player = GameObject.FindWithTag("Player").transform;
        enemyCamera = GetComponentInChildren<Camera>();
        playerCtrol = FindObjectOfType<PlayerController>();
        rb = GetComponent<Rigidbody>();
        Wall = LayerMask.GetMask("Wall");
        SetNewTargetPosition(); // ó�� ������ �� ��ǥ ��ġ�� ����
    }

    void Update()
    {
        if (!playerCtrol.IsGameOver())
        {
            // �÷��̾ �þ߿� �ִٸ�
            if (IsPlayerInCameraView())
            {
                // �÷��̾ �i�ư���
                ChasePlayer();

                // ���� �þ� �ȿ� �÷��̾ ���ٸ�
                if (!IsPlayerInCameraView())
                {
                    IsTurning = true;
                }
            }
            else // �÷��̾ �þ߿� ���ٸ�
            {
                if (IsTurning)
                {
                    TurnAround();
                }
                else
                {
                    // ������ ��ġ�� �̵�
                    Patrol();
                }
            }

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
            {
                if (hit.collider != null && hit.collider.CompareTag("Wall"))
                {
                    // ���� �������� ���� ó��
                    SetNewTargetPosition();
                }
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
        }     
    }


    // enemyCamera�� �������� ���� �÷��̾ �ִ��� Ȯ���ϴ� �޼���
    bool IsPlayerInCameraView()
    {
        // �÷��̾��� ��� ���ڸ� �����ɴϴ�.
        Bounds playerBounds = player.GetComponent<Renderer>().bounds;

        // enemyCamera�� ���������� �����ɴϴ�.
        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(enemyCamera);

        // �÷��̾��� ��� ���ڰ� ī�޶��� �������� ���� �ִ��� Ȯ���մϴ�.
        if (GeometryUtility.TestPlanesAABB(frustumPlanes, playerBounds))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // ���ο� ��ǥ ��ġ ����
    void SetNewTargetPosition()
    {
        float x = Random.Range(-Area.patrolRange.x / 2, Area.patrolRange.x / 2);
        float z = Random.Range(-Area.patrolRange.z / 2, Area.patrolRange.z / 2);
        targetPosition = Area.transform.position + new Vector3(x, 0, z);
    }

    // Ư�� ���� ���� ���ƴٴϴ� �޼���
    void Patrol()
    {
        // ��ǥ ��ġ���� �̵�
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);

        // ��ǥ ��ġ�� �����ϸ� ���ο� ��ǥ ��ġ ����
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNewTargetPosition();
        }

        // ��ǥ ������ �ٶ󺸵��� ȸ��
        transform.LookAt(targetPosition);
    }

    // �÷��̾ �Ѿư��� �޼���
    void ChasePlayer()
    {
        // �÷��̾� �������� �̵�
        transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * moveSpeed);

        // �÷��̾� ������ �ٶ󺸵��� ȸ��
        transform.LookAt(player.position);
    }

    void TurnAround()
    {
        elapsedTime += Time.deltaTime;

        // 90�� �������� ȸ�� (3��)
        if (elapsedTime <= turnDuration)
        {
            transform.Rotate(0f, -90f / turnDuration * Time.deltaTime, 0f);
        }
        // 180�� �������� ȸ�� (3��)
        else if (elapsedTime > turnDuration && elapsedTime <= turnDuration * 2)
        {
            transform.Rotate(0f, 180f / turnDuration * Time.deltaTime, 0f);
        }

        // ȸ���� �Ϸ�Ǿ��� ��
        if (elapsedTime >= turnDuration * 2)
        {
            IsTurning = false; // �θ��� �Ÿ��� ����
            elapsedTime = 0f; // elapsedTime �ʱ�ȭ
        }
    }
}