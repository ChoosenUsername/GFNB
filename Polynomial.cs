using System;
using System.Linq;

public class Polynomial
{
    public ulong[] array = new ulong[0];
    Operation operation = new Operation();
    public ulong[] one = new ulong[] {1};

    public int ArrayLength(int m)
    {
        int q = m / 64;
        return m % 64 != 0 ? q + 1 : q;
    }


    public Polynomial(int m, int deg2, int deg3, int deg4, int deg5)
    {
        if (deg2 == 0) { deg2 = m;}
        if (deg3 == 0) { deg3 = m; }
        if (deg4 == 0) { deg4 = m; }
        if (deg5 == 0) { deg5 = m; }
        ulong[] poly1 = operation.ShiftBitsToHigh(one, m - 1);
        ulong[] poly2 = operation.ShiftBitsToHigh(one, deg2 - 1);
        ulong[] poly3 = operation.ShiftBitsToHigh(one, deg3 - 1);
        ulong[] poly4 = operation.ShiftBitsToHigh(one, deg4 - 1);
        ulong[] poly5 = operation.ShiftBitsToHigh(one, deg5 - 1);

        array = Package(poly1, poly2, poly3, poly4, poly5);
    }


    public Polynomial(int m)
    {
        array = new ulong[ArrayLength(m)];
    }


    public Polynomial(string a)
    {
        if (string.IsNullOrWhiteSpace(a))
        {
            throw new ArgumentException();
        }

        string x;
        if (a != "0")
        {
            x = a.TrimStart('0');
        }
        else
        {
            x = a;
        }
        while (x.Length % 16 != 0)
        {
            x = "0" + x;
        }

        array = new ulong[x.Length / 16];

        for (int i = 0; i < x.Length; i += 16)
        {
            array[i / 16] = Convert.ToUInt64(x.Substring(i, 16), 16);
        }
        Array.Reverse(array);

    }


    public ulong[] Package(ulong[] poly1, ulong[] poly2, ulong[] poly3, ulong[] poly4, ulong[] poly5)
    {
        ulong[] result = new ulong[poly1.Length];
        operation.LengthControl(ref poly1, ref poly2);
        operation.LengthControl(ref poly1, ref poly3);
        operation.LengthControl(ref poly1, ref poly4);
        operation.LengthControl(ref poly1, ref poly5);

        for (int i = 0; i < poly1.Length; i++)
        {
            result[i] = poly1[i] | poly2[i] | poly3[i] | poly4[i] | poly5[i];
        }
        return result;
    }



    public ulong[] IdentityElement(int m)
    {
        ulong[] result = new ulong[]{ 1 };
        for(int i = 1; i < m; i++)
        {
            result = operation.OR(result, operation.ShiftBitsToHigh(one, i));
        }
        return result;
    }

    
    public string ToHexString()
    {
        string a = string.Concat(array.Select(chunk => chunk.ToString("X").PadLeft(sizeof(ulong) * 2, '0')).Reverse()).TrimStart('0');
        return a != "" ? a : "0";
    }
    

    public override string ToString()
    {
        string result = "";
        ulong word;
        for(int i = 0; i < array.Length - 1; i++)
        {
            word = array[i];
            for (int j = 0; j != 64; j++, word >>= 1)
            {
                ulong bit = word & 1;
                result += bit.ToString();
            }
        }

        word = array[array.Length - 1];
        for (; word != 0; word >>= 1)
        {
            ulong bit = word & 1;
            result += bit.ToString();
        }
        var q = new string(result.ToCharArray().Reverse().ToArray());
        return q.TrimStart('0');
    }
}
