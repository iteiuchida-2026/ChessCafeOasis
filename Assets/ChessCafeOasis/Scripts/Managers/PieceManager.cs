using UnityEngine;

//////// スクリプトの説明：【3D駒オブジェクトの管理を担当。初期配置位置にPrefabを生成、初期化。移動処理。】////////

public class PieceManager : MonoBehaviour
{
    [SerializeField] private GameObject[] PiecePrefabs;
    [SerializeField] private ChessBoardManager chessBoardManager;

    private void Awake()
    {
        Initialize3DBoard();
    }

    private void Initialize3DBoard()
    {

    }
}
