using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CustomMath
{
    public static float QRsqrt(float original)
    {
        int i;
        float _1;
        const float _threeHalves = 1.5f;
        _1 = original * 0.5f;
        i = 0x5f375a86 - (FtoI(original) >> 1);
        original = ItoF(i);
        original = original * (_threeHalves - (_1 * original * original));
        original = original * (_threeHalves - (_1 * original * original));

        return original;
    }

    [StructLayout(LayoutKind.Explicit)]
    struct DoubleLongUnion
    {
        [FieldOffset(0)]
        public int _int;
        [FieldOffset(0)]
        public float _float;
    }

    static int FtoI(float f)
    {
        return new DoubleLongUnion { _float = f }._int;
    }

    static float ItoF(int i)
    {
        return new DoubleLongUnion { _int = i }._float;
    }
}
