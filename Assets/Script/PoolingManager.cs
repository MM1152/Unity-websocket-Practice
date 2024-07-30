using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class PoolingManager<T> : MonoBehaviour where T : MonoBehaviour
{
    protected Queue<T> pooling;
    [SerializeField] protected T prefab;
    private static PoolingManager<T> poolingManager;
    public static PoolingManager<T> Instance {
        get {
            if(poolingManager == null) {
                poolingManager = FindObjectOfType<PoolingManager<T>>();
                
                if(poolingManager == null) {
                    Debug.Log("PoolingManger Setting Error");
                }
            }else {
                Debug.Log("Setting PoolingManager" + poolingManager.GetType().ToString());
            }
            return poolingManager;
        }
    }
    public virtual void Awake() {
        pooling = new Queue<T>();
        poolingManager = this;
    }
    public virtual void ShowObject(Transform dropPos, Transform userPos, int value) { }
    public virtual void ShowObject(Vector2 showPos , int value) { }
    public virtual void ReturnObject(T Object) {
        pooling.Enqueue(Object);
        Object.transform.SetParent(poolingManager.transform);
        Object.gameObject.SetActive(false);
    }
}
