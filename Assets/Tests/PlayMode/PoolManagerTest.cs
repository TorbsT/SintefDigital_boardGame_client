using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using View;
using Helpers;

public class PoolManagerTest
{
    [UnityTest]
    public IEnumerator SingletonTest()
    {
        new GameObject().AddComponent<PoolHelper>();
        yield return null;
        Assert.NotNull(PoolManager.Instance);
        yield return null;
    }
    [UnityTest]
    public IEnumerator SimpleSpawnTest()
    {
        new GameObject().AddComponent<PoolHelper>();
        yield return null;
        List<GameObject> gameObjects = new();
        for (int i = 0; i < 100; i++)
        {
            Assert.DoesNotThrow(() => {
                gameObjects.Add(PoolManager.Depool(new()));
            });
        }
        yield return null;
        foreach (GameObject go in gameObjects)
        {
            Assert.DoesNotThrow(() => {
                PoolManager.Enpool(go);
            });
        }
        yield return null;
        foreach (GameObject go in gameObjects)
        {
            Assert.IsNotNull(go);
            Assert.IsFalse(go.activeSelf);
        }
        yield return null;
    }
}
