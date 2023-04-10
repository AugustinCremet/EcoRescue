using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RotateObject : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.Rotate(new Vector3(0,2,0));
    }
}
