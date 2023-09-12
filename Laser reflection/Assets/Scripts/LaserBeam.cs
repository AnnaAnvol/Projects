using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    private LineRenderer laser;
    private List<Vector3> laserIndices = new List<Vector3>();

    public Material material;
    public Vector3 targetPosition;

    private bool reachedTarget = false;

    public void Setup(Vector3 pos, Vector3 dir, Material material)
    {
        GameObject laserObj = new GameObject("Laser Beam");
        laser = laserObj.AddComponent<LineRenderer>();
        laser.startWidth = 0.1f;
        laser.endWidth = 0.1f;
        laser.material = material;

        targetPosition = pos + dir * 100; // Default target position

        CastLaser(pos, dir);
    }

    private void CastLaser(Vector3 pos, Vector3 dir)
    {
        laserIndices.Clear();
        laserIndices.Add(pos);

        RaycastHit hit;
        int maxReflections = 100;
        int reflections = 0;

        while (reflections < maxReflections)
        {
            if (Physics.Raycast(pos, dir, out hit, 100))
            {
                if (hit.collider.CompareTag("Mirror"))
                {
                    pos = hit.point;
                    dir = Vector3.Reflect(dir, hit.normal);
                    laserIndices.Add(pos);
                    reflections++;
                }
                else if (hit.collider.CompareTag("Receiver"))
                {
                    Color sourceColor = Color.white;
                    Color receiverColor = Color.white;

                    LaserController laserController = hit.collider.GetComponent<LaserController>();
                    if (laserController != null)
                    {
                        sourceColor = laserController.sourceColor;
                        receiverColor = laserController.receiverColor;
                    }

                    if (IsColorMatch(sourceColor, receiverColor))
                    {
                        reachedTarget = true;
                        targetPosition = hit.point;
                        EndGame();
                    }
                }
                else
                {
                    laserIndices.Add(hit.point);
                    break;
                }
            }
            else
            {
                pos += dir * 100;
                laserIndices.Add(pos);
                break;
            }
        }
        UpdateLaser();
    }

    private void UpdateLaser()
    {
        laser.positionCount = laserIndices.Count;
        laser.SetPositions(laserIndices.ToArray());

        if (reachedTarget)
        {
            laser.positionCount = laserIndices.Count + 1;
            laser.SetPosition(laserIndices.Count, targetPosition);
        }
    }

    private bool IsColorMatch(Color sourceColor, Color receiverColor)
    {
        return sourceColor.Equals(receiverColor);
    }

    private void EndGame()
    {
        Debug.Log("Level Completed!!!");

        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}