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
    [SerializeField] private GameObject[] tileObjects;

    private ChessPieceType[,] DataLayerBoardState = new ChessPieceType[8, 8]; // 【① データ】8*8の盤面データ層の配列
    private GameObject[,] RealLayerBoardState = new GameObject[8, 8]; // 【② 3D】8*8の3D上の駒を管理する配列
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
                DataLayerBoardState[x, y] = ChessPieceType.None;
            }
        }

        // 白の駒を配置する
        DataLayerBoardState[0, 0] = ChessPieceType.WhiteRook;
        DataLayerBoardState[1, 0] = ChessPieceType.WhiteKnight;
        DataLayerBoardState[2, 0] = ChessPieceType.WhiteBishop;
        DataLayerBoardState[3, 0] = ChessPieceType.WhiteQueen;
        DataLayerBoardState[4, 0] = ChessPieceType.WhiteKing;
        DataLayerBoardState[5, 0] = ChessPieceType.WhiteBishop;
        DataLayerBoardState[6, 0] = ChessPieceType.WhiteKnight;
        DataLayerBoardState[7, 0] = ChessPieceType.WhiteRook;
        for (int x = 0; x < 8; x++) DataLayerBoardState[x, 1] = ChessPieceType.WhitePawn;

        // 黒の駒を配置する
        DataLayerBoardState[0, 7] = ChessPieceType.BlackRook;
        DataLayerBoardState[1, 7] = ChessPieceType.BlackKnight;
        DataLayerBoardState[2, 7] = ChessPieceType.BlackBishop;
        DataLayerBoardState[3, 7] = ChessPieceType.BlackQueen;
        DataLayerBoardState[4, 7] = ChessPieceType.BlackKing;
        DataLayerBoardState[5, 7] = ChessPieceType.BlackBishop;
        DataLayerBoardState[6, 7] = ChessPieceType.BlackKnight;
        DataLayerBoardState[7, 7] = ChessPieceType.BlackRook;
        for (int x = 0; x < 8; x++) DataLayerBoardState[x, 6] = ChessPieceType.BlackPawn;
    }

    // ▼【① データ】他のスクリプトがデータ層の特定マスの状態を調べるメソッド
    public ChessPieceType GetPieceAtDataLayer(int x, int y)
    {
        if (x < 0 || x >= 8 || y < 0 || y >= 8) // 盤面外を確認する場合のガード処理
        {
            return ChessPieceType.None;
        }
        return DataLayerBoardState[x, y];
    }

    // ▼【① データ】駒の移動が問題ない場合のデータ層盤面データ更新メソッド
    public void UpdateBoardState(int fromX, int fromY, int toX, int toY)
    {
        ChessPieceType movingPiece = DataLayerBoardState[fromX, fromY]; // 移動元の駒を取得

        DataLayerBoardState[fromX, fromY] = ChessPieceType.None; // 移動元のマスを空にする

        DataLayerBoardState[toX, toY] = movingPiece; // 移動先のマスに駒を置く
    }



    // ▼【② 3D】インデックスから駒を取得するメソッド
    public GameObject GetPieceAtRealLayer(Vector2Int index)
    {
        if (index.x >= 0 && index.x < 8 && index.y >= 0 && index.y < 8)
        {
            return RealLayerBoardState[index.x, index.y];
        }
        return null;
    }

    // ▼【② 3D】オブジェクトが駒かマスかを調べるメソッド
    public void IdentifyGameObject(GameObject gameObject)
    {
        // クリックされたオブジェクトがマスだった場合
        if (gameObject.TryGetComponent<TileController>(out TileController clickedSquare))
        {
            Vector2Int clickedSquareIndex = clickedSquare.BoardIndex;
            Debug.Log($"クリックされたマス:{clickedSquare.AlgebraicNotation}(インデックス:{clickedSquareIndex})");

            GameObject pieceOnSquare = GetPieceAtRealLayer(clickedSquareIndex); // 共通のインデックスからマスに乗っている駒オブジェクトを取得

            if (pieceOnSquare != null)
            {
                Debug.Log($"そのマスには{pieceOnSquare.name}が乗っています。");
                GetClickedPieceInfo(pieceOnSquare); // 駒情報を取得するメソッドへ渡す
            }
            else
            {
                Debug.Log("そのマスは空です。");
            }
        }

        // クリックされたオブジェクトが駒だった場合
        else if (gameObject.TryGetComponent<Piece>(out Piece clickedPiece))
        {
            GameObject clickedPieceGameObject = clickedPiece.gameObject;
            Debug.Log($"その駒は{clickedPieceGameObject.name}です。");
            GetClickedPieceInfo(clickedPieceGameObject); // 駒情報を取得するメソッドへ渡す
        }
        else
        {
            Debug.Log("クリックされたゲームオブジェクトはマスでも駒でもありません。");
            return;
        }
    }

    // ▼【② 3D】クリックされたオブジェクトが駒の場合、情報を取得するメソッド
    private void GetClickedPieceInfo(GameObject gameObject)
    {
        string clickedGameObjectPieceColor = gameObject.GetComponent<Piece>().pieceColor;
        string clickedGameObjectPieceType = gameObject.GetComponent<Piece>().pieceType;
        Vector2Int clickedGameObjectCurrentSquare = gameObject.GetComponent<Piece>().currentIndex;
        bool clickedGameObjectHasMoved = gameObject.GetComponent<Piece>().HasMoved;
        bool clickedGameObjectIsPromoted = gameObject.GetComponent<Piece>().IsPromoted;
    }


}
