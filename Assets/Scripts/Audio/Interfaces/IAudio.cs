public interface IAudio<T>
{
	public T Play(AudioType type, bool loop = true);
	public T Destroy(float time);
}