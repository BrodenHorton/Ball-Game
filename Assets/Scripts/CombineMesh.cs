using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
public class CombineMesh : MonoBehaviour {

    public void CombineMeshes(List<MeshFilter> meshFilters) {
        CombineInstance[] combine = new CombineInstance[meshFilters.Count];

        for (int i = 0; i < meshFilters.Count; i++) {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine);

        GetComponent<MeshFilter>().mesh = combinedMesh;
        GetComponent<MeshCollider>().sharedMesh = combinedMesh;
    }
}
