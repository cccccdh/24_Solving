using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StaticMeshGen))]
public class StaticMeshGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StaticMeshGen script = (StaticMeshGen)target;

        if (GUILayout.Button("Generate Star Mesh"))
        {
            script.GenerateStarMesh();
        }
    }
}

public class StaticMeshGen : MonoBehaviour
{
    public void GenerateStarMesh()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[]
        {
            new Vector3 (0.00f, 0.00f, 1.00f),
            new Vector3 (0.00f, 1.00f, 1.00f),
            new Vector3 (0.225f, 0.30f, 1.00f),
            new Vector3 (0.95f, 0.30f, 1.00f),
            new Vector3 (0.36f, -0.12f, 1.00f),
            new Vector3 (0.58f, -0.80f, 1.00f),
            new Vector3 (0.00f, -0.38f, 1.00f),
            new Vector3 (-0.58f, -0.80f, 1.00f),
            new Vector3 (-0.36f, -0.12f, 1.00f),
            new Vector3 (-0.95f, 0.30f, 1.00f),
            new Vector3 (-0.225f, 0.30f, 1.00f),

            new Vector3 (0.00f, 0.00f, 3.00f),
            new Vector3 (0.00f, 1.00f, 3.00f),
            new Vector3 (0.225f, 0.30f, 3.00f),
            new Vector3 (0.95f, 0.30f, 3.00f),
            new Vector3 (0.36f, -0.12f, 3.00f),
            new Vector3 (0.58f, -0.80f, 3.00f),
            new Vector3 (0.00f, -0.38f, 3.00f),
            new Vector3 (-0.58f, -0.80f, 3.00f),
            new Vector3 (-0.36f, -0.12f, 3.00f),
            new Vector3 (-0.95f, 0.30f, 3.00f),
            new Vector3 (-0.225f, 0.30f, 3.00f),
        };

        mesh.vertices = vertices;

        int[] triangleIndices = new int[]
        {
            0,1,2, 0,2,3, 0,3,4, 0,4,5, 0,5,6, 0,6,7, 0,7,8, 0,8,9, 0,9,10, 0,10,1,

            11,13,12, 11,14,13, 11,15,14, 11,16,15, 11,17,16, 11,18,17, 11,19,18, 11,20,19, 11,21,20, 11,12,21,

            1,12,13, 1,13,2, 2,13,14, 2,14,3, 3,14,15, 3,15,4, 4,15,16, 4,16,5, 5,16,17, 5,17,6,

            6,17,18, 6,18,7, 7,18,19, 7,19,8, 8,19,20, 8,20,9, 9,20,21, 9,21,10, 1,10,12, 12,10,21


        };

        mesh.triangles = triangleIndices;

        if (this.GetComponent<MeshFilter>() != null)
        {
            MeshFilter abc = this.GetComponent<MeshFilter>();
            DestroyImmediate(abc);
        }
        MeshFilter mf = gameObject.AddComponent<MeshFilter>();
        MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();

        mf.mesh = mesh;

        // Add material with yellow color
        mr.material = new Material(Shader.Find("Standard"));
        mr.material.color = Color.yellow;
    }
}