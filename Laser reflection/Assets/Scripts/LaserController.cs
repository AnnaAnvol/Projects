using UnityEngine;

public class LaserController : MonoBehaviour
{
    public Material laserMaterial;
    public Color sourceColor;
    public Color receiverColor;

    private LaserBeam laserBeam;

    private void Start()
    {
        laserBeam = gameObject.AddComponent<LaserBeam>();
        laserBeam.material = laserMaterial;
        laserBeam.Setup(transform.position, transform.forward, laserMaterial);
    }

    private void Update()
    {
        Renderer sourceRenderer = GetComponent<Renderer>();
        if (sourceRenderer != null && sourceRenderer.material.color != sourceColor)
        {
            sourceRenderer.material.color = sourceColor;
        }
    }
}