using UnityEngine;
 
public class PLAYERS : MonoBehaviour
{
    [SerializeField]
    float HP, MaxHP;
    public string userID;
    public DamageData DefensePoint;


    private void Start()
    {
        userID = MonobitEngine.MonobitNetwork.player.ID.ToString();
    }

    public float hp { get { return HP; } set { HP = value > MaxHP ? MaxHP : (value < 0 ? 0 : value); } }
    public float maxHP { get { return MaxHP; } set { MaxHP = value < HP ? HP : (value < 0 ? 0 : value); } }

    void OnValidate() => HP = MaxHP < HP ? MaxHP : HP;
}
