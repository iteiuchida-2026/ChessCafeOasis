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
    [SerializeField] private float SpawnY_Offset = 0.5f;

    private void Start()
    {
        InitializeAllPiece();
    }

    private void InitializeAllPiece()
    {
        GameObject a1 = chessBoardManager.GetPieceAtTileObjectsArray(0, 0);
    }
}
