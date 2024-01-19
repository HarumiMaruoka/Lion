using System;
using UnityEngine;

public static class ArrayExtensions
{
    public static void Shuffle<T>(this T[] array)
    {
        System.Random rng = new System.Random();
        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T temp = array[k];
            array[k] = array[n];
            array[n] = temp;
        }
    }

}