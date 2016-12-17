using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System.ComponentModel;

namespace WebCrawler
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        string url;
        string hiraganaChar = " ぁあぃいぅうぇえぉおかがきぎくぐけげこごさざしじすずせぜそぞただちぢっつづてでとどなにぬねのはばぱひびぴふぶぷへべぺほぼぽまみむめもゃやゅゆょよらりるれろゎわをん";
        string katakanaChar = " ァアィイゥウェエォオカガキギクグケゲコゴサザシジスズセゼソゾタダチヂッツヅテデトドナニヌネノハバパヒビピフブプヘベペホボポマミムメモャヤュユョヨラリルレロヮワヲン";
        string koreanChar = " ㅏ아ㅣ이ㅜ우ㅔ에ㅗ오카가키기쿠구케게코고사자시지수주세제소조타다치디ㅊ츠즈테데토도나니누네노하바파히비피후부푸헤베페호보포마미무메모ㅑ야ㅠ유ㅛ요라리루레로ㅘ와오ㄴ";
        string punctuatuin = "！？「」『』・、。…――　－（）-ヵヶ" + "\n";
        HtmlNode ourNone;

        
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private string kanjiToKorean(string japanese)
        {
            string word = japanese;
            string resString="";
            string url = "http://dic.daum.net/search.do?q=";
            var webGet = new HtmlWeb();
            var doc = webGet.Load(url + word + "&dic=jp");
            HtmlNode ourNone1 = doc.DocumentNode.SelectSingleNode("//a[@class='txt_searchword']");
            if (ourNone1 == null) ourNone1 = doc.DocumentNode.SelectSingleNode("//a[@class='txt_cleansch']");
            for(int i=0; i < ourNone1.InnerText.Length; i++)
            {
                resString += koreanChar[hiraganaChar.IndexOf(ourNone1.InnerText[i])];
            }
            if (ourNone1 != null) return japanese+'('+ourNone1.InnerText+')';
            else return "non";
        }
        private string intoKorean(string japanese)
        {
            int i;
            string korean="";
            string tempword = "";
            int last = 0;
            for (i = 0; i < japanese.Length; i++)
            {
                if (hiraganaChar.IndexOf(japanese[i]) != -1)
                {
                    //if (tempword != "") korean += kanjiToKorean(tempword);
                    //tempword = "";
                    if (japanese[i] == 'は') korean += "와(하)";
                    else korean += koreanChar[hiraganaChar.IndexOf(japanese[i])];
                    
                    //if (last != 1) korean += ' ';
                    //last = 1;
                }
                else if (katakanaChar.IndexOf(japanese[i]) != -1)
                {
                    //if (tempword != "") korean += kanjiToKorean(tempword);
                    //tempword = "";
                    korean += koreanChar[katakanaChar.IndexOf(japanese[i])];
                    //if (last != 2) korean += ' ';
                    //last = 2;
                }
                else if (punctuatuin.IndexOf(japanese[i]) != -1)
                { 
                    //if (tempword != "") korean += kanjiToKorean(tempword);
                    //tempword = "";
                    korean += japanese[i];
                    //if (last != 3) korean += ' ';
                    //last = 3;
                }
                else
                {
                    korean += japanese[i];
                    //if (last != 4) korean += ' ';
                    //last = 4;
                }

            }
            return korean;
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            url = "http://ncode.syosetu.com/n2267be/";
            var webGet = new HtmlWeb();
            int i;
            for (i = 1; i <= 430; i++)
            {
                var doc = webGet.Load(url+i.ToString());
                ourNone = doc.DocumentNode.SelectSingleNode("//div[@id='novel_honbun']");
                string filename="";
                filename += i.ToString();
                //if (i == 198) filename += "Ｒｅ：ゼロから始める異世界生活 - 第四章３2 『1 of 4』";
                //else filename += doc.DocumentNode.SelectSingleNode("//title").InnerText;
                filename += ".txt";
                StreamWriter sw = new StreamWriter(filename);
                if (ourNone != null)
                {
                    string res = intoKorean(ourNone.InnerText);
                    sw.Write(res);
                    textBox.Text = res; 
                }
                else textBox.Text = "non";
            }
            MessageBox.Show("Finish!","finish",MessageBoxButton.OK);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Window1 window = new Window1();
            window.Owner = this;
            window.Show();
            if (ourNone.InnerText != null)
            {
                window.setOut(ourNone.InnerText);
            }
        }
    }
}
