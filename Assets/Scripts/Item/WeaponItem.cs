using UnityEngine;

// 현재 미구현입니다.
// HERE
namespace ST07.Items
{
    public enum WeaponType{
        //무기 종류
    }
    [CreateAssetMenu(fileName = "New Weapon Item", menuName = "Items/Weapon Item")]
    public class WeaponItem : ItemDefinition
    {
        [Header("Weapon Type")]
        public WeaponType weaponType;
    }
}
