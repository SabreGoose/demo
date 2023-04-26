using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuParticles : MonoBehaviour
{
    private static MainMenuParticles instance;
    public static MainMenuParticles Instance
    {
        get
        {
            return instance;
        }
    } 

    
    private void Awake() 
    {
        if (instance is null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            return;
        }
        Destroy(this.gameObject);
    }

    public void Stop()
    {
        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particleSystem in particleSystems)
        {
            particleSystem.Stop();
        }
    }

    public void Destroy()
    {
        instance = null;
        Destroy(this.gameObject);
    }

}
