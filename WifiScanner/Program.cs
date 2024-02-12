using NativeWifi;

namespace WifiScanner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WlanClient client = new WlanClient();

            foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
            {
                Console.WriteLine($"Interface Name: {wlanIface.InterfaceName}");
                Console.WriteLine($"Description: {wlanIface.InterfaceDescription}");
                Console.WriteLine($"Radio state: {wlanIface.InterfaceState}");
                Console.WriteLine();

                Wlan.WlanAvailableNetwork[] networks = wlanIface.GetAvailableNetworkList(0);

                foreach (Wlan.WlanAvailableNetwork network in networks)
                {
                    string ssid = GetStringForSSID(network.dot11Ssid);
                    string authentication = GetAuthenticationAlgorithm(network.dot11DefaultAuthAlgorithm);
                    string encryption = GetEncryptionType(network.dot11DefaultCipherAlgorithm);

                    Console.WriteLine($"SSID: {ssid}");
                    Console.WriteLine($"Signal strength: {network.wlanSignalQuality}%");
                    Console.WriteLine($"Connectable: {network.networkConnectable}");
                    Console.WriteLine($"Profile name: {network.profileName}");
                    Console.WriteLine($"Authentication: {authentication}");
                    Console.WriteLine($"Encryption: {encryption}");

                 
                    Wlan.WlanBssEntry[] bssEntries = wlanIface.GetNetworkBssList(network.dot11Ssid, Wlan.Dot11BssType.Any, true);
                    if (bssEntries.Length > 0)
                    {
                        Wlan.WlanBssEntry bssEntry = bssEntries[0];
                        Console.WriteLine($"BSSID: {BitConverter.ToString(bssEntry.dot11Bssid).Replace("-", ":")}");
                        Console.WriteLine($"Channel: {bssEntry.chCenterFrequency / 1000}"); 
                        Console.WriteLine($"Infrastructure mode: {bssEntry.dot11BssType}");
                    }

                    Console.WriteLine();
                }
            }

            Console.ReadLine();
        }

        static string GetStringForSSID(Wlan.Dot11Ssid ssid)
        {
            return System.Text.Encoding.ASCII.GetString(ssid.SSID, 0, (int)ssid.SSIDLength);
        }

        static string GetAuthenticationAlgorithm(Wlan.Dot11AuthAlgorithm authAlgorithm)
        {
            return authAlgorithm.ToString();
        }

        static string GetEncryptionType(Wlan.Dot11CipherAlgorithm cipherAlgorithm)
        {
            return cipherAlgorithm.ToString();
        }
    }
}

