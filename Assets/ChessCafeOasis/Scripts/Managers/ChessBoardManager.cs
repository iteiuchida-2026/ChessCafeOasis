using UnityEngine;

//////// スクリプトの説明：【チェス盤の各マスにどの駒が存在するかデータで記録し管理する】////////

//////// データの流れ②：＜InputManagerでクリックした■オブジェクト情報　→　ChessBoardManager：IdentifyGameObjectメソッドで■駒オブジェクト情報を取得＞　→　＜GameManagerが■受け取る＞ ////////

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

// ◆概要：データ層の駒を定義
public enum ChessPieceType_SimulatedBoard
{
    None, // 空のマス
    WhitePawn, WhiteKnight, WhiteBishop, WhiteRook, WhiteQueen, WhiteKing,
    BlackPawn, BlackKnight, BlackBishop, BlackRook, BlackQueen, BlackKing
}

// ◆概要：
// ①チェス盤面の管理（2次元配列を以下の3種類保持）
// 　1.データ層（シミュレート用）
// 　2.3D駒オブジェクト層（3D駒オブジェクト）
//   3.3Dタイルオブジェクト層（3Dタイルオブジェクト）
// ②GameManager ⇔ ChessBoardManager ⇔ 管理対象クラスとの中継役兼指示役
public class ChessBoardManager : MonoBehaviour
{
    [Header("管理対象クラス")]
    [SerializeField] private PieceManager pieceManager;
    [SerializeField] private TileController[] tileObjects = new TileController[64];

    private ChessPieceType_SimulatedBoard[,] simulatedBoard = new ChessPieceType_SimulatedBoard[8, 8]; //【データ】8*8の盤面データ層の配列
    public Vector2Int WhiteKingPos { get; set; } //【データ】白キングの現在のポジション
    public Vector2Int BlackKingPos { get; set; } //【データ】黒キングの現在のポジション
    public Vector2Int clickedIndex { get; set; } // 【3D】クリックされた座標
    private Piece[,] pieceObjectBoard = new Piece[8, 8]; //【3D】8*8配列
    private GameObject clickedGameObject; //【3D】クリックされたゲームオブジェクト保持用の変数
    private TileController[,] tileBoard = new TileController[8, 8]; //【タイル】 8*8のチェス盤に合わせた2次元配列

    private void Awake()
    {
        RearrangeTileObjects();// 【タイル】シリアライズしたタイルを2次元配列に変換
    }

    // ▼初期化処理をまとめたメソッド
    // ＜GameManagerから呼ばれる＞
    public void InitializeBoards()
    {
        InitializeSimulatedBoard(); // 【データ】初期配置に設定
        pieceManager.InitializePieceObject(); // 【3D】ピースマネジャーに各駒オブジェクトの初期化を指示
    }

    // ▼【データ】チェス盤面を初期配置に設定するメソッド
    private void InitializeSimulatedBoard()
    {
        // すべてのマスを一旦空にする
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                simulatedBoard[x, y] = ChessPieceType_SimulatedBoard.None;
            }
        }

        // 白の駒を配置する
        simulatedBoard[0, 7] = ChessPieceType_SimulatedBoard.WhiteRook;
        simulatedBoard[1, 7] = ChessPieceType_SimulatedBoard.WhiteKnight;
        simulatedBoard[2, 7] = ChessPieceType_SimulatedBoard.WhiteBishop;
        simulatedBoard[3, 7] = ChessPieceType_SimulatedBoard.WhiteQueen;
        simulatedBoard[4, 7] = ChessPieceType_SimulatedBoard.WhiteKing;
        simulatedBoard[5, 7] = ChessPieceType_SimulatedBoard.WhiteBishop;
        simulatedBoard[6, 7] = ChessPieceType_SimulatedBoard.WhiteKnight;
        simulatedBoard[7, 7] = ChessPieceType_SimulatedBoard.WhiteRook;
        for (int x = 0; x < 8; x++) simulatedBoard[x, 7] = ChessPieceType_SimulatedBoard.WhitePawn;

        // 黒の駒を配置する
        simulatedBoard[0, 0] = ChessPieceType_SimulatedBoard.BlackRook;
        simulatedBoard[1, 0] = ChessPieceType_SimulatedBoard.BlackKnight;
        simulatedBoard[2, 0] = ChessPieceType_SimulatedBoard.BlackBishop;
        simulatedBoard[3, 0] = ChessPieceType_SimulatedBoard.BlackQueen;
        simulatedBoard[4, 0] = ChessPieceType_SimulatedBoard.BlackKing;
        simulatedBoard[5, 0] = ChessPieceType_SimulatedBoard.BlackBishop;
        simulatedBoard[6, 0] = ChessPieceType_SimulatedBoard.BlackKnight;
        simulatedBoard[7, 0] = ChessPieceType_SimulatedBoard.BlackRook;
        for (int x = 0; x < 8; x++) simulatedBoard[x, 1] = ChessPieceType_SimulatedBoard.BlackPawn;
    }

    // ▼【タイル】2次元配列に変換するメソッド
    private void RearrangeTileObjects()
    {
        // serializeしたtileObjectsを2次元配列に変換
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                tileBoard[x, y] = tileObjects[y * 8 + x];
            }
        }
    }

    // ▼【データ】インデックスから指定された座標の状態を調べるメソッド
    public ChessPieceType_SimulatedBoard GetPieceAtSimulatedBoard(Vector2Int index)
    {
        if (index.x < 0 || index.x >= 8 || index.y < 0 || index.y >= 8) // 盤面外を確認する場合のガード処理
        {
            return ChessPieceType_SimulatedBoard.None;
        }
        return simulatedBoard[index.x, index.y];
    }

    // ▼【3D】インデックスから駒を取得するメソッド
    public Piece GetPieceAtPieceObjectBoard(Vector2Int index)
    {
        if (index.x >= 0 && index.x < 8 && index.y >= 0 && index.y < 8)
        {
            return pieceObjectBoard[index.x, index.y];
        }
        return null;
    }

    // ▼【タイル】2次元配列からマスを取得するメソッド
    public TileController GetPieceAtTileBoard(int x, int y)
    {
        if (x >= 0 && x < 8 && y >= 0 && y < 8)
        {
            return tileBoard[x, y];
        }
        return null;
    }

    // ▼【3D】オブジェクトの座標を調べてGamaManagerへ渡すメソッド
    public void IdentifyGameObject(GameObject gameObject)
    {
        // クリックされたオブジェクトがマスだった場合
        if (gameObject.TryGetComponent(out TileController clickedSquare))
        {
            clickedIndex = clickedSquare.BoardIndex;
            Debug.Log($"クリックされたマス:{clickedSquare.AlgebraicNotation}(インデックス:{clickedIndex})");

            Piece pieceOnSquare = GetPieceAtPieceObjectBoard(clickedIndex); // 共通のインデックスからマスに乗っている駒オブジェクトを取得

            if (pieceOnSquare != null)
            {
                Debug.Log($"そのマスには{pieceOnSquare.name}が乗っています。");
                GameManager.Instance.OnBoardClicked(clickedIndex); // GammeManagerへクリックされた座標を渡す
            }
            else
            {
                Debug.Log("そのマスは空です。");
            }
        }

        // クリックされたオブジェクトが駒だった場合
        else if (gameObject.TryGetComponent(out Piece clickedPiece))
        {
            clickedIndex = clickedPiece.CurrentIndex;
            Debug.Log($"その駒は{clickedPiece.name}です。");
            GameManager.Instance.OnBoardClicked(clickedIndex); // GammeManagerへクリックされた座標を渡す
        }
        else
        {
            Debug.Log("クリックされたゲームオブジェクトはマスでも駒でもありません。");
            return;
        }
    }

    // ▼【データ】駒の移動許可後のデータ層の盤面データ更新メソッド
    public void UpdateBoardState(int fromX, int fromY, int toX, int toY)
    {
        ChessPieceType_SimulatedBoard movingPiece = simulatedBoard[fromX, fromY]; // 移動元の駒を取得

        simulatedBoard[fromX, fromY] = ChessPieceType_SimulatedBoard.None; // 移動元のマスを空にする

        simulatedBoard[toX, toY] = movingPiece; // 移動先のマスに駒を置く
    }

    // ▼【3D】駒の移動許可後の3D駒オブジェクト層のデータ更新 + 3Dオブジェクトの移動指示メソッド
    public void MovePiece(Vector2Int from, Vector2Int to)
    {
        Piece piece = pieceObjectBoard[from.x, from.y];

        // 配列データの更新
        pieceObjectBoard[to.x, to.y] = piece;
        pieceObjectBoard[from.x, from.y] = null;

        simulatedBoard[to.x, to.y] = simulatedBoard[from.x, from.y];
        simulatedBoard[from.x, from.y] = ChessPieceType_SimulatedBoard.None;

        // PieceManager.csに3Dオブジェクトの物理的な移動を指示
        pieceManager.AnimateMove(piece, to);
    }
}
