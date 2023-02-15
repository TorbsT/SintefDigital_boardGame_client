using UnityEngine;

namespace View
{
    public static class PoolManager
    {
        public static IPoolManager Instance { get; set; }

        public static void Enpool(GameObject gameObject)
            => Instance.Enpool(gameObject);
        public static GameObject Depool(GameObject prefab)
            => Instance.Depool(prefab);
    }
    public interface IPoolManager
    {
        void Enpool(GameObject gameObject);
        GameObject Depool(GameObject prefab);
    }
}

