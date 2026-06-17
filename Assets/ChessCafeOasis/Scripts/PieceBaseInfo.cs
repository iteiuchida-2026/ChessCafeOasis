using UnityEngine;

//////// スクリプトの説明：【各駒にアタッチするScriptableObject型の基本情報。駒の名前、種類、色、最大移動可能マス数、初期配置マスを持つ】////////

public enum PieceType // 駒の種類を定義
{
    Pawn,
    Knight,
    Bishop,
    Rook,
    Queen,
    King
}
public enum PieceColor // 駒の色を定義
{
    White,
    Black
}

public enum BoardSquare // チェス盤のマスを定義（チェス盤の配置とは異なる、名称のみの羅列）
{
    a1, a2, a3, a4, a5, a6, a7, a8,
    b1, b2, b3, b4, b5, b6, b7, b8,
    c1, c2, c3, c4, c5, c6, c7, c8,
    d1, d2, d3, d4, d5, d6, d7, d8,
    e1, e2, e3, e4, e5, e6, e7, e8,
    f1, f2, f3, f4, f5, f6, f7, f8,
    g1, g2, g3, g4, g5, g6, g7, g8,
    h1, h2, h3, h4, h5, h6, h7, h8,
}

// チェス駒それぞれに持つ情報：駒の名前、種類、色、最大移動可能マス数、初期配置マス
[CreateAssetMenu(fileName = "PieceBaseInfo", menuName = "Scriptable Objects/PieceBaseInfo")]
public class PieceBaseInfo : ScriptableObject
{
    public string pieceName; // 駒の名前（a2ポーン等）

    public PieceType pieceType; // 駒の種類

    public PieceColor pieceColor; // 駒の色

    public int maxMoveSquares; // 駒の最大移動可能マス数（ルークなら7）

    public BoardSquare startingSquare; // 駒の初期配置マス
}
