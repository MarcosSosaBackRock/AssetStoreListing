using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolver : MonoBehaviour
{
    public float dissolveDuration = 1f;

    public float dissolveStrenght;


    void OnEnable()
    {
        StartCoroutine(Dissolve());
    }


    public IEnumerator Dissolve()
    {
        float elapsedTime = 0;

        Material dissolveMat = GetComponent<Renderer>().material;

        while (elapsedTime < dissolveDuration)
        {
            elapsedTime += Time.deltaTime;

            dissolveStrenght = Mathf.Lerp(0, 1, elapsedTime / dissolveDuration);

            dissolveMat.SetFloat("_DissolveStrenght", dissolveStrenght);

            yield return null;

        }

    }
}
