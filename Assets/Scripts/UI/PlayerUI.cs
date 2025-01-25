using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerUI : MonoBehaviour
{
	public Slider healthSlider;
	public TMP_Text healthText;

	public void UpdateHealth(int health)
	{
		healthSlider.value = health;
		if (health <= 0)
		{
			healthText.text = "DEAD";
			return;
		}
		healthText.text = health.ToString();
	}
}
