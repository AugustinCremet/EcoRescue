using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class RandomScale : MonoBehaviour
{
    private void OnEnable()
    {
        foreach (Transform child in transform)
        {
            Debug.Log("cone");
            child.localScale = new Vector3(Random.Range(0.6f, 1.4f), Random.Range(0.6f, 1.4f), Random.Range(0.6f, 1.4f));
        }
    }
}
