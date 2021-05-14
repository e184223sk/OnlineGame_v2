using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendUiParameterFROM_Player : MonoBehaviour
{
    PLAYERS players;
    WeaponSelector wSel;
    // Start is called before the first frame update
    void Start()
    {
        players = GetComponent<PLAYERS>();
        wSel = GetComponent<WeaponSelector>();
    }

    // Update is called once per frame
    void Update()
    {
        GameSceneUIController.HP = players.hp / players.maxHP;
        GunBehavior gb_A = wSel.weaponA.GetComponent<GunBehavior>();
        GunBehavior gb_B = wSel.weaponA.GetComponent<GunBehavior>();
        if (gb_A != null)
        {
            GameSceneUIController.Weapon0.enable = true;
            GameSceneUIController.Weapon0.icon = gb_A.icon;
            GameSceneUIController.Weapon0.weaponName = gb_A.WeaponName;
            GameSceneUIController.Weapon0.weaponNumber = 0;
            GameSceneUIController.Weapon0.reload = gb_A.ReloadingF;
            GameSceneUIController.Weapon0.now = gb_A.Loaded.now;
            GameSceneUIController.Weapon0.max = gb_A.Spare.now;
        }
        
    }
}
