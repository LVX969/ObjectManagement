
#region Using Statements

using UnityEngine;

#endregion

public class LocalObjectRecycler : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private GameObject Prefab;
    [SerializeField] private bool UnparentObjectOnGet = false;

    #endregion

    #region Private Fields

    private RecyclableComponentCollection recycledObjects;

    #endregion

    #region Methods

    public void InitializeAs<T>() where T : MonoBehaviour, IRecycledComponent
    {
        recycledObjects = new RecyclableComponentCollection(Prefab, transform, UnparentObjectOnGet);
        recycledObjects.InstantiateObjectsAs<T>();
    }
    public T Get<T>() where T : MonoBehaviour, IRecycledComponent
    {
        return recycledObjects.GetInstance<T>();
    }

    #endregion
}
