﻿namespace Hexa.NET.ImPlot
{
    using System;
    using System.Runtime.InteropServices;
#if !NET5_0_OR_GREATER
    using HexaGen.Runtime;
#endif
    public static class LibraryLoader
    {
        public static nint LoadLibrary()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return LoadLocalLibrary("cimplot");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return LoadLocalLibrary("cimplot");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return LoadLocalLibrary("cimplot");
            }
            else
            {
                return LoadLocalLibrary("cimplot");
            }
        }

        public static string GetExtension()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return ".dll";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return ".dylib";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return ".so";
            }

            return ".so";
        }

        public static IntPtr LoadLocalLibrary(string libraryName)
        {
            var extension = GetExtension();

            if (!libraryName.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
            {
                libraryName += extension;
            }

            var osPlatform = GetOSPlatform();
            var architecture = GetArchitecture();

            var libraryPath = GetNativeAssemblyPath(osPlatform, architecture, libraryName);

            static string GetOSPlatform()
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return "win";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return "linux";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    return "osx";
                }

                throw new ArgumentException("Unsupported OS platform.");
            }

            static string GetArchitecture()
            {
                return RuntimeInformation.ProcessArchitecture switch
                {
                    Architecture.X86 => "x86",
                    Architecture.X64 => "x64",
                    Architecture.Arm => "arm",
                    Architecture.Arm64 => "arm64",
                    _ => throw new ArgumentException("Unsupported architecture."),
                };
            }

            static string GetNativeAssemblyPath(string osPlatform, string architecture, string libraryName)
            {
                var assemblyLocation = AppContext.BaseDirectory;
                if (string.IsNullOrEmpty(assemblyLocation))
                {
                    assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    assemblyLocation = Path.GetDirectoryName(assemblyLocation);
                    
                    if (string.IsNullOrEmpty(assemblyLocation))
                    {
                        throw new InvalidOperationException("Unable to determine the location of the executing assembly.");
                    }
                }
                Console.WriteLine($"Assembly location: {assemblyLocation}");
                
                var paths = new[]
                {
                    Path.Combine(assemblyLocation, libraryName),
                    Path.Combine(assemblyLocation, "runtimes", osPlatform, "native", libraryName),
                    Path.Combine(assemblyLocation, "runtimes", $"{osPlatform}-{architecture}", "native", libraryName),
                };

                foreach (var path in paths)
                {
                    if (File.Exists(path))
                    {
                        return path;
                    }
                }

                return libraryName;
            }

            IntPtr handle;

            Console.WriteLine($"Loading library '{libraryPath}'.");
            handle = NativeLibrary.Load(libraryPath);

            if (handle == IntPtr.Zero)
            {
                throw new DllNotFoundException($"Unable to load library '{libraryName}'.");
            }

            return handle;
        }
    }
}