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
        }

 

        static void TranslaterText()
        {            
            string host = "https://api.cognitive.microsofttranslator.com";  //グローバル
            // string route = "/translate?api-version=3.0&to=de&to=it";  //ドイツ語とイタリア語
            string route = "/translate?api-version=3.0&to=en";             
            string subscriptionKey = "dea1b00c495e466591053385f8d1955d";

            string Japanese_txt = "";
            // System.Object[] body = new System.Object[] { new { Text = @"Hello world" }};
            //var requestBody = JsonConvert.SerializeObject(body);
            
            //日本語テキストを指定する。
   
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Japanese_txt = ofd.FileName;
            }

            //textファイルの読み込み
            StreamReader sr = new StreamReader(Japanese_txt, Encoding.GetEncoding("Shift_jis"));
            string str_Japanese = sr.ReadToEnd();
            sr.Close();

            System.Object[] body = new System.Object[] { new { Text = str_Japanese } };
            var requestBody = JsonConvert.SerializeObject(body);



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

                // Print the response
                Console.WriteLine(jsonResponse);
                Console.WriteLine("Press any key to continue.");
            }




            


        }
    }
}
