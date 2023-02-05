using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    [SerializeField] private AI aiPrefab;
    [SerializeField] private Tile tileUnderneath;
    public List<Vector3> aiPath = new List<Vector3>();

    public List<Vector3> GetAIPath()
    {
        List<Vector3> path = new List<Vector3>();
        aiPath.ForEach(x => path.Add(x));
        return path;
    }
    
    public void UpdatePath(Tile targetPosition, float smoothness)
    {
        Queue<Tile> toVisit = new Queue<Tile>();
        toVisit.Enqueue(tileUnderneath);
        Tile current = tileUnderneath;
        List<Tile> aiRoughPath = new List<Tile>();
        while (current != null)
        {
            aiRoughPath.Add(current);
            if (current.GetPosition() == targetPosition.GetPosition())
            {
                break;
            }
            float currentCost = current.AICost;
            List<Tile> neighbours = current.GetNeighboursTile();
            current = null;
            foreach (Tile neighbour in neighbours)
            {
                if (neighbour.AICost < currentCost)
                {
                    current = neighbour;
                    currentCost = current.AICost;
                }
            }

        }

        List<Vector3> newPath = new List<Vector3>();
        if (aiRoughPath.Count < 2)
        {
            foreach (Tile tile in aiRoughPath)
            {
                newPath.Add(tile.transform.position);
            }

            aiPath = newPath;
            return;
        }
        newPath.Add(aiRoughPath[0].transform.position);
        
        for (int j = 0; j < aiRoughPath.Count - 2; j += 2)
        {
            
            Vector3 p1;
            if (newPath.Count <= 0)
            {
                p1 = aiRoughPath[0].transform.position;
            }
            else
            {
                p1 = newPath[^1];
            }
            Vector3 p2 = aiRoughPath[j + 1].transform.position;
            Vector3 p3 = aiRoughPath[j + 2].transform.position;
            for (float t = 0f; t <= 1f; t += 1f/smoothness)
            {
                float tDiff = 1f - t;
                Vector3 b = p1 * Mathf.Pow(tDiff, 2) + p2 * (2f * tDiff * t) + p3 * Mathf.Pow(t, 2);
                newPath.Add(b);
            }
            // newPath.RemoveAt(newPath.Count - 1);
        }

        if (aiRoughPath.Count % 3 == 0)
        {
            newPath.Add(aiRoughPath[^1].transform.position);
            newPath.Add(aiRoughPath[^2].transform.position);
        }
        aiPath = newPath;
    }
    
    public AI SpawnAI(float gameDifficulty)
    {
        AI spawnedAI = Instantiate(aiPrefab, transform.position, Quaternion.identity);
        spawnedAI.UpdateDifficulty(gameDifficulty);
        return spawnedAI;
    }



    public void UpdatePosition(Tile tile)
    {
        tileUnderneath = tile;
        Vector3 spawnerPosition = tileUnderneath.transform.position + new Vector3(0f, 1.5f, 0f);
        transform.position = spawnerPosition;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position, 0.5f);

    }
#endif
}
