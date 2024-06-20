using UnityEngine;

public interface Istate{
    public void Enter(IMoveObj moveObject);
    public void Enter(IMoveObj moveObject , Vector2 targetPos);
    public void Update();
    public void Exit();
}