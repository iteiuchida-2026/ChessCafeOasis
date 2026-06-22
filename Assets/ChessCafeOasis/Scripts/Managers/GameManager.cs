using UnityEngine;

//////// スクリプトの説明：【各スクリプト間の情報の中継ハブ役、かつ指示役。】////////

// ◆概要：プレイヤーターンをenumで用意する
public enum PlayerTurn
{
    White,
    Black,
}

// ◆概要：
public class GameManager : MonoBehaviour
{
    [Header("管理対象クラス")]
    [SerializeField] private GameRoomSetUp gameRoomSetUp;
    [SerializeField] private ChessRuleReferee chessRuleReferee;
    [SerializeField] private RecordManager recordManager;
    [SerializeField] private ChessBoardManager chessBoardManager;

    public PlayerTurn currentTurn { get; set; } // 現在のプレイヤーターン変数を宣言

    public static GameManager Instance { get; private set; } // シングルトンのインスタンスを作成

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

        currentTurn = PlayerTurn.White; // 先攻の白番を起動時に設定する
    }

    // ▼ターンを切り替えるメソッド
    private void SwitchTurn()
    {
        currentTurn = (currentTurn == PlayerTurn.White) ? PlayerTurn.Black : PlayerTurn.White;
    }

}
