
using System.Linq;
using UnityEngine;

public class CheckAttack : ISocket
{
    public override void RunNetworkCode(Data data)
    {
        MoveObject attackingUser = GameObject.Find(data.id.ToString()).GetComponent<MoveObject>();
        StartCoroutine(attackingUser.AttackShow());
    }
}