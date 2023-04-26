using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionScaler : MonoBehaviour
{
    private const float basicWidth = 2400;
    private const float basicHeight = 1080;
    private static float scaler;
    
    public static void SetScaler()
    {
        float basicAspectRatio = basicWidth / basicHeight;
        float width = Screen.width;
        float height = Screen.height;
        float aspectRatio = width / height;
        scaler = aspectRatio / basicAspectRatio;
    }

    public static float GetScaler()
    {
        return scaler;
    }

    public void ScaleObject()
    {
        transform.localScale *= scaler;
    }

    private void Start() 
    {
        ScaleObject();
    }

}
