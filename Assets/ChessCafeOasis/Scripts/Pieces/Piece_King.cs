// キングの駒用スクリプト
using System.Collections.Generic;
using UnityEngine;

public class Piece_King : Piece
{
    public override void Start()
    {
        base.Start();
    }

    public override List<Vector2Int> GetMoveVectors()
    {
        return new List<Vector2Int>
        {
            new Vector2Int(0, 1), // 前
            new Vector2Int(0, -1), // 後
            new Vector2Int(1, 0), //右
            new Vector2Int(-1, 0), //左
            new Vector2Int(1, 1), // 右斜め前
            new Vector2Int(-1, -1), // 左斜め後
            new Vector2Int(-1, 1), //左斜め前
            new Vector2Int(1, -1) //右斜め後
        };
    }

    public override bool IsRangedPiece() => false; // キングは該当しない

    public override void Move(Vector2Int index)
    {
        base.Move(index);
    }

    public override void OnTaken()
    {
        base.OnTaken();
    }
}
