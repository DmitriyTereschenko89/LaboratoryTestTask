using System.ComponentModel.DataAnnotations;

namespace DataProcessor.Entities;

public class DeviceStatusEntity
{
    [Key]
    public string ModuleCategoryID { get; set; }
    public string ModuleState { get; set; }
}
