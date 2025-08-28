using Hukiry.UI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Joystick))]
public class JoystickEditor : Editor
{
    Joystick joystick;
    private void OnEnable()
    {
        joystick = target as Joystick;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (joystick.transform.childCount !=1)
        {
            if (GUILayout.Button("重置摇杆"))
            {
                joystick.ResetLayout();
            }
        }
    }
}
