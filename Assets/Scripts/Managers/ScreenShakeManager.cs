using UnityEngine;
using Unity.Cinemachine;

public class ScreenShakeManager : MonoBehaviour
{
    //Singleton
    public static ScreenShakeManager Instance;

    //Global shake force
    [SerializeField] private float _globalShakeForce = 1.0f;

    // Check if the instance is null, if it is, set it to this
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Shake the camera
    public void CameraShake (CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulse(_globalShakeForce);
    }
}
