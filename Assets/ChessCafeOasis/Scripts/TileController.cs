using UnityEngine;

// ◆概要：3Dのチェス盤のマスの座標設定とマスのアクション
public class TileController : MonoBehaviour
{
    [Header("チェス盤の座標設定")]
    [SerializeField] private Vector2Int boardIndex;

    public Vector2Int BoardIndex => boardIndex;

    // ▼棋譜用の文字列を返すプロパティ
    public string AlgebraicNotation // AlgebraicNotationは要するにチェスの棋譜でe4等の表記（代数記法）
    {
        get
        {
            char file = (char)('a' + boardIndex.x); // 0→a,1→b
            int rank = boardIndex.y + 1; // 0→1,1→2
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
