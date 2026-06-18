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
// ①チェス盤面情報をデータ層のみで管理（シミュレート用）
// ②3Dチェス盤面情報の管理（実物）
// ③GameManager ⇔ ChessBoardManager ⇔ 管理対象クラスとの中継役兼指示役
public class ChessBoardManager : MonoBehaviour
{
    [Header("管理対象クラス")]
    [SerializeField] private PieceManager pieceManager;
    [SerializeField] private List<TileController> tileControllers;

    [Header("チェス盤の各マスオブジェクト")]
    [SerializeField] private GameObject[] tileObjects;

    private ChessPieceType[,] boardState = new ChessPieceType[8, 8]; // 【① データ】8*8の盤面データ層の配列
    private GameObject[,] pieceGrid = new GameObject[8, 8]; // 【② 3D】8*8の3D上の駒を管理する配列
    private GameObject clickedGameObject; // 【② 3D】クリックされたゲームオブジェクト保持用の変数を宣言


    private void Awake()
    {
        InitializeBoard(); // 【① データ】起動時にデータ層のチェス盤面を初期配置に設定する
    }

    // ▼【① データ】データ層のチェス盤面を初期配置に設定するメソッド
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

    // ▼【① データ】他のスクリプトがデータ層の特定マスの状態を調べるメソッド
    public ChessPieceType GetPieceAt(int x, int y)
    {
        if (x < 0 || x >= 8 || y < 0 || y >= 8) // 盤面外を確認する場合のガード処理
        {
            return ChessPieceType.None;
        }
        return boardState[x, y];
    }

    // ▼【① データ】駒の移動が問題ない場合のデータ層盤面データ更新メソッド
    public void UpdateBoardState(int fromX, int fromY, int toX, int toY)
    {
        ChessPieceType movingPiece = boardState[fromX, fromY]; // 移動元の駒を取得

        boardState[fromX, fromY] = ChessPieceType.None; // 移動元のマスを空にする

        boardState[toX, toY] = movingPiece; // 移動先のマスに駒を置く
    }

    // ▼【② 3D】オブジェクトが駒かマスかを調べるメソッド
    public void IdentifyGameObject(GameObject gameObject)
    {
        // A.駒の場合、情報を取得するメソッドへ渡す
        if (gameObject.CompareTag("Piece"))
        {
            GetClickedPieceInfo(gameObject);
        }
        // B.マスの場合、マス上にある駒を調べるメソッドへ渡す
        else if (gameObject.CompareTag("Square"))
        {
            CheckUpPieceOnSquare(gameObject);
        }
        else return;
    }

    //▼【② 3D】クリックされたオブジェクトがマスの場合、マスと同じ座標にある駒を調べるメソッド
    private void CheckUpPieceOnSquare(GameObject gameObject)
    {

    }

    // ▼【② 3D】クリックされたオブジェクトが駒の場合、情報を取得するメソッド
    private void GetClickedPieceInfo(GameObject gameObject)
    {
        string clickedGameObjectPieceColor = gameObject.GetComponent<Piece>().pieceColor;
        string clickedGameObjectPieceType = gameObject.GetComponent<Piece>().pieceType;
        string clickedGameObjectCurrentSquare = gameObject.GetComponent<Piece>().currentSquare;
        bool clickedGameObjectHasMoved = gameObject.GetComponent<Piece>().HasMoved;
        bool clickedGameObjectIsPromoted = gameObject.GetComponent<Piece>().IsPromoted;
    }


}
