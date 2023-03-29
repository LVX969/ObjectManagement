
#region Using Statements

using UnityEngine;

#endregion

public class RecyclableComponent : MonoBehaviour, IRecycledComponent
{
    #region Properties

    public IRecycler RecyclerOwner
    {
        get;
        protected set;
    }

    #endregion

    #region Methods

    public virtual void OnInstantiated(IRecycler recycler)
    {
        RecyclerOwner = recycler;
        RecyclerOwner.RecyclerDestroyed += OnRecyclerDestroyed;
    }
    protected virtual void OnRecyclerDestroyed()
    {
        RecyclerOwner.RecyclerDestroyed -= OnRecyclerDestroyed;
        Destroy(gameObject);
    }

    public virtual void Recycle()
    {
        if (RecyclerOwner != null)
        {
            RecyclerOwner.ReturnInstance(this);
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Originating Recycler is null in " + gameObject.name);
        }
    }

    #endregion
}
