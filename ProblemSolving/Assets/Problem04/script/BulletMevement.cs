using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float BulletSpeed;
    public float BulletRotationSpeed;
    public Vector3 BulletDirection;
    public DataStrucuture.Stack<GameObject> list;

    // Start is called before the first frame update
    void Start()
    {
        //Init(new Vector3(0,0,0), null);
        BulletDirection.Normalize();
    }

    public void Init(Vector3 InitialPos, DataStrucuture.Stack<GameObject> stack)
    {
        gameObject.transform.position = InitialPos;
        list = stack;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += BulletDirection * BulletSpeed;


        transform.Rotate(BulletDirection, BulletRotationSpeed);

        // 현재 오브젝트의 위치를 기준으로 주변에 있는 모든 Collider와의 충돌을 검사
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(0.5f, 0.5f, 0.5f));

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Red"))
            {
                // 충돌한 물체가 플레이어일 때, 현재 오브젝트를 파괴
                gameObject.SetActive(false);
                list.Push(gameObject);
                break; // 더 이상 검사하지 않음
            }
        }


    }
}
