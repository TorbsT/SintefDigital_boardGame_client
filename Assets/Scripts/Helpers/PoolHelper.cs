using UnityEngine;
using TorbuTils.EzPools;
using Common;

namespace Helpers
{
    public class PoolHelper : MonoBehaviour, IPoolManager
    {
        private void Awake()
        {
            PoolManager.Instance = this;
            gameObject.AddComponent<Pools>();
        }
        public void Enpool(GameObject gameObject)
            => Pools.Instance.Enpool(gameObject);
        public GameObject Depool(GameObject prefab)
            => Pools.Instance.Depool(prefab);
    }
}
