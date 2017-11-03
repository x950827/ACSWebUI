using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace ACSWebUI.Common.Functions.Generator {
    public static class UniqueIdGenerator {
        private static ManagementObjectSearcher Processor => new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
        private static ManagementObjectSearcher Os => new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM CIM_OperatingSystem");

        public static string GetId() {
            return GetSystemName().Aggregate("", (current, r) => current + r).Replace("-", string.Empty);
        }

        private static IEnumerable<string> GetSystemName() {
            var result = new List<string>();

            try {
                result.AddRange(from ManagementBaseObject s in Processor.Get() select s["ProcessorId"].ToString().Trim());
                result.AddRange(from ManagementBaseObject s in Os.Get() select s["SerialNumber"].ToString().Trim());
            }
            catch (Exception e) {
                throw new ArgumentException(e.Message);
            }
            return result;
        }
    }
}