using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Linq;

public enum CharacterAnimations { Grilling, Standing, Sitting, SittingGround, Walking, Spawn };

[System.Serializable]
public class ClothingItemData
{
    [HideInInspector] public string DisplayName;
    [DisplayInspector] public ItemData Item;
    [SerializeField] private List<GameObject> _meshes;
    [SerializeField] private SetMaterialField _materialField;
    [SerializeField] private bool _useFavoriteColor;

    public void Initialize(Color favoriteColor)
    {
        if (_useFavoriteColor && _materialField) _materialField.SetColor(favoriteColor);
    }

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
    [SerializeField] private ItemData _defaultClothing;
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
        //print(gameObject.name + " loading random clothing item");
        var inventory = CharacterManager.i.GetInventory(ID);
        if (inventory.Count == 0) ShowClothingItem(_defaultClothing);
        else ShowClothingItem(inventory[Random.Range(0, inventory.Count)]);

        //print(gameObject.name + " inventory: " + string.Join(",", inventory.Select(i => i.Name).ToArray()));
    }

    public void ShowClothingItem(ItemData item)
    {
        //print(gameObject.name + " showing clothing item: " + item.Name);

        foreach(var clothingItem in _clothingItems) clothingItem.SetState(false);
        foreach (var clothingItem in _clothingItems) if (clothingItem.Item == item) clothingItem.SetState(true);
    }

    public async Task LoadFromString(string saveString)
    {
        gameObject.SetActive(true);
        //print(gameObject.name + " loading from string1");

        _saveString = saveString;
        //print(gameObject.name + " loading from string2");
        _characterController.LoadFromString(saveString);
        //print(gameObject.name + " loading from string3");
        ID = _characterController.Data.ID;
        
        //print(gameObject.name + " loading from string4");

        gameObject.name = _characterController.Data.Name + " (spawned character)";

        var color = CharacterManager.i.GetClothingColor(_characterController.Data.FavColor);
        //print("fav color: " + _characterController.Data.FavColor + ", color from manager: " + color);
        foreach (var clothingItem in _clothingItems) clothingItem.Initialize(color);

        //print(_characterController.Data.Name + "loading from string");
        LoadRandomClothing();

        //await Task.Delay(100);
        //_characterController.LoadFromString(saveString);
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
