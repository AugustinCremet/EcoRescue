using UnityEngine;
using System.Collections.Generic;
using System; 

[System.Serializable]
public struct EnemyType
{
    public GameObject enemyPrefab;
    public int quantity;
}

public class EnemyGroup : MonoBehaviour
{
    [SerializeField] public List<EnemyType> _enemies = new List<EnemyType>();
    private List<EnemySM> _activeEnemies = new List<EnemySM>();
    private int nbOfActiveEnemies;
    private bool _groupIsHostile = false;

    private void InitializeGroup(Dictionary<string,object> message)
    {
        foreach (EnemyType enemy in _enemies)
        {
            for (int i = 0; i < enemy.quantity; i++)
            {
                GameObject e = Instantiate(enemy.enemyPrefab, this.transform);
                e.GetComponent<NeutralBehaviour>().InitEnemy();
                _activeEnemies.Add(e.GetComponent<EnemySM>());      

                nbOfActiveEnemies++;
                EventManager.TriggerEvent(Events.ENEMY_SPAWNED, null);

                GameManager.instance.ActiveDungeonManager.AddActiveEnemy(e);
            }
        }
    }

    public void WarnGroup(Transform player)
    {
        foreach (var sm in _activeEnemies)
        {
            if (!sm.GetComponent<HostileBehaviour>().enabled)
                sm.ActivateHostileBehaviour(player);
        }
    }

    private void OnEnable()
    {
        EventManager.StartListening(Events.INITIALIZE_ENEMIES, InitializeGroup);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.INITIALIZE_ENEMIES, InitializeGroup);
    }

    #region PROPRETIES
    public bool GroupIsHostile
    {
        get { return _groupIsHostile; }
        set { _groupIsHostile = value; }
    }

    public int NbOfActiveEnemies
    {
        get { return nbOfActiveEnemies; }
        set { nbOfActiveEnemies = value; }
    }
    #endregion 
}