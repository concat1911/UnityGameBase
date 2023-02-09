using UnityEngine;
using UnityEngine.Pool;

public class GameObjPoolItem : MonoBehaviour
{
    public IObjectPool<GameObjPoolItem> pool; // Who spawn me

    private void OnDisable()
    {
        pool.Release(this);
    }
}
