using UnityEngine;

public class LAUNCHER_BURNNER_FIRE : MonoBehaviour
{
    public string path; 

    void Start()
    {
        transform.Find(path).GetComponent<ParticleSystem>().Play();
        Destroy(this);
    }     
}
