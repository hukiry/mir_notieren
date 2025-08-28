using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using System.Linq;
using UnityEngine;


using Object = UnityEngine.Object;

namespace Hukiry.Editor
{
    public class HukiryGUIEditor
    {
        private const string CLASS_TAG = "";// "Class";
        #region 编辑UI绘制
        /// <summary>
        /// 支持 一维数组，List泛型集合,Unity存在的元素类型，自定义嵌套类
        /// </summary>
        /// <typeparam name="H"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T DrawType<T>(T t, bool isSubFieldClass = false)
        {
            MemberInfo[] infoArray = t.GetType().GetMembers(BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.Public);
            for (int i = 0; i < infoArray.Length; i++)
            {
                if (infoArray[i].GetCustomAttribute<SpaceAttribute>() != null)
                {
                    SpaceAttribute spaceAttribute = infoArray[i].GetCustomAttribute<SpaceAttribute>();
                    GUILayout.Space(spaceAttribute.height); //EditorGUILayout.Space();
                }

                if (infoArray[i].GetCustomAttribute<HideInInspector>() != null)
                {
                    continue;
                }

                if (infoArray[i].MemberType == MemberTypes.Field)
                {
                    FieldInfo field = infoArray[i] as FieldInfo;
                    if (field.FieldType.Name == typeof(T).Name)
                        continue;

                    var obj = DrawMemberInfo(field.FieldType, GetTypeInfoName(infoArray, i, field, isSubFieldClass), field.GetValue(t));
                    field.SetValue(t, obj);
                }
                else if (infoArray[i].MemberType == MemberTypes.Property)
                {
                    PropertyInfo property = infoArray[i] as PropertyInfo;
                    if (property.PropertyType.Name == typeof(T).Name)
                        continue;

                    var obj = DrawMemberInfo(property.PropertyType, GetTypeInfoName(infoArray, i, property, isSubFieldClass), property.GetValue(t, null));
                    property.SetValue(t, obj, null);
                }
            }
            return t;
        }
        public static T DrawType<T, T1>(T t, bool isSubFieldClass = false)
        {
            MemberInfo[] infoArray = t.GetType().GetMembers(BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.Public);
            for (int i = 0; i < infoArray.Length; i++)
            {
                if (infoArray[i].GetCustomAttribute<SpaceAttribute>() != null)
                {
                    SpaceAttribute spaceAttribute = infoArray[i].GetCustomAttribute<SpaceAttribute>();
                    GUILayout.Space(spaceAttribute.height); //EditorGUILayout.Space();
                }

                if (infoArray[i].GetCustomAttribute<HideInInspector>() != null)
                {
                    continue;
                }

                if (infoArray[i].MemberType == MemberTypes.Field)
                {
                    FieldInfo field = infoArray[i] as FieldInfo;
                    if (field.FieldType.Name == typeof(T).Name)
                        continue;

                    var obj = DrawMemberInfo<T1>(field.FieldType, GetTypeInfoName(infoArray, i, field, isSubFieldClass), field.GetValue(t));
                    field.SetValue(t, obj);
                }
                else if (infoArray[i].MemberType == MemberTypes.Property)
                {
                    PropertyInfo property = infoArray[i] as PropertyInfo;
                    if (property.PropertyType.Name == typeof(T).Name)
                        continue;

                    var obj = DrawMemberInfo<T1>(property.PropertyType, GetTypeInfoName(infoArray, i, property, isSubFieldClass), property.GetValue(t, null));
                    property.SetValue(t, obj, null);
                }
            }
            return t;
        }
        public static T DrawType<T, T1, T2>(T t, bool isSubFieldClass = false)
        {
            MemberInfo[] infoArray = t.GetType().GetMembers(BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.Public);
            for (int i = 0; i < infoArray.Length; i++)
            {
                if (infoArray[i].GetCustomAttribute<SpaceAttribute>() != null)
                {
                    SpaceAttribute spaceAttribute = infoArray[i].GetCustomAttribute<SpaceAttribute>();
                    GUILayout.Space(spaceAttribute.height); //EditorGUILayout.Space();
                }

                if (infoArray[i].GetCustomAttribute<HideInInspector>() != null)
                {
                    continue;
                }

                if (infoArray[i].MemberType == MemberTypes.Field)
                {
                    FieldInfo field = infoArray[i] as FieldInfo;
                    if (field.FieldType.Name == typeof(T).Name)
                        continue;

                    var obj = DrawMemberInfo<T1, T2>(field.FieldType, GetTypeInfoName(infoArray, i, field, isSubFieldClass), field.GetValue(t));
                    field.SetValue(t, obj);
                }
                else if (infoArray[i].MemberType == MemberTypes.Property)
                {
                    PropertyInfo property = infoArray[i] as PropertyInfo;
                    if (property.PropertyType.Name == typeof(T).Name)
                        continue;

                    var obj = DrawMemberInfo<T1, T2>(property.PropertyType, GetTypeInfoName(infoArray, i, property, isSubFieldClass), property.GetValue(t, null));
                    property.SetValue(t, obj, null);
                }
            }
            return t;
        }
        public static T DrawType<T, T1, T2, T3>(T t, bool isSubFieldClass = false)
        {
            MemberInfo[] infoArray = t.GetType().GetMembers(BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.Public);
            for (int i = 0; i < infoArray.Length; i++)
            {
                if (infoArray[i].GetCustomAttribute<SpaceAttribute>() != null)
                {
                    SpaceAttribute spaceAttribute = infoArray[i].GetCustomAttribute<SpaceAttribute>();
                    GUILayout.Space(spaceAttribute.height); //EditorGUILayout.Space();
                }

                if (infoArray[i].GetCustomAttribute<HideInInspector>() != null)
                {
                    continue;
                }

                if (infoArray[i].MemberType == MemberTypes.Field)
                {
                    FieldInfo field = infoArray[i] as FieldInfo;
                    if (field.FieldType.Name == typeof(T).Name)
                        continue;

                    var obj = DrawMemberInfo<T1, T2, T3>(field.FieldType, GetTypeInfoName(infoArray, i, field, isSubFieldClass), field.GetValue(t));
                    field.SetValue(t, obj);
                }
                else if (infoArray[i].MemberType == MemberTypes.Property)
                {
                    PropertyInfo property = infoArray[i] as PropertyInfo;
                    if (property.PropertyType.Name == typeof(T).Name)
                        continue;

                    var obj = DrawMemberInfo<T1, T2, T3>(property.PropertyType, GetTypeInfoName(infoArray, i, property, isSubFieldClass), property.GetValue(t, null));
                    property.SetValue(t, obj, null);
                }
            }
            return t;
        }
        public static T DrawType<T, T1, T2, T3, T4>(T t, bool isSubFieldClass = false)
        {
            MemberInfo[] infoArray = t.GetType().GetMembers(BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.Public);
            for (int i = 0; i < infoArray.Length; i++)
            {
                if (infoArray[i].GetCustomAttribute<SpaceAttribute>() != null)
                {
                    SpaceAttribute spaceAttribute = infoArray[i].GetCustomAttribute<SpaceAttribute>();
                    GUILayout.Space(spaceAttribute.height); //EditorGUILayout.Space();
                }

                if (infoArray[i].GetCustomAttribute<HideInInspector>() != null)
                {
                    continue;
                }

                if (infoArray[i].MemberType == MemberTypes.Field)
                {
                    FieldInfo field = infoArray[i] as FieldInfo;
                    if (field.FieldType.Name == typeof(T).Name)
                        continue;

                    var obj = DrawMemberInfo<T1, T2, T3, T4>(field.FieldType, GetTypeInfoName(infoArray, i, field, isSubFieldClass), field.GetValue(t));
                    field.SetValue(t, obj);
                }
                else if (infoArray[i].MemberType == MemberTypes.Property)
                {
                    PropertyInfo property = infoArray[i] as PropertyInfo;
                    if (property.PropertyType.Name == typeof(T).Name)
                        continue;

                    var obj = DrawMemberInfo<T1, T2, T3, T4>(property.PropertyType, GetTypeInfoName(infoArray, i, property, isSubFieldClass), property.GetValue(t, null));
                    property.SetValue(t, obj, null);
                }
            }
            return t;
        }

        private static AttriData GetTypeInfoName(MemberInfo[] infoArray, int i, MemberInfo field, bool isSubFieldClass)
        {
            string showName = field.Name;
            FieldNameAttribute attr = infoArray[i].GetCustomAttribute<FieldNameAttribute>();
            if (attr != null)
            {
                showName = attr.name;
            }
            showName = isSubFieldClass ? "    " + showName : showName;
            return new AttriData(attr, showName);
        }

        private static Dictionary<string, FoldOut> FoldoutDic = new Dictionary<string, FoldOut>();
        private static object DrawMemberInfo(Type ty, AttriData attr, object obj)
        {
            if (ty == typeof(bool))
            {
                return EditorGUILayout.Toggle(attr.GetGUIContent(), (bool)obj);
            }
            else if (ty == typeof(string))
            {
                return EditorGUILayout.TextField(attr.GetGUIContent(), (string)obj);
            }
            else if (ty.IsEnum)
            {
                return EditorGUILayout.EnumPopup(attr.GetGUIContent(), (System.Enum)obj, "DropDown");
            }
            else if (ty == typeof(float))
            {
                return EditorGUILayout.FloatField(attr.GetGUIContent(), (float)obj);
            }
            else if (ty == typeof(double) || ty == typeof(decimal))
            {
                return EditorGUILayout.DoubleField(attr.GetGUIContent(), (double)obj);
            }
            else if (ty == typeof(int) || ty == typeof(uint))
            {
                return EditorGUILayout.IntField(attr.GetGUIContent(), (int)obj);
            }
            else if (ty == typeof(long) || ty == typeof(ulong))
            {
                return EditorGUILayout.LongField(attr.GetGUIContent(), (int)obj);
            }
            else if (ty == typeof(Vector2))
            {
                return EditorGUILayout.Vector2Field(attr.GetGUIContent(), (Vector2)obj);
            }
            else if (ty == typeof(Vector2Int))
            {
                return EditorGUILayout.Vector2IntField(attr.GetGUIContent(), (Vector2Int)obj);
            }
            else if (ty == typeof(Vector3))
            {
                return EditorGUILayout.Vector3Field(attr.GetGUIContent(), (Vector3)obj);
            }
            else if (ty == typeof(Vector3Int))
            {
                return EditorGUILayout.Vector3IntField(attr.GetGUIContent(), (Vector3Int)obj);
            }
            else if (ty == typeof(Vector4))
            {
                return EditorGUILayout.Vector4Field(attr.GetGUIContent(), (Vector4)obj);
            }
            else if (ty == typeof(Rect))
            {
                return EditorGUILayout.RectField(attr.GetGUIContent(), (Rect)obj);
            }
            else if (ty == typeof(RectInt))
            {
                return EditorGUILayout.RectIntField(attr.GetGUIContent(), (RectInt)obj);
            }
            else if (ty == typeof(Color))
            {
                return EditorGUILayout.ColorField(attr.GetGUIContent(), (Color)obj);
            }
            else if (ty == typeof(AnimationCurve))
            {
                return EditorGUILayout.CurveField(attr.GetGUIContent(), (AnimationCurve)obj);
            }
            else if (ty == typeof(Bounds))
            {
                return EditorGUILayout.BoundsField(attr.GetGUIContent(), (Bounds)obj);
            }
            else if (ty == typeof(BoundsInt))
            {
                return EditorGUILayout.BoundsIntField(attr.GetGUIContent(), (BoundsInt)obj);
            }
            else if (ty == typeof(Gradient))
            {
                return EditorGUILayout.GradientField(attr.GetGUIContent(), (Gradient)obj);
            }
            else if (ty == typeof(GameObject) || ty == typeof(Transform))
            {
                return EditorGUILayout.ObjectField(attr.GetGUIContent(), (UnityEngine.Object)obj, ty, true);
            }
            else if (ty.IsGenericType)
            {
                if (IsGenericTypeArgument(ty, attr, obj, out List<int> m_int))
                {
                    return m_int;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<long> m_long))
                {
                    return m_long;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<float> m_float))
                {
                    return m_float;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<double> m_double))
                {
                    return m_double;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<string> m_string))
                {
                    return m_string;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<bool> m_bool))
                {
                    return m_bool;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<System.Enum> m_Enum))
                {
                    return m_Enum;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<Color> m_Color))
                {
                    return m_Color;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<Vector2> m_Vector2))
                {
                    return m_Vector2;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<Vector3> m_Vector3))
                {
                    return m_Vector3;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<Vector4> m_Vector4))
                {
                    return m_Vector4;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<AnimationCurve> m_AnimationCurve))
                {
                    return m_AnimationCurve;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<Gradient> m_Gradient))
                {
                    return m_Gradient;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<Transform> m_Transform))
                {
                    return m_Transform;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<GameObject> m_GameObject))
                {
                    return m_GameObject;
                }
                return obj;
            }
            else if (ty.IsArray)
            {
                if (IsArrayTypeArgument(ty, attr, obj, out int[] m_int))
                {
                    return m_int;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out long[] m_long))
                {
                    return m_long;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out float[] m_float))
                {
                    return m_float;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out double[] m_double))
                {
                    return m_double;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out string[] m_string))
                {
                    return m_string;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out bool[] m_bool))
                {
                    return m_bool;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out System.Enum[] m_Enum))
                {
                    return m_Enum;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out Color[] m_Color))
                {
                    return m_Color;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out Vector2[] m_Vector2))
                {
                    return m_Vector2;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out Vector3[] m_Vector3))
                {
                    return m_Vector3;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out Vector4[] m_Vector4))
                {
                    return m_Vector4;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out AnimationCurve[] m_AnimationCurve))
                {
                    return m_AnimationCurve;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out Gradient[] m_Gradient))
                {
                    return m_Gradient;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out Transform[] m_Transform))
                {
                    return m_Transform;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out GameObject[] m_GameObject))
                {
                    return m_GameObject;
                }

                return obj;
            }
            else if (ty.IsClass)
            {
                if (ty.BaseType == typeof(Component) ||
                    ty.BaseType == typeof(MonoBehaviour) ||
                    ty.BaseType == typeof(Object) ||
                    ty.BaseType?.BaseType == typeof(Object) ||
                    ty.BaseType?.BaseType == typeof(MonoBehaviour) ||
                    ty.BaseType?.BaseType == typeof(Component) ||
                    ty.BaseType?.BaseType?.BaseType == typeof(Object) ||
                    ty.BaseType?.BaseType?.BaseType == typeof(MonoBehaviour) ||
                    ty.BaseType?.BaseType?.BaseType == typeof(Component))
                {
                    return EditorGUILayout.ObjectField(attr.GetGUIContent(), (UnityEngine.Object)obj, ty, true);
                }
                else
                {
                    return ClassTypeCustom(ty, attr, obj);
                }
            }
            else
            {
                Debug.LogWarning("集合元素 无法显示" + ty.Name);
            }

            return obj;
        }
        private static object DrawMemberInfo<T1>(Type ty, AttriData attr, object obj)
        {
            if (ty == typeof(bool))
            {
                return EditorGUILayout.Toggle(attr.GetGUIContent(), (bool)obj);
            }
            else if (ty == typeof(string))
            {
                return EditorGUILayout.TextField(attr.GetGUIContent(), (string)obj);
            }
            else if (ty.IsEnum)
            {
                return EditorGUILayout.EnumPopup(attr.GetGUIContent(), (System.Enum)obj, "DropDown");
            }
            else if (ty == typeof(float))
            {
                return EditorGUILayout.FloatField(attr.GetGUIContent(), (float)obj);
            }
            else if (ty == typeof(double) || ty == typeof(decimal))
            {
                return EditorGUILayout.DoubleField(attr.GetGUIContent(), (double)obj);
            }
            else if (ty == typeof(int))
            {
                return EditorGUILayout.IntField(attr.GetGUIContent(), (int)obj);
            }
            else if (ty == typeof(long) || ty == typeof(ulong))
            {
                return EditorGUILayout.LongField(attr.GetGUIContent(), (int)obj);
            }
            else if (ty == typeof(Vector2))
            {
                return EditorGUILayout.Vector2Field(attr.GetGUIContent(), (Vector2)obj);
            }
            else if (ty == typeof(Vector2Int))
            {
                return EditorGUILayout.Vector2IntField(attr.GetGUIContent(), (Vector2Int)obj);
            }
            else if (ty == typeof(Vector3))
            {
                return EditorGUILayout.Vector3Field(attr.GetGUIContent(), (Vector3)obj);
            }
            else if (ty == typeof(Vector3Int))
            {
                return EditorGUILayout.Vector3IntField(attr.GetGUIContent(), (Vector3Int)obj);
            }
            else if (ty == typeof(Vector4))
            {
                return EditorGUILayout.Vector4Field(attr.GetGUIContent(), (Vector4)obj);
            }
            else if (ty == typeof(Rect))
            {
                return EditorGUILayout.RectField(attr.GetGUIContent(), (Rect)obj);
            }
            else if (ty == typeof(RectInt))
            {
                return EditorGUILayout.RectIntField(attr.GetGUIContent(), (RectInt)obj);
            }
            else if (ty == typeof(Color))
            {
                return EditorGUILayout.ColorField(attr.GetGUIContent(), (Color)obj);
            }
            else if (ty == typeof(AnimationCurve))
            {
                return EditorGUILayout.CurveField(attr.GetGUIContent(), (AnimationCurve)obj);
            }
            else if (ty == typeof(Bounds))
            {
                return EditorGUILayout.BoundsField(attr.GetGUIContent(), (Bounds)obj);
            }
            else if (ty == typeof(BoundsInt))
            {
                return EditorGUILayout.BoundsIntField(attr.GetGUIContent(), (BoundsInt)obj);
            }
            else if (ty == typeof(Gradient))
            {
                return EditorGUILayout.GradientField(attr.GetGUIContent(), (Gradient)obj);
            }
            else if (ty == typeof(T1))
            {
                return ClassTypeCustom<T1>(attr, obj);
            }
            else if (ty.IsGenericType)
            {
                if (IsGenericTypeArgument(ty, attr, obj, out List<int> m_int))
                {
                    return m_int;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<long> m_long))
                {
                    return m_long;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<float> m_float))
                {
                    return m_float;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<double> m_double))
                {
                    return m_double;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<string> m_string))
                {
                    return m_string;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<bool> m_bool))
                {
                    return m_bool;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<System.Enum> m_Enum))
                {
                    return m_Enum;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<Color> m_Color))
                {
                    return m_Color;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<Vector2> m_Vector2))
                {
                    return m_Vector2;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<Vector3> m_Vector3))
                {
                    return m_Vector3;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<Vector4> m_Vector4))
                {
                    return m_Vector4;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<AnimationCurve> m_AnimationCurve))
                {
                    return m_AnimationCurve;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<Gradient> m_Gradient))
                {
                    return m_Gradient;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<Transform> m_Transform))
                {
                    return m_Transform;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<GameObject> m_GameObject))
                {
                    return m_GameObject;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<T1> m_T1))
                {
                    return m_T1;
                }
                return obj;
            }
            else if (ty.IsArray)
            {
                if (IsArrayTypeArgument(ty, attr, obj, out int[] m_int))
                {
                    return m_int;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out long[] m_long))
                {
                    return m_long;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out float[] m_float))
                {
                    return m_float;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out double[] m_double))
                {
                    return m_double;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out string[] m_string))
                {
                    return m_string;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out bool[] m_bool))
                {
                    return m_bool;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out System.Enum[] m_Enum))
                {
                    return m_Enum;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out Color[] m_Color))
                {
                    return m_Color;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out Vector2[] m_Vector2))
                {
                    return m_Vector2;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out Vector3[] m_Vector3))
                {
                    return m_Vector3;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out Vector4[] m_Vector4))
                {
                    return m_Vector4;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out AnimationCurve[] m_AnimationCurve))
                {
                    return m_AnimationCurve;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out Gradient[] m_Gradient))
                {
                    return m_Gradient;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out Transform[] m_Transform))
                {
                    return m_Transform;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out GameObject[] m_GameObject))
                {
                    return m_GameObject;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out T1[] m_T1))
                {
                    return m_T1;
                }
                return obj;
            }
            else if (ty.IsClass)
            {
                if (ty.BaseType == typeof(Component) ||
                     ty.BaseType == typeof(MonoBehaviour) ||
                     ty.BaseType == typeof(Object) ||
                     ty.BaseType?.BaseType == typeof(Object) ||
                     ty.BaseType?.BaseType == typeof(MonoBehaviour) ||
                     ty.BaseType?.BaseType == typeof(Component) ||
                     ty.BaseType?.BaseType?.BaseType == typeof(Object) ||
                     ty.BaseType?.BaseType?.BaseType == typeof(MonoBehaviour) ||
                     ty.BaseType?.BaseType?.BaseType == typeof(Component))
                {
                    return EditorGUILayout.ObjectField(attr.GetGUIContent(), (UnityEngine.Object)obj, ty, true);
                }
                else
                {
                    return ClassTypeCustom(ty, attr, obj);
                }
            }
            else
            {
                Debug.LogWarning("集合元素 无法显示" + ty.Name);
            }

            return obj;
        }
        private static object DrawMemberInfo<T1, T2>(Type ty, AttriData attr, object obj)
        {
            if (ty == typeof(bool))
            {
                return EditorGUILayout.Toggle(attr.GetGUIContent(), (bool)obj);
            }
            else if (ty == typeof(string))
            {
                return EditorGUILayout.TextField(attr.GetGUIContent(), (string)obj);
            }
            else if (ty.IsEnum)
            {
                return EditorGUILayout.EnumPopup(attr.GetGUIContent(), (System.Enum)obj, "DropDown");
            }
            else if (ty == typeof(float))
            {
                return EditorGUILayout.FloatField(attr.GetGUIContent(), (float)obj);
            }
            else if (ty == typeof(double) || ty == typeof(decimal))
            {
                return EditorGUILayout.DoubleField(attr.GetGUIContent(), (double)obj);
            }
            else if (ty == typeof(int))
            {
                return EditorGUILayout.IntField(attr.GetGUIContent(), (int)obj);
            }
            else if (ty == typeof(long) || ty == typeof(ulong))
            {
                return EditorGUILayout.LongField(attr.GetGUIContent(), (int)obj);
            }
            else if (ty == typeof(Vector2))
            {
                return EditorGUILayout.Vector2Field(attr.GetGUIContent(), (Vector2)obj);
            }
            else if (ty == typeof(Vector2Int))
            {
                return EditorGUILayout.Vector2IntField(attr.GetGUIContent(), (Vector2Int)obj);
            }
            else if (ty == typeof(Vector3))
            {
                return EditorGUILayout.Vector3Field(attr.GetGUIContent(), (Vector3)obj);
            }
            else if (ty == typeof(Vector3Int))
            {
                return EditorGUILayout.Vector3IntField(attr.GetGUIContent(), (Vector3Int)obj);
            }
            else if (ty == typeof(Vector4))
            {
                return EditorGUILayout.Vector4Field(attr.GetGUIContent(), (Vector4)obj);
            }
            else if (ty == typeof(Rect))
            {
                return EditorGUILayout.RectField(attr.GetGUIContent(), (Rect)obj);
            }
            else if (ty == typeof(RectInt))
            {
                return EditorGUILayout.RectIntField(attr.GetGUIContent(), (RectInt)obj);
            }
            else if (ty == typeof(Color))
            {
                return EditorGUILayout.ColorField(attr.GetGUIContent(), (Color)obj);
            }
            else if (ty == typeof(AnimationCurve))
            {
                return EditorGUILayout.CurveField(attr.GetGUIContent(), (AnimationCurve)obj);
            }
            else if (ty == typeof(Bounds))
            {
                return EditorGUILayout.BoundsField(attr.GetGUIContent(), (Bounds)obj);
            }
            else if (ty == typeof(BoundsInt))
            {
                return EditorGUILayout.BoundsIntField(attr.GetGUIContent(), (BoundsInt)obj);
            }
            else if (ty == typeof(Gradient))
            {
                return EditorGUILayout.GradientField(attr.GetGUIContent(), (Gradient)obj);
            }
            else if (ty == typeof(T1))
            {
                return ClassTypeCustom<T1>(attr, obj);
            }
            else if (ty == typeof(T2))
            {
                return ClassTypeCustom<T2>(attr, obj);
            }
            else if (ty.IsGenericType)
            {
                if (IsGenericTypeArgument(ty, attr, obj, out List<int> m_int))
                {
                    return m_int;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<long> m_long))
                {
                    return m_long;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<float> m_float))
                {
                    return m_float;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<double> m_double))
                {
                    return m_double;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<string> m_string))
                {
                    return m_string;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<bool> m_bool))
                {
                    return m_bool;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<System.Enum> m_Enum))
                {
                    return m_Enum;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<Color> m_Color))
                {
                    return m_Color;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<Vector2> m_Vector2))
                {
                    return m_Vector2;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<Vector3> m_Vector3))
                {
                    return m_Vector3;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<Vector4> m_Vector4))
                {
                    return m_Vector4;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<AnimationCurve> m_AnimationCurve))
                {
                    return m_AnimationCurve;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<Gradient> m_Gradient))
                {
                    return m_Gradient;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<Transform> m_Transform))
                {
                    return m_Transform;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<GameObject> m_GameObject))
                {
                    return m_GameObject;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<T1> m_T1))
                {
                    return m_T1;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<T2> m_T2))
                {
                    return m_T2;
                }
                return obj;
            }
            else if (ty.IsArray)
            {
                if (IsArrayTypeArgument(ty, attr, obj, out int[] m_int))
                {
                    return m_int;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out long[] m_long))
                {
                    return m_long;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out float[] m_float))
                {
                    return m_float;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out double[] m_double))
                {
                    return m_double;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out string[] m_string))
                {
                    return m_string;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out bool[] m_bool))
                {
                    return m_bool;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out System.Enum[] m_Enum))
                {
                    return m_Enum;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out Color[] m_Color))
                {
                    return m_Color;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out Vector2[] m_Vector2))
                {
                    return m_Vector2;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out Vector3[] m_Vector3))
                {
                    return m_Vector3;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out Vector4[] m_Vector4))
                {
                    return m_Vector4;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out AnimationCurve[] m_AnimationCurve))
                {
                    return m_AnimationCurve;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out Gradient[] m_Gradient))
                {
                    return m_Gradient;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out Transform[] m_Transform))
                {
                    return m_Transform;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out GameObject[] m_GameObject))
                {
                    return m_GameObject;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out T1[] m_T1))
                {
                    return m_T1;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out T2[] m_T2))
                {
                    return m_T2;
                }
                return obj;
            }
            else if (ty.IsClass)
            {
                if (ty.BaseType == typeof(Component) ||
                    ty.BaseType == typeof(MonoBehaviour) ||
                    ty.BaseType == typeof(Object) ||
                    ty.BaseType?.BaseType == typeof(Object) ||
                    ty.BaseType?.BaseType == typeof(MonoBehaviour) ||
                    ty.BaseType?.BaseType == typeof(Component) ||
                    ty.BaseType?.BaseType?.BaseType == typeof(Object) ||
                    ty.BaseType?.BaseType?.BaseType == typeof(MonoBehaviour) ||
                    ty.BaseType?.BaseType?.BaseType == typeof(Component))
                {
                    return EditorGUILayout.ObjectField(attr.GetGUIContent(), (UnityEngine.Object)obj, ty, true);
                }
                else
                {
                    return ClassTypeCustom(ty, attr, obj);
                }
            }
            else
            {
                Debug.LogWarning("集合元素 无法显示" + ty.Name);
            }

            return obj;
        }
        private static object DrawMemberInfo<T1, T2, T3>(Type ty, AttriData attr, object obj)
        {
            if (ty == typeof(bool))
            {
                return EditorGUILayout.Toggle(attr.GetGUIContent(), (bool)obj);
            }
            else if (ty == typeof(string))
            {
                return EditorGUILayout.TextField(attr.GetGUIContent(), (string)obj);
            }
            else if (ty.IsEnum)
            {
                return EditorGUILayout.EnumPopup(attr.GetGUIContent(), (System.Enum)obj, "DropDown");
            }
            else if (ty == typeof(float))
            {
                return EditorGUILayout.FloatField(attr.GetGUIContent(), (float)obj);
            }
            else if (ty == typeof(double) || ty == typeof(decimal))
            {
                return EditorGUILayout.DoubleField(attr.GetGUIContent(), (double)obj);
            }
            else if (ty == typeof(int))
            {
                return EditorGUILayout.IntField(attr.GetGUIContent(), (int)obj);
            }
            else if (ty == typeof(long) || ty == typeof(ulong))
            {
                return EditorGUILayout.LongField(attr.GetGUIContent(), (int)obj);
            }
            else if (ty == typeof(Vector2))
            {
                return EditorGUILayout.Vector2Field(attr.GetGUIContent(), (Vector2)obj);
            }
            else if (ty == typeof(Vector2Int))
            {
                return EditorGUILayout.Vector2IntField(attr.GetGUIContent(), (Vector2Int)obj);
            }
            else if (ty == typeof(Vector3))
            {
                return EditorGUILayout.Vector3Field(attr.GetGUIContent(), (Vector3)obj);
            }
            else if (ty == typeof(Vector3Int))
            {
                return EditorGUILayout.Vector3IntField(attr.GetGUIContent(), (Vector3Int)obj);
            }
            else if (ty == typeof(Vector4))
            {
                return EditorGUILayout.Vector4Field(attr.GetGUIContent(), (Vector4)obj);
            }
            else if (ty == typeof(Rect))
            {
                return EditorGUILayout.RectField(attr.GetGUIContent(), (Rect)obj);
            }
            else if (ty == typeof(RectInt))
            {
                return EditorGUILayout.RectIntField(attr.GetGUIContent(), (RectInt)obj);
            }
            else if (ty == typeof(Color))
            {
                return EditorGUILayout.ColorField(attr.GetGUIContent(), (Color)obj);
            }
            else if (ty == typeof(AnimationCurve))
            {
                return EditorGUILayout.CurveField(attr.GetGUIContent(), (AnimationCurve)obj);
            }
            else if (ty == typeof(Bounds))
            {
                return EditorGUILayout.BoundsField(attr.GetGUIContent(), (Bounds)obj);
            }
            else if (ty == typeof(BoundsInt))
            {
                return EditorGUILayout.BoundsIntField(attr.GetGUIContent(), (BoundsInt)obj);
            }
            else if (ty == typeof(Gradient))
            {
                return EditorGUILayout.GradientField(attr.GetGUIContent(), (Gradient)obj);
            }
            else if (ty == typeof(T1))
            {
                return ClassTypeCustom<T1>(attr, obj);
            }
            else if (ty == typeof(T2))
            {
                return ClassTypeCustom<T2>(attr, obj);
            }
            else if (ty == typeof(T3))
            {
                return ClassTypeCustom<T3>(attr, obj);
            }
            else if (ty.IsGenericType)
            {
                if (IsGenericTypeArgument(ty, attr, obj, out List<int> m_int))
                {
                    return m_int;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<long> m_long))
                {
                    return m_long;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<float> m_float))
                {
                    return m_float;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<double> m_double))
                {
                    return m_double;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<string> m_string))
                {
                    return m_string;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<bool> m_bool))
                {
                    return m_bool;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<System.Enum> m_Enum))
                {
                    return m_Enum;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<Color> m_Color))
                {
                    return m_Color;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<Vector2> m_Vector2))
                {
                    return m_Vector2;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<Vector3> m_Vector3))
                {
                    return m_Vector3;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<Vector4> m_Vector4))
                {
                    return m_Vector4;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<AnimationCurve> m_AnimationCurve))
                {
                    return m_AnimationCurve;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<Gradient> m_Gradient))
                {
                    return m_Gradient;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<Transform> m_Transform))
                {
                    return m_Transform;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<GameObject> m_GameObject))
                {
                    return m_GameObject;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<T1> m_T1))
                {
                    return m_T1;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<T2> m_T2))
                {
                    return m_T2;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<T3> m_T3))
                {
                    return m_T3;
                }

                return obj;
            }
            else if (ty.IsArray)
            {
                if (IsArrayTypeArgument(ty, attr, obj, out int[] m_int))
                {
                    return m_int;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out long[] m_long))
                {
                    return m_long;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out float[] m_float))
                {
                    return m_float;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out double[] m_double))
                {
                    return m_double;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out string[] m_string))
                {
                    return m_string;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out bool[] m_bool))
                {
                    return m_bool;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out System.Enum[] m_Enum))
                {
                    return m_Enum;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out Color[] m_Color))
                {
                    return m_Color;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out Vector2[] m_Vector2))
                {
                    return m_Vector2;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out Vector3[] m_Vector3))
                {
                    return m_Vector3;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out Vector4[] m_Vector4))
                {
                    return m_Vector4;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out AnimationCurve[] m_AnimationCurve))
                {
                    return m_AnimationCurve;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out Gradient[] m_Gradient))
                {
                    return m_Gradient;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out Transform[] m_Transform))
                {
                    return m_Transform;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out GameObject[] m_GameObject))
                {
                    return m_GameObject;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out T1[] m_T1))
                {
                    return m_T1;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out T2[] m_T2))
                {
                    return m_T2;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out T3[] m_T3))
                {
                    return m_T3;
                }

                return obj;
            }
            else if (ty.IsClass)
            {
                if (ty.BaseType == typeof(Component) ||
                     ty.BaseType == typeof(MonoBehaviour) ||
                     ty.BaseType == typeof(Object) ||
                     ty.BaseType?.BaseType == typeof(Object) ||
                     ty.BaseType?.BaseType == typeof(MonoBehaviour) ||
                     ty.BaseType?.BaseType == typeof(Component) ||
                     ty.BaseType?.BaseType?.BaseType == typeof(Object) ||
                     ty.BaseType?.BaseType?.BaseType == typeof(MonoBehaviour) ||
                     ty.BaseType?.BaseType?.BaseType == typeof(Component))
                {
                    return EditorGUILayout.ObjectField(attr.GetGUIContent(), (UnityEngine.Object)obj, ty, true);
                }
                else
                {
                    return ClassTypeCustom(ty, attr, obj);
                }
            }
            else
            {
                Debug.LogWarning("集合元素 无法显示" + ty.Name);
            }

            return obj;
        }
        private static object DrawMemberInfo<T1, T2, T3, T4>(Type ty, AttriData attr, object obj)
        {
            if (ty == typeof(bool))
            {
                return EditorGUILayout.Toggle(attr.GetGUIContent(), (bool)obj);
            }
            else if (ty == typeof(string))
            {
                return EditorGUILayout.TextField(attr.GetGUIContent(), (string)obj);
            }
            else if (ty.IsEnum)
            {
                return EditorGUILayout.EnumPopup(attr.GetGUIContent(), (System.Enum)obj, "DropDown");
            }
            else if (ty == typeof(float))
            {
                return EditorGUILayout.FloatField(attr.GetGUIContent(), (float)obj);
            }
            else if (ty == typeof(double) || ty == typeof(decimal))
            {
                return EditorGUILayout.DoubleField(attr.GetGUIContent(), (double)obj);
            }
            else if (ty == typeof(int))
            {
                return EditorGUILayout.IntField(attr.GetGUIContent(), (int)obj);
            }
            else if (ty == typeof(long) || ty == typeof(ulong))
            {
                return EditorGUILayout.LongField(attr.GetGUIContent(), (int)obj);
            }
            else if (ty == typeof(Vector2))
            {
                return EditorGUILayout.Vector2Field(attr.GetGUIContent(), (Vector2)obj);
            }
            else if (ty == typeof(Vector2Int))
            {
                return EditorGUILayout.Vector2IntField(attr.GetGUIContent(), (Vector2Int)obj);
            }
            else if (ty == typeof(Vector3))
            {
                return EditorGUILayout.Vector3Field(attr.GetGUIContent(), (Vector3)obj);
            }
            else if (ty == typeof(Vector3Int))
            {
                return EditorGUILayout.Vector3IntField(attr.GetGUIContent(), (Vector3Int)obj);
            }
            else if (ty == typeof(Vector4))
            {
                return EditorGUILayout.Vector4Field(attr.GetGUIContent(), (Vector4)obj);
            }
            else if (ty == typeof(Rect))
            {
                return EditorGUILayout.RectField(attr.GetGUIContent(), (Rect)obj);
            }
            else if (ty == typeof(RectInt))
            {
                return EditorGUILayout.RectIntField(attr.GetGUIContent(), (RectInt)obj);
            }
            else if (ty == typeof(Color))
            {
                return EditorGUILayout.ColorField(attr.GetGUIContent(), (Color)obj);
            }
            else if (ty == typeof(AnimationCurve))
            {
                return EditorGUILayout.CurveField(attr.GetGUIContent(), (AnimationCurve)obj);
            }
            else if (ty == typeof(Bounds))
            {
                return EditorGUILayout.BoundsField(attr.GetGUIContent(), (Bounds)obj);
            }
            else if (ty == typeof(BoundsInt))
            {
                return EditorGUILayout.BoundsIntField(attr.GetGUIContent(), (BoundsInt)obj);
            }
            else if (ty == typeof(Gradient))
            {
                return EditorGUILayout.GradientField(attr.GetGUIContent(), (Gradient)obj);
            }
            else if (ty == typeof(T1))
            {
                return ClassTypeCustom<T1>(attr, obj);
            }
            else if (ty == typeof(T2))
            {
                return ClassTypeCustom<T2>(attr, obj);
            }
            else if (ty == typeof(T3))
            {
                return ClassTypeCustom<T3>(attr, obj);
            }
            else if (ty == typeof(T4))
            {
                return ClassTypeCustom<T4>(attr, obj);
            }
            else if (ty.IsGenericType)
            {
                if (IsGenericTypeArgument(ty, attr, obj, out List<int> m_int))
                {
                    return m_int;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<long> m_long))
                {
                    return m_long;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<float> m_float))
                {
                    return m_float;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<double> m_double))
                {
                    return m_double;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<string> m_string))
                {
                    return m_string;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<bool> m_bool))
                {
                    return m_bool;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<System.Enum> m_Enum))
                {
                    return m_Enum;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<Color> m_Color))
                {
                    return m_Color;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<Vector2> m_Vector2))
                {
                    return m_Vector2;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<Vector3> m_Vector3))
                {
                    return m_Vector3;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<Vector4> m_Vector4))
                {
                    return m_Vector4;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<AnimationCurve> m_AnimationCurve))
                {
                    return m_AnimationCurve;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<Gradient> m_Gradient))
                {
                    return m_Gradient;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<Transform> m_Transform))
                {
                    return m_Transform;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<GameObject> m_GameObject))
                {
                    return m_GameObject;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<T1> m_T1))
                {
                    return m_T1;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<T2> m_T2))
                {
                    return m_T2;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<T3> m_T3))
                {
                    return m_T3;
                }
                else if (IsGenericTypeArgument(ty, attr, obj, out List<T4> m_T4))
                {
                    return m_T4;
                }
                return obj;
            }
            else if (ty.IsArray)
            {
                if (IsArrayTypeArgument(ty, attr, obj, out int[] m_int))
                {
                    return m_int;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out long[] m_long))
                {
                    return m_long;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out float[] m_float))
                {
                    return m_float;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out double[] m_double))
                {
                    return m_double;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out string[] m_string))
                {
                    return m_string;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out bool[] m_bool))
                {
                    return m_bool;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out System.Enum[] m_Enum))
                {
                    return m_Enum;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out Color[] m_Color))
                {
                    return m_Color;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out Vector2[] m_Vector2))
                {
                    return m_Vector2;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out Vector3[] m_Vector3))
                {
                    return m_Vector3;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out Vector4[] m_Vector4))
                {
                    return m_Vector4;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out AnimationCurve[] m_AnimationCurve))
                {
                    return m_AnimationCurve;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out Gradient[] m_Gradient))
                {
                    return m_Gradient;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out Transform[] m_Transform))
                {
                    return m_Transform;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out GameObject[] m_GameObject))
                {
                    return m_GameObject;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out T1[] m_T1))
                {
                    return m_T1;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out T2[] m_T2))
                {
                    return m_T2;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out T3[] m_T3))
                {
                    return m_T3;
                }
                else if (IsArrayTypeArgument(ty, attr, obj, out T4[] m_T4))
                {
                    return m_T4;
                }
                return obj;
            }
            else if (ty.IsClass)
            {
                if (ty.BaseType == typeof(Component) ||
                    ty.BaseType == typeof(MonoBehaviour) ||
                    ty.BaseType == typeof(Object) ||
                    ty.BaseType?.BaseType == typeof(Object) ||
                    ty.BaseType?.BaseType == typeof(MonoBehaviour) ||
                    ty.BaseType?.BaseType == typeof(Component) ||
                    ty.BaseType?.BaseType?.BaseType == typeof(Object) ||
                    ty.BaseType?.BaseType?.BaseType == typeof(MonoBehaviour) ||
                    ty.BaseType?.BaseType?.BaseType == typeof(Component))
                {
                    return EditorGUILayout.ObjectField(attr.GetGUIContent(), (UnityEngine.Object)obj, ty, true);
                }
                else
                {
                    return ClassTypeCustom(ty, attr, obj);
                }
            }
            else
            {
                Debug.LogWarning("集合元素 无法显示" + ty.Name);
            }

            return obj;
        }


        #region 数组和集合,类
        private static object ClassTypeCustom(Type ty, AttriData attr, object obj)
        {
            if (obj == null)
            {
                obj = System.Activator.CreateInstance(ty);
            }

            if (!FoldoutDic.ContainsKey(attr.memberName))
            {
                FoldoutDic[attr.memberName] = new FoldOut();
            }
            FoldoutDic[attr.memberName].IsOpen = EditorGUILayout.Foldout(FoldoutDic[attr.memberName].IsOpen, attr.GetClassContent( CLASS_TAG));
            if (FoldoutDic[attr.memberName].IsOpen)
            {
                return DrawType(obj, true);
            }
            return obj;
        }

        private static object ClassTypeCustom<T>(AttriData attr, object obj)
        {
            if (obj == null)
            {
                obj = System.Activator.CreateInstance<T>();
            }

            if (!FoldoutDic.ContainsKey(attr.memberName))
            {
                FoldoutDic[attr.memberName] = new FoldOut();
            }
            FoldoutDic[attr.memberName].IsOpen = EditorGUILayout.Foldout(FoldoutDic[attr.memberName].IsOpen, attr.GetClassContent(CLASS_TAG));
            if (FoldoutDic[attr.memberName].IsOpen)
            {
                return DrawType(obj, true);
            }
            return obj;
        }
        /// <summary>
        /// List泛型集合
        /// </summary>
        /// <typeparam name="T">所有字段类型</typeparam>
        /// <param name="ty">字段类型</param>
        /// <param name="memberName">成员名称</param>
        /// <param name="obj">字段值</param>
        /// <param name="Count">输出集合长度</param>
        /// <param name="outList">输出集合</param>
        /// <returns></returns>
        private static bool IsGenericTypeArgument<T>(Type ty, AttriData memberName, object obj, out List<T> outList)
        {
            int Count = 0;
            bool isOneArgument = ty.GenericTypeArguments.Length == 1 && ty.GenericTypeArguments[0] == typeof(T);
            if (isOneArgument)
            {
                outList = GetArrayAndListGenericType<T>("Count", ty, memberName, obj, ref Count);
                if (outList != null && outList.Count > 0)
                {
                    for (int i = 0; i < Count; i++)
                    {
                        outList[i] = (T)DrawArrayAndList(outList[i], i);
                    }

                }
                return outList != null;
            }
            else
            {
                outList = null;
            }

            return isOneArgument;
        }
        /// <summary>
        /// 数组泛型集合
        /// </summary>
        /// <typeparam name="T">所有字段类型</typeparam>
        /// <param name="ty">字段类型</param>
        /// <param name="memberName">成员名称</param>
        /// <param name="obj">字段值</param>
        /// <param name="Count">输出集合长度</param>
        /// <param name="outArray">输出集合</param>
        /// <returns></returns>
        private static bool IsArrayTypeArgument<T>(Type ty, AttriData memberName, object obj, out T[] outArray)
        {
            int Count = 0;
            bool isOneArgument = ty.GetArrayRank() == 1 && ty == typeof(T[]);
            if (isOneArgument)
            {
                var outList = GetArrayAndListGenericType<T>("Length", ty, memberName, obj, ref Count, false);
                outArray = outList?.ToArray();
                if (outArray != null && outArray.Length > 0)
                {
                    for (int i = 0; i < Count; i++)
                    {
                        outArray[i] = (T)DrawArrayAndList(outArray[i], i);
                    }
                }
                return outArray != null;
            }
            else
            {
                outArray = null;
            }

            return isOneArgument;
        }

        private static object DrawArrayAndList<T>(T t, int i)
        {
            Type fieldType = typeof(T);
            string arrayIndex = "     Element_" + i;
            object obj = t;
            if (fieldType == typeof(bool))
            {
                return EditorGUILayout.Toggle(arrayIndex, (bool)obj);
            }
            else if (fieldType == typeof(string))
            {
                return EditorGUILayout.TextField(arrayIndex, (string)obj);
            }
            else if (fieldType.IsEnum)
            {
                return EditorGUILayout.EnumPopup(arrayIndex, (System.Enum)obj, "DropDown");
            }
            else if (fieldType == typeof(float))
            {
                return EditorGUILayout.FloatField(arrayIndex, (float)obj);
            }
            else if (fieldType == typeof(double) || fieldType == typeof(decimal))
            {
                return EditorGUILayout.DoubleField(arrayIndex, (double)obj);
            }
            else if (fieldType == typeof(int))
            {
                return EditorGUILayout.IntField(arrayIndex, (int)obj);
            }
            else if (fieldType == typeof(long) || fieldType == typeof(ulong))
            {
                return EditorGUILayout.LongField(arrayIndex, (int)obj);
            }
            else if (fieldType == typeof(Vector2))
            {
                return EditorGUILayout.Vector2Field(arrayIndex, (Vector2)obj);
            }
            else if (fieldType == typeof(Vector2Int))
            {
                return EditorGUILayout.Vector2IntField(arrayIndex, (Vector2Int)obj);
            }
            else if (fieldType == typeof(Vector3))
            {
                return EditorGUILayout.Vector3Field(arrayIndex, (Vector3)obj);
            }
            else if (fieldType == typeof(Vector3Int))
            {
                return EditorGUILayout.Vector3IntField(arrayIndex, (Vector3Int)obj);
            }
            else if (fieldType == typeof(Vector4))
            {
                return EditorGUILayout.Vector4Field(arrayIndex, (Vector4)obj);
            }
            else if (fieldType == typeof(Rect))
            {
                return EditorGUILayout.RectField(arrayIndex, (Rect)obj);
            }
            else if (fieldType == typeof(RectInt))
            {
                return EditorGUILayout.RectIntField(arrayIndex, (RectInt)obj);
            }
            else if (fieldType == typeof(Color))
            {
                return EditorGUILayout.ColorField(arrayIndex, (Color)obj);
            }
            else if (fieldType == typeof(AnimationCurve))
            {
                return EditorGUILayout.CurveField(arrayIndex, (AnimationCurve)obj);
            }
            else if (fieldType == typeof(Bounds))
            {
                return EditorGUILayout.BoundsField(arrayIndex, (Bounds)obj);
            }
            else if (fieldType == typeof(BoundsInt))
            {
                return EditorGUILayout.BoundsIntField(arrayIndex, (BoundsInt)obj);
            }
            else if (fieldType == typeof(Gradient))
            {
                return EditorGUILayout.GradientField(arrayIndex, (Gradient)obj);
            }
            else if (fieldType.IsClass)
            {
                if (fieldType.BaseType == typeof(Component) ||
               fieldType.BaseType == typeof(MonoBehaviour) ||
               fieldType.BaseType?.BaseType == typeof(Object) ||
               fieldType.BaseType?.BaseType == typeof(Behaviour) ||
               fieldType.BaseType?.BaseType == typeof(Component))
                {
                    return EditorGUILayout.ObjectField(arrayIndex, (UnityEngine.Object)obj, fieldType, true);
                }
                else
                {
                    return ClassTypeCustom(fieldType, new AttriData(null, arrayIndex), obj);
                }
            }
            return obj;
        }

        /// <summary>
        /// 应用类型 开关页签展开
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="ty"></param>
        /// <param name="attr"></param>
        /// <param name="obj"></param>
        /// <param name="Count"></param>
        /// <param name="isList">区分数组和List集合</param>
        /// <returns></returns>
        private static List<T> GetArrayAndListGenericType<T>(string propertyName, Type ty, AttriData attr, object obj, ref int Count, bool isList = true)
        {
            List<T> typelist = new List<T>();
            if (obj == null)
            {
                if (isList)
                    obj = new List<T>();
                else
                    obj = new T[1];
            }
            else
            {
                if (isList)
                {
                    typelist = (List<T>)obj;
                }
                else
                {
                    typelist.AddRange((T[])obj);
                }
            }

            Count = (int)obj.GetType().GetProperty(propertyName).GetValue(obj);
            if (!FoldoutDic.ContainsKey(attr.memberName))
            {
                FoldoutDic[attr.memberName] = new FoldOut();
                FoldoutDic[attr.memberName].ArrayLength = Count;
            }
            FoldoutDic[attr.memberName].IsOpen = EditorGUILayout.Foldout(FoldoutDic[attr.memberName].IsOpen, attr.GetGUIContent());
            if (FoldoutDic[attr.memberName].IsOpen)
            {
                string text = EditorGUILayout.TextField("  Size", FoldoutDic[attr.memberName].ArrayLength >= 0 ? FoldoutDic[attr.memberName].ArrayLength.ToString() : "");
                FoldoutDic[attr.memberName].ArrayLength = string.IsNullOrEmpty(text) ? -1 : int.TryParse(text, out int result) ? result : -1;
                if (FoldoutDic[attr.memberName].ArrayLength != Count && FoldoutDic[attr.memberName].ArrayLength >= 0)
                {
                    List<T> temp = new List<T>();
                    for (int i = 0; i < FoldoutDic[attr.memberName].ArrayLength; i++)
                    {
                        if (i < Count)
                        {
                            temp.Add(typelist[i]);
                        }
                        else
                        {
                            temp.Add(default);
                        }
                    }
                    typelist = temp;
                    Count = FoldoutDic[attr.memberName].ArrayLength;
                }
                return typelist;
            }
            return null;
        }
        protected class FoldOut
        {
            public bool IsOpen;
            public int ArrayLength = 0;
        }

        protected struct AttriData
        {
            private FieldNameAttribute attrField;
            public string memberName;

            public AttriData(FieldNameAttribute attrField, string memberName)
            {
                this.attrField = attrField;
                this.memberName = memberName;
            }

            public GUIContent GetGUIContent()
            {
                if (attrField == null) return new GUIContent(memberName);
                if (string.IsNullOrEmpty(attrField.icon)) return new GUIContent(memberName, attrField.tipText);
                var tex = HukiryUtilEditor.GetTexture2D(attrField.icon);
                if (tex) 
                    return new GUIContent(memberName, tex, attrField.tipText);
                return new GUIContent(memberName, attrField.tipText);
            }

            public GUIContent GetClassContent(string classTag)
            {
                memberName = memberName + classTag;
                return GetGUIContent();
            }

        }
        #endregion
        #endregion

        #region 文件名导出
        /// <summary>
        /// 返回目录名。文件名集合
        /// </summary>
        /// <param name="callFunc"><see cref="Action"/></param>
        /// <returns></returns>
        static Dictionary<string, List<string>> GetFilePath(Action<string, float> callFunc = null)
        {
            Dictionary<string, List<string>> temp = new Dictionary<string, List<string>>();
            string[] ids = Selection.assetGUIDs;
            foreach (string id in ids)
            {
                string dir = AssetDatabase.GUIDToAssetPath(id);
                if (dir.IndexOf(".") < 0)
                {
                    string root = Application.dataPath + dir.Substring(dir.IndexOf("/"));
                    string[] files = Directory.GetFiles(root, "*.*", SearchOption.AllDirectories);
                    if (files.Length > 0)
                    {
                        int index = 0;
                        int len = files.Length;
                        foreach (string file in files)
                        {
                            FileInfo fi = new FileInfo(file);
                            string dirName = fi.Directory.Name;
                            string ext = Path.GetExtension(file);
                            string fileName = Path.GetFileName(file);

                            if (ext == ".meta") continue;

                            if (!temp.ContainsKey(dirName))
                            {
                                temp.Add(dirName, new List<string>());
                            }
                            temp[dirName].Add(fileName);
                            index++;
                            callFunc?.Invoke(dirName + "/" + fileName, (float)index / (float)len);
                        }
                    }
                }

            }
            if (callFunc != null)
            {
                EditorUtility.ClearProgressBar();
            }
            return temp;
        }

        static void ExistsIfDirectory(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        public static void ExportFileName(bool isHasExt=false)
        {
            string fileDir = "ExportListName";
            var ids = Selection.instanceIDs;
            Dictionary<string, List<string>> dic = GetFilePath((info, progressvalue) =>
            {
                EditorUtility.DisplayProgressBar("文件名导出", info, progressvalue);
            });

            ExistsIfDirectory(fileDir);

            foreach (var item in dic)
            {
                if (item.Value.Count > 0)
                {
                    string[] result = item.Value.ToArray();
                    if (isHasExt)
                    {
                        result = item.Value.Select(p => p.Split('.')[0]).ToArray();
                    }
                    File.WriteAllLines(fileDir + "/" + item.Key + ".txt", result);
                }
            }

            Application.OpenURL(Path.GetFullPath(fileDir));
        }

        public static void ExportSpriteAtlasFile()
        {
            var ids = Selection.instanceIDs;
            Dictionary<string, List<string>> dic = GetFilePath((info, progressvalue) =>
            {
                EditorUtility.DisplayProgressBar("文件名导出", info, progressvalue);
            });

            //ExistsIfDirectory(fileDir);

            List<string> lines = new List<string>();
            lines.Add("local _res = EAtlasResPath;");
            lines.Add("---@class ESpriteAtlasResource");
            lines.Add("ESpriteAtlasResource = {");
            (int index, string temp) = (0,string.Empty);
            foreach (var item in dic)
            {
                int length = item.Value.Count;
                 
                for (int i = 0; i < length; i++)
                {
                    index++;
                    temp += $"['{Path.GetFileNameWithoutExtension(item.Value[i])}'] = _res.{item.Key},";
                    if (index % 3 == 0)
                    {
                        lines.Add($"    {temp}");
                        temp = string.Empty;
                    }
                }
            }

            if (index % 3 > 0)
            {
                lines.Add($"    {temp}");
            }
            lines.Add("}");

            File.WriteAllLines(UIPathConfig.ESpriteAtlasResourcePath, lines.ToArray());
        }
        #endregion


        private void OnDrawGizmos()
        {
            Color color = new Color(255, 0.5F, 0.5F, 1);
            GUI.contentColor = color;
            Gizmos.color = new Color(255, 0, 0, 1);
            //图片资源必须在Gizmos文件夹下；Gizmos文件夹必须在Assets文件夹下
            //Gizmos.DrawIcon(pos, "Image/sprite.tga", true);图片绘制
            //Gizmos.DrawLine(child.position, child2.position);直线绘制

            //场景UI布局对象
            //Handles.BeginGUI();
            //EditorGUI.DrawRect();
            //Handles.EndGUI();
        }

        /// <summary>
        /// 获取枚举特性上的值
        /// </summary>
        /// <typeparam name="T">需要继承Attribute的类</typeparam>
        /// <typeparam name="K">返回继承Attribute的类的指定类型<see cref="K"/></typeparam>
        /// <param name="_enum">枚举值</param>
        /// <returns>返回指定<see cref="K"/>类型值</returns>
        public K ReflectEnum<T, K>(Enum _enum) where T : Attribute where K : IComparable
        {
            Enum enumValue = _enum;
            var objs = enumValue.GetType().GetField(enumValue.ToString()).GetCustomAttributes(typeof(T), false);
            if (objs != null && objs.Length > 0)
            {
                T fn = objs[0] as T;
                FieldInfo[] fieldInfos = fn.GetType().GetFields();
                //特性反射值
                Dictionary<string, object> dicAttribute = new Dictionary<string, object>();
                foreach (var item in fieldInfos)
                {
                    var val = item.GetValue(fn);
                    dicAttribute[item.Name] = val != null ? val : null;
                }


                foreach (var item in dicAttribute)
                {
                    if (item.Value is K)
                    {
                        return (K)item.Value;
                    }
                }

            }
            return default(K);
        }



        public static UnityEngine.Object GetObject(string fileName)
        {
            return EditorGUIUtility.Load(fileName);
        }
    }
}