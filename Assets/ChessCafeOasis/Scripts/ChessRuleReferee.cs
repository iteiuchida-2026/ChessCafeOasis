using UnityEngine;

//////// スクリプトの説明：【チェスのルール審判係。主に移動における判定を担当する】////////


public class ChessRuleReferee : MonoBehaviour
{
    [Header("データ層2次元配列の参照先")]
    [SerializeField] private ChessBoardManager chessBoardManager;

    [Header("直前の移動履歴の参照先")]
    [SerializeField] private RecordManager recordManager;


    // ▼駒の移動の判定、移動時の障害物の有無、自殺手のチェックを行うメソッド。
    public bool IsValidMove()
    {
        return true;
    }

    // ▼ポーンの移動判定
    public bool CheckPawnMove()
    {
        return true;
    }

    // ▼ナイトの移動判定
    public bool CheckKnightMove()
    {
        return true;
    }

    // ▼ビショップの移動判定
    public bool CheckBishopMove()
    {
        return true;
    }

    // ▼ルークの移動判定
    public bool CheckRookMove()
    {
        return true;
    }

    // ▼クイーンの移動判定
    public bool CheckQQueenMove()
    {
        return true;
    }

    // ▼キングの移動判定
    public bool CheckKingMove()
    {
        return true;
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
    public bool CanPromote() // 引数は後ほど設定
    {
        return true;
    }

    // ▼キングのチェック判定
    public bool IsKingInCheck() // 引数は後ほど設定
    {
        return true;
    }
}
