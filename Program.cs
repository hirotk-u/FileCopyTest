using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCopyTest
{
    internal class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        static void Main()
        {
            try
            {
                // 今日の日付を取得
                string today = DateTime.Now.ToString("yyyyMMdd");

                // コピー元とコピー先のディレクトリを指定
                string sourceDirectory = @"..\..\TEST\SrcFolder";
                string destinationDirectory = @"..\..\TEST\DestFolder";

                // コピー元ディレクトリ内のファイルとサブフォルダを再帰的に取得
                CopyFilesRecursively(sourceDirectory, destinationDirectory, today);

                Console.WriteLine("ファイルコピー完了！");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine($"エラー発生[{e.Message}]");
            }
        }

        /// <summary>
        /// CopyFilesRecursively
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destinationPath"></param>
        /// <param name="today"></param>
        static void CopyFilesRecursively(string sourcePath, string destinationPath, string today)
        {
            // コピー先ディレクトリが存在しない場合は作成
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            // コピー元ディレクトリ内のファイルを取得
            foreach (string file in Directory.GetFiles(sourcePath))
            {
                // ファイル名を取得
                string fileName = Path.GetFileName(file);

                // ファイルの作成日を取得
                DateTime creationDate = File.GetLastWriteTime(file);

                // ファイルの作成日が今日の日付と一致する場合、コピーする
                if (creationDate.ToString("yyyyMMdd") == today)
                {
                    string destFile = Path.Combine(destinationPath, fileName);
                    File.Copy(file, destFile, true);
                    Console.WriteLine($"Copied: {fileName}");
                }
            }

            // コピー元ディレクトリ内のサブフォルダを再帰的に処理
            foreach (string directory in Directory.GetDirectories(sourcePath))
            {
                string destDirectory = Path.Combine(destinationPath, Path.GetFileName(directory));
                CopyFilesRecursively(directory, destDirectory, today);
            }
        }
    }
}