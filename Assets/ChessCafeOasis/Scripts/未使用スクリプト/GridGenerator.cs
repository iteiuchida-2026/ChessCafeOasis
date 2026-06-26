using UnityEngine;

//////// スクリプトの説明：【マスオブジェクトをチェス盤の形に整列して敷き詰める】////////
public class GridGenerator : MonoBehaviour
{
    public GameObject SquarePrefab; // 整列させるマスオブジェクト
    public int gridCount = 8;       // 8×8
    public float spacing = 0f;    // オブジェクト間の間隔（今回は0とする）

    void Awake()
    {
        Vector3 size = SquarePrefab.GetComponent<Renderer>().bounds.size;
        Debug.Log($"マスのサイズは{size}です");
    }

    void Start()
    {
        for (int x = 0; x < gridCount; x++)
        {
            for (int z = 0; z < gridCount; z++)
            {
                // X と Z の位置を計算
                Vector3 spawnPos = new Vector3(x, 0, z);
                Instantiate(SquarePrefab, spawnPos, Quaternion.identity);
            }
        }
    }
}