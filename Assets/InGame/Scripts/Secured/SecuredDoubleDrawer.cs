using UnityEditor;
using UnityEngine;
using System;
[CustomPropertyDrawer(typeof(SecuredDouble))]
public class SecuredDoubleDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Find the encryptedValue and key fields
        SerializedProperty encryptedValueProp = property.FindPropertyRelative("encryptedValue");
        SerializedProperty keyProp = property.FindPropertyRelative("key");

        // Decrypt the current value to display
        long encryptedValue = encryptedValueProp.longValue;
        long key = keyProp.longValue;
        double currentValue = SecuredDoubleDecrypt(encryptedValue, key);

        // Draw the double field
        double newValue = EditorGUI.DoubleField(position, label, currentValue);

        // If the value changed, re-encrypt it
        if (newValue != currentValue)
        {
            encryptedValueProp.longValue = SecuredDoubleEncrypt(newValue, key);
        }

        EditorGUI.EndProperty();
    }

    private static long SecuredDoubleEncrypt(double value, long key)
    {
        long rawValue = BitConverter.DoubleToInt64Bits(value);
        return rawValue ^ key;
    }

    private static double SecuredDoubleDecrypt(long encrypted, long key)
    {
        long rawValue = encrypted ^ key;
        return BitConverter.Int64BitsToDouble(rawValue);
    }
}
