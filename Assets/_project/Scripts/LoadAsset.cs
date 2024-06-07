using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadAsset : MonoBehaviour
{
    [SerializeField] private AssetReferenceGameObject _ground;
    private AsyncOperationHandle<GameObject> _groundOpHandle;
    private bool _called = false;

    void Update()
    {
        if((Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0)) && !_called)
        {
            InstantiateGround();
        }
    }

    private void InstantiateGround()
    {
        if(!_ground.RuntimeKeyIsValid())
            return;
        if(_called)
            return;

        _called = true;
        _groundOpHandle = _ground.LoadAssetAsync<GameObject>();
        _groundOpHandle.Completed += OnGroundComplete;
    }

    private void OnGroundComplete(AsyncOperationHandle<GameObject> asyncOperationHandle)
    {
        if(asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
        {
            Instantiate(asyncOperationHandle.Result, transform);
        }
    }
}
