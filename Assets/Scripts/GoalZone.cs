using Assets.Scripts;
using UnityEngine;

public class GoalZone : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag(Tags.Player))
		{
			return;
		}

		GameManager.Instance.PlayerWon();
	}
}
