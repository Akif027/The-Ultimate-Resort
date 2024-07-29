using System;
using UnityEngine;

[System.Serializable]
public struct SecuredDouble
{
    // Private fields for the secure value and the encryption key
    [SerializeField] private long encryptedValue;
    [SerializeField] private long key;
    static System.Func<double, Unit> unitFinder;

    static SecuredDouble()
    {
        unitFinder = Unit0.Find;
    }

    public static void SetUnit(int u)
    {
        if (u == 0)
        {
            unitFinder = Unit0.Find;
        }
        else
        {
            unitFinder = Unit1.Find;
        }
    }
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
    public SecuredDouble IncreaseChargesByLevel(Level level)
    {
        int levelValue = (int)level; // Cast the enum to int to get its numeric value

        // Check if the level is greater than 1 before applying the increase
        if (levelValue > 1)
        {
            double increaseFactor = 1 + ((levelValue - 1) * 0.05); // Calculate the total increase factor, adjusted for level > 1
            double increasedValue = Value * increaseFactor; // Apply the increase to the current value
            return new SecuredDouble(increasedValue); // Return a new SecuredDouble with the increased value
        }
        else
        {
            // If level is 1, return the current value without any increase
            return this;
        }
    }
    public SecuredDouble Round()
    {
        return new SecuredDouble(System.Math.Round(Value));
    }

    public string FormattedString
    {
        get
        {
            return ToString(FindUnit());
        }
    }

    public Unit FindUnit()
    {
        return unitFinder.Invoke(Value);
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
    public string ToString(Unit unit, string format = "0.##")
    {
        if (double.IsInfinity(Value) || double.IsNaN(Value))
        {
            return "Infinity or NaN";
        }

        return (Value / System.Math.Pow(10, unit.exponent)).ToString(format) + unit.name;
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


    public class Unit0
    {
        static Unit[] units;
        static Unit Infinity;
        static Unit Zero;

        static Unit0()
        {
            Infinity.exponent = 0;
            Infinity.name = "(VeryBIG)";
            Zero.exponent = 0;
            Zero.name = "";

            units = new Unit[120];
            int i = 0;

            units[i++].name = "";
            units[i - 1].exponent = (i - 1) * 3;

            units[i++].name = "K";
            units[i - 1].exponent = (i - 1) * 3;

            units[i++].name = "M";
            units[i - 1].exponent = (i - 1) * 3;

            units[i++].name = "B";
            units[i - 1].exponent = (i - 1) * 3;

            units[i++].name = "T";
            units[i - 1].exponent = (i - 1) * 3;

            for (char c0 = 'a'; c0 <= 'z'; c0++)
            {
                for (char c1 = c0; c1 <= 'z'; c1++)
                {
                    if (i >= units.Length)
                    {
                        break;
                    }

                    units[i++].name = c0.ToString() + c1.ToString();
                    units[i - 1].exponent = (i - 1) * 3;
                }
            }
        }
        public static Unit Find(double value)
        {
            //extract
            long exponent;

            double e = System.Math.Log10(System.Math.Abs(value));
            double fe = System.Math.Floor(e);

            long remainder;
            exponent = System.Math.DivRem((long)fe, 3, out remainder) * 3;

            //find
            if (exponent < 0)
                return Zero;
            return exponent / 3 < units.Length ? units[exponent / 3] : Infinity;
        }
    }

    public class Unit1
    {
        static Unit[] units;
        static Unit Infinity;
        static Unit Zero;

        static Unit1()
        {
            Infinity.exponent = 0;
            Infinity.name = "(VeryBIG)";
            Zero.exponent = 0;
            Zero.name = "";

            units = new Unit[304];
            int i = 0;

            units[i++].name = "";
            units[i - 1].exponent = (i - 1) * 3;

            units[i++].name = "K";
            units[i - 1].exponent = (i - 1) * 3;

            units[i++].name = "M";
            units[i - 1].exponent = (i - 1) * 3;

            units[i++].name = "B";
            units[i - 1].exponent = (i - 1) * 3;

            units[i++].name = "T";
            units[i - 1].exponent = (i - 1) * 3;

            int exp = 14;
            for (i = i; i < units.Length; i++)
            {
                units[i].name = "e" + (++exp);
                units[i].exponent = exp;
            }
        }

        public static Unit Find(double value)
        {
            //extract
            long exponent;

            double e = System.Math.Log10(System.Math.Abs(value));
            double fe = System.Math.Floor(e);

            long remainder = 0;
            if (fe < 15)
            {
                exponent = System.Math.DivRem((long)fe, 3, out remainder) * 3;
            }
            else
                exponent = (long)fe;

            //find
            if (exponent < 0)
                return Zero;
            if (exponent < 15)
                return units[exponent / 3];
            else
                return exponent < units.Length + 5 ? units[15 / 3 + exponent - 15] : Infinity;
        }
    }

    public struct Unit
    {
        public int exponent;
        public string name;
    }


}
