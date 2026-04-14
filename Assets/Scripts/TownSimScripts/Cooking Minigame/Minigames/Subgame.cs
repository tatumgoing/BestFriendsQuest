using UnityEngine;

public class Subgame : MonoBehaviour
{
    protected SubgameController Controller;
    protected SubgameData Data;
    protected float SuccessTime;

    private bool _initialized;

    protected virtual void Update()
    {
        if (!_initialized) {
            Initialize();
            return;
        }

        Controller.UpdateSlider(SuccessTime / Data.TargetTime);
        if (SuccessTime >= Data.TargetTime) {
            gameObject.SetActive(false);
            Controller.CompleteSubgame();
        }
    }

    public virtual void StartSubgame(SubgameData data) 
    {
        Data = data;

        if (!_initialized) Initialize();
        gameObject.SetActive(true);
    }

    protected virtual void Initialize()
    {
        if (Data == null || Data.TargetTime == 0) gameObject.SetActive(false);

        _initialized = true;
        SuccessTime = 0;
        Controller = GetComponentInParent<SubgameController>(true);
    }
}
