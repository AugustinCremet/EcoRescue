using System.Collections.Generic;
using UnityEngine;

public class FadeTree : MonoBehaviour
{
    private Material[] _materialsInTree;
    [SerializeField] private List<Material> _transparentMaterials;
    [SerializeField] private Material[] _originalMaterials;
    //[SerializeField] private int _foxSpace = 100;
    private MeshCollider _collider;
    [SerializeField] private GameObject _focus;
    [SerializeField] private GameObject _cube;

    private void Start()
    {
        _collider = GetComponent<MeshCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _materialsInTree = other.GetComponent<Renderer>().materials;
        Material[] materials = other.GetComponent<Renderer>().materials;
        
        for (int i = 0; i < _materialsInTree.Length; i++)
        {
            Material m = _materialsInTree[i];
            if (m.name.IndexOf("TFF_Atlas_1A_D") != -1)
            {
                materials[i] = new (_transparentMaterials[i]);
            }
            else
            {
                materials[i] = new (_transparentMaterials[i]);
            }
        }

        other.GetComponent<Renderer>().materials = materials;
    }

    private void OnTriggerStay(Collider other)
    {
        Camera cam = Camera.main;
        _materialsInTree = other.GetComponent<Renderer>().materials;
        Material[] materials = other.GetComponent<Renderer>().materials;
        
        float distToFoxSpace = Mathf.Abs(Screen.width / 2 - cam.WorldToScreenPoint(other.transform.position).x) - 350f;
        //Debug.Log("distToFoxSpace " + distToFoxSpace);
        distToFoxSpace = distToFoxSpace < 20 ? 20 : distToFoxSpace;
        float opacity = distToFoxSpace / 310;

        for (int i = 0; i < _materialsInTree.Length; i++)
        {
            Material m = _materialsInTree[i];
            if (m.name.IndexOf("TFF_Atlas_1A_D") != -1)
            {
                materials[i].SetColor("_Color", new Color(1, 1, 1, opacity));
            }
            else
            {
                materials[i].SetColor("_Color", new Color(1, 1, 1, 0.5f * opacity));
            }
        }

        other.GetComponent<Renderer>().materials = materials;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<Renderer>().materials.Length > 1)
            other.GetComponent<Renderer>().materials = _originalMaterials;
        else
        {
            Material[] mat = new Material[1];
            if (_originalMaterials[0].name.IndexOf("TFF_Atlas_1A_D") != -1)
                mat[0] = _originalMaterials[0];
            else
                mat[0] = _originalMaterials[1];
            
            other.GetComponent<Renderer>().materials = mat;
        }
    }
}