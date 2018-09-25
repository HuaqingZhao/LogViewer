using System;
using System.IO;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;

namespace LogViewer.Utilities
{
    public class ZipHelper
    {
        private static OpenFileDialog openFileDialog1;
        public static void UnZip()
        {
            openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Un Zip file";
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter = "(.zip)|*.zip";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    foreach (var fileName in openFileDialog1.FileNames)
                    {
                        var folder = Directory.GetParent(fileName).FullName;
                        var tempFolder = folder + "\\temp";
                        var captureConsoleLogFile = tempFolder + "\\CaptureConsoleLogs.zip";
                        UnZip(fileName, tempFolder, string.Empty);

                        if (File.Exists(captureConsoleLogFile))
                        {
                            UnZip(captureConsoleLogFile, tempFolder, string.Empty);
                        }

                        foreach (var item in Directory.GetFiles(tempFolder))
                        {
                            if (Path.GetExtension(item).Equals(".log"))
                            {
                                File.Copy(item, Path.Combine(folder, Path.GetFileName(item)), true);
                            }
                        }

                        if (Directory.Exists(tempFolder))
                        {
                            Directory.Delete(tempFolder, true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("UnZip file(s) Failed! " + ex.Message);
                }
            }
        }

        /// <summary>   
        /// 解压功能(解压压缩文件到指定目录)   
        /// </summary>   
        /// <param name="fileToUnZip">待解压的文件</param>   
        /// <param name="zipedFolder">指定解压目标目录</param>   
        /// <param name="password">密码</param>   
        /// <returns>解压结果</returns>   
        public static bool UnZip(string fileToUnZip, string zipedFolder, string password)
        {
            bool result = true;
            ZipInputStream zipStream = null;
            ZipEntry ent = null;
            string fileName;

            if (!File.Exists(fileToUnZip))
                return false;

            if (!Directory.Exists(zipedFolder))
                Directory.CreateDirectory(zipedFolder);

            try
            {
                using (FileStream fls = File.OpenRead(fileToUnZip))
                {
                    zipStream = new ZipInputStream(fls);

                    if (!string.IsNullOrEmpty(password)) zipStream.Password = password;
                    while ((ent = zipStream.GetNextEntry()) != null)
                    {
                        if (!string.IsNullOrEmpty(ent.Name))
                        {
                            fileName = Path.Combine(zipedFolder, ent.Name);
                            fileName = fileName.Replace('/', '\\');//change by Mr.HopeGi   

                            if (fileName.EndsWith("\\"))
                            {
                                Directory.CreateDirectory(fileName);
                                continue;
                            }

                            using (FileStream fs = File.Create(fileName))
                            {
                                int size = 2048000;
                                byte[] data = new byte[size];
                                while (true)
                                {
                                    size = zipStream.Read(data, 0, data.Length);
                                    if (size > 0)
                                        fs.Write(data, 0, size);
                                    else
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                result = false;
                throw;
            }
            finally
            {
                if (zipStream != null)
                {
                    zipStream.Close();
                    zipStream.Dispose();
                }
                if (ent != null)
                {
                    ent = null;
                }
                GC.Collect();
                GC.Collect(1);
            }
            return result;
        }
    }
}
