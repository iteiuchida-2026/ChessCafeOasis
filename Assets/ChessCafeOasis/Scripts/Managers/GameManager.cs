using UnityEngine;

//////// スクリプトの説明：【各スクリプト間の情報の中継ハブ役、かつ指示役。】////////


// ◆概要：
public class GameManager : MonoBehaviour
{
    [Header("管理対象クラス")]
    [SerializeField] private GameRoomSetUp gameRoomSetUp;
    [SerializeField] private ChessRuleReferee chessRuleReferee;
    [SerializeField] private RecordManager recordManager;
    [SerializeField] private ChessBoardManager chessBoardManager;

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
    }

    void Update()
    {

    }
}
