using UnityEngine;
using UnityEngine.InputSystem;

public class Exit : MonoBehaviour
{
    [SerializeField]
    private Key exitKey = Key.Escape; // ここで終了キーを指定（例: Escapeキー）

    private void Update()
    {
        if (Keyboard.current[exitKey].wasPressedThisFrame)
        {
            // エディタ上で実行している場合はエディタを終了する
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
            // ビルドされたアプリケーションの場合はアプリケーションを終了する
            Application.Quit();
#endif
        }
    }
}