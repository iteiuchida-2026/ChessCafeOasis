using System;
using UnityEngine;

//////// スクリプトの説明：【各スクリプト間の情報の中継ハブ役、かつ指示役。】////////

//////// データの流れ③：＜ChessBoardManagerが■駒オブジェクト情報を取得＞ 　////////
////////　→　＜GameManager：OnPieceClickedメソッドで該当ターンの駒か判別し、1回目のクリックで移動可能タイルを光らせる＞ ////////
////////　→　＜GameManager：2回目のクリックで合法手なら移動を実行する＞ 　　////////
////////　                                                              　　 ////////
////////　データは状態で分岐する：                                      　　 ////////
////////　＜A.1回目のクリックの場合：　→　ChessBoardManagerが■受け取る＞　 ////////
////////　＜B.2回目のクリックの場合：　→　ChessRuleRefereeが■受け取る＞ 　////////

// ◆概要：ゲームの状態をenumで用意する
public enum GameState
{
    Setup,
    WhiteTurn,
    BlackTurn,
    PieceSelected,
    Moving,
    Checkmate,
    GameOver
}

// ◆概要：基本的にフェーズ管理専用の役目とし、移動処理やルール判定のコードを持たせない。
public class GameManager : MonoBehaviour
{
    [Header("管理対象クラス")]
    [SerializeField] private GameRoomSetUp gameRoomSetUp;
    [SerializeField] private ChessRuleReferee chessRuleReferee;
    [SerializeField] private RecordManager recordManager;
    [SerializeField] private ChessBoardManager chessBoardManager;

    public GameState CurrentState { get; set; } // 現在のプレイヤーターン変数
    private Vector2Int _selectedPos; // 現在選択されている座標

    public static GameManager Instance { get; private set; } // シングルトンのインスタンスを作成

    // シングルトン処理をAwakeで実施
    private void Awake()
    {
        // シングルトンで永続化処理
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ゲーム開始
    private void Start()
    {
        //ChangeState(GameState.Setup);
        ////ここでGameRoomSetUpを呼んで部屋設定を行う

        chessBoardManager.InitializeBoards(); // 盤面の初期化処理

        ChangeState(GameState.WhiteTurn); // 先手の白番とする
    }

    // ▼ゲーム状態を変更するメソッド
    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
    }

    // ▼ChessBoardManagerから「駒がクリックされた」と通知を受け取るメソッド
    public void OnPieceClicked(GameObject clickedPiece)
    {
        if (CurrentState != GameState.WhiteTurn && CurrentState != GameState.BlackTurn && CurrentState != GameState.PieceSelected) return;

        // 既に駒を選択中で今回クリックしたマスへ移動を試みる場合
        if (CurrentState == GameState.PieceSelected)
        {
            if (chessRuleReferee.IsValidMove(_selectedPos, clickedPos, chessBoardManager.GetPieceAtSimulatedBoard())) // 引数の調整追加が必要
            {
                // 合法手なら移動を実行

            }
        }
    }

    // ▼選択先の駒を取得するメソッド
    private void TrySelectPiece(Vector2Int pos)
    {
        Piece piece = chessBoardManager.GetPieceAtPieceObjectBoard(pos);
        if (Enum.TryParse(piece.PieceColor, out PieceColor pieceColor))
        {
            Debug.Log($"{pieceColor}に変換成功しました。");
            if (piece != null && IsCurrentTurnColor(pieceColor))
            {
                _selectedPos = pos;
                ChangeState(GameState.PieceSelected);
                // ＜ここにタイルを光らせる処理を後ほど追加する＞
            }
        }
        Debug.Log($"{piece.PieceColor}をEnumのPieceColorに変換できませんでした。");


    }

    // ▼現在ターンの色を判断するメソッド
    private bool IsCurrentTurnColor(PieceColor color)
    {
        if (CurrentState == GameState.WhiteTurn && color == PieceColor.White) return true;
        if (CurrentState == GameState.BlackTurn && color == PieceColor.Black) return true;
        return false;
    }

    // ▼ターン終了メソッド
    private void EndTurn()
    {
        // ターン交代処理
        // ＜ここにチェックメイト判定を後ほど追加する＞
        if (CurrentState == GameState.PieceSelected || CurrentState == GameState.Moving)
        {
            GameState nextTurn = (_selectedPos == Vector2Int.zero) ?
                GameState.BlackTurn : GameState.WhiteTurn; // 簡易判定
            // 実際は直前に動かした駒の色と逆にする
            ChangeState(chessBoardManager.GetPieceAtPieceObjectBoard(_selectedPos)?.PieceColor == // PieceObjectBoardにPieceが入っていないためエラー
                PieceColor.White ? GameState.BlackTurn : GameState.WhiteTurn);
        }
    }
}
