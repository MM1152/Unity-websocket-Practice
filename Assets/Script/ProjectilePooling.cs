using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePooling : PoolingManager<Projectile>
{

    public override void Awake()
    {
        base.Awake();
    }
    public override void ShowObject(Transform dropPos, Transform userPos, int value)
    {
        Projectile projectile;
        if(pooling.Count > 0){
            projectile = pooling.Dequeue();
            projectile.transform.position = userPos.position;
            projectile.followTarget = dropPos.gameObject;   
            projectile.  gameObject.SetActive(true);
        }
        else {
            projectile = Instantiate(prefab , transform);
            projectile.gameObject.SetActive( false );
            projectile.transform.position = userPos.position;
            projectile.followTarget = dropPos.gameObject;
            projectile.gameObject.SetActive( true );
        }
    
    }
}
