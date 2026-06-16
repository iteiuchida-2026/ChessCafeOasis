using UnityEngine;
using UnityEngine.InputSystem;

//////// スクリプトの説明：【マウスクリックとタッチパネルのタッチを検知して、Rayを飛ばした先にあるコライダーから対象物を判断する】////////

public class InputHandler : MonoBehaviour
{

    public GameObject clickedGameObject { get; set; } // クリックされたゲームオブジェクト用の変数を宣言

    private void Update()
    {
        // マウスクリックによる検知
        if (Mouse.current != null)　// nullチェック
        {
            if (Mouse.current.leftButton.wasPressedThisFrame) // マウスの左クリックが押された瞬間
            {
                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.value); // rayを飛ばす 
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit)) // rayが当たった場合
                {
                    clickedGameObject = hit.collider.gameObject; // コライダーのゲームオブジェクトを検出してclickedGameObjectに格納
                    Debug.Log($"クリックされたオブジェクト = {clickedGameObject}");
                }
            }
        }

        // タッチパネルのタッチによる検知
        if (Touchscreen.current != null) // nullチェック
        {
            if (Input.touchCount > 0) // ひとつ以上のタッチがある場合
            {
                Touch touch = Input.GetTouch(0); // 一つ目のタッチを取得

                Ray ray = Camera.main.ScreenPointToRay(touch.position); // rayを飛ばす
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit)) // rayが当たった場合
                {
                    clickedGameObject = hit.collider.gameObject; // コライダーのゲームオブジェクトを検出してclickedGameObjectに格納
                    Debug.Log($"タッチされたオブジェクト = {clickedGameObject}");
                }
            }
        }
    }
}
