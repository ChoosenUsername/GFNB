using System;
using LongModArithmetic;

public class Operation
{
    ulong[] zero = new ulong[1];
    public ulong[] one = new ulong[] { 1 };
    Number One = new Number("1");
    Number Zero = new Number(1);
    Calculator calculator = new Calculator();
    ModCalculator modcalculator = new ModCalculator();
    ulong carry = 0;

    public void LengthControl(ref ulong[] a, ref ulong[] b)
    {
        var requiredlenght = Math.Max(a.Length, b.Length);
        Array.Resize(ref a, requiredlenght);
        Array.Resize(ref b, requiredlenght);
    }

    public int ArrayLength(int m)
    {
        int q = m / 64;
        return m % 64 != 0 ? q + 1 : q;
    }


    public int LongCmp(ulong[] z, ulong[] x)
    {
        LengthControl(ref z, ref x);
        for (int i = z.Length - 1; i > -1; i--)
        {
            if (z[i] > x[i]) return 1;
            if (z[i] < x[i]) return -1;
        }
        return 0;
    }


    public ulong[] RemoveHighZeros(ulong[] c)
    {
        if (LongCmp(c, zero) == 0) { return zero; }
        int i = c.Length - 1;
        while (c[i] == 0)
        {
            i--;
        }
        ulong[] result = new ulong[i + 1];
        Array.Copy(c, result, i + 1);
        return result;
    }


    public ulong[] LongAdd(ulong[] z, ulong[] x)
    {
        LengthControl(ref z, ref x);
        var c = new ulong[z.Length];
        for (int i = 0; i < z.Length; i++)
        {
            ulong temp = unchecked(z[i] + x[i] + carry);
            c[i] = temp;
            carry = temp < z[i] ? 1ul : 0ul;
        }
        return c;
    }


    public ulong[] LongSub(ulong[] a, ulong[] b)
    {
        ulong[] z = new ulong[a.Length];
        ulong[] x = new ulong[b.Length];
        Array.Copy(a, z, a.Length);
        Array.Copy(b, x, b.Length);

        LengthControl(ref z, ref x);
        ulong[] c = new ulong[z.Length];
        if (LongCmp(z, x) == 0) { return c; }
        var borrow = 0UL;
        for (int i = 0; i < c.Length; i++)
        {
            c[i] = z[i] - x[i] - borrow;
            borrow = c[i] > z[i] ? 1UL : 0UL;
        }
        return c;
    }


    public ulong[] ShiftBitsToHigh(ulong[] a, int shift_num)
    {
        if (shift_num == 0) return a;
        int t = shift_num / 64;
        int shift = shift_num - t * 64;
        ulong[] result = new ulong[a.Length + t + 1];

        var carriedBits = 0UL;
        int i = 0;

        for (; i < a.Length; i++)
        {
            var temp = a[i];
            result[i + t] = (temp << shift) | carriedBits;
            if (shift == 0)
            {
                carriedBits = 0;
            }
            else
            {
                carriedBits = temp >> (64 - shift);
            }
        }
        if (64 - shift == 64)
        {
            result[i + t] = 0;
        }
        else
        {
            result[i + t] = a[i - 1] >> (64 - shift);
        }

        int l;
        if (result[result.Length-1] == 0)
        {
            l = result.Length - 1;
        }
        else
        {
            l = result.Length;
        }

        ulong[] spare = new ulong[l];
        Array.Copy(result, spare, l);
        return spare;
    }


    public ulong[] ShiftBitsToLow(ulong[] a, int shift_num)
    {
        ulong[] c = new ulong[a.Length];
        ulong[] surrogate = new ulong[a.Length];
        Array.Copy(a, surrogate, a.Length);
        int shift;
        c = new ulong[a.Length];
        while (shift_num > 0)
        {
            var carriedBits = 0UL;
            if (shift_num < 64) { shift = shift_num; }
            else { shift = 63; }
            int i = a.Length - 1;
            for (; i >= 0; i--)
            {
                var temp = surrogate[i];
                c[i] = (temp >> shift) | carriedBits;
                carriedBits = temp << (64 - shift);
            }
            shift_num -= 63;
            surrogate = c;
        }
        return c;
    }


    public ulong[] AND(ulong[] a, ulong[] b)
    {
        LengthControl(ref a, ref b);
        ulong[] result = new ulong[a.Length];
        for (int i = 0; i < a.Length; i++)
        {
            result[i] = a[i] & b[i];
        }
        return result;
    }


    public ulong[] XOR(ulong[] x, ulong[] y)
    {
        ulong[] a = new ulong[x.Length];
        ulong[] b = new ulong[y.Length];
        Array.Copy(x, a, x.Length);
        Array.Copy(y, b, y.Length);

        LengthControl(ref a, ref b);
        ulong[] result = new ulong[a.Length];
        for (int i = 0; i < a.Length; i++)
        {
            result[i] = a[i] ^ b[i];
        }
        return result;
    }


    public ulong[] OR(ulong[] x, ulong[] y)
    {
        ulong[] a = new ulong[x.Length];
        ulong[] b = new ulong[y.Length];
        Array.Copy(x, a, x.Length);
        Array.Copy(y, b, y.Length);

        LengthControl(ref a, ref b);
        ulong[] result = new ulong[a.Length];
        for (int i = 0; i < a.Length; i++)
        {
            result[i] = a[i] | b[i];
        }
        return result;
    }


    public ulong[] ROL(ulong[] a, int offset, int m)
    {
        var left = ShiftBitsToHigh(a, offset);
        var right = ShiftBitsToLow(a, (m - offset));
        var or = OR(left, right);
        ulong[] result = new ulong[ArrayLength(m)];
        Array.Copy(or, result, result.Length);
        return CutLeadingZeros(result, m);
    }


    public ulong[] CutLeadingZeros(ulong[] array, int m)
    {
        var bitsToCut = 64 - (m % 64);
        array[array.Length - 1] = (array[array.Length - 1] << bitsToCut) >> bitsToCut;
        return array;
    }


    public int HighNotZeroIndex(ulong[] a)
    {
        for (var i = a.Length - 1; i >= 0; i--)
        {
            if (a[i] > 0) { return i; }
        }
        return 0;
    }


    public int BitLength(ulong[] a)
    {
        var bits = 0;
        var index = HighNotZeroIndex(a);
        var temp = a[index];
        while (temp > 0)
        {
            temp >>= 1;
            bits++;
        }
        return bits + sizeof(ulong) * 8 * index;
    }


    public ulong[] Squaring(ulong[] x, int offset, int m)
    {
        Polynomial spare = new Polynomial(1);
        ulong[] result = new ulong[x.Length];
        Array.Copy(x, result, x.Length);
        var mask = AND(result, one);
        result = OR(ShiftBitsToLow(result, 1), ShiftBitsToHigh(mask, m - 1));
        ulong[] b = new ulong[spare.ArrayLength(m)];
        Array.Copy(result, b, b.Length);
        return b;
    }


    public ulong Trace(ulong[] a)
    {
        ulong counter = 0;
        ulong word;
        for (int i = 0; i < a.Length; i++)
        {
            word = a[i];
            for (int j = 0; j != 64; j++, word >>= 1)
            {
                ulong bit = word & 1;
                if (bit == 1)
                {
                    counter++;
                }
            }
        }
        return counter % 2;
    }

    
    public ulong LambdaValue(ulong[][] powers, int i, int j , Number p)
    {
        Number One = new Number("1");
        
        Number right_twin = new Number(1);
        Number left_twin = new Number(1);

        left_twin.array = powers[i];
        right_twin.array = powers[j];

        Number PlusMinus = new Number("1");
        Number MinusPlus = new Number("1");

        Number PlusPlus = calculator.LongAdd(left_twin, right_twin);

        if (LongCmp(powers[i], powers[j]) < 0)
        {
            PlusMinus = calculator.LongSub(right_twin, left_twin);
            PlusMinus.sign = -1;
        }
        else
        {
            PlusMinus = calculator.LongSub(left_twin, right_twin);
        }

        if (calculator.LongCmp(right_twin, left_twin) < 0)
        {
            MinusPlus = calculator.LongSub(left_twin, right_twin);
            MinusPlus.sign = -1;
        }
        else
        {
            MinusPlus = calculator.LongSub(right_twin, left_twin);
        }

        Number MinusMinus = calculator.LongAdd(left_twin, right_twin);
        MinusMinus.sign = -1;

        if (calculator.LongCmp(modcalculator.Mod(PlusPlus, p), One) == 0) { return 1ul; }
        if (calculator.LongCmp(modcalculator.Mod(PlusMinus, p), One) == 0) { return 1ul; }
        if (calculator.LongCmp(modcalculator.Mod(MinusPlus, p), One) == 0) { return 1ul; }
        if (calculator.LongCmp(modcalculator.Mod(MinusMinus, p), One) == 0) { return 1ul; }
        else { return 0ul; }
    }
    

    public ulong[,] LambdaMatrix(int m, Number p)
    {
        ulong[,] lambda = new ulong[m, m];

        var powers = new ulong[174][];
        for (int k = 0; k <= 173; k++)
        {
            var array = new ulong[3];
            array[k / 64] = 1ul << (k % 64);
            powers[k] = array;
        }

        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < m; j++)
            {
                lambda[i, j] = LambdaValue( powers, i, j, p);
            }
        }
        return lambda;
    }


    public ulong[] GetUlong(ulong[,] lambda, int j)
    {
        Polynomial spare = new Polynomial("1");
        ulong[] result = new ulong[spare.ArrayLength(lambda.GetLength(1))];
        ulong[] buffer = new ulong[1];
        for (int k = lambda.GetLength(0) - 1; k >= 0; k--)
        {
            buffer[0] = lambda[k, j];
            result = OR(result, ShiftBitsToHigh(buffer, (lambda.GetLength(0) - 1) - k));
        }
        return result;
    }


    public ulong MultiplicationResultComponent(ulong[] x, ulong[] y, int m, ulong[,] lambda, int i)
    {
        ulong[] a = new ulong[x.Length];
        ulong[] b = new ulong[y.Length];
        Array.Copy(x, a, x.Length);
        Array.Copy(y, b, y.Length);
        Polynomial spare = new Polynomial("1");
        ulong[] multiplier1 = new ulong[spare.ArrayLength(m)];
        ulong[] multiplier2 = new ulong[spare.ArrayLength(m)];
        ulong[] multiplier3 = new ulong[spare.ArrayLength(m)];
        ulong[,] matrix = new ulong[m, 1];
        ulong[] column = new ulong[spare.ArrayLength(m)];

        multiplier1 = ROL(a, i, m);
        for (int j = 0; j < m; j++)
        {
            column = GetUlong(lambda, j);
            matrix[j, 0] = Trace(AND(column, multiplier1));
        }
        multiplier2 = GetUlong(matrix, 0);
        multiplier3 = ROL(b, i, m);
        return Trace(AND(multiplier2, multiplier3));
    }


    public ulong[] Multiplication(ulong[,] lambda, ulong[] x, ulong[] y, int m)
    {
        Polynomial spare = new Polynomial("1");
   
        ulong[] a = new ulong[x.Length];
        ulong[] b = new ulong[y.Length];
        Array.Copy(x, a, x.Length);
        Array.Copy(y, b, y.Length);
        
        ulong[,] ComponentMatrix = new ulong[m, 1];
        for (int i = 0; i < m; i++)
        {
            ComponentMatrix[i, 0] = MultiplicationResultComponent(a, b, m, lambda, i);
        }
        return GetUlong(ComponentMatrix, 0);
    }


    public ulong[] Gorner(ulong[] x, ulong[] y, int m, ulong[,] lambda)
    {
        Polynomial spare = new Polynomial(1);
        ulong[] result = spare.IdentityElement(m);

        ulong[] a = new ulong[x.Length];
        ulong[] b = new ulong[y.Length];
        Array.Copy(x, a, x.Length);
        Array.Copy(y, b, y.Length);

        ulong word;
        for (int i = 0; i < b.Length - 1; i++)
        {
            word = b[i];
            for (int j = 0; j != 64; j++, word >>= 1)
            {
                ulong bit = word & 1;
                if (bit == 1)
                {
                    result = Multiplication(lambda, result, a, m);
                }
                a = Squaring(a, 1, m);
            }
        }

        word = b[b.Length - 1];
        for (; word != 0; word >>= 1)
        {
            ulong bit = word & 1;
            if (bit == 1)
            {
                result = Multiplication(lambda, result, a, m);
            }
            a = Squaring(a, 1, m);
        }
        return result;
    }







    /*
    public ulong[] GrandmaTsujiPatty(ref ulong[] b, ulong[] a, ref ulong k, ulong[ , ] lambda, ulong[] buffer)
    {
        var power = 
        b = 

        return 
    }


    public ulong[] ItohTsujii(ulong[] a, int extension, string sm)
    {
        ulong[] b = new ulong[a.Length];
        Array.Copy(a, b, a.Length);

        ulong k = 1ul ;

        Polynomial m = new Polynomial(sm);
        ulong[] carrier = LongSub(m.array, one);


        var p = new Number(sm);
        p = calculator.LongAdd(calculator.ShiftBitsToHigh(p, 1), One);
        var lambda = LambdaMatrix(extension, p);


        ulong word;
        word = carrier[carrier.Length - 1];
        ulong[] buffer = new ulong[word];
        int n = BitLength(buffer);
        buffer = ShiftBitsToHigh(buffer, 64 - n);
        for( int i = 0; i < n - 1 ; i++)
        {
            b = GrandmaTsujiPatty(ref b, a, ref k, lambda, buffer);
            buffer = ShiftBitsToHigh(buffer, 1);
        }
        */
    
}


