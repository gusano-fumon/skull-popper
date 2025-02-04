public interface ISlider
{
    public string StringFormat { get; set; }
    public SliderType SliderType { get; set; }
    public void UpdateValue(float value);
    public void Load(float value);
    public void SaveChanges();
}