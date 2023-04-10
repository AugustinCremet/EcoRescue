using UnityEngine;

public class DisableOtherListeners : MonoBehaviour
{
    void Awake()
    {
        AudioListener[] aL = FindObjectsOfType<AudioListener>();
        foreach (var t in aL)
        {
            //Disable if AudioListener is not on the MainCamera
            if (!t.CompareTag("Player"))
            {
                t.enabled = false;
            }
        }
    }
}
