namespace Serious;

public record DomId(string Value)
{
    public override string ToString() => Value;

    public static implicit operator string(DomId value) => value.Value;

    public DomId WithSuffix(string suffix) => new($"{Value}-{suffix}");
}
