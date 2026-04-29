using UnityEngine;

public class Subgame : MonoBehaviour
{
    protected SubgameController Controller;
    protected SubgameData Data;
    protected float SuccessTime;

    private bool _initialized;

    protected virtual void ShowCam(int camIndex) => Controller.AreaController.ShowSubgameSceneCam(Data.Type, camIndex);
    protected virtual void ResetCam() => Controller.AreaController.ResetCamera();

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

        if (SuccessTime < 0.0f) { 
            SuccessTime = 0.0f;
        }
    }


    public virtual void StartSubgame(SubgameData data) 
    {
        Data = data;

        SuccessTime = 0;

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
