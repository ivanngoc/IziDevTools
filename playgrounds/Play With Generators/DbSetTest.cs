#pragma warning  disable
using System.Collections;
using System.Security.Policy;
using Microsoft.EntityFrameworkCore;
using IziHardGames.Libs.ForJson;

namespace TestPackage;

internal class DbSetTest : DbContext
{
    public DbSet<MainModel> Models { get; set; }
    public DbSet<NestedModel0> Nested0 { get; set; }
    public DbSet<NestedModel1> Nested1 { get; set; }
    public DbSet<NestedModel2> Nested2 { get; set; }
}

internal class MainModel
{
    public Guid Id { get; set; }
    public string? ValueAsStringNull { get; set; }
    public string ValueAsString { get; set; } = string.Empty;
    public long ValueAsLong { get; set; }

    public NestedModel0? NestedModel0Navigation { get; set; }
    public MainModel? MainModelSelf { get; set; }


    // revserse proxies 
    // public ICollection<NestedModel2> NestedModel2s { get; set; }
    // public ICollection<NestedModel3> NestedModel3s { get; set; }
}

internal class NestedModel0
{
    public Guid NestedModel0Id { get; set; }
    public NestedModel1 NestedModel1Navigation { get; set; }

    [SelectPropertyJsonConverter(typeof(PropSelectJsonConverter<MainModel, string>), nameof(MainModelReverse.Id))]
    public MainModel? MainModelReverse { get; set; }
}

internal class NestedModel1
{
    public Guid NestedModel1Id { get; set; }
    public float ValueAsFloat { get; set; }
    public float? ValueAsIntNull { get; set; }


    public NestedModel2? NestedModel2NullNavigation { get; set; }
    public NestedModel2 NestedModel2RequiredNavigation { get; set; }
    public ICollection<NestedModel3> NestedModel3Navigations { get; }
}

internal class NestedModel2
{
    public Guid NestedModel2Id { get; set; }
    public string? ValueAsString { get; set; }
    public DateTime ValueAsDateTime { get; set; }
    public DateTime? ValueAsDateTimeNull { get; set; }
    public ICollection<NestedModel3> NestedModel3 { get; }
}

internal class NestedModel3
{
    public Guid Id { get; set; }
}