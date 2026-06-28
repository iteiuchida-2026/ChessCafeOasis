// ナイトの駒用スクリプト
using System.Collections.Generic;
using UnityEngine;

public class Piece_Knight : Piece
{
    public override void Start()
    {
        base.Start();
    }

    public override List<Vector2Int> GetMoveVectors()
    {
        return new List<Vector2Int>
        {
            // ナイトは自分の座標から将棋の桂馬のように各方向へ飛べる
            new Vector2Int(1, 2), new Vector2Int(2, 1),
            new Vector2Int(2, -1), new Vector2Int(1, -2),
            new Vector2Int(-1, -2), new Vector2Int(-2, -1),
            new Vector2Int(-2, 1), new Vector2Int(-1, 2),
        };
    }

    public override bool IsRangedPiece() => false; // ナイトは該当しない

    public override void Move(Vector2Int index)
    {
        // 駒それぞれの移動処理を後ほど追加する
    }

    public override void OnTaken()
    {
        base.OnTaken();
    }
}
