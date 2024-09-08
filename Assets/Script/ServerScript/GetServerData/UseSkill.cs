
using UnityEngine;
using UnityEngine.UI;

public class UseSkill : ISocket
{
    public override void RunNetworkCode(Data data)
    {   
        if(data.id == Socket.Instance.this_player.name) {
            Socket.Instance.this_player_MoveObject.setUserData(data.this_player);
        } else {
            GameObject useSkillPlayer = GameObject.Find(data.id).gameObject;
            SkillPooling.Instance.ShowObject(GameObject.Find(data.id).gameObject.transform , data.skillinfo);
        }
        
    }
}
