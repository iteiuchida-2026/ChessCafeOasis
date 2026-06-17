using System.Collections.Generic;
using UnityEngine;

//////// スクリプトの説明：【チェス盤の各マスにどの駒が存在するかデータで記録し管理する】////////

// 駒の種類と色を定義
public enum ChessPieceType
{
    None, // 空のマス
    WhitePawn, WhiteKnight, WhiteBishop, WhiteRook, WhiteQueen, WhiteKing,
    BlackPawn, BlackKnight, BlackBishop, BlackRook, BlackQueen, BlackKing
}

public class ChessBoardManager : MonoBehaviour
{
    // ChessBoardManagerが指示を出すクラスをアタッチする
    [SerializeField] private PieceManager pieceManager;
    [SerializeField] private List<TileController> tileControllers;
    [SerializeField] private InputHandler inputHandler;

    // 8*8の盤面データ層
    private ChessPieceType[,] boardState = new ChessPieceType[8, 8];

    private void Awake()
    {
        InitializeBoard();
    }

    // 盤面データ層の初期配置を設定する
    private void InitializeBoard()
    {
        // すべてのマスを一旦空にする
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                boardState[x, y] = ChessPieceType.None;
            }
        }

        // 白の駒を配置する
        boardState[0, 0] = ChessPieceType.WhiteRook;
        boardState[1, 0] = ChessPieceType.WhiteKnight;
        boardState[2, 0] = ChessPieceType.WhiteBishop;
        boardState[3, 0] = ChessPieceType.WhiteQueen;
        boardState[4, 0] = ChessPieceType.WhiteKing;
        boardState[5, 0] = ChessPieceType.WhiteBishop;
        boardState[6, 0] = ChessPieceType.WhiteKnight;
        boardState[7, 0] = ChessPieceType.WhiteRook;
        for (int x = 0; x < 8; x++) boardState[x, 1] = ChessPieceType.WhitePawn;

        // 黒の駒を配置する
        boardState[0, 7] = ChessPieceType.BlackRook;
        boardState[1, 7] = ChessPieceType.BlackKnight;
        boardState[2, 7] = ChessPieceType.BlackBishop;
        boardState[3, 7] = ChessPieceType.BlackQueen;
        boardState[4, 7] = ChessPieceType.BlackKing;
        boardState[5, 7] = ChessPieceType.BlackBishop;
        boardState[6, 7] = ChessPieceType.BlackKnight;
        boardState[7, 7] = ChessPieceType.BlackRook;
        for (int x = 0; x < 8; x++) boardState[x, 6] = ChessPieceType.BlackPawn;
    }

    // 他のスクリプトが特定のマスの状態を調べるメソッド
    public ChessPieceType GetPieceAt(int x, int y)
    {
        // 盤面外を確認する場合のガード処理
        if (x < 0 || x >= 8 || y < 0 || y >= 8)
        {
            return ChessPieceType.None;
        }
        return boardState[x, y];
    }

    // 駒の移動が問題ない場合の盤面データ更新
    public void UpdateBoardState(int fromX, int fromY, int toX, int toY)
    {
        // 移動元の駒を取得
        ChessPieceType movingPiece = boardState[fromX, fromY];

        // 移動元のマスを空にする
        boardState[fromX, fromY] = ChessPieceType.None;

        // 移動先のマスに駒を置く
        boardState[toX, toY] = movingPiece;
    }
}
