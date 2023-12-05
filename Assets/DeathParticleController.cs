using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.SDF;

public class DeathParticleController : MonoBehaviour
{
    MeshToSDFBaker Baker;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    Mesh mesh;
    public VisualEffect vfx;
    public int maxResolution = 64;
    public int signPassCount = 1;
    public float threshold = 0.5f;
    public Bounds Bounds;

    public Material[] materials;
    public float delay = 5;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        skinnedMeshRenderer.BakeMesh(mesh);
        Baker = new MeshToSDFBaker(
            Bounds.size, 
            Bounds.center, 
            maxResolution, mesh, 
            signPassCount, threshold
            );
        Baker.BakeSDF();
        vfx.SetTexture("SDFData", Baker.SdfTexture);
        vfx.SetVector3("BoxSize", Baker.GetActualBoxSize());

    }

    public void Death()
    {
        skinnedMeshRenderer.BakeMesh(mesh);
        Baker.BakeSDF();
        vfx.SetTexture("SDFData", Baker.SdfTexture);
        vfx.SendEvent("OnDeath");
        Invoke(nameof(DisableMeshRenderer), delay);

        foreach(var material in materials)
        {
            material.SetFloat("_StartTime", Time.time);
            material.SetFloat("_Delay", delay);
            material.SetFloat("_Enabled", 1.0f);
        }
    }

    private void DisableMeshRenderer()
    {
        skinnedMeshRenderer.enabled = false;
        foreach (var material in materials)
        {
            material.SetFloat("_Enabled", 0.0f);
        }
    }

    private void OnDestroy()
    {
        if (Baker != null) Baker.Dispose();
    }
}
