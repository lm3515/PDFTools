/*************************************************************************************
 * CLR版本：       4.0.30319.42000
 * 类 名 称：      Program
 * 机器名称：      9GX1UOWROPIAEJ4
 * 命名空间：      PDFTools
 * 文 件 名：      Program
 * 创建时间：      2021/9/11 11:40:53
 * 作    者：      Richard Liu
 * 说   明：       应用程序主入口
 * 修改时间：      2021/7/21 11:40:53
 * 修 改 人：      Richard Liu
*************************************************************************************/

using PDFLibNet32;
using System;
using System.IO;

namespace PDFTools
{
    class Program
    {
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ExceptionObject.ToString());
            // 有此句则不弹“xxx已停止工作”异常对话框
            Environment.Exit(-1); 
        }

        static void Main(string[] args)
        {
            // 显示软件版本信息
            Version();

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            // 输入文件路径
            string inputFile = null;
            inputFile = args[0];

            // 文件名不包含后缀
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(inputFile);

            // 创建一个PDF对象
            PDFWrapper pdfWrapper = new PDFWrapper(fileNameWithoutExtension + ".pdf");
            pdfWrapper.LoadPDF(inputFile);

            // 获取文件所在目录
            string outFile = Path.GetDirectoryName(inputFile) + fileNameWithoutExtension;
            // 创建文件夹
            if (!Directory.Exists(outFile))
            {
                Directory.CreateDirectory(outFile);
            }

            // 按照页数来转
            for (int i = 1; i <= pdfWrapper.PageCount; i++)
            {
                // 这里可以设置输出图片的页数、大小和图片质量
                string pagePath = outFile + "\\" + i.ToString() + ".jpg";
                pdfWrapper.ExportJpg(pagePath, i, i, 180, 80);
                if (pdfWrapper.IsJpgBusy) {
                    System.Threading.Thread.Sleep(100); 
                }
            }

            pdfWrapper.Dispose();
        }

        private static void Version()
        {
            Console.WriteLine(@"PDFTools - PDF to image");
            Console.WriteLine(@"Copyright (c) 2021 Xiamen iLeadTek Technology Co., Ltd");
            Console.WriteLine(@"Copyright (c) 2021 Richard Liu");
            Console.WriteLine(@"Version：1.0." + DateTime.Now.ToString("yyyyMMdd"));
        }
    }
}
