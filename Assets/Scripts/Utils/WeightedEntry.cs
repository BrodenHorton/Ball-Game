public class WeightedEntry<T> {
    private T value;
    private float weight;

    public WeightedEntry(T value, float weight) {
        this.value = value;
        this.weight = weight;
    }

    public T Value => value;

    public float Weight => weight;
}
