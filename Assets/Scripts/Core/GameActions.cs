using UnityEngine;

public class GameActions : MonoBehaviour
{
    public static void ExitGame()
    {
#if UNITY_EDITOR
        // �ڱ༭����ֹͣ����
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // �ڹ����汾���˳�Ӧ��
        Application.Quit();
#endif
    }
}
