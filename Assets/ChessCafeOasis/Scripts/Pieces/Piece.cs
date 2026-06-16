using UnityEngine;

// Override前提のPieceクラスの基本クラス
public class Piece : MonoBehaviour
{
    // PieceBaseInfoをアタッチして駒の基本情報を設定する
    [Header("駒の基本情報を設定")]
    [SerializeField] private PieceBaseInfo pieceBaseInfo;

    // Piece側でPieceBaseInfoの情報を保持するための変数を初期化
    private string pieceName { get; set; } // 駒の名前
    private string pieceType { get; set; } // 駒の種類
    private string pieceColor { get; set; } // 駒の色
    private int maxMoveSquares { get; set; } // 駒の最大移動可能マス数
    private string startingSquare { get; set; } // 駒の初期配置マス


    // スタート時にPieceBaseInfoから情報を取得しておく
    public virtual void Start()
    {
        pieceName = pieceBaseInfo.pieceName; // 駒の名前をPieceBaseInfoから取得
        pieceType = pieceBaseInfo.pieceType.ToString(); // 駒の種類をPieceBaseInfoから取得
        pieceColor = pieceBaseInfo.pieceColor.ToString(); // 駒の色をPieceBaseInfoから取得
        maxMoveSquares = pieceBaseInfo.maxMoveSquares; // 駒の最大移動可能マス数をPieceBaseInfoから取得
        startingSquare = pieceBaseInfo.startingSquare.ToString(); // 駒の初期配置マスをPieceBaseInfoから取得
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
