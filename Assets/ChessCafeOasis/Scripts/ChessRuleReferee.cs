using System.Collections.Generic;
using UnityEngine;

//////// スクリプトの説明：【チェスのルール審判係。主に移動における判定を担当する】////////


public class ChessRuleReferee : MonoBehaviour
{
    [Header("データ層2次元配列の参照先")]
    [SerializeField] private ChessBoardManager chessBoardManager;

    [Header("直前の移動履歴の参照先")]
    [SerializeField] private RecordManager recordManager;


    // ▼駒の移動の判定、移動時の障害物の有無、自殺手のチェックを行うメソッド。
    public bool IsValidMove(Piece piece, Vector2Int targetPos)
    {
        List<Vector2Int> baseMoveVectors = piece.GetMoveVectors(); // 3Dデータの各駒クラスから移動ベクトルの定義を取得
        Vector2Int currentPos = piece.CurrentIndex; // 3Dデータの駒の現在位置を駒の保持情報より取得

        if (piece.PieceType == PieceType.Pawn.ToString()) // 駒がポーンの場合の処理
        {
            return CheckPawnMove(piece, targetPos);
        }

        foreach (Vector2Int baseMoveVector in baseMoveVectors)
        {
            if (piece.IsRangedPiece())
            {
                // ビショップ、ルーク、クイーンの走査
                for (int i = 1; i < 8; i++)
                {
                    Vector2Int nextPos = currentPos + (baseMoveVector * i);

                    // 盤面外ならこの方向の走査を終了
                    if (!IsWithinBoard(nextPos)) break;

                    if (nextPos == targetPos)
                    {
                        string myColor = piece.PieceColor;
                        // ターゲットマスが空、または敵の駒なら移動可能
                        return IsTileEmpty(nextPos) || IsEnemyPiece(myColor, nextPos);
                    }
                }

            }
        }

        return true;
    }

    // ▼ポーンの移動判定
    public bool CheckPawnMove(Piece pawn, Vector2Int targetPos)
    {
        return true;
    }

    // ▼ナイトの移動判定
    public bool CheckKnightMove()
    {
        return true;
    }

    // ▼ビショップの移動判定
    public bool CheckBishopMove()
    {
        return true;
    }

    // ▼ルークの移動判定
    public bool CheckRookMove()
    {
        return true;
    }

    // ▼クイーンの移動判定
    public bool CheckQueenMove()
    {
        return true;
    }

    // ▼キングの移動判定
    public bool CheckKingMove()
    {
        return true;
    }

    // ▼キャスリングの可否判定
    public bool CanCastling() // 引数は後ほど設定
    {
        return true;
    }

    // ▼アンパッサンの可否判定
    public bool CanEnPassant() // 引数は後ほど設定
    {
        return true;
    }

    // ▼プロモーションの可否判定
    public bool CanPromote() // 引数は後ほど設定
    {
        return true;
    }

    // ▼キングのチェック判定
    public bool IsKingInCheck() // 引数は後ほど設定
    {
        return true;
    }

    // ▼盤面外かチェックするメソッド
    private bool IsWithinBoard(Vector2Int pos) => pos.x >= 0 && pos.x < 8 && pos.y >= 0 && pos.y < 8;

    // ▼マスが空かチェックするメソッド
    private bool IsTileEmpty(Vector2Int pos) => chessBoardManager.GetPieceAtDataLayer(pos) == ChessPieceType_DataLayer.None;

    // ▼駒が敵の駒かどうかチェックするメソッド
    private bool IsEnemyPiece(string myColor, Vector2Int pos)
    {
        GameObject targetGameObject = chessBoardManager.GetPieceAtRealLayer(pos);
        string pieceColor = targetGameObject.GetComponent<Piece>().PieceColor;

        if (pieceColor != myColor)
        {
            return true;
        }
        return false;
    }



}
