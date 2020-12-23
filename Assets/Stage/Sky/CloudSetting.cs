using UnityEngine;

public class CloudSetting : MonoBehaviour
{
    void Start()
    {
        GetComponent<ParticleSystem>().Simulate(700);
        GetComponent<ParticleSystem>().Play();
        Destroy(this);
    } 
}
