using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderSpawner : MonoBehaviour, IGridObject
{
    public string Colour { get; set; }

    [SerializeField] private GameObject Boulder;
    GameObject BoulderManager;

    private void Start()
    {        
        if (Colour != "" && Colour != null)
        {
            ColourEventManager.ColourEvents[Colour].OnActivated += BoulderSpawner_OnActivated;
        }

        LevelManager.LevelRestartEvent += LevelManager_LevelRestartEvent;
        SpawnBoulder();
    }

    private void LevelManager_LevelRestartEvent(object sender, System.EventArgs e)
    {
        Activate();
    }

    private void OnDestroy()
    {
        if (Colour != "" && Colour != null && ColourEventManager.ColourEvents != null)
        {
            ColourEventManager.ColourEvents[Colour].OnActivated -= BoulderSpawner_OnActivated;
        }
        LevelManager.LevelRestartEvent -= LevelManager_LevelRestartEvent;
        Destroy(BoulderManager);
        BoulderManager = null;
    }

    private void BoulderSpawner_OnActivated(object sender, System.EventArgs e)
    {
        Activate();
    }

    private void Activate()
    {
        Destroy(BoulderManager);
        BoulderManager = null;
        SpawnBoulder();
    }

    private void SpawnBoulder()
    {
        BoulderManager = Instantiate(Boulder, transform.position + new Vector3(0f, 1f, 0f), new Quaternion()) as GameObject;

        if (Colour != "" && Colour != null)
        {
            GridPlacer.UpdateAllMaterials(BoulderManager, "Colour (Instance)", ColourEventManager.ColourMaterials[Colour]);
        }
    }
}
