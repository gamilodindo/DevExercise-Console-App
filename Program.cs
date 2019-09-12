using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;
using GetDataFromApi.Models;
using GetDataFromApi.Repository;

namespace GetDataFromApi
{
    class Program
    {
        static void Main(string[] args)
        {
            Start:
            string answer;
            string apiUrl = String.Format("https://iata-and-icao-codes.p.rapidapi.com/airlines");
            string key = "e19036bdddmshec8b9d620c9b4b6p1dfc44jsn5e42aada67de";
            string host = "iata-and-icao-codes.p.rapidapi.com";
            string project = "5cf1da516aca1a303720e78e";

            var apiHeaders = new Dictionary<string, string>();
            apiHeaders.Add("X-RapidAPI-Key", key);
            apiHeaders.Add("X-RapidAPI-Host", host);
            apiHeaders.Add("RapidAPI-Project", project);

            Console.WriteLine("APO Production Unit Software Dev Exercise");
            Console.WriteLine("Developer: Engr.Dindo Gamilo");
            Console.Write("Do you want to update the database ? (Y/N) :");
            answer = Console.ReadLine();

            if(answer.ToUpper() == "Y")
            {
                WebRequest request = WebRequest.Create(apiUrl);
                request.Headers["X-RapidAPI-Key"] = key;
                request.Headers["X-RapidAPI-Host"] = host;
                request.Headers["RapidAPI-Project"] = project;
                request.Method = "GET";
                HttpWebResponse response = null;
                response = (HttpWebResponse)request.GetResponse();

                string result = null;

                using(Stream stream = response.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(stream);
                    result = sr.ReadToEnd();
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    IList<APO> apos = js.Deserialize<List<APO>>(result);

                    int rowCount = 0;
                    Console.WriteLine("Please wait ...");
                    foreach(APO apoList in apos)
                    {
                        if (!GetAPOByIATACode(apoList.iata_code)){
                            rowCount++;
                            bool isSaved = SaveAPO(apoList);
                            //Console.WriteLine("Row num={0}:isSaved?={1}", rowCount, isSaved);
                        }
                    }
                    
                    sr.Close();
                    Console.WriteLine("Finished Updating Database:{0} new records(s) added!!",rowCount);
                    
                }


            }
            else if(answer.ToUpper() == "N")
            {
                Console.WriteLine("You entered no");
            }
            else
            {
                Console.WriteLine("You entered an invalid key");
            }

            Console.WriteLine("=====================================================");

            goto Start;
        }

        public static bool SaveAPO(APO apo)
        {
            try
            {
                
                using (var db = new APOContext())
                {
                    APO model = new APO();
                    model.iata_code = apo.iata_code;
                    model.name = apo.name;
                    model.icao_code = apo.icao_code;
                    db.APOs.Add(model);
                    db.SaveChanges();
                }
                    return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public static bool GetAPOByIATACode(string iataCode)
        {
            var context = new APOContext();
            bool checkIfExists = context.APOs.Any(x => x.iata_code == iataCode);
            return checkIfExists;
        }
    }
}
