using UnityEngine;
using MonobitEngine;
public class PLAYERS :MonobitEngine.MonoBehaviour
{
    [SerializeField]
    float HP, MaxHP;
    public string userID;
    public DamageData DefensePoint;


    private void Start()
    {
        userID = GetComponent<MonobitEngine.MonobitView>().owner.ID.ToString();
    }

    public float hp { get { return HP; } set { HP = value > MaxHP ? MaxHP : (value < 0 ? 0 : value); } }
    public float maxHP { get { return MaxHP; } set { MaxHP = value < HP ? HP : (value < 0 ? 0 : value); } }

    void OnValidate() => HP = MaxHP < HP ? MaxHP : HP;


    public override void OnMonobitSerializeViewWrite(MonobitEngine.MonobitStream stream, MonobitEngine.MonobitMessageInfo info)
    {
        stream.Enqueue(userID);
    }
    public override void OnMonobitSerializeViewRead(MonobitEngine.MonobitStream stream, MonobitEngine.MonobitMessageInfo info)
    {
        userID = (string)stream.Dequeue();
    }
}
