using UnityEngine;

public class ItemEffectColorSetter : MonoBehaviour
{
    void Start()
    {
        var p = GetComponent<ParticleSystem>();
        var s = transform.Find("sub").GetComponent<ParticleSystem>();
        p.startColor = s.startColor = Color.HSVToRGB(Random.Range(0f, 1f), 0.5f, 0.5f);
        p.Play();
        s.Play();
        Destroy(this);
    }
}