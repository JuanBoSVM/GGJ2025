using UnityEngine;
using Unity.Cinemachine;

public class ShakeCameraManager : MonoBehaviour
{
    private static ShakeCameraManager _instance;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float _globalShakeForce = 1.0f;

    public static ShakeCameraManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<ShakeCameraManager>();
            }

            if (_instance == null)
            {
                GameObject gm = new GameObject("ShakeCameraManager");
                _instance = gm.AddComponent<ShakeCameraManager>();
            }
            return _instance;
        }
    }

    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }

        DontDestroyOnLoad(this);
    }

    public void ShakeCamera(CinemachineImpulseSource _impulseSource)
    {
        _impulseSource.GenerateImpulseWithForce(_globalShakeForce);
    }
}
