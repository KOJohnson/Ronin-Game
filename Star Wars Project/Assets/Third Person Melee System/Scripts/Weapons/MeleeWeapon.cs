using System;
using Sirenix.OdinInspector;
using ThirdPersonMeleeSystem.Managers;
using ThirdPersonMeleeSystem.ScriptableObjects;
using ThirdPersonMeleeSystem.Structs;
using UnityEditor;
using UnityEngine;
using XftWeapon;
using Random = UnityEngine.Random;

namespace ThirdPersonMeleeSystem.Weapons
{
    [RequireComponent(typeof(WeaponDamageComponent), typeof(AudioSource))]
    public class MeleeWeapon : MonoBehaviour
    {
        #region Private Fields
        
        private WeaponDamageComponent _weaponDamageComponent;
        
        #endregion
        
        #region Public Fields

        [Header("Stats")] 
        [SerializeField]private int damage;
        [SerializeField]private int postureDamage;
        
        [Header("Attack Animations")]
        public AttackData[] lightAttacks;
        public AttackData[] heavyAttacks;
        public AttackData jumpAttackStart, jumpAttackEnd;
        public AnimationData jumpAttackLoop;
        public AttackData sprintAttack;

        [Header("Finisher Animations")] 
        public FinisherAnimationsAsset[] finisherAnimations;

        [Header("Equip Animations")] 
        public AnimationData equipAnimation; 
        public AnimationData sheatheAnimation;
        
        #endregion

        #region Serialized Fields

        [Header("Audio Sources")] 
        [SerializeField] private AudioSource weaponAudioSource;

        [Header("Locomotion Animations")] 
        [SerializeField] private CombatLocomotionAsset locomotionAsset;

        [Header("Weapon Effects")] 
        [SerializeField] private WeaponSounds weaponSounds;
        [SerializeField] private XWeaponTrail weaponTrail;

        #endregion
    
        #region Getters

        public CombatLocomotionAsset LocomotionAsset => locomotionAsset;
        protected WeaponManager WeaponManager { get; private set; }

        #endregion

        private void Awake()
        {
            InitWeaponManager();
            InitWeaponDamageComponent();
            SetupAttacks();
        }

        private void OnEnable()
        {
            RegisterWeaponEvents();
        }

        private void OnDisable()
        {
            UnregisterWeaponEvents();
            ToggleDamageComponent(false);
            ToggleWeaponTrail(false);
        }

        #region Setup

        private void SetupAttacks()
        {
            if (lightAttacks.Length > 0)
            {
                foreach (AttackData attack in lightAttacks)
                {
                    if (attack == null) continue;
                    attack.attackAnimationData.attackAnimation.IsInteracting = true;
                    attack.attackAnimationData.attackType = AttackType.LightAttack;
                }
            }

            if (heavyAttacks.Length > 0)
            {
                foreach (AttackData attack in heavyAttacks)
                {
                    if (attack == null) continue;
                    attack.attackAnimationData.attackAnimation.IsInteracting = true;
                    attack.attackAnimationData.attackType = AttackType.HeavyAttack;
                }
            }
        }
        private void InitWeaponManager()
        {
            WeaponManager = transform.root.GetComponent<WeaponManager>();
        }
        private void InitWeaponDamageComponent()
        {
            _weaponDamageComponent = GetComponent<WeaponDamageComponent>();
            ToggleDamageComponent(false);
            ToggleWeaponTrail(false);
        }
        
        protected virtual void RegisterWeaponEvents()
        {
            if (WeaponManager)
            {
                WeaponManager.PlayerWeaponEvents.ToggleDamageComponent += ToggleDamageComponent;
                WeaponManager.PlayerWeaponEvents.ToggleDamageComponent += ToggleWeaponTrail;
            }
        }
        
        protected virtual void UnregisterWeaponEvents()
        {
            WeaponManager.PlayerWeaponEvents.ToggleDamageComponent -= ToggleDamageComponent;
            WeaponManager.PlayerWeaponEvents.ToggleDamageComponent -= ToggleWeaponTrail;
        }

        #endregion

        private void ToggleDamageComponent(bool toggle)
        {
            _weaponDamageComponent.enabled = toggle;
        }

        private void ToggleWeaponTrail(bool toggle)
        {
            weaponTrail.enabled = toggle;
        }

        public int GetWeaponDamage()
        {
            return WeaponManager.LastAttack.OverrideDamage ? WeaponManager.LastAttack.damageOverride : damage;
        }
        
        public int GetPostureDamage()
        {
            return postureDamage;
        }

        public AttackType GetAttackType()
        {
            return WeaponManager.LastAttack.attackType;
        }

        public void PlayAttackSound()
        {
            int random = Random.Range(0, weaponSounds.weaponSwingSound.Length);
            weaponAudioSource.PlayOneShot(weaponSounds.weaponSwingSound[random]);
        }
        
        #if UNITY_EDITOR

        [Button]
        private void CreateNewAttackDataAsset()
        {
            AttackData newAttackSO = ScriptableObject.CreateInstance<AttackData>();
            string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Third Person Melee System/Scriptable Objects/Attack Presets/New Attack Data Asset.asset");
            AssetDatabase.CreateAsset(newAttackSO, path);
            EditorUtility.OpenPropertyEditor(newAttackSO);
        }
        
        #endif
        
    }
}
