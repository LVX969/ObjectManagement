
#region Using Statements

using System.Collections.Generic;
using UnityEngine;

#endregion

public static class GlobalRecycler
{
    #region Private Static Fields

    private static GameObject GlobalInstanceContainer;
    private static Dictionary<GameObject, RecyclableComponentCollection> recycledComponentsLookup;

    #endregion

    #region Static Methods

    public static void SetContainer(GameObject _container)
    {
        GlobalInstanceContainer = _container;
        recycledComponentsLookup = new Dictionary<GameObject, RecyclableComponentCollection>();
    }
    public static T GetInstance<T>(GameObject _prefabObject, int _initialAmount = 10, int _incrementAmount = 10) where T : MonoBehaviour, IRecycledComponent
    {
        if (recycledComponentsLookup.TryGetValue(_prefabObject, out RecyclableComponentCollection pool))
        {
            return pool.GetInstance<T>();
        }
        else
        {
            CreatePool<T>(_prefabObject);
            return GetInstance<T>(_prefabObject);
        }
    }
    public static void DestroyPrefabPoolAndInstances(GameObject prefab)
    {
        if (recycledComponentsLookup.TryGetValue(prefab, out RecyclableComponentCollection value))
        {
            value.DestroyRecycler();
            value = null;
            recycledComponentsLookup.Remove(prefab);
        }
        else
        {
            Debug.Log("Trying to destroy a pool that doesn't exist. : " + prefab.name);
        }
    }

    private static void CreatePool<T>(GameObject _prefabObject) where T : MonoBehaviour, IRecycledComponent
    {
        RecyclableComponentCollection compCollection = new RecyclableComponentCollection(
            _prefabObject, GlobalInstanceContainer.transform);
        compCollection.InstantiateObjectsAs<T>();

        recycledComponentsLookup.Add(_prefabObject, compCollection);
    }

    #endregion
}
