using UnityEngine;
public enum PieceType // 駒の種類を定義
{
    Pawn, // ポーン
    Knight, // ナイト
    Bishop, // ビショップ
    Rook, // ルーク
    Queen, // クイーン
    King //キング
}
public enum PieceColor // 駒の色を定義
{
    White, // 白
    Black // 黒
}

// チェス駒それぞれに持つ情報
[CreateAssetMenu(fileName = "PieceBaseInfo", menuName = "Scriptable Objects/PieceBaseInfo")]
public class PieceBaseInfo : ScriptableObject
{
    public string pieceName; // 駒の名前（a2ポーン等）

    public PieceType pieceType; // 駒の種類

    public PieceColor pieceColor; // 駒の色

    public int maxMoveSquares; // 駒の最大移動可能マス数（ルークなら7）

    public GameObject startingSquare; // 駒の初期配置マス（a1）をアタッチする
}
