using UnityEngine;

public class RecycledObjectFactory : MonoBehaviour
{
    [SerializeField] private GameObject ObjectPrefab;

    public T Get<T>() where T : MonoBehaviour, IRecycledComponent
    {
        return GlobalRecycler.GetInstance<T>(ObjectPrefab);
    }
}
