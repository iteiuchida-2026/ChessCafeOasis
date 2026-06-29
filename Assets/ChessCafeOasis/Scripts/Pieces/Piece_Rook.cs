// ルークの駒用スクリプト
using System.Collections.Generic;
using UnityEngine;

public class Piece_Rook : Piece
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
            new Vector2Int(-1, 0) //左
        };
    }

    public override bool IsRangedPiece() => true; // 上下左右に移動

    public override void Move(Vector2Int index)
    {
        base.Move(index);
    }

    public override void OnTaken()
    {
        base.OnTaken();
    }
}
