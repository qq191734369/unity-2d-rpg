using System;
using System.Text;
using UnityEngine;

public class NoGenerator
{
    public static string GenerateRandomDigits(int length = 10)
    {
        System.Random random = new System.Random();
        StringBuilder sb = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            sb.Append(random.Next(0, 10));
        }

        return sb.ToString();
    }
}
