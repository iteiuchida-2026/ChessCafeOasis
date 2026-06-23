using System.Collections.Generic;
using UnityEngine;

//////// スクリプトの説明：【チェスのルール審判係。主に移動における判定を担当する】////////


public class ChessRuleReferee : MonoBehaviour
{
    [Header("データ層2次元配列の参照先")]
    [SerializeField] private ChessBoardManager chessBoardManager;

    [Header("直前の移動履歴の参照先")]
    [SerializeField] private RecordManager recordManager;

    // クラス内で共有する変数を宣言
    private string _myColor;
    private Vector2Int _currentPos;
    private Vector2Int _nextPos;
    private List<Vector2Int> _baseMoveVectors;

    // ▼駒の移動の判定、移動時の障害物の有無、自殺手のチェックを行うメソッド。
    // 移動可能ならtrueを返す
    public bool IsValidMove(Piece piece, Vector2Int targetPos)
    {
        _baseMoveVectors = piece.GetMoveVectors(); // 3Dデータの各駒クラスから移動ベクトルの定義を取得
        _currentPos = piece.CurrentIndex; // 3Dデータの駒の現在位置を駒の保持情報より取得
        _myColor = piece.PieceColor; // 3Dデータの駒から色を取得

        if (piece.PieceType == PieceType.Pawn.ToString()) // 駒がポーンの場合の処理
        {
            return CheckPawnMove(piece, targetPos);
        }

        foreach (Vector2Int baseMoveVector in _baseMoveVectors)
        {
            if (piece.IsRangedPiece())
            {
                // ①ビショップ、ルーク、クイーンの走査
                for (int i = 1; i < 8; i++)
                {
                    _nextPos = _currentPos + (baseMoveVector * i);

                    // 盤面外ならこの方向の走査を終了
                    if (!IsWithinBoard(_nextPos)) break;

                    if (_nextPos == targetPos)
                    {
                        // ターゲットマスが空、または敵の駒なら移動可能
                        return IsTileEmpty(_nextPos) || IsEnemyPiece(_myColor, _nextPos);
                    }

                    // 途中に駒がある場合はその方向にそれ以上進めない
                    if (!IsTileEmpty(_nextPos)) break;
                }
            }
            else
            {
                // ②ナイト、キングの走査
                NotRangedPieceMoveCheck(baseMoveVector, targetPos);
            }
        }
        return false;
    }

    // ▼ポーンの移動判定
    public bool CheckPawnMove(Piece pawn, Vector2Int targetPos)
    {
        foreach (Vector2Int baseMoveVector in _baseMoveVectors)
        {
            if (pawn.HasMoved == true) NotRangedPieceMoveCheck(baseMoveVector, targetPos); // 既に移動していたら1マスしか進めない
            else
            {
                Vector2Int pawnFirstMoveVector = baseMoveVector + baseMoveVector; // まだ動いていないなら2マス進める
                NotRangedPieceMoveCheck(pawnFirstMoveVector, targetPos);
            }
        }
        return false;
    }

    // ナイト、キングの移動走査用メソッド
    private bool NotRangedPieceMoveCheck(Vector2Int baseMoveVector, Vector2Int targetPos)
    {
        _nextPos = _currentPos + baseMoveVector;
        if (_nextPos == targetPos && IsWithinBoard(_nextPos))
        {
            return IsTileEmpty(_nextPos) || IsEnemyPiece(_myColor, _nextPos);
        }
        return false;
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
    // 今回、駒の色データ等は3Dのオブジェクトにアタッチされている。
    // データ層の2次元配列と3D側の2次元配列からそれぞれ参照しているが、後から問題になる可能性があるため注意する
    private bool IsEnemyPiece(string myColor, Vector2Int pos)
    {
        GameObject targetGameObject = chessBoardManager.GetPieceAtRealLayer(pos); // 3Dデータ層から対象マスのピースを取得
        string pieceColor = targetGameObject.GetComponent<Piece>().PieceColor; // ピースの色を取得

        // 色が最初に選択した駒と同じ色でない場合はtrueを返し、敵（相手）の駒を判断する。
        if (pieceColor != myColor)
        {
            return true;
        }
        return false;
    }



}
