using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteAlways, ExecuteInEditMode]
public class RandomRotationGameObjects : MonoBehaviour
{
    private void Start()
    {
        
    }

    private void OnEnable()
    {
        transform.Rotate(new Vector3( 0, 0,Random.Range(0f, 360f)));
    }
}
