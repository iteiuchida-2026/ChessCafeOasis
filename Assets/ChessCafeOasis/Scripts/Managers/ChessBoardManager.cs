using UnityEngine;

//////// スクリプトの説明：【チェス盤の各マスにどの駒が存在するかデータで記録し管理する】////////

//////// データの流れ①：＜InputManagerでクリックしたオブジェクト情報＞　→　＜ChessBoardManager＞ ////////
//////// データの流れ②：＜InputManagerでクリックしたオブジェクト情報＞　→　＜ChessBoardManager＞ ////////

////// ★盤面の値：                                                                              //////
////// すべての配列および座標は以下のとおりに統一する。
////// (0,0)がチェス盤のa8に該当する。白番から見る向きで、a1は(7,0)                              //////
////// ________________________________________________________________________________________  //////
////// |(0,0 / a8)|(1,0 / b8)|(2,0 / c8)|(3,0 / d8)|(4,0 / e8)|(5,0 / f8)|(6,0 / g8)|(7,0 / h8)| //////
////// |(0,1 / a7)|(1,1 / b7)|(2,1 / c7)|(3,1 / d7)|(4,1 / e7)|(5,1 / f7)|(6,1 / g7)|(7,1 / h7)| //////
////// |(0,2 / a6)|(1,2 / b6)|(2,2 / c6)|(3,2 / d6)|(4,2 / e6)|(5,2 / f6)|(6,2 / g6)|(7,2 / h6)| //////
////// |(0,3 / a5)|(1,3 / b5)|(2,3 / c5)|(3,3 / d5)|(4,3 / e5)|(5,3 / f5)|(6,3 / g5)|(7,3 / h5)| //////
////// |(0,4 / a4)|(1,4 / b4)|(2,4 / c4)|(3,4 / d4)|(4,4 / e4)|(5,4 / f4)|(6,4 / g4)|(7,4 / h4)| //////
////// |(0,5 / a3)|(1,5 / b3)|(2,5 / c3)|(3,5 / d3)|(4,5 / e3)|(5,5 / f3)|(6,5 / g3)|(7,5 / h3)| //////
////// |(0,6 / a2)|(1,6 / b2)|(2,6 / c2)|(3,6 / d2)|(4,6 / e2)|(5,6 / f2)|(6,6 / g2)|(7,6 / h2)| //////
////// |(0,7 / a1)|(1,7 / b1)|(2,7 / c1)|(3,7 / d1)|(4,7 / e1)|(5,7 / f1)|(6,7 / g1)|(7,7 / h1)| //////
////// ￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣￣  //////

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
    [SerializeField] private GameObject[] tileObjects = new GameObject[64];

    private ChessPieceType[,] dataLayerBoardState = new ChessPieceType[8, 8]; //【① データ】8*8の盤面データ層の配列
    public Vector2Int whiteKingPos; //【① データ】データ層用白キングの現在のポジション
    public Vector2Int blackKingPos; //【① データ】データ層用黒キングの現在のポジション
    private GameObject[,] realLayerBoardState = new GameObject[8, 8]; //【② 3D】8*8の3D上の駒を管理する配列
    private GameObject clickedGameObject; //【② 3D】クリックされたゲームオブジェクト保持用の変数を宣言
    private GameObject[,] tileObjectsArray = new GameObject[8, 8]; //【③ マスOBJ】 tileObjectsを8*8の実際のチェス盤に合わせるため2次元配列を用意


    private void Awake()
    {
        InitializeDataLayerBoard(); // 【① データ】起動時にデータ層のチェス盤面を初期配置に設定する

        // serializeしたtileObjectsを2次元配列に変換
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                tileObjectsArray[x, y] = tileObjects[y * 8 + x];
            }
        }
    }

    // ▼【① データ】データ層のチェス盤面を初期配置に設定するメソッド
    ////// ★盤面の値に留意 //////
    private void InitializeDataLayerBoard()
    {
        // すべてのマスを一旦空にする
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                dataLayerBoardState[x, y] = ChessPieceType.None;
            }
        }

        // 白の駒を配置する
        dataLayerBoardState[0, 7] = ChessPieceType.WhiteRook;
        dataLayerBoardState[1, 7] = ChessPieceType.WhiteKnight;
        dataLayerBoardState[2, 7] = ChessPieceType.WhiteBishop;
        dataLayerBoardState[3, 7] = ChessPieceType.WhiteQueen;
        dataLayerBoardState[4, 7] = ChessPieceType.WhiteKing;
        dataLayerBoardState[5, 7] = ChessPieceType.WhiteBishop;
        dataLayerBoardState[6, 7] = ChessPieceType.WhiteKnight;
        dataLayerBoardState[7, 7] = ChessPieceType.WhiteRook;
        for (int x = 0; x < 8; x++) dataLayerBoardState[x, 7] = ChessPieceType.WhitePawn;

        // 黒の駒を配置する
        dataLayerBoardState[0, 0] = ChessPieceType.BlackRook;
        dataLayerBoardState[1, 0] = ChessPieceType.BlackKnight;
        dataLayerBoardState[2, 0] = ChessPieceType.BlackBishop;
        dataLayerBoardState[3, 0] = ChessPieceType.BlackQueen;
        dataLayerBoardState[4, 0] = ChessPieceType.BlackKing;
        dataLayerBoardState[5, 0] = ChessPieceType.BlackBishop;
        dataLayerBoardState[6, 0] = ChessPieceType.BlackKnight;
        dataLayerBoardState[7, 0] = ChessPieceType.BlackRook;
        for (int x = 0; x < 8; x++) dataLayerBoardState[x, 1] = ChessPieceType.BlackPawn;
    }

    // ▼【① データ】駒の移動が問題ない場合のデータ層盤面データ更新メソッド
    public void UpdateBoardState(int fromX, int fromY, int toX, int toY)
    {
        ChessPieceType movingPiece = dataLayerBoardState[fromX, fromY]; // 移動元の駒を取得

        dataLayerBoardState[fromX, fromY] = ChessPieceType.None; // 移動元のマスを空にする

        dataLayerBoardState[toX, toY] = movingPiece; // 移動先のマスに駒を置く
    }

    // ▼【① データ】インデックスからデータ層特定マスの状態を調べるメソッド
    public ChessPieceType GetPieceAtDataLayer(Vector2Int index)
    {
        if (index.x < 0 || index.x >= 8 || index.y < 0 || index.y >= 8) // 盤面外を確認する場合のガード処理
        {
            return ChessPieceType.None;
        }
        return dataLayerBoardState[index.x, index.y];
    }

    // ▼【② 3D】インデックスから駒を取得するメソッド
    public GameObject GetPieceAtRealLayer(Vector2Int index)
    {
        if (index.x >= 0 && index.x < 8 && index.y >= 0 && index.y < 8)
        {
            return realLayerBoardState[index.x, index.y];
        }
        return null;
    }

    // ▼【③ マスOBJ】2次元配列からマスを取得するメソッド
    public GameObject GetPieceAtTileObjectsArray(int x, int y)
    {
        if (x >= 0 && x < 8 && y >= 0 && y < 8)
        {
            return tileObjectsArray[x, y];
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
