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
    public string currentSquare { get; set; } // 駒の現在位置


    // スタート時にPieceBaseInfoから情報を取得しておく
    public virtual void Start()
    {
        pieceType = pieceBaseInfo.pieceType.ToString();
        pieceColor = pieceBaseInfo.pieceColor.ToString();
    }

    private void Update()
    {

    }

    // 移動処理
    public virtual void Move()
    {

    }

    // 駒を取る処理
    public virtual void Take()
    {

    }

    // 駒が取られる処理（thisObject）
    public virtual void OnTaken()
    {

    }

}
