using UnityEngine;

//////// スクリプトの説明：【3Dのチェス盤のマスにアタッチする。駒の移動時にマスを光らせる。】////////

public class TileController : MonoBehaviour
{
    // マスを光らせる
    public void LightUpSquare()
    {
        // 後ほど追加する
        Debug.Log($"{gameObject}のマスが光ります。");
    }
}
