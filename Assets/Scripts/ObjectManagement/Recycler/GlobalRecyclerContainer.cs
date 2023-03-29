
#region Using Statements

using UnityEngine;

#endregion

public class GlobalRecyclerContainer : MonoBehaviour
{
    #region Methods

    private void Awake()
    {
        GlobalRecycler.SetContainer(gameObject);
    }

    #endregion
}
