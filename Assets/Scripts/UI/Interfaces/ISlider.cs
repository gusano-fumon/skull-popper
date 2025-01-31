

public interface ISlider
{
    public SliderType SliderType { get; set; }
    public float DefaultValue { get; set; }
    public float CurrentValue { get; set; }
    public void SaveChanges();
    public void UpdateValue(float value);
    public void Load();
    public void ResetToDefault();
}