using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Dependancies")]
    public TMPro.TMP_Text ResourcesTextbox;
    public UnityEngine.UI.Button PlayButton;
    public Sprite PlaySprite;
    public Sprite PauseSprite;

    [Space(10)]
    public RadialMenu ColourMenu;
    public RadialMenu BuildingMenu;
    public GridPlacer GridPlacer;

    [Header("Level Design")]
    public Transform EnemyPrefab;
    public int TotalResources;

    [Header("Player Details")]
    public int CurrentResources;

    private bool IsPlaying;
    private bool HasStarted;
    private Transform ParentEnemy;

    // Toggle the play button
    public void OnPlayToggle()
    {
        SetIsPlaying(!IsPlaying);
    }

    // Restart the level
    public void OnRestart()
    {
        HasStarted = false;

        // Destroy all the old enemies
        for (int i = 0; i < ParentEnemy.childCount; ++i)
        {
            Destroy(ParentEnemy.GetChild(i).gameObject);
        }

        // Add a new prefab
        Transform enemy = Instantiate(EnemyPrefab);
        enemy.parent = ParentEnemy;

        // Set default play state the game
        SetIsPlaying(false);
    }

    // Set whether the game is running or not
    public void SetIsPlaying(bool value)
    {
        // One way toggle
        HasStarted = HasStarted || value;

        IsPlaying = value;
        Time.timeScale = IsPlaying ? 1.0f : 0.0f;
        PlayButton.image.sprite = IsPlaying ? PauseSprite : PlaySprite;

        ColourMenu.IsEnabled = !HasStarted;
        BuildingMenu.IsEnabled = !HasStarted;
        GridPlacer.CanBuild = !HasStarted;
    }

    public void Start()
    {
        ParentEnemy = new GameObject().transform;
        ParentEnemy.name = "Enemies";

        OnRestart();
        UpdateResources(TotalResources);

        Grid3D.OnTileAdded += Grid3D_OnTileAdded;
        Grid3D.OnTileRemoved += Grid3D_OnTileRemoved;
    }

    private void UpdateResources(int value)
    {
        CurrentResources = value;
        ResourcesTextbox.text = "Resources: " + CurrentResources;
    }

    private void Grid3D_OnTileRemoved(object sender, BlockArgs e)
    {
        UpdateResources(CurrentResources + e.ObjectData.Cost);
    }

    private void Grid3D_OnTileAdded(object sender, BlockArgs e)
    {
        UpdateResources(CurrentResources - e.ObjectData.Cost);
    }
}
