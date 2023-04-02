using System;
using System.Reflection;
using System.Text;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
	public static SaveData DATA;

	public static int key = 414;

	public static void Save()
	{
		string value = SaveDataToString(DATA);
		PlayerPrefs.SetString("DATA", value);
		PlayerPrefs.Save();
	}

	public static void Load()
	{
		string @string = PlayerPrefs.GetString("DATA", "");
		if (@string != "")
		{
			DATA = LoadDataFromString(EncryptDecrypt(@string));
		}
		else
		{
			DATA = new SaveData();
		}
		Save();
	}

	public static string SaveDataToString(SaveData DATA)
	{
		string text = "";
		FieldInfo[] fields = typeof(SaveData).GetFields();
		for (int i = 0; i < fields.Length; i++)
		{
			object value = fields[i].GetValue(DATA);
			if (value != null)
			{
				text += value.ToString();
			}
			if (i != fields.Length - 1)
			{
				text += "|";
			}
		}
		return EncryptDecrypt(text);
	}

	public static SaveData LoadDataFromString(string DATA)
	{
		string[] array = DATA.ToString().Split('|');
		SaveData saveData = new SaveData();
		FieldInfo[] fields = typeof(SaveData).GetFields();
		for (int i = 0; i < fields.Length && i < array.Length; i++)
		{
			FieldInfo fieldInfo = fields[i];
			object prop = getProp(array[i], fieldInfo.FieldType);
			fieldInfo.SetValue(saveData, prop);
		}
		return saveData;
	}

	public static object getProp(string input, Type startType)
	{
		if (startType.FullName == typeof(string).FullName)
		{
			return input.TrimEnd(default(char));
		}
		return Convert.ChangeType(input, startType);
	}

	public static string EncryptDecrypt(string textToEncrypt)
	{
		StringBuilder stringBuilder = new StringBuilder(textToEncrypt);
		StringBuilder stringBuilder2 = new StringBuilder(textToEncrypt.Length);
		for (int i = 0; i < textToEncrypt.Length; i++)
		{
			char c = stringBuilder[i];
			c = (char)(c ^ key);
			stringBuilder2.Append(c);
		}
		return stringBuilder2.ToString();
	}
}
