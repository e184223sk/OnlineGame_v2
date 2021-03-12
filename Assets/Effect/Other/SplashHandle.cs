using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashHandle : MonoBehaviour
{
    [SerializeField, Range(0.5f, 4)] float Radius; 
    public float Radius_
    {
        get => Radius;
        set => Radius = (value > 4? 4: (value < 0.5f ? 0.5f :value));
    }
    


    void Update()
    {
        SETSIZE();
        Destroy(this);
    }

#if UNITY_EDITOR
    void OnDrawGizmos() => SETSIZE(false); 
#endif

    public void SETSIZE(bool i = true)
    {
        Radius_ = Radius;
        var p = GetComponent<ParticleSystem>();
        var ps = p.shape;
        ps.radius = Radius ;
        var pe = p.emission;
        pe.SetBurst
        (
            0,
            new ParticleSystem.Burst()
            {
                count = 1302.3f * Radius, // 1302.3は密度の補正値
                time = 0f,
                cycleCount = 1,
                repeatInterval = 0.01f,
                probability = 1f
            }
        );

        if(i) p.Play();
    }
}
