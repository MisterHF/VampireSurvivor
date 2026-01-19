using UnityEngine;

public class ShaderGraphProgressBar : MonoBehaviour
{
    private Renderer renderer;
    private MaterialPropertyBlock mpb;
    private bool initialized;

    private static readonly int FillRate = Shader.PropertyToID("_FillRate");

    void Awake()
    {
        Init();
    }

    void Init()
    {
        if (initialized) { return; }

        renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            return;
        }

        mpb = new MaterialPropertyBlock();
        initialized = true;
    }

    public void SetFill(float value)
    {
        if (!initialized)
        {
            Init();
        }

        if (!initialized)
        {
            return;
        }

        renderer.GetPropertyBlock(mpb);
        mpb.SetFloat(FillRate, Mathf.Clamp(value, -1f, 1f));
        renderer.SetPropertyBlock(mpb);
    }
}
