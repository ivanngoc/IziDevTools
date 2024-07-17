namespace СommissioningService.IntegrationTests;

/// <summary>
/// Набор входных объектов для тестирования  
/// </summary>
public class SetForInput
{
    public IEnumerable<object>? Ok { get; set; }
    public IEnumerable<object>? Fail { get; set; }
}