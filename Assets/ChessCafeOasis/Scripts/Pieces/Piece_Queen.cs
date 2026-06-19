// クイーンの駒用スクリプト
using UnityEngine;

public class Piece_Queen : Piece
{
    public override void Start()
    {
        base.Start();
    }

    public override void Move(Vector2Int index)
    {
        // 駒それぞれの移動処理を後ほど追加する
    }

    public override void OnTaken()
    {
        base.OnTaken();
    }
}
