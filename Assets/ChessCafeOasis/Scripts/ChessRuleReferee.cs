using System.Collections.Generic;
using UnityEngine;

//////// スクリプトの説明：【チェスのルール審判係。主に移動における判定を担当する】////////

////////　データの流れ④：＜GameManager■＞　→　＜ChessRuleRefereeのIsValidMoveメソッドで合法手か判別＞　→　＜GameManager＞　////////
////////　データの流れ⑦：＜GameManager■＞　→　＜ChessRuleRefereeのIsValidMoveメソッドでチェックメイトか判別＞　→　＜GameManager＞　////////

// ★追加が必要なもの
// ①キャスリング判定用：＜KingとRookの間のマスに敵駒の利きがないかチェックするメソッド＞
// ②public bool IsKingInCheck()
// ③public bool IsKingInCheckmate()


public class ChessRuleReferee : MonoBehaviour
{
    [Header("データ層2次元配列の参照先")]
    [SerializeField] private ChessBoardManager chessBoardManager;

    [Header("直前の移動履歴の参照先")]
    [SerializeField] private RecordManager recordManager;

    // クラス内で共有する変数を宣言
    private PieceColor _myColor;
    private Vector2Int _currentPos;
    private Vector2Int _movableRange;
    private List<Vector2Int> _baseMoveVectors;

    // ▼駒の移動の判定、移動時の障害物の有無、自殺手のチェックを行うメソッド。
    public bool IsValidMove(Piece piece, Vector2Int targetPos, ChessPieceType_SimulatedBoard[,] simulatedBoard)
    {
        _baseMoveVectors = piece.GetMoveVectors(); // 3Dデータの各駒クラスから移動ベクトルの定義を取得
        _currentPos = piece.CurrentIndex; // 3Dデータの駒の現在位置を駒の保持情報より取得
        _myColor = piece.PieceColor; // 3Dデータの駒から色を取得

        Debug.Log($"IsValidMove called: piece={piece.name}, current={_currentPos}, target={targetPos}, color={_myColor}");

        if (piece.PieceType == PieceType.Pawn) // 駒がポーンの場合の処理
        {
            bool result = CheckPawnMove(piece, targetPos);
            Debug.Log($"Pawn move result: {result}");
            return result;
        }

        // 移動可能な座標のリストを作成
        List<Vector2Int> validMoves = new List<Vector2Int>();

        foreach (Vector2Int baseMoveVector in _baseMoveVectors)
        {
            if (piece.IsRangedPiece())
            {
                // ①ビショップ、ルーク、クイーンの走査
                for (int i = 1; i < 8; i++)
                {
                    _movableRange = _currentPos + (baseMoveVector * i);

                    // 盤面外ならこの方向の走査を終了
                    if (!IsWithinBoard(_movableRange)) break;

                    // ターゲットマスが空、または敵の駒なら移動可能座標に追加
                    if (IsTileEmpty(_movableRange) || IsEnemyPiece(_myColor, _movableRange))
                    {
                        validMoves.Add(_movableRange);
                        Debug.Log($"  Valid move added: {_movableRange}");

                        // 敵の駒がある場合はその先は進めない
                        if (IsEnemyPiece(_myColor, _movableRange)) break;
                    }
                    else
                    {
                        // 途中に自駒がある場合はその方向にそれ以上進めない
                        Debug.Log($"  Blocked at: {_movableRange}");
                        break;
                    }
                }
            }
            else
            {
                // ②ナイト、キングの走査
                Vector2Int potentialMove = _currentPos + baseMoveVector;
                if (IsWithinBoard(potentialMove) && (IsTileEmpty(potentialMove) || IsEnemyPiece(_myColor, potentialMove)))
                {
                    validMoves.Add(potentialMove);
                    Debug.Log($"  Valid move added: {potentialMove}");
                }
            }
        }

        bool isValid = validMoves.Contains(targetPos);
        Debug.Log($"IsValidMove result: {isValid}, validMoves count: {validMoves.Count}");
        return isValid;
    }

    // ▼ ナイト、キングの移動走査用補助メソッド（削除予定：現在は IsValidMove 内で直接実装済み）
    // TODO: 後で削除可能
    [System.Obsolete("IsValidMove メソッド内で直接実装されているため、このメソッドは使用されていません")]
    private bool NotRangedPieceMoveCheck(Vector2Int baseMoveVector, Vector2Int targetPos)
    {
        _movableRange = _currentPos + baseMoveVector;
        if (_movableRange == targetPos && IsWithinBoard(_movableRange))
        {
            return IsTileEmpty(_movableRange) || IsEnemyPiece(_myColor, _movableRange);
        }
        return false;
    }

    // ▼ポーンの移動判定メソッド
    public bool CheckPawnMove(Piece pawn, Vector2Int targetPos)
    {
        List<Vector2Int> validMoves = new List<Vector2Int>();

        // ポーンの移動方向を決定（白は正の方向、黒は負の方向）
        int moveDirection = (_myColor == PieceColor.White) ? -1 : 1;

        Debug.Log($"CheckPawnMove: pawn at {_currentPos}, target at {targetPos}, moveDirection={moveDirection}, hasMovedBefore={pawn.HasMoved}");

        // 1. 通常の前進移動
        Vector2Int oneStepForward = _currentPos + new Vector2Int(0, moveDirection);
        if (IsWithinBoard(oneStepForward) && IsTileEmpty(oneStepForward))
        {
            validMoves.Add(oneStepForward);
            Debug.Log($"  One step forward: {oneStepForward} is valid");

            // 初回移動の場合のみ2マス前進が可能
            if (!pawn.HasMoved)
            {
                Vector2Int twoStepsForward = _currentPos + new Vector2Int(0, moveDirection * 2);
                if (IsWithinBoard(twoStepsForward) && IsTileEmpty(twoStepsForward))
                {
                    validMoves.Add(twoStepsForward);
                    Debug.Log($"  Two steps forward: {twoStepsForward} is valid");
                }
            }
        }
        else
        {
            Debug.Log($"  One step forward: {oneStepForward} is blocked or out of bounds");
        }

        // 2. 斜めの攻撃移動（敵駒がある場合のみ）
        Vector2Int rightAttack = _currentPos + new Vector2Int(1, moveDirection);
        Vector2Int leftAttack = _currentPos + new Vector2Int(-1, moveDirection);

        if (IsWithinBoard(rightAttack) && IsEnemyPiece(_myColor, rightAttack))
        {
            validMoves.Add(rightAttack);
            Debug.Log($"  Right attack: {rightAttack} is valid");
        }

        if (IsWithinBoard(leftAttack) && IsEnemyPiece(_myColor, leftAttack))
        {
            validMoves.Add(leftAttack);
            Debug.Log($"  Left attack: {leftAttack} is valid");
        }

        // 3. アンパッサン（実装は後ほど）
        // TODO: CanEnPassant メソッドを使用して判定を追加する

        bool result = validMoves.Contains(targetPos);
        Debug.Log($"CheckPawnMove result: {result}, validMoves count: {validMoves.Count}");
        return result;
    }

    // ▼ ポーンの移動走査用の補助メソッド（削除予定：CheckPawnMove内で直接実装済み）
    // TODO: 後で削除可能
    [System.Obsolete("CheckPawnMove メソッド内で直接実装されているため、このメソッドは使用されていません")]
    private bool PawnMoveCheckAssist(Vector2Int baseMoveVector, Vector2Int targetPos)
    {
        // 旧実装（削除予定）
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
        // a1ルーク側（クイーンサイド）の対象座標：b1(1, 7) c1(2, 7) d1(3, 7) / h1ルーク側（キングサイド）の対象座標： f1(5, 7) g1(6, 7)
        if (piece.PieceColor == PieceColor.White)　//
        {
            if (targetRookPos.x == 7 && targetRookPos.y == 7) // キャスリング対象がh1(7, 7)ルークの場合
            {
                Vector2Int f1 = chessBoardManager.GetPieceAtTileBoard(5, 7).BoardIndex;
                Vector2Int g1 = chessBoardManager.GetPieceAtTileBoard(6, 7).BoardIndex;
                if (IsTileEmpty(f1) && IsTileEmpty(g1)) return true; // SimulatedBoard上でNone（マスが空）ならtrueを返す
            }
            else // キャスリング対象がa1(0, 7)ルークの場合
            {
                Vector2Int b1 = chessBoardManager.GetPieceAtTileBoard(1, 7).BoardIndex;
                Vector2Int c1 = chessBoardManager.GetPieceAtTileBoard(2, 7).BoardIndex;
                Vector2Int d1 = chessBoardManager.GetPieceAtTileBoard(3, 7).BoardIndex;
                if (IsTileEmpty(b1) && IsTileEmpty(c1) && IsTileEmpty(d1)) return true; // SimulatedBoard上でNone（マスが空）ならtrueを返す
            }
            return false;
        }

        // 黒の場合： ルークの位置は a8(0, 0) もしくは h8(7, 0)となる
        // キングの位置： e8(4, 0)
        // a8ルーク側（クイーンサイド）の対象座標： b8(1, 0) c8(2, 0) d8(3, 0) / h8ルーク側（キングサイド）の対象座標： f8(5, 0) g8(6, 0)
        else
        {
            if (targetRookPos.x == 7 && targetRookPos.y == 0) // キャスリング対象がh8ルーク(7, 0)の場合
            {
                Vector2Int f8 = chessBoardManager.GetPieceAtTileBoard(5, 0).BoardIndex;
                Vector2Int g8 = chessBoardManager.GetPieceAtTileBoard(6, 0).BoardIndex;
                if (IsTileEmpty(f8) && IsTileEmpty(g8)) return true; // SimulatedBoard上でNone（マスが空）ならtrueを返す
            }
            else // キャスリング対象がa8ルーク(0, 0)の場合
            {
                Vector2Int b8 = chessBoardManager.GetPieceAtTileBoard(1, 0).BoardIndex;
                Vector2Int c8 = chessBoardManager.GetPieceAtTileBoard(2, 0).BoardIndex;
                Vector2Int d8 = chessBoardManager.GetPieceAtTileBoard(3, 0).BoardIndex;
                if (IsTileEmpty(b8) && IsTileEmpty(c8) && IsTileEmpty(d8)) return true; // SimulatedBoard上でNone（マスが空）ならtrueを返す
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
        Piece targetPiece = chessBoardManager.GetPieceAtPieceObjectBoard(pos); // 3Dデータ層から対象マスのピースを取得
        if (targetPiece == null) return false;
        PieceColor pieceColor = targetPiece.PieceColor; // ピースの色を取得

        // 色が最初に選択した駒と同じ色でない場合はtrueを返し、敵（相手）の駒を判断する。
        if (pieceColor != myColor)
        {
            return true;
        }
        return false;
    }

    // ▼チェックメイトか確認するメソッド
    public bool IsKingInCheckmate()
    {
        return true;
    }
}
