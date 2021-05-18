using UnityEngine;

public class combatWeapons : WeaponBehavior
{
    public float slash;
    public float blunt;
    public bool IsAttacked;
    DamageObject d_o;

    void Start()
    {
        d_o = gameObject.AddComponent<DamageObject>();
        d_o.Penetration = 3;
        d_o.IsDestroy = false;
        d_o.IsDamage = true;
        d_o.Damages = new DamageData() { Batting = blunt, Slash = slash };
    }

    void Update()
    {
        d_o.IsDamage = IsAttacked;
        d_o.playerStatus = transform.root.GetComponent<PLAYERS>();
    }

}
