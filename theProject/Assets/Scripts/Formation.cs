using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Formation : MonoBehaviour
{
        [SerializeField] Squad squad;

    void Awake()
    {
        squad = GetComponent<Squad>();
    }

    public Dictionary<Entity, Vector2Int> CalculatePositions(Vector2Int coordinates)
    {
        
        List<Entity> soldiers = new List<Entity>(squad.GetSoldiers());
        Dictionary<Entity, Vector2Int> soldiersNewCoordinates = new Dictionary<Entity, Vector2Int>();
        bool isEven = false;


        if(soldiers.Count % 2 == 0)
        {
            isEven = true;
            float shortestDistance = Mathf.Infinity; 
            Entity nearestSoldier = null;
            Vector2Int newCoordinates = new Vector2Int(coordinates.x, coordinates.y);
            foreach (Entity Entity in soldiers)
            {
                float distanceToTile = Vector2.Distance(Entity.GetTileCoord(), newCoordinates);
                if (distanceToTile < shortestDistance) 
                {
                    shortestDistance = distanceToTile;
                    nearestSoldier = Entity; 
                }
            }
            if (nearestSoldier != null) 
            {
                soldiersNewCoordinates.Add(nearestSoldier, newCoordinates);
                soldiers.Remove(nearestSoldier);
            }
        } 
        int numberOfSoldiers = soldiers.Count;
        /* numberOfSoldiers = 4
        i = 4 / 2
        i = 2
        soldier.Count = 2
        i = 3 
        i = 3 / 2 = 1
        sol
        */
        if(isEven)
        {
            for(int i = numberOfSoldiers; i >= 0; i-=2)
            {
                soldiersNewCoordinates = CalculateNewCoordinates(soldiersNewCoordinates, soldiers, coordinates, i);
            }
        }else{
            for(int i = numberOfSoldiers / 2; i >= 0; i--)
            {
                soldiersNewCoordinates = CalculateNewCoordinates(soldiersNewCoordinates, soldiers, coordinates, i);
            }
        }
        return soldiersNewCoordinates;
    }
    private Dictionary<Entity, Vector2Int> CalculateNewCoordinates(Dictionary<Entity, Vector2Int> soldiersNewCoordinates, List<Entity> soldiers, Vector2Int coordinates, int i)
    {
        float shortestDistance = Mathf.Infinity; 
        Entity nearestSoldier = null;
        Vector2Int newCoordinates = new Vector2Int(coordinates.x + i, coordinates.y);
        foreach (Entity Entity in soldiers)
        {
            float distanceToTile = Vector2.Distance(Entity.GetTileCoord(), newCoordinates);
            if (distanceToTile < shortestDistance) 
            {
                shortestDistance = distanceToTile;
                nearestSoldier = Entity; 
            }
        }
        if (nearestSoldier != null) 
        {
            soldiersNewCoordinates.Add(nearestSoldier, newCoordinates);
            soldiers.Remove(nearestSoldier);
        }
        shortestDistance = Mathf.Infinity; 
        nearestSoldier = null;
        newCoordinates = new Vector2Int(coordinates.x - i, coordinates.y);
        foreach (Entity Entity in soldiers)
        {
            float distanceToTile = Vector2.Distance(Entity.GetTileCoord(), newCoordinates);
            if (distanceToTile < shortestDistance) 
            {
                shortestDistance = distanceToTile;
                nearestSoldier = Entity; 
            }
        }
        if (nearestSoldier != null) 
        {
            soldiersNewCoordinates.Add(nearestSoldier, newCoordinates);
            soldiers.Remove(nearestSoldier);
        }
        return soldiersNewCoordinates;
    }

    // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/in-parameter-modifier
    private Vector2Int CalculateSoldierCoordinates(int soldierNumber, in Vector2Int coordinates)
    {
        // Horizontal line we change x
        if(soldierNumber % 2 == 1) soldierNumber = -1 * soldierNumber;
        TilemapManager.TileState tileState = TilemapManager.GetTileState(coordinates.x + soldierNumber, coordinates.y);
        if ( tileState == TilemapManager.TileState.free)
        {
            Vector2Int soldierCoordinates = new Vector2Int(coordinates.x + soldierNumber,  coordinates.y);
            return soldierCoordinates;
        } else if (tileState == TilemapManager.TileState.taken)
        {
            Vector2Int soldierCoordinates = new Vector2Int(coordinates.x,  coordinates.y);
            return soldierCoordinates;
        } else 
        {
            Vector2Int soldierCoordinates = new Vector2Int(coordinates.x,  coordinates.y);
            return soldierCoordinates;
        }
    }



}
