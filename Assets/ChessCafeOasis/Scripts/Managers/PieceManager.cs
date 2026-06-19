using UnityEngine;

//////// スクリプトの説明：【3D駒オブジェクトの管理を担当。初期配置位置にPrefabを生成、初期化。移動処理。】////////

public class PieceManager : MonoBehaviour
{
    [Header("各駒のPrefab")]
    [SerializeField] private GameObject WhitePawnPrefab;
    [SerializeField] private GameObject WhiteKnightPrefab;
    [SerializeField] private GameObject WhiteBishopPrefab;
    [SerializeField] private GameObject WhiteRookPrefab;
    [SerializeField] private GameObject WhiteQueenPrefab;
    [SerializeField] private GameObject WhiteKingPrefab;
    [SerializeField] private GameObject BlackPawnPrefab;
    [SerializeField] private GameObject BlackKnightPrefab;
    [SerializeField] private GameObject BlackBishopPrefab;
    [SerializeField] private GameObject BlackRookPrefab;
    [SerializeField] private GameObject BlackQueenPrefab;
    [SerializeField] private GameObject BlackKingPrefab;

    [Header("3Dチェス盤情報の取得先")]
    [SerializeField] private ChessBoardManager chessBoardManager;

    [Header("駒を生成する際のY軸（高さ）調整値")]
    [SerializeField] private float spawnY_Offset = 0.5f;

    private void Start()
    {
        InitializeAllPiece();
    }

    // ▼初期配置位置に各ピースを配置するメソッド
    private void InitializeAllPiece()
    {
        SpawnPiece(WhiteRookPrefab, 0, 0);
        SpawnPiece(WhiteBishopPrefab, 0, 1);
        SpawnPiece(WhiteKnightPrefab, 0, 2);
        SpawnPiece(WhiteQueenPrefab, 0, 3);
        SpawnPiece(WhiteKingPrefab, 0, 4);
        SpawnPiece(WhiteBishopPrefab, 0, 5);
        SpawnPiece(WhiteKnightPrefab, 0, 6);
        SpawnPiece(WhiteRookPrefab, 0, 7);

        SpawnPiece(BlackRookPrefab, 7, 0);
        SpawnPiece(BlackBishopPrefab, 7, 1);
        SpawnPiece(BlackKnightPrefab, 7, 2);
        SpawnPiece(BlackQueenPrefab, 7, 3);
        SpawnPiece(BlackKingPrefab, 7, 4);
        SpawnPiece(BlackBishopPrefab, 7, 5);
        SpawnPiece(BlackKnightPrefab, 7, 6);
        SpawnPiece(BlackRookPrefab, 7, 7);

        for (int y = 0; y < 8; y++)
        {
            SpawnPiece(WhitePawnPrefab, 1, y);
            SpawnPiece(BlackPawnPrefab, 6, y);
        }
    }

    // ▼駒を生成するメソッド
    public void SpawnPiece(GameObject piecePrefab, int x, int y)
    {
        GameObject targetSquare = chessBoardManager.GetPieceAtTileObjectsArray(x, y);
        if (targetSquare != null) Debug.Log("targetSquareがnullです");

        Vector3 spawnPosition = targetSquare.transform.position;

        spawnPosition.y += spawnY_Offset; // y軸の高さを調整してめり込まないようにする

        GameObject spawnedPiece = Instantiate(piecePrefab, spawnPosition, Quaternion.identity);
    }
}
