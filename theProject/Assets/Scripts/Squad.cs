using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour
{
	private Entity.Team ownTeam;
	public Entity.Team GetOwnTeam() { return ownTeam; }
	public void SetOwnTeam(Entity.Team newTeam) {ownTeam = newTeam; }
	#region Orders
	public abstract class Order // generic order (to keep in queue)
	{
		public virtual void Execute(Squad squad) // "translate" the order to the soldier
		{// depending on implementation, for example call soldier's method to execute/plan this task
			Debug.LogWarning($"Generic order execution not overriden\n All soldiers received generic order");
		}
    }

	public class MovementOrder : Order // example how to add new types of orders
	{
		public MovementOrder(Vector2Int movementTargetCoords)
		{
			targetCoords = movementTargetCoords;
		}
		public readonly Vector2Int targetCoords;
		public override void Execute(Squad squad)
		{// here we would set soldier's target position for example
			Debug.Log(squad);
			Dictionary<Entity, Vector2Int> targetCoordsDictionary = squad.
				GetFormation().
				CalculatePositions(
				targetCoords);
			foreach (Soldier soldier in targetCoordsDictionary.Keys)
			{
				Vector2Int targetPositionForSoldier = targetCoordsDictionary[soldier];
				soldier.HandleMovementOrder(targetPositionForSoldier);
				Debug.Log($"Soldier {soldier.name} received movement order towards coordinates {targetPositionForSoldier.x},{targetPositionForSoldier.y}");
			}
		}
	}
	#endregion
	[SerializeField] private Formation formation;
	public Formation GetFormation() { return formation; }
	[SerializeField] private List<Entity> soldiers = new List<Entity>(); // soldiers belonging to the squad
	public List<Entity> GetSoldiers() { return soldiers; }
    private Queue<Order> orders = new Queue<Order>(); // orders given to the squad

	public void AddSoldierToSquad(Entity soldier)
	{
		soldiers.Add(soldier);
		soldier.OnDeath.AddListener(RemoveSoldierFromSquad);
	}

	public void RemoveSoldierFromSquad(Entity soldier)
	{
		soldiers.Remove(soldier);
		soldier.OnDeath.RemoveListener(RemoveSoldierFromSquad);
	}

	private void Awake()
	{
		TickSystem.OnTick += HandleTick;
		formation = GetComponent<Formation>();
	}

	private void OnDestroy()
	{
		TickSystem.OnTick -= HandleTick;
	}

	private void HandleTick(TickSystem.OnTickEventArgs eventArgs)
	{// pass a single order to all soldiers
		if (orders.Count < 1)
			return; // for now nothing to do here

		Order currentOrder = orders.Dequeue();
		Debug.Log($"Passing order {currentOrder.ToString()} on tick #{eventArgs.tickNumber}");
		currentOrder.Execute(this);
	}

	public void EnqueueOrder(Order orderToEnqueue)
    {
		orders.Enqueue(orderToEnqueue);
    }
}
