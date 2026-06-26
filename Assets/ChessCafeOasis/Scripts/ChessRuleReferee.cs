using System.Collections.Generic;
using UnityEngine;

//////// スクリプトの説明：【チェスのルール審判係。主に移動における判定を担当する】////////

////////　データの流れ④：＜GameManager■＞　→　＜ChessRuleRefereeのIsValidMoveメソッドで合法手か判別＞　→ 　////////

// ★追加が必要なもの
// ①キャスリング判定用：＜KingとRookの間のマスに敵駒の利きがないかチェックするメソッド＞
// ②    public bool IsKingInCheck()


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

    // ▼ポーンの移動判定メソッド
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

    // ▼ ナイト、キングの移動走査用補助メソッド
    private bool NotRangedPieceMoveCheck(Vector2Int baseMoveVector, Vector2Int targetPos)
    {
        _nextPos = _currentPos + baseMoveVector;
        if (_nextPos == targetPos && IsWithinBoard(_nextPos))
        {
            return IsTileEmpty(_nextPos) || IsEnemyPiece(_myColor, _nextPos);
        }
        return false;
    }

    // ▼ ポーンの移動走査用の補助メソッド
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

    // ▼キャスリングの可否判定メソッド
    // キャスリング：キングとルークの位置を変更するチェスの特殊ルール
    // 条件① キングとキャスリング先のルークが一度も動いていない
    // 条件② キングとキャスリング先のルークの間に駒がない
    // 条件③ キングがチェックされていない
    // 条件④ キングが移動するマスと通過するマスに敵の駒の攻撃範囲が入っていない
    public bool CanCastling(Piece piece, Vector2Int targetRookPos)
    {
        if (piece.PieceType != PieceType.King) return false;
        if (IsKingInCheck() != false) return false; // 条件③
        Piece targetRook = chessBoardManager.GetPieceAtPieceObjectBoard(targetRookPos);
        if (piece.HasMoved == false && targetRook.HasMoved == false) // 条件①
        {
            CanCastlingAssistCheckNone(piece, targetRookPos); // 条件②
            // ＜KingとRookの間のマスに敵駒の利きがないかチェックするメソッドを後から追加する＞ // 条件④

            return true;
        }
        return false;
    }

    // ▼キャスリング補助メソッド（キングとルークの間の駒があるか調べる）
    // ※キングとルークが既に動いているか等は考慮していない
    public bool CanCastlingAssistCheckNone(Piece piece, Vector2Int targetRookPos)
    {
        // 白の場合： ルークの位置は a1(0, 7) もしくは h1(7, 7)となる
        // キングの位置： e1(4, 7)
        // 駒の有無チェック対象マス： b1(1, 7) c1(2, 7) d1(3, 7) f1(5, 7) g1(6, 7)
        if (piece.PieceColor == PieceColor.White)　//
        {
            if (targetRookPos.x == 7 && targetRookPos.y == 7) // キャスリング対象：h1ルークの場合(7, 7)
            {
                ChessPieceType_SimulatedBoard f1 = chessBoardManager.GetPieceAtSimulatedBoard(chessBoardManager.GetPieceAtTileBoard(5, 7).BoardIndex);
                ChessPieceType_SimulatedBoard g1 = chessBoardManager.GetPieceAtSimulatedBoard(chessBoardManager.GetPieceAtTileBoard(6, 7).BoardIndex);

                if (f1 == ChessPieceType_SimulatedBoard.None && g1 == ChessPieceType_SimulatedBoard.None) // SimulatedBoard上でNone（マスが空）ならtrueを返す
                {
                    return true;
                }
            }
            else // キャスリング対象：a1ルークの場合(0, 7)
            {
                ChessPieceType_SimulatedBoard b1 = chessBoardManager.GetPieceAtSimulatedBoard(chessBoardManager.GetPieceAtTileBoard(1, 7).BoardIndex);
                ChessPieceType_SimulatedBoard c1 = chessBoardManager.GetPieceAtSimulatedBoard(chessBoardManager.GetPieceAtTileBoard(2, 7).BoardIndex);
                ChessPieceType_SimulatedBoard d1 = chessBoardManager.GetPieceAtSimulatedBoard(chessBoardManager.GetPieceAtTileBoard(3, 7).BoardIndex);

                if (b1 == ChessPieceType_SimulatedBoard.None && c1 == ChessPieceType_SimulatedBoard.None && d1 == ChessPieceType_SimulatedBoard.None) // SimulatedBoard上でNone（マスが空）ならtrueを返す
                {
                    return true;
                }
            }
            return false;
        }

        // 黒の場合： ルークの位置は a8(0, 0) もしくは h8(7, 0)となる
        // キングの位置： e8(4, 0)
        // 駒の有無チェック対象マス： b8(1, 0) c8(2, 0) d8(3, 0) f8(5, 0) g8(6, 0)
        else
        {
            if (targetRookPos.x == 7 && targetRookPos.y == 0) // キャスリング対象：h8ルークの場合(7, 0)
            {
                ChessPieceType_SimulatedBoard f8 = chessBoardManager.GetPieceAtSimulatedBoard(chessBoardManager.GetPieceAtTileBoard(5, 0).BoardIndex);
                ChessPieceType_SimulatedBoard g8 = chessBoardManager.GetPieceAtSimulatedBoard(chessBoardManager.GetPieceAtTileBoard(6, 0).BoardIndex);

                if (f8 == ChessPieceType_SimulatedBoard.None && g8 == ChessPieceType_SimulatedBoard.None) // SimulatedBoard上でNone（マスが空）ならtrueを返す
                {
                    return true;
                }
            }
            else // a8ルークの場合(0, 0)
            {
                ChessPieceType_SimulatedBoard b8 = chessBoardManager.GetPieceAtSimulatedBoard(chessBoardManager.GetPieceAtTileBoard(1, 0).BoardIndex);
                ChessPieceType_SimulatedBoard c8 = chessBoardManager.GetPieceAtSimulatedBoard(chessBoardManager.GetPieceAtTileBoard(2, 0).BoardIndex);
                ChessPieceType_SimulatedBoard d8 = chessBoardManager.GetPieceAtSimulatedBoard(chessBoardManager.GetPieceAtTileBoard(3, 0).BoardIndex);

                if (b8 == ChessPieceType_SimulatedBoard.None && c8 == ChessPieceType_SimulatedBoard.None && d8 == ChessPieceType_SimulatedBoard.None) // SimulatedBoard上でNone（マスが空）ならtrueを返す
                {
                    return true;
                }
            }
            return false;
        }
    }

    // ▼アンパッサンの可否判定メソッド
    // アンパッサン：相手のポーンが2マス進んで来た次の自分のターンに、自分のポーンが相手のポーンをとれるチェスの特殊ルール
    // 条件① 自分のポーンが自陣から数えて5段目にいる
    // 条件② 自分のポーンの真横（同じ段の隣の列）にいる相手のポーンが最初の位置から2マス進んだ
    // 条件③ 相手のポーンが2マス進んだ直後のターン
    public bool CanEnPassant(Piece piece)
    {
        if (piece.PieceType != PieceType.Pawn) return false;
        // ＜RecordManager.csのメソッドで直前の手が相手の両サイドどちらかのポーンの2マス前進かを判断＞ 条件③②
        if (piece.PieceColor == PieceColor.White)
        {
            if (piece.CurrentIndex.y == 3) return true; // 条件① 白ポーンの場合はy座標が3で5段目 
        }
        else
        {
            if (piece.CurrentIndex.y == 4) return true; // 条件① 黒ポーンの場合はy座標が4で5段目
        }
        return false;
    }

    // ▼プロモーションの可否判定メソッド
    // プロモーション：ポーンがキング以外の好きな駒に昇格できるチェスの特殊ルール
    // 条件① 自分のポーンが敵陣最奥に到達
    public bool CanPromote(Piece piece, Vector2Int _nextPos)
    {
        if (piece.PieceType != PieceType.Pawn) return false;
        if (piece.IsPromoted == false)
        {
            if (piece.PieceColor == PieceColor.White)
            {
                if (_nextPos.y == 0) return true; // 条件① 白ポーンならy座標が0でボード最奥の8段目
            }
            else
            {
                if (_nextPos.y == 7) return true; // 条件① 黒ポーンならy座標が7でボード最奥の8段目
            }
        }
        return false;
    }

    // ▼キングのチェック判定メソッド
    // ※キングが相手の駒の攻撃に当たっているかどうか
    public bool IsKingInCheck() // 引数は後ほど設定
    {
        return true;
    }

    // ▼盤面外かチェックするメソッド
    private bool IsWithinBoard(Vector2Int pos) => pos.x >= 0 && pos.x < 8 && pos.y >= 0 && pos.y < 8;

    // ▼マスが空かチェックするメソッド
    private bool IsTileEmpty(Vector2Int pos) => chessBoardManager.GetPieceAtSimulatedBoard(pos) == ChessPieceType_SimulatedBoard.None;

    // ▼対象座標の駒が相手の駒かチェックするメソッド
    // ※今回、駒の色データ等は3Dのオブジェクトにアタッチされている。
    // ※データ層の2次元配列と3D側の2次元配列からそれぞれ参照しているが、後から問題になる可能性があるため注意する
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
