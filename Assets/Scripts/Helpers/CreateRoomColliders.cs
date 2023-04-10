using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class CreateRoomColliders : MonoBehaviour
{
    private Terrain _terrain;

    private void Awake()
    {
        _terrain = GetComponent<Terrain>();
        
        for(int i = 0; i < _terrain.terrainData.alphamapTextureCount; i++)
        {
            Texture2D texture = _terrain.terrainData.alphamapTextures[i];
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes("Alpha" + i +".png", bytes);
        }

        for (int i = 0; i < _terrain.terrainData.terrainLayers.Length; i++)
        {
            Texture2D texture = _terrain.terrainData.terrainLayers[i].normalMapTexture;
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes("Layer" + i +".png", bytes);
        }
    }
}
