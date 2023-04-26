//using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Range
{
    public float min = 0f;
    public float max = 0f;
    
    public float GetRandom()
    {
        return Random.Range(min,max);
    }

    public void IncreaseRange(float value)
    {
        min += value;
        max += value;
    }

    public bool isFloatInRange(float value)
    {
        if (value >= min && value <= max)
            return true;
        return false;
    }

    public static Range CreateCopy(Range rangeToCopy)
    {
        Range range = new Range();
        range.min = rangeToCopy.min;
        range.max = rangeToCopy.max;
        return range;
    }

}
