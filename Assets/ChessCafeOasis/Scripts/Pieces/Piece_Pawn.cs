// ポーンの駒用スクリプト（ポーンのみPromote()が有効）
public class Piece_Pawn : Piece
{
    public override void Start()
    {
        base.Start();
    }

    public override void Move()
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
