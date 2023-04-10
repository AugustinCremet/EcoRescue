using UnityEngine;

public class GodModeTrigger : MonoBehaviour
{
    public void TriggerGodMode()
    {
        UIManager _uiManager = FindObjectOfType<UIManager>();
        _uiManager.GodMode = name == "ONButton";
        
        Player player = FindObjectOfType<Player>();
        if (player == null)
            return;

        player.TriggerGodMode(name == "ONButton");
    }
}
