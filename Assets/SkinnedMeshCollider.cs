using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class SkinnedMeshCollider : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMesh;
    MeshCollider meshCollider;
    public float runEach = 2;
    int i = 0;
    Mesh mesh;
    // Start is called before the first frame update
    void Start()
    {
        meshCollider = GetComponent<MeshCollider>();

        meshCollider.cookingOptions = MeshColliderCookingOptions.CookForFasterSimulation | MeshColliderCookingOptions.UseFastMidphase;
        mesh = new Mesh();
    }

    // Update is called once per frame
    void Update()
    {
        if (skinnedMesh)
        {
            i++;
            if (i < runEach) return;
            skinnedMesh.BakeMesh(mesh);
            meshCollider.sharedMesh = mesh;
            meshCollider.enabled = false;
            meshCollider.enabled = true;
            i = 0;
        }
    }
}
