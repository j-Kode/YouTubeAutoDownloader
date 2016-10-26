using Supremes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YoutubeExtractor;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            //var youtubeLinks = File.ReadLines("C:\\Users\\jeff.si\\Documents\\music.txt");

            ////string metascore = doc.DocumentNode.SelectNodes("//*[@id=\"pl-load-more-destination\"]/tr[1]/td[4]/a")[0].InnerText;
            ///*
            // * Get the available video formats.
            // * We'll work with them in the video and audio download examples.
            // */

            //IEnumerable<VideoInfo> videoInfos;

            //foreach (var youtubeLink in youtubeLinks)
            //{
            //    try
            //    {
            //        videoInfos = DownloadUrlResolver.GetDownloadUrls(youtubeLink);
            //    }
            //    catch
            //    {
            //        continue;
            //    }
            //    /*
            //     * Select the first .mp4 video with 360p resolution
            //     */
            //    VideoInfo video = videoInfos
            //        .First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 360);

            //    /*
            //     * If the video has a decrypted signature, decipher it
            //     */
            //    if (video.RequiresDecryption)
            //    {
            //        DownloadUrlResolver.DecryptDownloadUrl(video);
            //    }

            //    /*
            //     * Create the video downloader.
            //     * The first argument is the video to download.
            //     * The second argument is the path to save the video file.
            //     */
            //    string str = video.Title;
            //    Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            //    str = rgx.Replace(str, "");

            //    var videoDownloader = new VideoDownloader(video, Path.Combine("C:\\test\\stuff\\", str + video.VideoExtension));


            //    /*
            //     * Execute the video downloader.
            //     * For GUI applications note, that this method runs synchronously.
            //     */
            //    videoDownloader.Execute();
            //}
            ProcessStartInfo startInfo = new ProcessStartInfo();

            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = @"C:\ffmpeg\bin\ffmpeg.exe";

            var files = Directory.GetFiles("C:\\test\\stuff\\", "*.mp4");

            foreach (var file in files) {
                startInfo.Arguments = "-i \"" + file + "\" -vn -ar 44100 -ac 2 -ab 192K -f mp3 \"C:\\test\\stuffmp3\\"+ Path.GetFileNameWithoutExtension(file) + ".mp3\"";
            startInfo.RedirectStandardOutput = true;
                //startInfo.RedirectStandardError = true;

                Console.WriteLine(string.Format(
                    "Executing \"{0}\" with arguments \"{1}\".\r\n",
                    startInfo.FileName,
                    startInfo.Arguments));

                try
                {
                    using (Process process = Process.Start(startInfo))
                    {
                        while (!process.StandardOutput.EndOfStream)
                        {
                            string line = process.StandardOutput.ReadLine();
                            Console.WriteLine(line);
                        }

                        process.WaitForExit();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Console.ReadKey();
        }
    }
}
