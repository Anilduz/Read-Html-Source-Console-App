using System.Net;
using System.Text;
using System.Text.Json;

internal class Program
{
    private static async Task Main(string[] args)
    {
        GetSourceCode("https://www.paperwork.com.tr/category-sitemap.xml");
        //GetSourceCode("https://www.tcmb.gov.tr/kurlar/kurlar_tr.html");
        
        //Console.WriteLine("1");
        //await  CalculateAsync();
        //Console.WriteLine("Hello, World!");

        //Task<int> downloading = DownloadDocsMainPageAsync();
        //Console.WriteLine($"{nameof(Main)}: Launched downloading.");

        //int bytesLoaded = await downloading;
        //Console.WriteLine($"{nameof(Main)}: Downloaded {bytesLoaded} bytes.");
        //Console.WriteLine("2");

        Console.ReadKey();
    }
    static void GetSourceCode(string url)
    {

        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        req.AllowAutoRedirect = true;
        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();


        using (StreamReader sRead = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
        {
            Console.WriteLine(":::" + DateTime.Now.ToString() + " Start GetSourceCode Function! URL is : " + url);

            string res = sRead.ReadToEnd().ToString();


            string[] subs = res.Split('>');
            var sources = "";
            var flag = false;
            var i = 0;
            foreach (string s in subs)
            {
                if (i < 1000)
                {
                    if (s.Contains("<"))
                    {
                        if (s.Contains("https"))
                        {
                            string[] trim = s.Split("<");

                            //

                            string url2 = trim[0];
                            HttpWebRequest req2 = (HttpWebRequest)WebRequest.Create(url2);
                            try
                            {
                                HttpWebResponse resp2 = (HttpWebResponse)req2.GetResponse();
                                using (StreamReader sRead2 = new StreamReader(resp2.GetResponseStream(), Encoding.UTF8))
                                {
                                    //Console.WriteLine("*****************************************************************");
                                    //Console.WriteLine(":::" + DateTime.Now.ToString() + " URL IS : " + "https" + trim[0]);
                                    string res2 = sRead2.ReadToEnd().ToString();

                                    string[] subs2 = res2.Split("<");
                                    string[] meta;

                                    foreach (string k in subs2)
                                    {

                                        if (k.Contains("property"))
                                        {
                                            meta = k.Split("<");
                                            string[] urI = url2.Split("/");
                                            var path = "";
                                            for(var a = 3; a < urI.Length; a++)
                                            {
                                                path += urI[a];
                                                if(a != urI.Length - 1)
                                                {
                                                    path += "/";
                                                }
                                            }
                                            sources += ":" + DateTime.Now.ToString() + " URL is : " + path + " ::::::::: ";
                                            sources += "<"+ meta[0];
                                        }
                                    }
                                    

                                }
                                i++;
                            }
                            catch (Exception e)
                            {
                                break;
                            }
                            
                        }
                    }
                }
            }
            Console.WriteLine(sources);
            WriteNotePad(url, sources.ToString(), @"C:\Users\aduz\Desktop\paperwork site tarama\test.txt");

        }
        Console.WriteLine(":::" + DateTime.Now.ToString() + " Finish GetSourceCode Function! ");
        Console.WriteLine("");
        Console.WriteLine("");

    }

    static void WriteNotePad(string link,string text,string fileName)
    {
        
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        // Create a new file
        using (FileStream fs = File.Create(fileName))
        {
            // Add some text to file
            Byte[] linkCreate = new UTF8Encoding(true).GetBytes(link+"\r\n");
            Byte[] textCreate = new UTF8Encoding(true).GetBytes(text);
            fs.Write(linkCreate, 0, linkCreate.Length);
            fs.Write(textCreate, 0, textCreate.Length);

        }

       
    }
    private static async Task<int> DownloadDocsMainPageAsync()
    {
        Console.WriteLine($"{nameof(DownloadDocsMainPageAsync)}: About to start downloading.");

        using (var client = new HttpClient(new HttpClientHandler()))
        {
            client.Timeout = TimeSpan.FromSeconds(300);
            byte[] content = await client.GetByteArrayAsync("https://www.paperwork.com.tr/");
            Console.WriteLine($"{nameof(DownloadDocsMainPageAsync)}: Finished downloading.");
            return content.Length;
        }

    }

    private static async Task CalculateAsync()
    {

        using (var client = new HttpClient(new HttpClientHandler()))
        {
            client.Timeout = TimeSpan.FromSeconds(300);
            var numbers = new number
            {
                a = 2,
                b = 3
            };
            var company = JsonSerializer.Serialize(numbers);
            var requestContent = new StringContent(company, Encoding.UTF8, "application/json");
            var response = await client.GetAsync("http://www.dneonline.com/calculator.asmx?wsdl");
            response.EnsureSuccessStatusCode();
            Console.WriteLine(response.ToString());
            Console.WriteLine($"{nameof(DownloadDocsMainPageAsync)}: Finished downloading.");
            Console.WriteLine(response.Content.ToString());

        }

    }
}