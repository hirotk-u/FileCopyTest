using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;

namespace FileCopyTest
{
    internal class Program
    {
        private static readonly string SRC_DIR = "srcDir";
        private static readonly string DEST_DIR = "destDir";

        /// <summary>
        /// Main
        /// </summary>
        static void Main()
        {
            try
            {
                Console.WriteLine("指定した日付以降のファイルをコピーします");
                Console.WriteLine("日付を指定してください(yyyyMMdd)");
                string inputDay = Console.ReadLine();

                // コピー元とコピー先のディレクトリを指定
                string srcDir = ConfigurationManager.AppSettings[SRC_DIR];
                string destDir = ConfigurationManager.AppSettings[DEST_DIR];

                //入力日付変換
                CultureInfo jaJP = new CultureInfo("ja-JP");
                DateTime targetDate;
                bool parseResult = DateTime.TryParseExact(inputDay, "yyyyMMdd", jaJP, DateTimeStyles.AssumeLocal, out targetDate);

                if (!parseResult)
                {
                    throw new Exception("日付フォーマットが正しくありません");
                }

                // コピー元ディレクトリ内のファイルとサブフォルダを再帰的に取得
                CopyFilesRecursively(srcDir, destDir, targetDate);

                Console.WriteLine("ファイルコピー完了！");
            }
            catch (Exception e)
            {
                Console.WriteLine($"エラー発生[{e.Message}]");
            }

            Console.ReadKey();
        }

        /// <summary>
        /// CopyFilesRecursively
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destinationPath"></param>
        /// <param name="targetDate"></param>
        static void CopyFilesRecursively(string sourcePath, string destinationPath, DateTime targetDate)
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

                // ファイルの更新日を取得
                DateTime updDate = File.GetLastWriteTime(file);

                if (targetDate <= updDate)
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
                CopyFilesRecursively(directory, destDirectory, targetDate);
            }
        }
    }
}