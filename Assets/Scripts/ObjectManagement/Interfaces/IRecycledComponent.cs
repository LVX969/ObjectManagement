
public interface IRecycledComponent
{
    #region Properties

    IRecycler RecyclerOwner
    {
        get;
    }

    #endregion

    #region Methods

    void OnInstantiated(IRecycler owner);

    #endregion
}
