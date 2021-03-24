using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenuBuildingAddon : MonoBehaviour
{
    public List<Sprite> SegmentIcons;

    private RadialMenu ParentMenu;
    private List<SpriteRenderer> Sprites;

    // Start is called before the first frame update
    void Start()
    {
        ParentMenu = GetComponent<RadialMenu>();
        Sprites = new List<SpriteRenderer>();

        for (int i = 0; i < Mathf.Min(SegmentIcons.Count, transform.childCount); ++i)
        {
            // Create a new icon object
            Transform icon = new GameObject().transform;
            icon.parent = transform.GetChild(i);
            icon.name = SegmentIcons[i].name;
            icon.gameObject.layer = LayerMask.NameToLayer("UI");

            // Update transform
            icon.localScale *= 3f;
            icon.localEulerAngles = new Vector3(90.0f, -icon.parent.localEulerAngles.y, 0.0f);
            icon.localPosition = new Vector3(0.5f * (ParentMenu.OuterRadius - ParentMenu.InnerRadius) + ParentMenu.InnerRadius, 1.0f, 0.0f);

            // Add icon sprite to object
            SpriteRenderer sprite = icon.gameObject.AddComponent<SpriteRenderer>();
            sprite.sprite = SegmentIcons[i];

            // Add so the alpha can update
            Sprites.Add(sprite);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (SpriteRenderer sprite in Sprites)
        {
            sprite.color = new Color(1.0f, 1.0f, 1.0f, ParentMenu.Alpha);
        }
    }
}
