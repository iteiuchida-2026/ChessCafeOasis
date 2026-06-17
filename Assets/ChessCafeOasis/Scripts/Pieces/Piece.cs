using UnityEngine;

//////// スクリプトの説明：【Pieceの基本スクリプト、これを継承して各駒用のスクリプトを作成する】////////

public class Piece : MonoBehaviour
{
    // PieceBaseInfoをアタッチして駒の基本情報を設定する
    [Header("駒の基本情報を設定")]
    [SerializeField] private PieceBaseInfo pieceBaseInfo;

    // Piece側でPieceBaseInfoの情報を保持するための変数を初期化
    public string pieceName { get; set; } // 駒の名前
    public string pieceType { get; set; } // 駒の種類
    public string pieceColor { get; set; } // 駒の色
    public int maxMoveSquares { get; set; } // 駒の最大移動可能マス数
    public string startingSquare { get; set; } // 駒の初期配置マス

    // 駒の現在位置把握用の設定
    public string currentSquare { get; set; } // 駒の現在位置


    // スタート時にPieceBaseInfoから情報を取得しておく
    public virtual void Start()
    {
        pieceType = pieceBaseInfo.pieceType.ToString(); // 駒の種類をPieceBaseInfoから取得
        pieceColor = pieceBaseInfo.pieceColor.ToString(); // 駒の色をPieceBaseInfoから取得
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
