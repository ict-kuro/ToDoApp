using System.IO;
using System.Text.RegularExpressions;

namespace Microsoft.AspNetCore.Hosting
{
    //wwwroot以下のファイルを取得するための拡張メソッド
    public static class HostEnvironmentExtension
    {
        /// <summary>
        /// ファイルの相対パスを物理パス区切りに変換して ContentRootPath と連結して返します
        /// </summary>
        /// <param name="env">IWebHostEnvironment を拡張します</param>
        /// <param name="filePath">ファイルの相対パス</param>
        /// <returns></returns>
        public static string MapContentRootPath(this IWebHostEnvironment env, string filePath)
        {
            var path = GetPhysicalPath(filePath);
            var result = Path.Combine(env.ContentRootPath, path);
            return result;
        }

        /// <summary>
        /// ファイルの相対パスを物理パス区切りに変換して WebRootPath と連結して返します
        /// </summary>
        /// <param name="env">IWebHostEnvironment を拡張します</param>
        /// <param name="filePath">ファイルの相対パス</param>
        /// <returns></returns>
        public static string MapWebRootPath(this IWebHostEnvironment env, string filePath)
        {
            var path = GetPhysicalPath(filePath);
            var result = Path.Combine(env.WebRootPath, path);
            return result;
        }

        /// <summary>
        /// 仮想ファイルパスを物理ファイルパスに変換します
        /// </summary>
        /// <param name="virtualPath">仮想ファイルパス</param>
        /// <returns></returns>
        private static string GetPhysicalPath(string virtualPath)
        {
            var result = Regex.Replace(virtualPath, @"^~/|^/", string.Empty).Replace("/", @"\");
            return result;
        }

    }
}
