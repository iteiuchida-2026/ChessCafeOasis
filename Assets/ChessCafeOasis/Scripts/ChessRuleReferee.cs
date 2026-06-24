using System.Collections.Generic;
using UnityEngine;

//////// スクリプトの説明：【チェスのルール審判係。主に移動における判定を担当する】////////

// ★アンパッサン、プロモーション、キャスリングは未処理


public class ChessRuleReferee : MonoBehaviour
{
    [Header("データ層2次元配列の参照先")]
    [SerializeField] private ChessBoardManager chessBoardManager;

    [Header("直前の移動履歴の参照先")]
    [SerializeField] private RecordManager recordManager;

    // クラス内で共有する変数を宣言
    private PieceColor _myColor;
    private Vector2Int _currentPos;
    private Vector2Int _nextPos;
    private List<Vector2Int> _baseMoveVectors;

    // ▼駒の移動の判定、移動時の障害物の有無、自殺手のチェックを行うメソッド。
    // 移動可能ならtrueを返す
    public bool IsValidMove(Piece piece, Vector2Int targetPos, ChessPieceType_SimulatedBoard[,] simulatedBoard)
    {
        _baseMoveVectors = piece.GetMoveVectors(); // 3Dデータの各駒クラスから移動ベクトルの定義を取得
        _currentPos = piece.CurrentIndex; // 3Dデータの駒の現在位置を駒の保持情報より取得
        _myColor = piece.PieceColor; // 3Dデータの駒から色を取得

        if (piece.PieceType == PieceType.Pawn) // 駒がポーンの場合の処理
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
            if (pawn.HasMoved == true) PawnMoveCheckAssist(baseMoveVector, targetPos); // 既に移動していたら1マスしか進めない
            else
            {
                Vector2Int pawnFirstMoveVector = baseMoveVector + baseMoveVector; // まだ動いていないなら2マス進める
                PawnMoveCheckAssist(pawnFirstMoveVector, targetPos);
            }
        }
        return false;
    }

    // ▼ 移動走査用の補助メソッド（ナイト、キング用）
    private bool NotRangedPieceMoveCheck(Vector2Int baseMoveVector, Vector2Int targetPos)
    {
        _nextPos = _currentPos + baseMoveVector;
        if (_nextPos == targetPos && IsWithinBoard(_nextPos))
        {
            return IsTileEmpty(_nextPos) || IsEnemyPiece(_myColor, _nextPos);
        }
        return false;
    }

    // ▼ 移動走査用の補助メソッド（ポーン用）
    private bool PawnMoveCheckAssist(Vector2Int baseMoveVector, Vector2Int targetPos)
    {
        // メソッド内の変数を共有
        Vector2Int pawnRightAttackPos;
        Vector2Int pawnLeftAttackPos;
        Vector2Int pawnAttackPos;

        if (_myColor == PieceColor.White) // 駒の色が白の場合は正の向きで移動マスと攻撃マスを取得
        {
            _nextPos = _currentPos + baseMoveVector;

            pawnRightAttackPos = new Vector2Int(1, 1);
            pawnLeftAttackPos = new Vector2Int(-1, 1);
            pawnAttackPos = _currentPos + pawnRightAttackPos + pawnLeftAttackPos;
        }
        else // 駒の色が黒の場合は負の向きで移動マスと攻撃マスを取得
        {
            _nextPos = _currentPos - baseMoveVector;

            pawnRightAttackPos = new Vector2Int(-1, -1);
            pawnLeftAttackPos = new Vector2Int(1, -1);
            pawnAttackPos = _currentPos + pawnRightAttackPos + pawnLeftAttackPos;
        }

        // 移動先のマスが移動可能範囲内また、攻撃先のマスに相手の駒がいある場合はtrueを返す
        if (_nextPos == targetPos && IsWithinBoard(_nextPos) || IsEnemyPiece(_myColor, pawnAttackPos))
        {
            return IsTileEmpty(_nextPos) || IsEnemyPiece(_myColor, pawnAttackPos);
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
    public bool CanPromote(Piece piece) // 引数は後ほど設定
    {
        if (piece.PieceType != PieceType.Pawn) return false;
        if (piece.IsPromoted == false)
        {
            if (piece.PieceColor == PieceColor.White)
            {
                if (_nextPos.y == 0) return true; // 白ポーンならy座標が0でボード最奥
            }
            else
            {
                if (_nextPos.y == 7) return true; // 黒ポーンならy座標が7でボード最奥
            }
        }
        return false;
    }

    // ▼キングのチェック判定
    public bool IsKingInCheck() // 引数は後ほど設定
    {
        return true;
    }

    // ▼盤面外かチェックするメソッド
    private bool IsWithinBoard(Vector2Int pos) => pos.x >= 0 && pos.x < 8 && pos.y >= 0 && pos.y < 8;

    // ▼マスが空かチェックするメソッド
    private bool IsTileEmpty(Vector2Int pos) => chessBoardManager.GetPieceAtSimulatedBoard(pos) == ChessPieceType_SimulatedBoard.None;

    // ▼駒が敵の駒かどうかチェックするメソッド
    // 今回、駒の色データ等は3Dのオブジェクトにアタッチされている。
    // データ層の2次元配列と3D側の2次元配列からそれぞれ参照しているが、後から問題になる可能性があるため注意する
    private bool IsEnemyPiece(PieceColor myColor, Vector2Int pos)
    {
        Piece targetGameObject = chessBoardManager.GetPieceAtPieceObjectBoard(pos); // 3Dデータ層から対象マスのピースを取得
        PieceColor pieceColor = targetGameObject.GetComponent<Piece>().PieceColor; // ピースの色を取得

        // 色が最初に選択した駒と同じ色でない場合はtrueを返し、敵（相手）の駒を判断する。
        if (pieceColor != myColor)
        {
            return true;
        }
        return false;
    }
}
