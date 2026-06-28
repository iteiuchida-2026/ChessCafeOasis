// ポーンの駒用スクリプト（ポーンのみPromote()が有効）
using System.Collections.Generic;
using UnityEngine;

public class Piece_Pawn : Piece
{
    public override void Start()
    {
        base.Start();
    }

    // ポーン固有の初回のみ2歩進めるのはChessRuleReferee側で処理する

    public override List<Vector2Int> GetMoveVectors()
    {
        // ポーンは移動方向をベクトルとして返す（色による方向は IsValidMove 内で処理）
        // 白ポーンは y を負の方向に移動（盤面座標系で上に移動）
        // 黒ポーンは y を正の方向に移動（盤面座標系で下に移動）
        // ここでは単純に前進の方向を示すベクトルを返す
        return new List<Vector2Int>
        {
            new Vector2Int(0, 1), // 前進方向を示すベクトル
        };
    }

    public override bool IsRangedPiece() => false; // ポーンは該当しない

    public override void Move(Vector2Int index)
    {
        // 駒それぞれの移動処理を後ほど追加する
    }

    public override void Promote()
    {
        // 昇格機能は継承元のスクリプトに後ほど追加する
    }

    public override void OnTaken()
    {
        base.OnTaken();
    }
}
