namespace IziHardGames.Libs.Methods.Tester;

internal class SomeClass
{
    public float PropFloat { get; set; }
    public string? PropString { get; set; }
    public SomeClassNested? SomeClassNested { get; set; }
    public DateTime DateTime { get; set; }
    public SomeCustomStruct SomeCustomStruct { get; set; }
}

internal class SomeClassNested
{
    public int PropInt { get; set; }
    public double PropDouble { get; set; }
    public string? PropString { get; set; }
}

internal struct SomeCustomStruct
{
    public SomeClassNested? SomeClassNested { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime? DateTimeNullable { get; set; }
    public string? PropString { get; set; }
    public long PropLong { get; set; }
}