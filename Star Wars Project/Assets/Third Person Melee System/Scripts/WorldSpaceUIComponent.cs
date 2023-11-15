using UnityEngine;

public class WorldSpaceUIComponent : MonoBehaviour
{
    #region Public Fields
    #endregion
    
    #region Private Fields
    
    private Camera _camera;
    
    #endregion
    
    #region Serialized Fields
    #endregion
    
    #region Getters
    #endregion

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_camera == null) return;
        transform.rotation = _camera.transform.rotation;
    }
}
