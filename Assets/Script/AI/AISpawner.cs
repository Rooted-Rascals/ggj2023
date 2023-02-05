using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Script.Decorators.Enemies;
using Script.Decorators.Plants;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class AISpawner : MonoBehaviour
{
    [SerializeField] private Tile tileUnderneath;
    public List<Vector3> aiPath = new List<Vector3>();
    private Dictionary<EnemiesType, AI> AICache;

    public List<Vector3> GetAIPath()
    {
        List<Vector3> path = new List<Vector3>();
        aiPath.ForEach(x => path.Add(x));
        return path;
    }

    public void Awake()
    {
        GenerateAICache();
    }

    public void GenerateAICache()
    {
        if (AICache is not null) 
            return;
        
        AICache = new Dictionary<EnemiesType, AI>();
        
        //We load all prefabs in the Buildings folder.
        foreach (Object aiPrefab in Resources.LoadAll("Enemies"))
        {
            AI decorator = aiPrefab.GetComponent<AI>();
            AICache.Add(decorator.EnemiesType, decorator);
            Debug.Log($"Found {decorator.EnemiesType.ToString()}");
        }
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
        Array aiTypes = Enum.GetValues(typeof(EnemiesType));
        EnemiesType aiSelectedType = (EnemiesType) aiTypes.GetValue(Random.Range(0, aiTypes.Length));
        AI spawnedAI = Instantiate(AICache[aiSelectedType], transform.position, Quaternion.identity);
        spawnedAI.UpdateDifficulty(gameDifficulty);
        return spawnedAI;
    }



    public void UpdatePosition(Tile tile)
    {
        tileUnderneath = tile;
        Vector3 spawnerPosition = new Vector3(tileUnderneath.transform.position.x, 1f, tileUnderneath.transform.position.z);
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
