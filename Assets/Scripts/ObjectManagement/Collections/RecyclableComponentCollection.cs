
#region Using Statements

using System;
using UnityEngine;
using UnityEngine.Pool;

#endregion

public class RecyclableComponentCollection : IRecycler
{
    #region Events

    public event Action RecyclerDestroyed;

    #endregion

    #region Fields

    private GameObject prefabObject;
    private GameObject container;
    private ObjectPool<MonoBehaviour> recycledComponents;
    private bool unparentOnGet = true;

    #endregion

    #region Properties

    public int Count => recycledComponents.CountAll;
    public bool IsInternalPoolNull => recycledComponents?.CountAll == 0;

    #endregion

    #region Constructor

    public RecyclableComponentCollection(GameObject _prefabObject, Transform root, bool _unparentOnGet = true)
    {
        container = new GameObject(_prefabObject.name + " : Recycler Container");
        container.transform.SetParent(root);
        container.transform.position = root.position;

        prefabObject = _prefabObject;
        unparentOnGet = _unparentOnGet;
    }

    #endregion

    #region Initialization

    public void InstantiateObjectsAs<T>() where T : MonoBehaviour, IRecycledComponent
    {
        if (prefabObject.GetComponent<T>() == null)
        {
            Debug.LogError("Prefab object does not contain component of Type T : " + typeof(T).ToString());

            return;
        }

        recycledComponents = new ObjectPool<MonoBehaviour>(
        RecycledObjectCreate<T>,
        RecycledObjectGet,
        RecycledObjectRelease,
        RecycledObjectDestroy, true);
    }

    private T RecycledObjectCreate<T>() where T : MonoBehaviour, IRecycledComponent
    {
        GameObject obj = UnityEngine.Object.Instantiate(prefabObject, container.transform);

        obj.name = prefabObject.name + "_CLONE_" + (recycledComponents.CountAll + 1);
        obj.transform.position = container.transform.position;
        obj.SetActive(false);

        T instance = obj.GetComponent<T>();
        instance.OnInstantiated(this);

        return instance;
    }
    private void RecycledObjectGet(MonoBehaviour obj)
    {
        if (unparentOnGet)
        {
            obj.transform.SetParent(null);
        }
    }
    private void RecycledObjectRelease(MonoBehaviour obj)
    {
        obj.gameObject.SetActive(false);

        if (unparentOnGet)
        {
            obj.transform.SetParent(container.transform);
            obj.transform.position = container.transform.position;
        }
    }
    private void RecycledObjectDestroy(MonoBehaviour obj)
    {
        GameObject.Destroy(obj.gameObject);
    }

    #endregion

    #region Get & Return

    public T GetInstance<T>() where T : MonoBehaviour, IRecycledComponent
    {
        return recycledComponents.Get() as T;
    }
    public void ReturnInstance(MonoBehaviour obj)
    {
        if (obj.gameObject == null)
            return;

        recycledComponents.Release(obj);
    }

    #endregion

    #region Destroy

    public void DestroyRecycler()
    {
        RecyclerDestroyed?.Invoke();

        recycledComponents.Dispose();
        recycledComponents = null;

        GameObject.Destroy(container);
    }

    #endregion
}
