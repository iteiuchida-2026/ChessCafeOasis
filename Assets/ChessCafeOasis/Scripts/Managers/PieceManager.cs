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
    // ★盤面の値に留意
    private void InitializeAllPiece()
    {
        SpawnPiece(WhiteRookPrefab, 0, 7);
        SpawnPiece(WhiteBishopPrefab, 1, 7);
        SpawnPiece(WhiteKnightPrefab, 2, 7);
        SpawnPiece(WhiteQueenPrefab, 3, 7);
        SpawnPiece(WhiteKingPrefab, 4, 7);
        SpawnPiece(WhiteBishopPrefab, 5, 7);
        SpawnPiece(WhiteKnightPrefab, 6, 7);
        SpawnPiece(WhiteRookPrefab, 7, 7);

        SpawnPiece(BlackRookPrefab, 0, 0);
        SpawnPiece(BlackBishopPrefab, 1, 0);
        SpawnPiece(BlackKnightPrefab, 2, 0);
        SpawnPiece(BlackQueenPrefab, 3, 0);
        SpawnPiece(BlackKingPrefab, 4, 0);
        SpawnPiece(BlackBishopPrefab, 5, 0);
        SpawnPiece(BlackKnightPrefab, 6, 0);
        SpawnPiece(BlackRookPrefab, 7, 0);

        for (int x = 0; x < 8; x++)
        {
            SpawnPiece(WhitePawnPrefab, x, 6);
            SpawnPiece(BlackPawnPrefab, x, 1);
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
