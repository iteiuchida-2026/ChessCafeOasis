using UnityEngine;

//////// スクリプトの説明：【各駒にアタッチするScriptableObject型の基本情報。駒の種類、色を持つ】////////

public enum PieceType
{
    Pawn,
    Knight,
    Bishop,
    Rook,
    Queen,
    King
}
public enum PieceColor
{
    White,
    Black
}

[CreateAssetMenu(fileName = "PieceBaseInfo", menuName = "Scriptable Objects/PieceBaseInfo")]
public class PieceBaseInfo : ScriptableObject
{
    public PieceType pieceType;

    public PieceColor pieceColor;
}
