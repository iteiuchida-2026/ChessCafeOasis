using UnityEngine;

public class Piece : MonoBehaviour
{
    [Header("駒の基本情報を設定")]
    [SerializeField] private PieceBaseInfo pieceBaseInfo;

    private string pieceName { get; set; } // 駒の名前
    private string pieceType { get; set; } // 駒の種類
    private string pieceColor { get; set; } // 駒の色
    private int maxMoveSquares { get; set; } // 駒の最大移動可能マス数
    private string startingSquare { get; set; } // 駒の初期配置マス


    // スタート時にPieceBaseInfoから情報を取得しておく
    private void Start()
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

    // 移動する
    public void Move()
    {

    }

    // 駒を取る
    public void Take()
    {

    }

    // 駒が取られる（thisObject）
    public void OnTaken()
    {

    }

}
