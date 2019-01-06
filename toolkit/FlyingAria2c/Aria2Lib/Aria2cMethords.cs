using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlyingAria2c.Aria2Lib
{
    /// <summary>
    /// Aria2 Rpc接口的方法库
    /// </summary>
    internal static class Aria2Methords
    {
        private static string Base64Encode(string Path)
        {
            try
            {
                return Convert.ToBase64String(System.IO.File.ReadAllBytes(Path));
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// 新建一个任务
        /// </summary>
        /// <param name="Uri">下载链接的地址</param>
        /// <returns></returns>
        public static async Task<string> AddUri(JsonRpcConnection Connection, string Uri)
        {
            string[] Uris = { Uri };
            object[] Params = { "token:" + Connection.Token, Uris };
            string Gid = (await Connection.JsonRpcAsync<JsonRpcConnection.Response<string>>("aria2.addUri", Params)).Result;
            return Gid;
        }

        /// <summary>
        /// 新建一个任务，包含多个源
        /// </summary>
        /// <param name="Uris">这个包含多个下载链接的数组指向同一个文件</param>
        /// <returns></returns>
        public static async Task<string> AddUri(JsonRpcConnection Connection, string[] Uris)
        {
            object[] Params = { "token:" + Connection.Token, Uris };
            string Gid = (await Connection.JsonRpcAsync<JsonRpcConnection.Response<string>>("aria2.addUri", Params)).Result;
            return Gid;
        }

        /// <summary>
        /// 添加种子
        /// </summary>
        /// <param name="Path">种子文件在计算机上的位置</param>
        /// <returns></returns>
        public static async Task<string> AddTorrent(JsonRpcConnection Connection, string Path)
        {
            string TorrentBase64 = Base64Encode(Path);
            object[] Params = { "token:" + Connection.Token, TorrentBase64 };
            string Gid = (await Connection.JsonRpcAsync<JsonRpcConnection.Response<string>>("aria2.addTorrent", Params)).Result;
            return Gid;
        }

        /// <summary>
        /// 添加MetaLink
        /// </summary>
        /// <param name="Path">MetaLink文件在计算机上的位置</param>
        /// <returns></returns>
        public static async Task<string> AddMetalink(JsonRpcConnection Connection, string Path)
        {
            string MetalinkBase64 = Base64Encode(Path);
            object[] Params = { "token:" + Connection.Token, MetalinkBase64 };
            string Gid = (await Connection.JsonRpcAsync<JsonRpcConnection.Response<string>>("aria2.addMetalink", Params)).Result;
            return Gid;
        }

        public static async Task<string> Remove(JsonRpcConnection Connection, string Gid)
        {
            object[] Params = { "token:" + Connection.Token, Gid };
            string Result = (await Connection.JsonRpcAsync<JsonRpcConnection.Response<string>>("aria2.remove", Params)).Result;
            return Result;
        }

        public static async Task<string> Pause(JsonRpcConnection Connection, string Gid)
        {
            object[] Params = { "token:" + Connection.Token, Gid };
            string Result = (await Connection.JsonRpcAsync<JsonRpcConnection.Response<string>>("aria2.pause", Params)).Result;
            return Result;
        }

        public static async Task PauseAll(JsonRpcConnection Connection)
        {
            object[] Params = { "token:" + Connection.Token };
            await Connection.JsonRpcAsync<JsonRpcConnection.Response<string>>("aria2.pauseAll", Params);
        }

        public static async Task<string> UpPause(JsonRpcConnection Connection, string Gid)
        {
            object[] Params = { "token:" + Connection.Token, Gid };
            string Result = (await Connection.JsonRpcAsync<JsonRpcConnection.Response<string>>("aria2.unpause", Params)).Result;
            return Result;
        }

        public static async Task UpPauseAll(JsonRpcConnection Connection)
        {
            object[] Params = { "token:" + Connection.Token };
            await Connection.JsonRpcAsync<JsonRpcConnection.Response<string>>("aria2.unpauseAll", Params);
        }

        public static async Task<Dictionary<string, string>> TellStatus(JsonRpcConnection Connection, string Gid)
        {
            string[] Keys = new string[] { "status", "totalLength", "completedLength", "downloadSpeed", "gid" };
            object[] Params = { "token:" + Connection.Token, Gid, Keys };
            JsonRpcConnection.Response<Dictionary<string, string>> Result = await Connection.JsonRpcAsync<JsonRpcConnection.Response<Dictionary<string, string>>>("aria2.tellStatus", Params);
            return Result.Result;
        }

        public static async Task<Dictionary<string, string>> TellStatus(JsonRpcConnection Connection, string Gid, string[] Keys)
        {
            object[] Params = { "token:" + Connection.Token, Gid, Keys };
            JsonRpcConnection.Response<Dictionary<string, string>> Result = await Connection.JsonRpcAsync<JsonRpcConnection.Response<Dictionary<string, string>>>("aria2.tellStatus", Params);
            return Result.Result;
        }

        public static async Task<Dictionary<string, string>> TellActive(JsonRpcConnection Connection)
        {
            string[] Keys = new string[] { "status", "totalLength", "completedLength", "downloadSpeed", "gid" };
            object[] Params = { "token:" + Connection.Token, Keys };
            JsonRpcConnection.Response<Dictionary<string, string>> Result = await Connection.JsonRpcAsync<JsonRpcConnection.Response<Dictionary<string, string>>>("aria2.tellActive", Params);
            return Result.Result;
        }

        public static async Task<Dictionary<string, string>> TellWaiting(JsonRpcConnection Connection)
        {
            string[] Keys = new string[] { "status", "totalLength", "completedLength", "downloadSpeed", "gid" };
            object[] Params = { "token:" + Connection.Token, Keys };
            JsonRpcConnection.Response<Dictionary<string, string>> Result = await Connection.JsonRpcAsync<JsonRpcConnection.Response<Dictionary<string, string>>>("aria2.tellWaiting", Params);
            return Result.Result;
        }

        /// <summary>
        /// 查询已停止的任务
        /// </summary>
        /// <returns>返回最近50个结果</returns>
        public static async Task<Dictionary<string, string>> TellStopped(JsonRpcConnection Connection)
        {
            string[] Keys = new string[] { "status", "totalLength", "completedLength", "downloadSpeed", "gid" };
            object[] Params = { "token:" + Connection.Token, 0, 50, Keys };
            JsonRpcConnection.Response<Dictionary<string, string>> Result = await Connection.JsonRpcAsync<JsonRpcConnection.Response<Dictionary<string, string>>>("aria2.tellStopped", Params);
            return Result.Result;
        }

        public static async Task<File[]> GetFiles(JsonRpcConnection Connection, string Gid)
        {
            object[] Params = { "token:" + Connection.Token, Gid };
            JsonRpcConnection.Response<File[]> Result = await Connection.JsonRpcAsync<JsonRpcConnection.Response<File[]>>("aria2.getFiles", Params);
            return Result.Result;
        }

        //public static async Task<T> GetGlobalStat<T>(JsonRpcConnection Connection)
        //{
        //    object[] Params = { "token:" + Connection.Token };
        //    JsonRpcConnection.Response<T> Result = await Connection.JsonRpcAsync<JsonRpcConnection.Response<T>>("aria2.getGlobalStat", Params);
        //    return Result.Result;
        //}

        /// <summary>
        /// 关闭Aria2C，调用Aria2自己的方法
        /// </summary>
        public static void ShutDown(JsonRpcConnection Connection)
        {
            object[] Params = { "token:" + Connection.Token };
            Connection.JsonRpcWithoutRes("aria2.shutdown", Params);
        }
    }
}
