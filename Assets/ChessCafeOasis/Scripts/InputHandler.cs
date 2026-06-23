using UnityEngine;
using UnityEngine.InputSystem;

//////// スクリプトの説明：【マウスクリックとタッチパネルのタッチを検知して、Rayを飛ばした先にあるコライダーから対象物を判断する】 ////////

//////// データの流れ①：＜InputManagerでクリックした■オブジェクト情報＞　→　＜ChessBoardManagerが■受け取る＞ ////////



// ◆概要：プレイヤー操作の検知
public class InputHandler : MonoBehaviour
{
    [Header("情報送付先クラス")]
    [SerializeField] private ChessBoardManager chessBoardManager;

    private GameObject clickedGameObject; // クリックされたゲームオブジェクト用の変数を宣言

    // ▼マウスとタッチパネルでプレイヤーの操作を検知
    private void Update()
    {
        // ①マウスクリックによる検知
        if (Mouse.current != null) // マウス接続があれば以下の処理が有効
        {
            if (Mouse.current.leftButton.wasPressedThisFrame) // マウスの左クリックが押された瞬間
            {
                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.value); // rayを飛ばす 
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    OnHitRay(hit); // rayが当たった場合はChessBoardManagerにオブジェクト情報を渡す
                }
            }
        }

        // ②タッチパネルのタッチによる検知
        if (Touchscreen.current != null) // タッチスクリーンがあれば以下の処理が有効
        {
            if (Input.touchCount > 0) // ひとつ以上のタッチがある場合
            {
                Touch touch = Input.GetTouch(0); // 一つ目のタッチを取得

                Ray ray = Camera.main.ScreenPointToRay(touch.position); // rayを飛ばす
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    OnHitRay(hit); // rayが当たった場合はChessBoardManagerにオブジェクト情報を渡す
                }
            }
        }
    }

    // ▼オブジェクト情報をChessBoardManagerに渡すメソッド
    private void OnHitRay(RaycastHit hit)
    {
        if (hit.collider != null)
        {
            clickedGameObject = hit.collider.gameObject; // hitしたコライダーのゲームオブジェクトを検出してclickedGameObjectに格納

            Debug.Log($"クリックされたオブジェクト = {clickedGameObject}");

            chessBoardManager.IdentifyGameObject(clickedGameObject); // ChessBoardManagerにクリックされたゲームオブジェクトの情報を渡す
        }
        Debug.Log("選択先のコライダーがありません。");
        return;

    }
}
