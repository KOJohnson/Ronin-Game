using System;
using ThirdPersonMeleeSystem.ScriptableObjects;
using ThirdPersonMeleeSystem.Structs;
using UnityEngine;
using ThirdPersonMeleeSystem.Weapons;

namespace ThirdPersonMeleeSystem.Managers
{
    public class WeaponManager : MonoBehaviour
    {
        #region Public Fields

        public static WeaponManager Instance;

        #endregion
    
        #region Private Fields

        #endregion
    
        #region Serialized Fields
        
        [SerializeField] private MeleeWeapon currentWeapon;
        [SerializeField] private WeaponEvents playerWeaponEvents;
        [SerializeField] private AvatarMask armsOnlyMask;

        #endregion
    
        #region Getters

        public WeaponEvents PlayerWeaponEvents => playerWeaponEvents;
        public bool IsWeaponDrawn { get; private set; }
        public bool CanCombo { get; private set; }
        public AttackAnimationData LastAttack { get; private set; }
        
        #endregion

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }

            if (IsValidWeapon())
            {
                currentWeapon.gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            playerWeaponEvents.ToggleCanCombo += ToggleCanCombo;
            playerWeaponEvents.ToggleWeaponEvent += ToggleCurrentWeapon;
        }

        private void OnDisable()
        {
            playerWeaponEvents.ToggleCanCombo -= ToggleCanCombo;
            playerWeaponEvents.ToggleWeaponEvent -= ToggleCurrentWeapon;
        }
        
        public bool IsValidWeapon()
        {
            return currentWeapon != null;
        }

        public MeleeWeapon GetCurrentWeapon()
        {
            return currentWeapon;
        }

        public void EquipWeapon()
        {
            PlayerAnimationManager.Instance.PlayAction(currentWeapon.equipAnimation, armsOnlyMask);
            SetIsWeaponDrawn(true);
        }
        
        //skips equip animation
        public void FastEquipWeapon()
        {
            SetIsWeaponDrawn(true);
            currentWeapon.gameObject.SetActive(true);
        }
        
        public void SheatheWeapon()
        {
            PlayerAnimationManager.Instance.PlayAction(currentWeapon.sheatheAnimation, armsOnlyMask);
            SetIsWeaponDrawn(false);
        }

        public void SetCurrentWeapon(MeleeWeapon newWeapon)
        {
            if (IsValidWeapon()) currentWeapon.gameObject.SetActive(false);
            currentWeapon = newWeapon;
        }

        private void ToggleCurrentWeapon(bool status)
        {
            currentWeapon.gameObject.SetActive(status);
        }

        public void SetIsWeaponDrawn(bool status)
        {
            IsWeaponDrawn = status;
        }

        public void SetLastAttack(AttackAnimationData lastAttack)
        {
            LastAttack = lastAttack;
        }

        private void ToggleCanCombo(bool toggle)
        {
            CanCombo = toggle;
        }

    }
}
