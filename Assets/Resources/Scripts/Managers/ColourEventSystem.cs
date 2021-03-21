using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourEventSystem : MonoBehaviour
{
    public struct ColourEvent
    {
        // Is the colour currently active
        public bool IsActive;

        // Called whenever a trigger is sent
        public EventHandler OnActivated;
        public EventHandler OnDeactivated;

        // These are not called when for example triggerActivated is called when it is already active
        public EventHandler OnChangedActivation;
        public EventHandler OnChangedDeactivation;

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

    public static Dictionary<string, ColourEvent> ColourEvents;

    // Start is called before the first frame update
    void Start()
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
    }
}
