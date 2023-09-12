using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    public Material material;
    public LaserBeam prefabs;

    private LaserBeam beam;

    private void Start()
    {
        beam = Instantiate(prefabs, gameObject.transform.position, gameObject.transform.rotation);
    }

    private void Update()
    {
        Destroy(GameObject.Find("Laser Beam"));
        beam.Setup(gameObject.transform.position, gameObject.transform.right, material);
    }
}
