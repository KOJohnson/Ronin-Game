using Sirenix.OdinInspector;
using ThirdPersonMeleeSystem.ScriptableObjects;
using UnityEngine;

namespace ThirdPersonMeleeSystem.Managers
{
    public class FootstepManager : MonoBehaviour
    {
        #region Public Fields

        #endregion

        #region Private Fields
        
        [ShowInInspector]private Transform _leftFootBoneRef;
        [ShowInInspector]private Transform _rightFootBoneRef;
        
        private RaycastHit _leftFootHit;
        private RaycastHit _rightFootHit;
        private bool _inList;
        private readonly int _mainTex = Shader.PropertyToID("_MainTex");
        
        #endregion

        #region Serialized Fields
        
        [SerializeField]private Animator animator;
        [SerializeField]private ThirdPersonController controller;
        [SerializeField]private AudioSource audioSource;

        [SerializeField]private float rayLength;
        [SerializeField]private LayerMask whatIsGround;
        [SerializeField]private FootStepSoundsAsset footStepSounds;
        [SerializeField]private AudioClip genericSound;
        [SerializeField]private bool blendTerrainSounds;
        [SerializeField]private Vector3 offset;

        #endregion

        #region Getters

        #endregion

        private void Awake()
        {
            GetFeetBones();
        }

        private void Update()
        {
            
        }
        
        [Button]
        private void GetFeetBones()
        {
            if (animator != null)
            {
                _leftFootBoneRef = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
                _rightFootBoneRef = animator.GetBoneTransform(HumanBodyBones.RightFoot);
            }
        }

        private void OnLeftFootStepEnter()
        {
            if (!controller.PlayerGrounded) return;
            if (!Physics.Raycast(_leftFootBoneRef.position + offset, Vector3.down, out _leftFootHit, rayLength, whatIsGround)) return;
            
            if (_leftFootHit.collider.TryGetComponent(out Terrain terrain))
            {
                PlayFootstepFromTerrain(terrain, _leftFootHit.point);
            }
            else if (_leftFootHit.collider.TryGetComponent(out Renderer renderer))
            {
                PlayFootstepFromRenderer(renderer);
            }
        }

        private void OnRightFootStepEnter()
        {
            if (!controller.PlayerGrounded) return;
            if (!Physics.Raycast(_rightFootBoneRef.position + offset, Vector3.down, out _rightFootHit, rayLength, whatIsGround)) return;
            
            if (_rightFootHit.collider.TryGetComponent(out Terrain terrain))
            {
                    PlayFootstepFromTerrain(terrain, _rightFootHit.point);
            }
            else if (_rightFootHit.collider.TryGetComponent(out Renderer renderer))
            {
                PlayFootstepFromRenderer(renderer);
            }
        }
        
        private void PlayFootstepFromTerrain(Terrain terrain, Vector3 hitPoint)
        {
            _inList = false;
            Vector3 terrainPosition = hitPoint - terrain.transform.position;
            Vector3 splatMapPosition = new Vector3(terrainPosition.x / terrain.terrainData.size.x, 0,
                terrainPosition.z / terrain.terrainData.size.z);

            int x = Mathf.FloorToInt(splatMapPosition.x * terrain.terrainData.alphamapWidth);
            int z = Mathf.FloorToInt(splatMapPosition.z * terrain.terrainData.alphamapHeight);

            float[,,] alphaMap = terrain.terrainData.GetAlphamaps(x, z, 1, 1);

            if (alphaMap.Length < 1) return;

            if (!blendTerrainSounds)
            {
                int primaryIndex = 0;
                
                for (int i = 1; i < alphaMap.Length; i++)
                {
                    if (alphaMap[0,0,i] > alphaMap[0,0, primaryIndex] )
                    {
                        primaryIndex = i;
                    }
                }
                
                foreach (TextureSound textureSound in footStepSounds.textureSounds)
                {
                    if (textureSound.texture == terrain.terrainData.terrainLayers[primaryIndex].diffuseTexture)
                    {
                        _inList = true;

                        if (textureSound.sound != null)
                        {
                            audioSource.PlayOneShot(textureSound.sound);
                        }
                        else
                        {
                            Debug.LogWarning($"{textureSound.sound} is null, please assign a valid path, playing generic sound");
                            PlayGenericSound();
                        }
                    }
                }

                if (_inList) return;
                Debug.LogWarning($"{terrain.terrainData.terrainLayers[primaryIndex].diffuseTexture} could not be found in list");
                PlayGenericSound();
            }
        }

        private void PlayFootstepFromRenderer(Renderer renderer)
        {
            _inList = false;
            
            foreach (TextureSound textureSound in footStepSounds.textureSounds)
            {
                if (textureSound.texture == renderer.material.GetTexture(_mainTex))
                {
                    _inList = true;

                    if (textureSound.sound != null)
                    {
                        audioSource.PlayOneShot(textureSound.sound);
                    }
                    else
                    {
                        Debug.LogWarning($"{textureSound.sound} is null, please assign a valid path, playing generic sound");
                        PlayGenericSound();
                    }
                }
            }

            if (_inList) return;
            Debug.LogWarning($"{renderer.material.GetTexture(_mainTex)} could not be found in list");
            PlayGenericSound();
        }

        private void PlayGenericSound()
        {
            audioSource.PlayOneShot(genericSound);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(_leftFootBoneRef.position + offset, _leftFootBoneRef.position + Vector3.down * rayLength);
            Gizmos.DrawLine(_rightFootBoneRef.position + offset, _rightFootBoneRef.position + Vector3.down * rayLength);
        }
    }
}
