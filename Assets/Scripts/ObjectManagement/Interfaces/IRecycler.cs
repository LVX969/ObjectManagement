
#region Using Statements

using System;
using UnityEngine;

#endregion

public interface IRecycler
{
    #region Events

    event Action RecyclerDestroyed;

    #endregion

    #region Methods

    void ReturnInstance(MonoBehaviour component);

    #endregion
}
