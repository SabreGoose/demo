using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    // [SerializeField] ParticleSystem smashEffect;
    // public static EffectManager Instance {get; private set;}

    // private void Awake() 
    // {
    //     Instance = this;    
    // }
    

    // public void CreateSmashEffect(Vector3 pos, Color smashColor)
    // {
    //     ParticleSystem instance = Instantiate(smashEffect,pos,smashEffect.transform.rotation);
    //     var main = instance.main;
    //     main.startColor = smashColor;
    //     Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
    // }

    public static void CreateEffect(ParticleSystem prefab, Vector3 pos, Color color)
    {
        ParticleSystem instance = Instantiate(prefab,pos,prefab.transform.rotation);
        var main = instance.main;
        main.startColor = color;
        Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
    }

    public static void CreateEffect(ParticleSystem prefab, Vector3 pos)
    {
        ParticleSystem instance = Instantiate(prefab,pos,prefab.transform.rotation);
        Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
    }
}
