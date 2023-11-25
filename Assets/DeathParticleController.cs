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
        skinnedMeshRenderer.enabled = false;
        vfx.SendEvent("OnDeath");
    }

    private void OnDestroy()
    {
        if (Baker != null) Baker.Dispose();
    }
}
