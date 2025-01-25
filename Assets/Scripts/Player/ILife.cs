public interface ILife
{
	public int Health { get; set;}

	public void TakeDamage(int damage);
	public void RestoreHealth(int health);
}