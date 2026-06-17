using System.Collections.Generic;
using UnityEngine;

//////// スクリプトの説明：【チェス盤の各マスにどの駒が存在するかデータで記録し管理する】////////

//////// データの流れ①：＜InputManagerでクリックしたオブジェクト情報＞　→　＜ChessBoardManager＞ ////////
//////// データの流れ②：＜InputManagerでクリックしたオブジェクト情報＞　→　＜ChessBoardManager＞ ////////




// ◆概要：データ層のチェス盤用に駒の種類と色を定義
public enum ChessPieceType
{
    None, // 空のマス
    WhitePawn, WhiteKnight, WhiteBishop, WhiteRook, WhiteQueen, WhiteKing,
    BlackPawn, BlackKnight, BlackBishop, BlackRook, BlackQueen, BlackKing
}


// ◆概要：
// ①チェス盤面情報をデータ層で管理
// ②GameManager ⇔ ChessBoardManager ⇔ 管理対象クラスとの中継役兼指示役
public class ChessBoardManager : MonoBehaviour
{
    [Header("管理対象クラス")]
    [SerializeField] private PieceManager pieceManager;
    [SerializeField] private List<TileController> tileControllers;

    [Header("チェス盤の各マスオブジェクト")]
    [SerializeField] private GameObject[] tileObjects;

    // 8*8の盤面データ層の配列
    private ChessPieceType[,] boardState = new ChessPieceType[8, 8];

    // 起動時にデータ層のチェス盤面を初期配置に設定する
    private void Awake()
    {
        InitializeBoard();
    }

    // ▼データ層のチェス盤面を初期配置に設定するメソッド
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

    // ▼他のスクリプトが特定のマスの状態を調べるメソッド
    public ChessPieceType GetPieceAt(int x, int y)
    {
        if (x < 0 || x >= 8 || y < 0 || y >= 8) // 盤面外を確認する場合のガード処理
        {
            return ChessPieceType.None;
        }
        return boardState[x, y];
    }

    // ▼駒の移動が問題ない場合の盤面データ更新メソッド
    public void UpdateBoardState(int fromX, int fromY, int toX, int toY)
    {
        ChessPieceType movingPiece = boardState[fromX, fromY]; // 移動元の駒を取得

        boardState[fromX, fromY] = ChessPieceType.None; // 移動元のマスを空にする

        boardState[toX, toY] = movingPiece; // 移動先のマスに駒を置く
    }

    // ▼InputHandlerからクリックされたオブジェクトを受け取るメソッド
    public void ClickedGameObject(GameObject gameObject)
    {

    }
}
