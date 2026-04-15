using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public enum CharacterAnimations { Grilling, Standing, Sitting, SittingGround, Walking, Spawn };

[System.Serializable]
public class ClothingItemData
{
    [HideInInspector] public string DisplayName;
    [DisplayInspector] public ItemData Item;
    [SerializeField] private List<GameObject> _meshes;

    public void OnValidate() 
    { 
        if (Item) DisplayName = Item.Name;
        else DisplayName = "Empty";
    }

    public void SetState(bool active)
    {
        foreach (var item in _meshes) item.SetActive(active);
    }
}

[SelectionBase]
public class SpawnedCharacter : MonoBehaviour
{
    [SerializeField] private CharacterMetaController _characterController;
    [SerializeField] public ID ID;

    [SerializeField] private Animator animator;

    [Header("Clothing")]
    [SerializeField] private List<ClothingItemData> _clothingItems;

    [Header("Head Look At")]
    [SerializeField] private bool _disableLookAt;
    [SerializeField] private Transform head;
    [SerializeField] private Transform headForward;
    [SerializeField] private float maxAngle, minAngle;
    [SerializeField] private float lookSpeed;
    [SerializeField] private AnimationCurve growCurve;

    private bool _isLooking;
    private Quaternion _lastRotation;
    private Transform _lookAtTarget;
    private string _saveString;

    //Growing
    private float _growTimer;
    private float _growRate;

    private void OnValidate()
    {
        foreach (var item in _clothingItems) item.OnValidate();
    }

    private void Update()
    {
        if (Time.time < _growTimer)
        {
            var progress = (_growTimer - Time.time / _growRate);
            progress = growCurve.Evaluate(progress);
            transform.localScale = Vector3.Lerp(new Vector3(1,1,1),new Vector3(0, 0, 0), progress );
        }
    }

    private void LateUpdate()
    {
        if (!_disableLookAt) UpdateLookAt();
    }

    public void RandomMannequinPose()
    {
        int numPoses = 4;
        animator.Play("Mannequin Default", -1, Random.Range(1, numPoses) / (float)numPoses);
        animator.speed = 0;
    }

    private void LoadRandomClothing()
    {
        var randomClothing = _clothingItems[Random.Range(0, _clothingItems.Count)];
        ShowClothingItem(randomClothing.Item);
    }

    public void ShowClothingItem(ItemData item)
    {
        //print(gameObject.name + " showing clothing item: " + item.Name);
        foreach(var clothingItem in _clothingItems) clothingItem.SetState(false);
        foreach (var clothingItem in _clothingItems) if (clothingItem.Item == item) clothingItem.SetState(true);
    }

    public async Task LoadFromString(string saveString)
    {
        LoadRandomClothing();

        _saveString = saveString;
        _characterController.LoadFromString(saveString);
        ID = _characterController.Data.ID;

        gameObject.name = _characterController.Data.Name + " (spawned character)";

        await Task.Delay(100);
        _characterController.LoadFromString(saveString);
    }

    [ButtonMethod]
    public void TESTSAVELOAD()
    {
        LoadFromString(_saveString);
    }

    public void CharacterLookAt(Transform target)
    {
        _lookAtTarget = target;
    }

    public void UpdateLookAt()
    {
        if (_lookAtTarget)
        {
            Vector3 Direction = (_lookAtTarget.position - head.position).normalized;
            float angle = Vector3.SignedAngle(Direction, headForward.position, headForward.up);

            if (angle < maxAngle && angle > minAngle)
            {

                if (!_isLooking)
                {
                    _isLooking = true;
                    _lastRotation = head.rotation;
                }

                Quaternion targetRotation = Quaternion.LookRotation(_lookAtTarget.position - head.position);
                _lastRotation = Quaternion.Slerp(_lastRotation, targetRotation, lookSpeed * Time.deltaTime);

                head.rotation = _lastRotation;
            }
        }
        else
        {
            _lastRotation = Quaternion.Slerp(_lastRotation, headForward.rotation, lookSpeed * Time.deltaTime);
            head.rotation = _lastRotation;   
        }
    }

    public void EndCharacterLookAt()
    {
        _lookAtTarget = null;
    }
    
    public void AnimateFromEnum(CharacterAnimations anim)
    {
        animator.SetBool(anim.ToString(), true);

    }

    public void AnimateFromString(string anim)
    {
        animator.SetBool(anim, true);

    }

    public void TriggerFromString(string anim)
    {
        animator.SetTrigger(anim.ToString());
        //Debug.Log(anim);
    }


    public void GrowCharacter(float growTime)
    {
        _growRate = growTime;

        _growTimer = Time.time + growTime;
        transform.localScale = new Vector3(0, 0, 0);

    }

}
