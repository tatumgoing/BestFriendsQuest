using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Linq;
using Unity.VisualScripting;

public enum CharacterAnimations { Grilling, Standing, Sitting, SittingGround, Walking, Spawn };

[System.Serializable]
public class ClothingItemData
{
    [HideInInspector] public string DisplayName;
    [DisplayInspector] public List<ItemData> Items;
    [SerializeField] private List<GameObject> _meshes;
    [SerializeField] private SetMaterialField _materialField;
    [SerializeField] private SetTexture _textureController;
    [SerializeField] private bool _useFavoriteColor;

    public SetMaterialField MeshController => _materialField;

    public void Initialize(Color favoriteColor)
    {
        if (_useFavoriteColor && _materialField) _materialField.SetColor(favoriteColor);
    }

    public void Configure(ItemData item)
    {
        if (item.Texture != null && _textureController) _textureController.Set(item.Texture);
    }

    public void OnValidate() 
    { 
        if (Items != null && Items.Count > 0 && Items[0] != null) DisplayName = Items[0].Name;
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

    private List<ClothingItemData> _nonHats => _clothingItems.Where(x => !x.Items.Where(y => y.ClothingType == ClothingType.HAT).Any()).ToList();

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

    public void SetHat(ItemData hat)
    {
        foreach(var item in _clothingItems) {
            bool isHat = item.Items.Where(x => x.ClothingType == ClothingType.HAT).ToList().Count() > 0;
            if (isHat) {
                item.SetState(item.Items.Contains(hat));
            }
        }
    }

    public void RandomMannequinPose()
    {
        int numPoses = 4;
        animator.Play("Mannequin Default", -1, Random.Range(1, numPoses) / (float)numPoses);
        animator.speed = 0;
    }

    private void LoadRandomClothing()
    {
        var inventory = CharacterManager.i.GetInventory(ID).Where(x => x.Type == ItemType.Clothing && x.ClothingType != ClothingType.HAT).ToList();
        if (inventory.Count == 0) ShowClothingItem(_defaultClothing);
        else {
            var selected = inventory[Random.Range(0, inventory.Count)];
            if (selected.ClothingType == ClothingType.OUTFIT) ShowClothingItem(selected);
            else {
                var outfit = new List<ItemData>() { selected };

                var missingPieceType = selected.ClothingType == ClothingType.TOP ? ClothingType.BOTTOM : ClothingType.TOP;
                var missingPiece = inventory.FirstOrDefault(item => item.ClothingType == missingPieceType);

                if (missingPiece) {
                    outfit.Add(missingPiece);
                    ShowClothingItem(outfit);
                }
                else {
                    ShowClothingItem(_defaultClothing);
                }
            }
        }
    }

    public void WearOrMakeOutfit(ItemData selected)
    {
        if (selected.ClothingType == ClothingType.OUTFIT) ShowClothingItem(selected);
        else {
            var inventory = CharacterManager.i.GetInventory(ID).Where(x => x.Type == ItemType.Clothing && x.ClothingType != ClothingType.HAT).ToList();
            var outfit = new List<ItemData>() { selected };

            var missingPieceType = selected.ClothingType == ClothingType.TOP ? ClothingType.BOTTOM : ClothingType.TOP;
            var missingPiece = inventory.FirstOrDefault(item => item.ClothingType == missingPieceType);

            if (missingPiece) {
                outfit.Add(missingPiece);
                ShowClothingItem(outfit);
            }
            else {
                ShowClothingItem(_defaultClothing);
            }
        }
    }

    private void ShowClothingItem(List<ItemData> items)
    {
        foreach (var clothingItem in _nonHats) clothingItem.SetState(false);
        foreach (var item in items) ShowClothingItem(item, false);
    }

    public void ShowClothingItem(ItemData item, bool disableOthers = true)
    {
        if (disableOthers) foreach(var clothingItem in _nonHats) clothingItem.SetState(false);
        foreach (var clothingItem in _clothingItems) {
            if (clothingItem.Items.Contains(item)) {
                clothingItem.SetState(true);
                item.AffectMesh(clothingItem.MeshController);

                clothingItem.Configure(item);
            }
        }
    }

    public void LoadFromString(string saveString)
    {
        gameObject.SetActive(true);

        _saveString = saveString;
        _characterController.LoadFromString(saveString);
        ID = _characterController.Data.ID;

        gameObject.name = _characterController.Data.Name + " (spawned character)";

        var color = CharacterManager.i.GetClothingColor(_characterController.Data.FavColor);
        foreach (var clothingItem in _clothingItems) clothingItem.Initialize(color);

        LoadRandomClothing();
    }

    public void CharacterLookAt(Transform target, bool snapTo = false)
    {
        _lookAtTarget = target;
        if (snapTo) {
            Quaternion targetRotation = Quaternion.LookRotation(_lookAtTarget.position - head.position);
            head.rotation = targetRotation;
        }
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
    
    public void AnimateFromEnum(CharacterAnimations anim, bool value = true)
    {
        animator.SetBool(anim.ToString(), value);
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
