﻿using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LeagueSharp;

namespace Jhin___The_Virtuoso
{
    class VersionCheck
    {
        public static void UpdateCheck()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    using (var c = new WebClient())
                    {
                        var rawVersion = c.DownloadString("https://raw.githubusercontent.com/HikigayaAss/LeagueSharp/master/Jhin%20-%20The%20Virtuoso/Properties/AssemblyInfo.cs");
                        var match = new Regex(@"\[assembly\: AssemblyVersion\(""(\d{1,})\.(\d{1,})\.(\d{1,})\.(\d{1,})""\)\]").Match(rawVersion);

                        if (match.Success)
                        {
                            var gitVersion = new Version(string.Format("{0}.{1}.{2}.{3}", match.Groups[1], match.Groups[2], match.Groups[3], match.Groups[4]));

                            if (gitVersion != typeof(Program).Assembly.GetName().Version)
                            {     
                                Game.PrintChat("<font color='#FF99CC'><b>鑰佺帇瀹氬埗-绱呴牁鐕间辅 娉ㄥ叆鎴愬姛</b></font><font color='#FF3300'><b>鑰佺帇VIP鑵虫湰浜ゆ祦缇や辅348902882</b></font>");
                            }
                            else
                            {
                                Game.PrintChat("Also try <font color='#66FF33'><b>鑰佺帇瀹氬埗</b></font> for a gamebreaking experience!");
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }
    }
}