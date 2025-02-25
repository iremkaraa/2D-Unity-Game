using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Transform layerTransform;
        public float speed;
        [HideInInspector] public float resetPositionX;
        [HideInInspector] public Vector3 startPosition;
    }

    [SerializeField] private List<ParallaxLayer> parallaxLayers;

    private void Start()
    {
        foreach (var layer in parallaxLayers)
        {
            InitializeLayer(layer);
        }
    }

    private void Update()
    {
        foreach (var layer in parallaxLayers)
        {
            MoveLayer(layer);
        }
    }

    private void MoveLayer(ParallaxLayer layer)
    {
        // Move the layer to the left
        layer.layerTransform.position += Vector3.left * (layer.speed * Time.deltaTime);

        // Check if the layer has reached the reset point
        if (layer.layerTransform.position.x <= layer.startPosition.x + layer.resetPositionX)
        {
            // Reset the layer to its original position
            layer.layerTransform.position = layer.startPosition;
        }
    }

    private void InitializeLayer(ParallaxLayer layer)
    {
        // Store the starting position of the layer
        layer.startPosition = layer.layerTransform.position;

        // Get the first sprite's width to determine reset position
        SpriteRenderer firstSpriteRenderer = layer.layerTransform.GetChild(0).GetComponent<SpriteRenderer>();
        if (firstSpriteRenderer != null)
        {
            layer.resetPositionX = -firstSpriteRenderer.bounds.size.x;
        }
    }
}