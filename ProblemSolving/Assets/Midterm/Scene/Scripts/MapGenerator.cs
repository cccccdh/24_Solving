using UnityEngine;

public class MapGererator : MonoBehaviour
{
    public GameObject[] tilePrefabs; // Ÿ�� ������ �迭
    public TextAsset csvFilePath; // CSV ������ ����Ű�� TextAsset

    Vector3 tilePosition;
    GameObject newobj;

    void Start()
    {
        LoadMapFromCSV();
    }

    public void LoadMapFromCSV()
    {
        if (csvFilePath == null)
        {
            Debug.LogError("CSV file is not assigned!");
            return;
        }

        // CSV ������ ������ ���ڿ��� ��������
        string[] csvLines = csvFilePath.text.Split('\n');

        // CSV �����ͷ� �� ����
        for (int z = 0; z < csvLines.Length; z++)
        {
            string[] values = csvLines[z].Split(',');

            for (int x = 0; x < values.Length; x++)
            {
                int tileIndex;
                if (int.TryParse(values[x], out tileIndex) && tileIndex >= 0 && tileIndex < tilePrefabs.Length)
                {
                    GameObject tilePrefab = tilePrefabs[tileIndex];

                    // Ÿ�� �������� �������� �о�ͼ� Ÿ�� ũ��� ���
                    Vector3 tileSize = tilePrefab.transform.localScale;

                    switch (tileIndex)
                    {
                        case 0:
                            break;
                        case 1:
                            tilePosition = new Vector3(x * tileSize.x - 25f, 0.5f, z * tileSize.z - 25f);
                            break;
                        case 2:
                            // Ÿ�� ���� �� ��ġ ����
                            tilePosition = new Vector3(x * tileSize.x - 25f, 1f, z * tileSize.z - 25f);
                            break;
                    }
                    newobj = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                    newobj.transform.SetParent(transform);
                }
                else
                {
                }
            }
        }
    }
}
