using UnityEngine;

public class PatrolArea : MonoBehaviour
{
    public Vector3 patrolRange; // 돌아다닐 범위의 크기

    // 범위를 시각적으로 나타내기 위한 메서드
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, patrolRange);
    }
}
