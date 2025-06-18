using UnityEngine;
using UnityEngine.Rendering;

public class CanvasPerEyeHack : MonoBehaviour
{
    public GameObject leftEyeCanvas;
    //public GameObject rightEyeCanvas;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        // Subscribe to per-eye rendering
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
    }

    private void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        if (camera.stereoActiveEye == Camera.MonoOrStereoscopicEye.Left)
        {
            leftEyeCanvas.SetActive(true);
            //rightEyeCanvas.SetActive(false);
        }
        else if (camera.stereoActiveEye == Camera.MonoOrStereoscopicEye.Right)
        {
            leftEyeCanvas.SetActive(false);
           // rightEyeCanvas.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
    }
}
