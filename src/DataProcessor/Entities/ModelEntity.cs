using System.ComponentModel.DataAnnotations;

namespace DataProcessor.Entities;

public class ModelEntity
{
    [Key]
    public string ModuleCategoryID { get; set; }
    public string ModuleState { get; set; }
}
