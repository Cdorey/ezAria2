using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FlyingAria2c.Aria2Lib;

namespace FlyingAria2c
{
    internal class Downloader
    {
        internal static Aria2cProgressHandle ProgressHandle;

        internal static JsonRpcConnection RpcConnection;

        static Downloader()
        {
            ProgressHandle = new Aria2cProgressHandle(@"aria2c.exe", Path.GetTempPath());
            RpcConnection = new JsonRpcConnection(ProgressHandle);
        }
    }
}
