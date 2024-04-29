using UnityEngine;

public class MapGererator : MonoBehaviour
{
    public GameObject[] tilePrefabs; // 타일 프리팹 배열
    public TextAsset csvFilePath; // CSV 파일을 가리키는 TextAsset

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

        // CSV 파일의 내용을 문자열로 가져오기
        string[] csvLines = csvFilePath.text.Split('\n');

        // CSV 데이터로 맵 생성
        for (int z = 0; z < csvLines.Length; z++)
        {
            string[] values = csvLines[z].Split(',');

            for (int x = 0; x < values.Length; x++)
            {
                int tileIndex;
                if (int.TryParse(values[x], out tileIndex) && tileIndex >= 0 && tileIndex < tilePrefabs.Length)
                {
                    GameObject tilePrefab = tilePrefabs[tileIndex];

                    // 타일 프리팹의 스케일을 읽어와서 타일 크기로 사용
                    Vector3 tileSize = tilePrefab.transform.localScale;

                    switch (tileIndex)
                    {
                        case 0:
                            break;
                        case 1:
                            tilePosition = new Vector3(x * tileSize.x - 25f, 0.5f, z * tileSize.z - 25f);
                            break;
                        case 2:
                            // 타일 생성 및 위치 설정
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
