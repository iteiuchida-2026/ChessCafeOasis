using System.Collections.Generic;
using UnityEngine;

//////// スクリプトの説明：【チェス盤の各マスにどの駒が存在するか記録し管理する】////////

public class ChessBoardManager : MonoBehaviour
{
    [SerializeField] private List<Piece> pieceList; // Pieceを格納するリストを宣言
    [SerializeField] private List<TileController> tileControllers; // TileControllerを格納するリストを宣言
    [SerializeField] private PieceManager pieceFactory; // PieceFactoryの参照を取得するための変数を宣言
    [SerializeField] private InputHandler inputHandler; // InputHandlerの参照を取得するための変数を宣言

    void Start()
    {

    }

    void Update()
    {

    }
}
