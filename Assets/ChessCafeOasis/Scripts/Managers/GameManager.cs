using UnityEngine;

//////// スクリプトの説明：【各スクリプト間の情報の中継ハブ役、かつ指示役。】////////

//////// データの流れ③-1：＜ChessBoardManagerが■駒オブジェクト情報を取得＞ 　                      ////////
////////　→　＜GameManager：OnBoardClickedメソッドで該当ターンの駒色かどうか、駒選択中かどうか判別＞ ////////
////////　                                                              　                         　 ////////
////////　分岐：                                                          　                         　 ////////
////////　1回目のクリック：データの流れ③-2：＜GameManager■＞　→　＜ChessBoardManagerが■受け取る＞　 ////////
////////　2回目のクリック：データの流れ④：＜GameManager■＞　→　＜ChessRuleRefereeが■受け取る＞ 　////////
////////　                                                              　                         　 ////////
//////// データの流れ⑤：＜GameManagerr＞ → ＜ChessBoardManagerでデータおよびOBJ更新＞ → ＜GameManagerでターン更新＞////////
//////// データの流れ⑥：＜GameManegerでターン更新時にチェックメイト確認を依頼＞ → ＜ChessRuleRefereeでチェックメイト確認＞////////
//////// データの流れ⑧：チェックメイトの場合：＜GameManeger＞ → ＜＞　＜＞　＜＞////////

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
    private Piece _selectedPiece; // 現在選択されている駒

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
        // ＜ここでGameRoomSetUpを呼んで部屋設定を後ほど行う＞

        chessBoardManager.InitializeBoards(); // 盤面の初期化処理

        ChangeState(GameState.WhiteTurn); // 先手の白番とする
    }

    // ▼ゲーム状態を変更するメソッド
    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
    }

    // ▼ChessBoardManagerから「駒がクリックされた」と通知を受け取るメソッド
    public void OnBoardClicked(Vector2Int clickedIndex)
    {
        if (CurrentState != GameState.WhiteTurn && CurrentState != GameState.BlackTurn && CurrentState != GameState.PieceSelected) return;

        // 既に駒を選択中で今回クリックしたマスへ移動を試みる場合
        if (CurrentState == GameState.PieceSelected)
        {
            if (chessRuleReferee.IsValidMove(_selectedPiece, clickedIndex, chessBoardManager.GetSimulatedBoard())) // 引数の調整追加が必要
            {
                // 合法手なら移動を実行
                chessBoardManager.UpdateBoardState(_selectedPos.x, _selectedPos.y, clickedIndex.x, clickedIndex.y); //データ層2次元配列更新
                chessBoardManager.MovePiece(_selectedPos, clickedIndex); //3D駒オブジェクト層2次元配列更新＋オブジェクト配置更新
                EndTurn();
            }
            else
            {
                // 不正な手なら選択解除、または自色の別の駒なら選択変更
                TrySelectPiece(clickedIndex);
            }
        }
        // まだ駒を選択していない場合
        else
        {
            TrySelectPiece(clickedIndex);
        }
    }

    // ▼選択先の駒を取得するメソッド
    private void TrySelectPiece(Vector2Int pos)
    {
        Piece piece = chessBoardManager.GetPieceAtPieceObjectBoard(pos);
        if (piece != null && IsCurrentTurnColor(piece.PieceColor))
        {
            _selectedPiece = piece; // 該当座標の駒オブジェクトを格納
            ChangeState(GameState.PieceSelected);
            // ＜ここにタイルを光らせる処理を後ほど追加する＞
        }
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
