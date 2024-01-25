using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ListExtentions
{
    public static List<T> Shuffle<T>(this List<T> source)
    {
        List<T> shuffledList = source;
        int n = shuffledList.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0,n+1);
            (shuffledList[k], shuffledList[n]) = (shuffledList[n], shuffledList[k]);
        }

        return shuffledList;
    }

    public static T TakeRandom<T>(this IEnumerable<T> source)
    {
        return source.TakeRandom(1).Single();
    }

    public static IEnumerable<T> TakeRandom<T>(this IEnumerable<T> source, int count, bool shuffle = false)
    {
        if (shuffle)
            return source.ToList().Shuffle().Take(count); 
        return source.Take(count);
    }
    
    public static void MovePosition<T>(this IList<Transform> list, Vector3 loc, float moveTime)
    {
        foreach (var e in list)
            e.DOMove(loc,moveTime);
    }
}
