//using UnityEditor;
//using UnityEngine;
//using System.Reflection;

//[CustomEditor(typeof(TestExample), true)]
//public class TestExampleEditor : Editor
//{

//    private TestExample obj;

//    private void OnEnable()
//    {
//        obj = target as TestExample;
//    }
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        if (GUILayout.Button("测试执行"))
//        {
//            obj.StartTest();
//        }
//    }
//}