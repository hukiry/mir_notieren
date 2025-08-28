using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Enum = System.Enum;

/* DescriptionAttribute 必须继承 UnityEngine.PropertyAttribute
 * CustomPropertyDrawer 属性自定义编辑
 */
[CanEditMultipleObjects]
[CustomPropertyDrawer(typeof(DescriptionAttribute))]
public class DescriptionDrawer : PropertyDrawer
{
	private Dictionary<System.Type, string[]> enumDic = new Dictionary<System.Type, string[]>();  
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		Rect objectRect = new Rect(position.x, position.y, position.width/*-50*/, position.height);
		
		//Rect nameRect = new Rect(position.x+ objectRect.width, position.y, position.width - objectRect.width, position.height);
		DescriptionAttribute desc = attribute as DescriptionAttribute;
		GUIContent descText = string.IsNullOrEmpty(desc.name) ? label : new GUIContent(desc.name);
		if (fieldInfo.FieldType.BaseType == typeof(Object) || fieldInfo.FieldType.BaseType?.BaseType == typeof(Object) || fieldInfo.FieldType.BaseType?.BaseType?.BaseType == typeof(Object))
		{
			property.objectReferenceValue = EditorGUI.ObjectField(objectRect, descText, property.objectReferenceValue, fieldInfo.FieldType, true);
		}
		else if (fieldInfo.FieldType == typeof(int))
		{
			property.intValue = EditorGUI.IntField(objectRect, descText, property.intValue);
		}
		else if (fieldInfo.FieldType == typeof(float))
		{
			property.floatValue = EditorGUI.FloatField(objectRect, descText, property.floatValue);
		}
		else if (fieldInfo.FieldType == typeof(string))
		{
			property.stringValue = EditorGUI.TextField(objectRect, descText, property.stringValue);
		}
		//else if (fieldInfo.FieldType == typeof(BoolObject))
		//{
		//	BoolObject obj = (BoolObject)fieldInfo.GetValue(property.serializedObject.targetObject);
		//	obj.value = EditorGUI.Toggle(objectRect, descText, obj.value);
		//	fieldInfo.SetValue(property.serializedObject.targetObject, obj);
		//}
		else if (fieldInfo.FieldType == typeof(bool))
		{
			property.boolValue = EditorGUI.Toggle(objectRect, descText, property.boolValue);
		}
		else if (fieldInfo.FieldType == desc.enumType && desc.enumType != null)//枚举
		{
			string[] displayedOptions = Enum.GetNames(desc.enumType);
			if (enumDic.ContainsKey(desc.enumType))
			{
				displayedOptions = enumDic[desc.enumType];
			}
			else
			{
				displayedOptions = Hukiry.HukiryUtil.ToEnumStringArray(desc.enumType);
				enumDic[desc.enumType] = displayedOptions;
			}
			property.enumValueIndex = EditorGUI.Popup(objectRect, desc.name, property.enumValueIndex, displayedOptions);
		}
		else if (fieldInfo.FieldType == typeof(Vector2))
		{
			property.vector2Value = EditorGUI.Vector2Field(objectRect, desc.name, property.vector2Value);
		}
		else if (fieldInfo.FieldType == typeof(Vector3))
		{
			property.vector3Value = EditorGUI.Vector3Field(objectRect, desc.name, property.vector3Value);
		}
		else if (fieldInfo.FieldType == typeof(Vector4))
		{
			property.vector4Value = EditorGUI.Vector4Field(objectRect, desc.name, property.vector4Value);
		}
		else if (fieldInfo.FieldType == typeof(Vector2Int))
		{
			property.vector2IntValue = EditorGUI.Vector2IntField(objectRect, desc.name, property.vector2IntValue);
		}
		else if (fieldInfo.FieldType == typeof(Vector3Int))
		{
			property.vector3IntValue = EditorGUI.Vector3IntField(objectRect, desc.name, property.vector3IntValue);
		}
		else if (fieldInfo.FieldType == typeof(Rect))
		{
			property.rectValue = EditorGUI.RectField(objectRect, desc.name, property.rectValue);
		}
		else if (fieldInfo.FieldType == typeof(RectInt))
		{
			property.rectIntValue = EditorGUI.RectIntField(objectRect, desc.name, property.rectIntValue);
		}

	}
}
