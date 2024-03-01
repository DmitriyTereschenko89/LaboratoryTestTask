using EventBus.Messages.Enums;
using EventBus.Messages.Models;
using System.Xml;

namespace HostedServiceXmlParser.Services;

public class CreateClass : ICreateClass
{
    private RapidControlStatus GetClass(string className) => className switch
    {
        "CombinedSamplerStatus" => new CombinedSamplerStatus(),
        "CombinedPumpStatus" => new CombinedPumpStatus(),
        "CombinedOvenStatus" => new CombinedOvenStatus(),
        _ => null,
    };


    public RapidControlStatus GetClass(string className, XmlNodeList nodeList)
    {
        var randomModuleStatus = new Random();
        var rapidControlStatus = GetClass(className);
        rapidControlStatus.ModuleState = (ModuleState)randomModuleStatus.Next(3);
        rapidControlStatus.IsBusy = Convert.ToBoolean(nodeList[1].InnerText);
        rapidControlStatus.IsReady = Convert.ToBoolean(nodeList[2].InnerText);
        rapidControlStatus.IsError = Convert.ToBoolean(nodeList[3].InnerText);
        rapidControlStatus.KeyLock = Convert.ToBoolean(nodeList[4].InnerText);
        switch (className)
        {
            case "CombinedSamplerStatus":
                ((CombinedSamplerStatus)rapidControlStatus).Status = Convert.ToInt32(nodeList[5].InnerText);
                ((CombinedSamplerStatus)rapidControlStatus).Vial = Convert.ToString(nodeList[6].InnerText);
                ((CombinedSamplerStatus)rapidControlStatus).Volume = Convert.ToInt32(nodeList[7].InnerText);
                ((CombinedSamplerStatus)rapidControlStatus).MaximumInjectionVolume = Convert.ToInt32(nodeList[8].InnerText);
                ((CombinedSamplerStatus)rapidControlStatus).RackL = Convert.ToString(nodeList[9].InnerText);
                ((CombinedSamplerStatus)rapidControlStatus).RackR = Convert.ToString(nodeList[10].InnerText);
                ((CombinedSamplerStatus)rapidControlStatus).RackInf = Convert.ToInt32(nodeList[11].InnerText);
                ((CombinedSamplerStatus)rapidControlStatus).Buzzer = Convert.ToBoolean(nodeList[12].InnerText);
                break;
            case "CombinedPumpStatus":
                ((CombinedPumpStatus)rapidControlStatus).Mode = Convert.ToString(nodeList[5].InnerText);
                ((CombinedPumpStatus)rapidControlStatus).Flow = Convert.ToInt32(nodeList[6].InnerText);
                ((CombinedPumpStatus)rapidControlStatus).PercentB = Convert.ToInt32(nodeList[7].InnerText);
                ((CombinedPumpStatus)rapidControlStatus).PercentC = Convert.ToInt32(nodeList[8].InnerText);
                ((CombinedPumpStatus)rapidControlStatus).PercentD = Convert.ToInt32(nodeList[9].InnerText);
                ((CombinedPumpStatus)rapidControlStatus).MinimumPressureLimit = Convert.ToInt32(nodeList[10].InnerText);
                ((CombinedPumpStatus)rapidControlStatus).MaximumPressureLimit = Convert.ToDouble(nodeList[11].InnerText);
                ((CombinedPumpStatus)rapidControlStatus).Pressure = Convert.ToInt32(nodeList[12].InnerText);
                ((CombinedPumpStatus)rapidControlStatus).PumpOn = Convert.ToBoolean(nodeList[13].InnerText);
                ((CombinedPumpStatus)rapidControlStatus).Channel = Convert.ToInt32(nodeList[14].InnerText);
                break;
            case "CombinedOvenStatus":
                ((CombinedOvenStatus)rapidControlStatus).UseTemperatureControl = Convert.ToBoolean(nodeList[5].InnerText);
                ((CombinedOvenStatus)rapidControlStatus).OvenOn = Convert.ToBoolean(nodeList[6].InnerText);
                ((CombinedOvenStatus)rapidControlStatus).Temperature_Actual = Convert.ToDouble(nodeList[7].InnerText);
                ((CombinedOvenStatus)rapidControlStatus).Temperature_Room = Convert.ToDouble(nodeList[8].InnerText);
                ((CombinedOvenStatus)rapidControlStatus).MaximumTemperatureLimit = Convert.ToInt32(nodeList[9].InnerText);
                ((CombinedOvenStatus)rapidControlStatus).Valve_Position = Convert.ToInt32(nodeList[10].InnerText);
                ((CombinedOvenStatus)rapidControlStatus).Valve_Rotations = Convert.ToInt32(nodeList[11].InnerText);
                ((CombinedOvenStatus)rapidControlStatus).Buzzer = Convert.ToBoolean(nodeList[12].InnerText);
                break;
        }
        return rapidControlStatus;
    }
}
