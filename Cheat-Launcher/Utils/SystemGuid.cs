using System.Text;
using System.Security.Cryptography;
using System.Management;


namespace Cheat_Launcher.Utils;

public class SystemGuid
{
    private static string _systemGuid = string.Empty;

    public static string Value()
    {
        if (!string.IsNullOrEmpty(_systemGuid)) return _systemGuid;

        var lCpuId = GetCpuId();
        var lBiosId = GetBiosId();
        var lMainboard = GetMainboardId();
        var lGpuId = GetGpuId();
        var lMac = GetMac();
        var lConcatStr = $"CPU: {lCpuId}\nBIOS:{lBiosId}\nMainboard: {lMainboard}\nGPU: {lGpuId}\nMAC: {lMac}";
        _systemGuid = GetHash(lConcatStr);

        return _systemGuid;
    }

    private static string GetHash(string s)
    {
        try
        {
            var lUtf8 = MD5.HashData(Encoding.UTF8.GetBytes(s));
            return new Guid(lUtf8).ToString().ToUpper();
        }
        catch (Exception lEx)
        {
            return lEx.Message;
        }
    }

    private static string GetIdentifier(string pWmiClass, List<string> pProperties)
    {
        var lResult = string.Empty;
        try
        {
            foreach (var o in new ManagementClass(pWmiClass).GetInstances())
            {
                var lItem = (ManagementObject)o;
                foreach (var lProperty in pProperties)
                {
                    try
                    {
                        switch (lProperty)
                        {
                            case "MACAddress":
                                if (string.IsNullOrWhiteSpace(lResult) == false)
                                    return lResult; //Return just the first MAC

                                if (lItem["IPEnabled"].ToString() != "True")
                                    continue;
                                break;
                        }

                        var lItemProperty = lItem[lProperty];
                        if (lItemProperty == null)
                            continue;

                        var lValue = lItemProperty.ToString();
                        if (string.IsNullOrWhiteSpace(lValue) == false)
                            lResult += $"{lValue}; ";
                    }
                    catch
                    {
                        // ignore
                    }
                }
            }
        }
        catch
        {
            // ignore
        }

        return lResult.TrimEnd(' ', ';');
    }

    private static readonly List<string> ListOfCpuProperties = new List<string>
        { "UniqueId", "ProcessorId", "Name", "Manufacturer" };

    private static string GetCpuId()
    {
        return GetIdentifier("Win32_Processor", ListOfCpuProperties);
    }

    private static readonly List<string> ListOfBiosProperties =
        ["Manufacturer", "SMBIOSBIOSVersion", "IdentificationCode", "SerialNumber", "ReleaseDate", "Version"];

    private static string GetBiosId()
    {
        return GetIdentifier("Win32_BIOS", ListOfBiosProperties);
    }

    private static readonly List<string> ListOfMainboardProperties =
        ["Model", "Manufacturer", "Name", "SerialNumber"];

    private static string GetMainboardId()
    {
        return GetIdentifier("Win32_BaseBoard", ListOfMainboardProperties);
    }

    private static readonly List<string> ListOfGpuProperties = ["Name"];

    private static string GetGpuId()
    {
        return GetIdentifier("Win32_VideoController", ListOfGpuProperties);
    }

    private static readonly List<string> ListOfNetworkProperties = ["MACAddress"];

    private static string GetMac()
    {
        return GetIdentifier("Win32_NetworkAdapterConfiguration", ListOfNetworkProperties);
    }
}