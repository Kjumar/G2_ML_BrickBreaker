using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    [SerializeField] GameObject brickPrefab;
    [SerializeField] int horizontalCells = 10;
    [SerializeField] int verticalCells = 5;
    [SerializeField] Vector2 cellDimensions = new Vector2(2, 0.5f);
    [SerializeField] Vector2 topLeft = new Vector2(-10, -4);

    GameObject[] bricks;

    private void Start()
    {
        bricks = new GameObject[horizontalCells * verticalCells];
    }

    public void SetBricks()
    {
        for (int i = 0; i < bricks.Length; i++)
        {
            Destroy(bricks[i]);
        }

        for (int y = 0; y < verticalCells; y++)
        {
            for (int x = 0; x < horizontalCells; x++)
            {
                GameObject newBrick = Instantiate(brickPrefab);
                newBrick.transform.position = new Vector3(topLeft.x + x * cellDimensions.x,
                    topLeft.y + y * cellDimensions.y, 0);
                newBrick.transform.parent = this.transform;
            }
        }
    }

    public int BricksRemaining()
    {
        int bricksLeft = 0;

        for (int i = 0; i < bricks.Length; i++)
        {
            if (bricks[i] != null)
            {
                bricksLeft++;
            }
        }

        return bricksLeft;
    }

    public float GetReward()
    {
        int bricksLeft = 0;

        for (int i = 0; i < bricks.Length; i++)
        {
            if (bricks[i] != null)
            {
                bricksLeft++;
            }
        }

        return (float)bricksLeft / bricks.Length;
    }
}
