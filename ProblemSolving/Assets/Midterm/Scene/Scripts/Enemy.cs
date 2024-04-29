using UnityEngine;

public class Enemy : MonoBehaviour
{
    PatrolArea Area; // 돌아다닐 범위를 가지고 있는 빈 오브젝트
    Transform player; // 플레이어
    Camera enemyCamera;
    PlayerController playerCtrol;
    Rigidbody rb;
    LayerMask Wall;

    public Vector3 targetPosition; // 목표 위치

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
        SetNewTargetPosition(); // 처음 시작할 때 목표 위치를 설정
    }

    void Update()
    {
        if (!playerCtrol.IsGameOver())
        {
            // 플레이어가 시야에 있다면
            if (IsPlayerInCameraView())
            {
                // 플레이어를 쫒아가기
                ChasePlayer();

                // 만약 시야 안에 플레이어가 없다면
                if (!IsPlayerInCameraView())
                {
                    IsTurning = true;
                }
            }
            else // 플레이어가 시야에 없다면
            {
                if (IsTurning)
                {
                    TurnAround();
                }
                else
                {
                    // 랜덤한 위치로 이동
                    Patrol();
                }
            }

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
            {
                if (hit.collider != null && hit.collider.CompareTag("Wall"))
                {
                    // 벽을 감지했을 때의 처리
                    SetNewTargetPosition();
                }
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
        }     
    }


    // enemyCamera의 프러스텀 내에 플레이어가 있는지 확인하는 메서드
    bool IsPlayerInCameraView()
    {
        // 플레이어의 경계 상자를 가져옵니다.
        Bounds playerBounds = player.GetComponent<Renderer>().bounds;

        // enemyCamera의 프러스텀을 가져옵니다.
        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(enemyCamera);

        // 플레이어의 경계 상자가 카메라의 프러스텀 내에 있는지 확인합니다.
        if (GeometryUtility.TestPlanesAABB(frustumPlanes, playerBounds))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // 새로운 목표 위치 설정
    void SetNewTargetPosition()
    {
        float x = Random.Range(-Area.patrolRange.x / 2, Area.patrolRange.x / 2);
        float z = Random.Range(-Area.patrolRange.z / 2, Area.patrolRange.z / 2);
        targetPosition = Area.transform.position + new Vector3(x, 0, z);
    }

    // 특정 범위 내를 돌아다니는 메서드
    void Patrol()
    {
        // 목표 위치까지 이동
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);

        // 목표 위치에 도착하면 새로운 목표 위치 설정
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNewTargetPosition();
        }

        // 목표 방향을 바라보도록 회전
        transform.LookAt(targetPosition);
    }

    // 플레이어를 쫓아가는 메서드
    void ChasePlayer()
    {
        // 플레이어 방향으로 이동
        transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * moveSpeed);

        // 플레이어 방향을 바라보도록 회전
        transform.LookAt(player.position);
    }

    void TurnAround()
    {
        elapsedTime += Time.deltaTime;

        // 90도 좌측으로 회전 (3초)
        if (elapsedTime <= turnDuration)
        {
            transform.Rotate(0f, -90f / turnDuration * Time.deltaTime, 0f);
        }
        // 180도 우측으로 회전 (3초)
        else if (elapsedTime > turnDuration && elapsedTime <= turnDuration * 2)
        {
            transform.Rotate(0f, 180f / turnDuration * Time.deltaTime, 0f);
        }

        // 회전이 완료되었을 때
        if (elapsedTime >= turnDuration * 2)
        {
            IsTurning = false; // 두리번 거리기 종료
            elapsedTime = 0f; // elapsedTime 초기화
        }
    }
}