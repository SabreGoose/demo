//using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RangeInt
{
    public int min = 0;
    public int max = 0;
    
    public int GetRandom()
    {
        return Random.Range(min,max);
    }

    public void ChangeRange(int value)
    {
        min += value;
        max += value;
    }

    public static RangeInt CreateCopy(RangeInt rangeToCopy)
    {
        RangeInt range = new RangeInt();
        range.min = rangeToCopy.min;
        range.max = rangeToCopy.max;
        return range;
    }

}
