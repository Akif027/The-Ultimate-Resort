using System;
using UnityEngine;

[System.Serializable]
public struct SecuredDouble
{
    // Private fields for the secure value and the encryption key
    [SerializeField] private long encryptedValue;
    [SerializeField] private long key;

    // Constructor to initialize the SecuredDouble with a value
    public SecuredDouble(double value)
    {
        key = BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0);
        encryptedValue = Encrypt(value, key);
    }

    // Property to get and set the secured value
    public double Value
    {
        get => Decrypt(encryptedValue, key);
        set => encryptedValue = Encrypt(value, key);
    }

    // Encrypt the value using XOR operation with the key
    private static long Encrypt(double value, long key)
    {
        long rawValue = BitConverter.DoubleToInt64Bits(value);
        return rawValue ^ key;
    }

    // Decrypt the value using XOR operation with the key
    private static double Decrypt(long encrypted, long key)
    {
        long rawValue = encrypted ^ key;
        return BitConverter.Int64BitsToDouble(rawValue);
    }

    // Override ToString() to display the secured value
    public override string ToString()
    {
        return Value.ToString();
    }

    // Override operators for ease of use
    public static implicit operator double(SecuredDouble s) => s.Value;
    public static implicit operator SecuredDouble(double d) => new SecuredDouble(d);
    public static SecuredDouble operator +(SecuredDouble a, SecuredDouble b) => new SecuredDouble(a.Value + b.Value);
    public static SecuredDouble operator -(SecuredDouble a, SecuredDouble b) => new SecuredDouble(a.Value - b.Value);
    public static SecuredDouble operator *(SecuredDouble a, SecuredDouble b) => new SecuredDouble(a.Value * b.Value);
    public static SecuredDouble operator /(SecuredDouble a, SecuredDouble b) => new SecuredDouble(a.Value / b.Value);
}
