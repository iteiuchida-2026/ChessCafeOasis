using UnityEngine;

// ◆概要：3Dのチェス盤のマスの座標設定とマスのアクション

//////// データの流れ③-3：＜ChessBoardManager■指定座標のマスを光らせる＞　→　＜各TileControllerで光る処理＞ ////////

public class TileController : MonoBehaviour
{
    [Header("チェス盤の座標設定")]
    [Tooltip("ここでは(0,0)～(7,7)の範囲で該当する値を設定する。例：a1 = (0,7)、 b3 = (1,5)、d8 = (3,1)")]
    [SerializeField] private Vector2Int boardIndex;

    public Vector2Int BoardIndex => boardIndex;

    // ▼棋譜用の文字列を返すプロパティ
    public string AlgebraicNotation // AlgebraicNotationは要するにチェスの棋譜でe4等の表記（代数記法）
    {
        get
        {
            // ★盤面の値を留意（ChessBoardManager.csの表に準ずる）
            char file = (char)('a' + boardIndex.x); // 0→a,1→b
            int rank = 8 - boardIndex.y; // 7→1,6→2,
            return $"{file}{rank}";
        }
    }

    // ▼マスを光らせるメソッド
    public void LightUpSquare()
    {
        // 後ほど追加する
        Debug.Log($"{gameObject}のマスが光ります。");
    }
}
