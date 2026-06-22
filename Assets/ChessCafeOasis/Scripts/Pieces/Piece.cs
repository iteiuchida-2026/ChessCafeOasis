using UnityEngine;

//////// スクリプトの説明：【Pieceの基本スクリプト、これを継承して各駒用のスクリプトを作成する】////////

public class Piece : MonoBehaviour
{
    // PieceBaseInfoをアタッチして駒の基本情報を設定する
    [Header("駒の基本情報を設定")]
    [SerializeField] private PieceBaseInfo pieceBaseInfo;

    // Piece側でPieceBaseInfoの情報を保持するための変数を初期化
    public string pieceType { get; set; }
    public string pieceColor { get; set; }

    // 駒の現在位置把握用の設定
    public Vector2Int currentIndex { get; set; } // 駒の現在位置

    // ルール判定用のフラグ
    public bool HasMoved { get; private set; } = false; // キャスリング判定用
    public bool IsPromoted { get; private set; } = false; // ポーンのプロモーション用

    // オブジェクトの現在の状態管理用
    public enum PieceStatus
    {
        Active,
        Moving, // クリックして移動待機中
        Destroyed
    }

    public PieceStatus currentStatus { get; private set; } = PieceStatus.Active;

    // スタート時にPieceBaseInfoから情報を取得しておく
    public virtual void Start()
    {
        pieceType = pieceBaseInfo.pieceType.ToString();
        pieceColor = pieceBaseInfo.pieceColor.ToString();
    }



    // ▼移動メソッド（各駒でOverrideしてカスタマイズして使用する）
    public virtual void Move(Vector2Int index)
    {
        // 移動処理が入る予定

        currentIndex = index; // 駒の現在位置を移動先のindexで更新

        HasMoved = true;
    }

    // ▼プロモーションメソッド（ポーンのみOverrideして使用する）
    public virtual void Promote()
    {
        IsPromoted = true;

        // 変更処理が入る予定
    }


    // ▼駒が取られるメソッド（各駒でOverrideしてカスタマイズして使用する）
    public virtual void OnTaken()
    {
        currentStatus = PieceStatus.Destroyed;

        // 現時点では削除処理とするが、今後はチェス盤外へ移動する演出としたい
        gameObject.SetActive(false);
    }

}
