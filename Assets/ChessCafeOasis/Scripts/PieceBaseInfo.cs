using UnityEngine;

// チェス駒それぞれに持つ情報
[CreateAssetMenu(fileName = "PieceBaseInfo", menuName = "Scriptable Objects/PieceBaseInfo")]
public class PieceBaseInfo : ScriptableObject
{
    public string pieceName; // 駒の名前（a1ポーン等）
    public enum pieceType // 駒の種類
    {
        Pawn, // ポーン
        Knight, // ナイト
        Bishop, // ビショップ
        Rook, // ルーク
        Queen, // クイーン
        King //キング
    }
    public string PieceColor; // 駒の色（白、黒）
    public int maxMoveSquares;　// 駒の最大移動可能マス数（ルークなら7）
    public GameObject startingSquare; // 駒の初期配置マス（a1）をアタッチする
}
