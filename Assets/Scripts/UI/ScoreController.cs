
using UnityEngine;

using Cysharp.Threading.Tasks;


public class ScoreController : MonoBehaviour
{
	private static int _score;

	public static void Init()
	{
		_score = 0;

		UniTask.Void(async () =>
		{
			while (GameMenu.inGame)
			{
				RemoveScore(1);
				await UniTask.Delay(1000);
			}
		});
	}

	public static void AddScore(int score)
	{
		_score += score;
	}

	public static void RemoveScore(int score)
	{
		_score -= score;
		if (_score < 0) _score = 0;
	}
}