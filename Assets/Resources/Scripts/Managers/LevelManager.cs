using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;


public class LevelManager : MonoBehaviour
{
    [Header("Dependancies")]
    public GameObject WinWindow;
    public NavMeshSurface LevelNavMesh;

    [Space(10)]
    public TMPro.TMP_Text ResourcesTextbox;
    public TMPro.TMP_Text ScoreTextbox;
    public TMPro.TMP_Text TargetScoreTextbox;

    [Space(10)]
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
    public int TargetScore;

    [Header("Player Details")]
    public int CurrentResources;
    public int CurrentScore;

    [Header("Script Use")]
    public bool IsPlaying;
    public bool HasStarted;

    private Transform ParentEnemy;

    // Score colours
    private readonly Color HasNotReachedGoal = Color.white;
    private readonly Color HasReachedGoal = Color.green;
    private readonly Color HasNotStarted = Color.gray;

    public static event EventHandler LevelRestartEvent;

    // Toggle the play button
    public void OnPlayToggle()
    {
        SetIsPlaying(!IsPlaying);
    }

    // Restart the level
    public void OnRestart()
    {
        WinWindow.SetActive(false);
        ScoreTextbox.color = HasNotStarted;
        HasStarted = false;

        // Destroy all the old enemies
        for (int i = 0; i < ParentEnemy.childCount; ++i)
        {
            Destroy(ParentEnemy.GetChild(i).gameObject);
        }

        // Add a new prefab
        Transform enemy = Instantiate(EnemyPrefab);
        enemy.parent = ParentEnemy;

        LevelRestartEvent?.Invoke(this, EventArgs.Empty);

        // Set default play state the game
        SetIsPlaying(false);
    }

    // Set whether the game is running or not
    public void SetIsPlaying(bool value)
    {
        //Bake NavMesh
        if (value == true)
        LevelNavMesh.BuildNavMesh();

        // Reset score to 0
        UpdateScore(!HasStarted && value ? 0 : CurrentScore);
        
        // One way toggle
        HasStarted = HasStarted || value;

        // Set store text colour
        ScoreTextbox.color = HasStarted ? (CurrentScore >= TargetScore ? HasReachedGoal : HasNotReachedGoal) : HasNotStarted;

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
        UpdateTargetScore(TargetScore);

        Grid3D.OnTileAdded += Grid3D_OnTileAdded;
        Grid3D.OnTileRemoved += Grid3D_OnTileRemoved;
    }

    public void AddScore(int value)
    {
        UpdateScore(CurrentScore + value);
    }

    private void UpdateTargetScore(int value)
    {
        TargetScore = value;
        TargetScoreTextbox.text = "Target:" + TargetScore;
    }

    private void UpdateScore(int value)
    {
        CurrentScore = value;
        ScoreTextbox.text = CurrentScore.ToString();
        WinWindow.SetActive(CurrentScore >= TargetScore && HasStarted);
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
