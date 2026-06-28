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
    [SerializeField] private float spawnY_Offset = 0f;

    //private void Start()
    //{
    //    InitializePieceObject();
    //}

    // ▼初期配置位置に各ピースを配置するメソッド
    // ★盤面の値に留意
    public void InitializePieceObject()
    {
        SpawnPiece(WhiteRookPrefab, 0, 7);
        SpawnPiece(WhiteKnightPrefab, 1, 7);
        SpawnPiece(WhiteBishopPrefab, 2, 7);
        SpawnPiece(WhiteQueenPrefab, 3, 7);
        SpawnPiece(WhiteKingPrefab, 4, 7);
        SpawnPiece(WhiteBishopPrefab, 5, 7);
        SpawnPiece(WhiteKnightPrefab, 6, 7);
        SpawnPiece(WhiteRookPrefab, 7, 7);

        SpawnPiece(BlackRookPrefab, 0, 0);
        SpawnPiece(BlackKnightPrefab, 1, 0);
        SpawnPiece(BlackBishopPrefab, 2, 0);
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
        TileController targetSquare = chessBoardManager.GetPieceAtTileBoard(x, y);
        if (targetSquare != null)
        {
            Vector3 spawnPosition = targetSquare.transform.position;

            spawnPosition.y += spawnY_Offset; // y軸の高さを調整してめり込まないようにする

            GameObject spawnedPiece = Instantiate(piecePrefab, spawnPosition, piecePrefab.transform.rotation);

            //一度削除
            //spawnedPiece.transform.SetParent(targetSquare.transform); // 生成した駒をマスの子要素にする

            spawnedPiece.GetComponent<Piece>().CurrentIndex = targetSquare.BoardIndex; // 生成時に対象マスのインデックスを現在位置を設定

            chessBoardManager.RegisterPiece(x, y, spawnedPiece.GetComponent<Piece>());
        }
    }

    // ▼駒のオブジェクトの移動メソッド
    public void AnimateMove(Piece piece, Vector2Int to)
    {
        Debug.Log($"[AnimateMove] Called: piece={piece.name}, to={to}");

        TileController destinationSquare = chessBoardManager.GetPieceAtTileBoard(to.x, to.y); // 移動先のタイルを取得

        Debug.Log($"[AnimateMove] destinationSquare is {(destinationSquare == null ? "null" : "not null")}");

        Piece destinationEnemyPiece = chessBoardManager.GetPieceAtPieceObjectBoard(to); // 移動先の敵の駒を取得

        if (destinationSquare != null)
        {
            Vector3 movePosition = destinationSquare.transform.position; // 移動先の座標を取得

            Debug.Log($"[AnimateMove] movePosition before offset: {movePosition}");

            movePosition.y += spawnY_Offset;

            Debug.Log($"[AnimateMove] movePosition after offset: {movePosition}");

            // 移動先に敵駒がある場合のみ OnTaken() を実行
            // ただし、移動する駒自身ではないことを確認
            if (destinationEnemyPiece != null && destinationEnemyPiece != piece)
            {
                Debug.Log($"敵駒 {destinationEnemyPiece.name} を捕獲しました。");
                destinationEnemyPiece.OnTaken(); // 移動先の敵の駒を削除処理
            }

            Debug.Log($"[AnimateMove] Setting piece.transform.position to {movePosition}");
            piece.transform.position = movePosition;
            Debug.Log($"[AnimateMove] piece.transform.position is now {piece.transform.position}");

            piece.Move(to); // 駒側で現在位置の更新とHasMovedフラグをOnにする
            Debug.Log($"[AnimateMove] piece.Move({to}) called");
        }
        else
        {
            Debug.LogError("[AnimateMove] destinationSquare is null! Movement not executed.");
        }
    }
}
