// Copyright (c) 2014 SIL International
// This software is licensed under the MIT License (http://opensource.org/licenses/MIT)
// Parts based on code by MJ Hutchinson http://mjhutchinson.com/journal/2010/01/25/integrating_gtk_application_mac
// Parts based on code by bugsnag-dotnet (https://github.com/bugsnag/bugsnag-dotnet/blob/v1.4/src/Bugsnag/Diagnostics.cs)
// adapted from libpalaso (https://github.com/sillsdev/libpalaso)

using System;
using System.Runtime.InteropServices;

namespace TestGeckofx60
{
	public static class Platform
	{
		private static bool? _isMono;

		public static bool IsUnix => Environment.OSVersion.Platform == PlatformID.Unix;

		public static bool IsMono
		{
			get
			{
				if (_isMono == null)
					_isMono = Type.GetType("Mono.Runtime") != null;
				return (bool)_isMono;
			}
		}
		public static bool IsDotNet => !IsMono;

		private static readonly string UnixNameMac = "Darwin";
		private static readonly string UnixNameLinux = "Linux";

		public static bool IsLinux => IsUnix && UnixName == UnixNameLinux;
		public static bool IsMac => IsUnix && UnixName == UnixNameMac;
		public static bool IsWindows => !IsUnix;

		public static bool IsDotNetCore => false;
		public static bool IsDotNetFramework => IsDotNet;

		[DllImport("libc")]
		private static extern int uname(IntPtr buf);
		private static string _unixName;
		private static string UnixName
		{
			get
			{
				if (!IsUnix)
					return null;
				if (_unixName == null)
				{
					IntPtr buf = IntPtr.Zero;
					try
					{
						buf = Marshal.AllocHGlobal(8192);
						// This is a hacktastic way of getting sysname from uname ()
						if (uname(buf) == 0)
							_unixName = Marshal.PtrToStringAnsi(buf);
					}
					catch
					{
						_unixName = String.Empty;
					}
					finally
					{
						if (buf != IntPtr.Zero)
							Marshal.FreeHGlobal(buf);
					}
				}

				return _unixName;
			}
		}
	}
}
