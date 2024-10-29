using System;
using System.Collections.Generic;

public static class DictionaryUtils
{
    public static double ScalarProduct(Dictionary<string, double> Dict1, Dictionary<string, double> Dict2)
    {
        return Dict1.Keys
            .Intersect(Dict2.Keys)
            .Sum(key => Dict1[key] * Dict2[key]);
    }
}