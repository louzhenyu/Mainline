using JetermClient.BLL;
using JetermEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetermEntity.Response;
namespace JetermClient.Test
{
    public class EtermRemoteTest
    {
        public static void Main1(string[] args)
        //public static void Main(string[] args)
        {
            EtermClient client = new EtermClient();
            string str = string.Empty;
            try
            {
                client.Invoke(100001, "http://114.80.79.158:8084/HelloWorld.rem", "RTR/JQF0B7", EtermCommand.ServerSource.EtermRemote, TimeSpan.FromSeconds(5));
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }                
            CommandResult<JetermEntity.Response.SeekPNR> response = new JetermEntity.Parser.SeekPNR().ParseCmdResult(str);
            Console.ReadLine();
        }
    }
}
