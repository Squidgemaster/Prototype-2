using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourEvent
{
    // Is the colour currently active
    public bool IsActive;

    // Called whenever a trigger is sent
    public event EventHandler OnActivated;
    public event EventHandler OnDeactivated;

    // These are not called when for example triggerActivated is called when it is already active
    public event EventHandler OnChangedActivation;
    public event EventHandler OnChangedDeactivation;

    // Trigger activation events
    public void TriggerActivated()
    {
        OnActivated?.Invoke(this, EventArgs.Empty);

        if (!IsActive)
        {
            IsActive = true;
            OnChangedActivation?.Invoke(this, EventArgs.Empty);
        }
    }

    // Trigger deactivation events
    public void TriggerDeactivated()
    {
        OnDeactivated?.Invoke(this, EventArgs.Empty);

        if (IsActive)
        {
            IsActive = false;
            OnChangedDeactivation?.Invoke(this, EventArgs.Empty);
        }
    }
}

public class ColourEventManager : MonoBehaviour
{
    public static Dictionary<string, Material> ColourMaterials;
    public static Dictionary<string, ColourEvent> ColourEvents;

    // Start is called before the first frame update
    void Awake()
    {
        // Make sure the events has no been already populated
        if (ColourEvents != null) { Debug.LogError("Multiple event systems within the same scene"); }

        // Populate colour events
        ColourEvents = new Dictionary<string, ColourEvent>();
        ColourEvents.Add("Red", new ColourEvent());
        ColourEvents.Add("Purple", new ColourEvent());
        ColourEvents.Add("Blue", new ColourEvent());
        ColourEvents.Add("Light Blue", new ColourEvent());
        ColourEvents.Add("Green", new ColourEvent());

        ColourMaterials = new Dictionary<string, Material>();
        Shader universalShader = Shader.Find("Universal Render Pipeline/Lit");

        // Red
        Material redMaterial = new Material(universalShader);
        redMaterial.color = new Color(255.0f / 255.0f, 10.0f / 255.0f, 0.0f / 255.0f);
        ColourMaterials.Add("Red", redMaterial);

        // Purple
        Material purpleMaterial = new Material(universalShader);
        purpleMaterial.color = new Color(190.0f / 255.0f, 0.0f / 255.0f, 255.0f / 255.0f);
        ColourMaterials.Add("Purple", purpleMaterial);

        // Blue
        Material blueMaterial = new Material(universalShader);
        blueMaterial.color = new Color(0.0f / 255.0f, 9.0f / 255.0f, 255.0f / 255.0f);
        ColourMaterials.Add("Blue", blueMaterial);

        // Light Blue
        Material lightBlueMaterial = new Material(universalShader);
        lightBlueMaterial.color = new Color(0.0f / 255.0f, 255.0f / 255.0f, 194.0f / 255.0f);
        ColourMaterials.Add("Light Blue", lightBlueMaterial);

        // Green
        Material greenMaterial = new Material(universalShader);
        greenMaterial.color = new Color(7.0f / 255.0f, 255.0f / 255.0f, 0.0f / 255.0f);
        ColourMaterials.Add("Green", greenMaterial);
    }
}
