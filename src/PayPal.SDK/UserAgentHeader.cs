﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PayPal.Api
{
    /// <summary>
    /// PayPal User-Agent Header implementation class
    /// </summary>
    internal class UserAgentHeader
    {
        /// <summary>
        /// Returns a PayPal specific User-Agent HTTP Header
        /// </summary>
        /// <returns>Dictionary containing User-Agent HTTP Header</returns>
        public static Dictionary<string, string> GetHeader()
        {
            var userAgentDictionary = new Dictionary<string, string>();
            userAgentDictionary.Add(BaseConstants.UserAgentHeader, UserAgentHeader.GetUserAgentHeader());
            return userAgentDictionary;
        }

        /// <summary>
        /// Creates the signature for the UserAgent header.
        /// </summary>
        /// <returns>A string containing the signature for the UserAgent header.</returns>
        private static string GetUserAgentHeader()
        {
            var header = new StringBuilder("PayPalSDK/" + BaseConstants.SdkName + " " + BaseConstants.SdkVersion + " (");
            header.Append(string.Join(";", new string[] 
            {
                FormatUserAgentParameter("lang", "DOTNET"),
                FormatUserAgentParameter("clr", DotNetClrVersion),
                FormatUserAgentParameter("bit", OperatingSystemBitness),
                FormatUserAgentParameter("os", OperatingSystemName)
            }));
            header.Append(")");
            return header.ToString();
        }

        /// <summary>
        /// Formats a parameter name and value to be used in the signature of a UserAgent header.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>A formatted string containing both the parameter name and value.</returns>
        private static string FormatUserAgentParameter(string name, object value)
        {
            return string.Format("{0}={1}", name, value);
        }

        /// <summary>
        /// Returns whether or not the operating system is 64-bit.
        /// </summary>
        /// <returns>True = 64-bit, False = 32-bit</returns>
        private static bool Is64Bit()
        {
            return Environment.Is64BitOperatingSystem;
        }

        /// <summary>
        /// Gets the bitness of the operating system.
        /// </summary>
        private static int OperatingSystemBitness { get { return Is64Bit() ? 64 : 32; } }

        /// <summary>
        /// Gets the name of the operating system.
        /// </summary>
        private static string OperatingSystemName { get { return Environment.OSVersion.ToString(); } }

        /// <summary>
        /// Gets the version of the current .NET common language runtime environment.
        /// </summary>
        private static string DotNetClrVersion { get { return Environment.Version.ToString().Trim(); } }

    }
}
