using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using static System.Console;





namespace Quick1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_File_Click(object sender, EventArgs e)
        {


            //MakeJson(ref requestBody);
            TranslaterText();
 
            //MakeJSON();
        }

 

        static void TranslaterText()
        {            
            string host = "https://api.cognitive.microsofttranslator.com";  //グローバルサイトの指定
            // string route = "/translate?api-version=3.0&to=de&to=it";  //ドイツ語とイタリア語
            //string route = "/translate?api-version=3.0&to=en";             //To English
            string route = "/translate?api-version=3.0&from=ja&to=en";             //From Japanese To English
            string subscriptionKey = "dea1b00c495e466591053385f8d1955d";



            //、翻訳対象のテキストを含む JSON オブジェクトを作成してシリアル化する必要があります。
            //body 配列には複数のオブジェクトを渡すことができる点に注目してください。

            //System.Object[] body = new System.Object[] { new { Text = @"Hello world!" }, new { Text = @"This is my World" }};　を複数文章を登録するように修正


            int counter = 0;
            string line;

            string Japanese_txt = "";
            

            //日本語テキストを指定する。

            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Japanese_txt = ofd.FileName;
            }
           

            //行数を配列の要素数にする。ほかに方法はないのか？？
            object[] body = new object[File.ReadAllLines(Japanese_txt).Length];

            //textファイルの読み込み
            StreamReader sr = new StreamReader(Japanese_txt, Encoding.GetEncoding("Shift_jis"));

            //body 部分に日本語データを追記。
            while ((line = sr.ReadLine()) != null)
            {                
                body[counter] = new {text= line};
                counter++;
            }

            sr.Close();

            //bodyをシリアライズ          
            var requestBody = JsonConvert.SerializeObject(body);


            /***
            // Stream Test
            byte[] body2 = Encoding.UTF8.GetBytes(requestBody);

            var uri = host + route;
            HttpWebRequest Wreq = (HttpWebRequest)HttpWebRequest.Create(uri);
            Wreq.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            Wreq.ContentType = "application/json";
            Wreq.Method = "POST";

            Stream Wstream = Wreq.GetRequestStream();
            Wstream.Write(body2, 0, body2.Length);

            //HttpWebResponse Wres = (HttpWebResponse)Wreq.GetResponse();
            HttpWebResponse Wres = (HttpWebResponse)Wreq.GetResponse();

            Stream WresStream = Wres.GetResponseStream();
            StreamReader WSreader = new StreamReader(WresStream, Encoding.GetEncoding("utf-8"));
            string responseJJ = WSreader.ReadToEnd();
            
            //JObject jobj = (JObject)JsonConvert.DeserializeObject(responseJJ);

            //responseJJ ="[{\"translations\":[{\"text\":\"Mitsuei-like has always been very indebted. It is a medium forest of Daiwabo information system.\",\"to\":\"en\"}]}]";

            //RootObject RO2 = new RootObject();


            RootObject RO2 = JsonConvert.DeserializeObject<RootObject>("responseJJ");
           // var RO2 = JsonConvert.DeserializeObject<RootObject>(responseJJ);
           
            foreach (Translation translation in RO2.translations)
            {
                string English_text = translation.text;
            }

            ***/


            using (var client = new HttpClient())
            using (var request=new HttpRequestMessage())
            {

                //HttpRequestMessageないで、要求を構成する。
                /***
                      • HTTP メソッドを宣言する
                      • 要求 URI を構成する
                      • 要求本文 (シリアル化した JSON オブジェクト) を挿入する
                      • 必要なヘッダーを追加する
                      • 非同期要求を実行する
                      • 応答の出力
                   ****/

                // Set the method to POST
                request.Method = HttpMethod.Post;

                // Construct the full URI
                request.RequestUri = new Uri(host + route);

                // Add the serialized JSON object to your request
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                // Add the authorization header
                request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                // Send request, get response
                var response = client.SendAsync(request).Result;
                var jsonResponse = response.Content.ReadAsStringAsync().Result;

                //https://qiita.com/hiraokusky/items/98f61df3291158eca301
                //デシリアライズで 英訳されたテキストを取り出す。
                dynamic jarray = JsonConvert.DeserializeObject(jsonResponse);



                /*
                foreach(var item in jarray[0]["translations"])
                {
                    string English_tet =item["text"];
                }
                */

                //int i = 0; 
                //https://kuroeveryday.blogspot.com/2014/04/convert-vs-parse-vs-tostring.html
                //https://teratail.com/questions/56407
                //http://japanese.sugawara-systems.com/systemverilog/dynamic_array.htm


                foreach (var translations in jarray.tranlations)
                {
                    foreach(var text in translations.text)
                    {
                        string English_text = Convert.ToString(text);
                    }
                    
                    //i++;
                } 


            }




        }
        

    }

}
