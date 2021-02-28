using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogEmitter : MonoBehaviour
{   
    ParticleSystem fogParticles;
    private void Awake() 
    {
        fogParticles = this.gameObject.GetComponent<ParticleSystem>();
        if(!fogParticles.isPlaying)
            fogParticles.Play();
    }

    void Update()
    {
        if(!fogParticles.isPlaying)
            fogParticles.Play();
    }
}
