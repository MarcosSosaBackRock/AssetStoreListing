using UnityEngine;

[ExecuteAlways]
public class WaterFoamController : MonoBehaviour
{
    public Transform[] floatingObjects;
    public Material waterMaterial;

    void Update()
    {
        int count = Mathf.Min(floatingObjects.Length, 10);

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = floatingObjects[i].position;
            waterMaterial.SetVector("_ObjectPositions" + i, new Vector4(pos.x, pos.y, pos.z, 0));
        }

        waterMaterial.SetFloat("_NumObjects", count);
    }
}
