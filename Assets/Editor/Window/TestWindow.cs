using UnityEditor;
using UnityEngine;

public class TestWindow : EditorWindow
{
    [MenuItem("Window/Open Test")]
    static void OpenTest()
    {
        TestWindow testWindow = GetWindow<TestWindow>("test");
        testWindow.ShowAuxWindow();
    }

    private Vector2 start, end;
    private Vector2 startTangent, endTangent;
    private void OnEnable()
    {
        start = new Vector2(100, 200);
        end = new Vector2(800, 200);
        startTangent = new Vector2(start.x, start.y);
        endTangent = new Vector2(end.x , end.y);
    }

    private void OnGUI()
    {
        //Hukiry.HukiryGLDraw.MultiLine = true;
        if (EditorApplication.isCompiling) return;
        for (int i = 0; i < 4; i++)
        {
            var d = i * 5;
            start = new Vector2(100, 200 + d);
            end = new Vector2(800, 200 + d);
            startTangent = new Vector2(start.x, start.y + 150);
            endTangent = new Vector2(end.x, end.y - 150);
            Hukiry.HukiryGLDraw.DrawBezier(start, startTangent, end, endTangent, Color.green, Color.red, 0.8F, 50);
        }
    }

    private void OnDisable()
    {
        Hukiry.HukiryGLDraw.Destroy();
    }
}
