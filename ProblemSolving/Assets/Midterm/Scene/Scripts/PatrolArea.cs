using UnityEngine;

public class PatrolArea : MonoBehaviour
{
    public Vector3 patrolRange; // ���ƴٴ� ������ ũ��

    // ������ �ð������� ��Ÿ���� ���� �޼���
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, patrolRange);
    }
}
