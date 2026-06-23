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
        return new List<Vector2Int>
        {
            new Vector2Int(0, 1), // 前
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
