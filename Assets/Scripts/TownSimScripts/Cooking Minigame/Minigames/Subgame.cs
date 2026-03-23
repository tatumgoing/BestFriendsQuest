using UnityEngine;

public class Subgame : MonoBehaviour
{
    protected SubgameController controller;
    protected SubgameData data;

    private bool _initialized;

    public virtual void StartSubgame(SubgameData data) 
    {
        if (!_initialized) Initialize();

        this.data = data;
        gameObject.SetActive(true);
    }

    protected virtual void Initialize()
    {
        _initialized = true;
        controller = GetComponentInParent<SubgameController>(true);
    }
}
