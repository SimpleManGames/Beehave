using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QueenSteering : MonoBehaviour
{
    public float rotationSpeed = 10f;

    private int currentTileIndex = 302;

    private Vector3 targetPosition;

    private bool reachedTarget;

    private float currentTime;
    private float timeToMove = 0.5f;

    private void Start()
    {
        transform.position = Grid.FindHexObject(currentTileIndex).transform.position;
    }

    private void Update()
    {
        reachedTarget = (Vector3.Distance(targetPosition, transform.position) < .1f);

        if (reachedTarget)
        {
            Hex hex = Grid.FindHexObject(currentTileIndex).hex;
            var neighbours = Hex.Neighbours(hex);

            List<HexObject> availableMovements = new List<HexObject>();

            neighbours.ToList().ForEach(n =>
            {
                HexObject hexObject = Grid.FindHexObject(n.cubeCoords);
                try
                {
                    if (HeatMapInfo.Instance.Field[LayerType.Terrain][hexObject.Index] != float.MinValue)
                        availableMovements.Add(hexObject);
                }
                catch { }
            });

            currentTime += Time.deltaTime;
            if (currentTime >= timeToMove)
            {
                var hexToGo = availableMovements[new System.Random().Next() % availableMovements.Count()];
                targetPosition = hexToGo.transform.position;

                currentTileIndex = hexToGo.Index;
                currentTime = 0;
            }
        }

        Vector3 targetDir = targetPosition - transform.position;
        float step = rotationSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, .5f);
        transform.rotation = Quaternion.LookRotation(newDir);

        transform.position += (targetPosition - transform.position) * Time.deltaTime;
    }
}
