using System;
using System.Windows;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Dh.Wpf
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {


        public static long lastMembNumb = 1;
        public static string xmlFilePath = @"xml.xml";

        public MainWindow()
        {
            InitializeComponent();
            

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
          
        }


        public class Member : INotifyPropertyChanged
        {
            public Member(string numb)
            {
                this.numb = numb;
            }
            public Member(string id, string pwd)
            {
                this.id = id;
                this.pwd = pwd;
            }

            public Member()
            {
            }


            private string Type;
            public string type
            {
                get { return Type; }
                set
                {
                    if (Type != value)
                    {
                        Type = value;
                    }
                }
            }

            public string numb
            {
                get;
                set;
            }

            public string id
            {
                get;
                set;
            }

            public string pwd
            {
                get;
                set;
            }

            public string name
            {
                get;
                set;
            }

            public string phon
            {
                get;
                set;
            }

            public string emai
            {
                get;
                set;
            }

            public string regiDt
            {
                get;
                set;
            }

            public string withDrawDt
            {
                get;
                set;
            }


            bool checkedVar = false;

            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyUpdate(string propertyName)
            {
                if(PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }


            public bool CheckedVar
            {
                get { return checkedVar; }
                set
                {
                    checkedVar = value;
                }
            }
        }

        private void dataGrid_Loaded(object sender, EventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            List<Member> memberList = ReadXML();
            dataGrid.ItemsSource = memberList;
        }

        public void addData(Member member)
        {
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(xmlFilePath);
                if (xmldoc.ChildNodes.Item(1).ChildNodes.Item(0) == null)
                {
                    CreateXML();
                } else
                {
                    XmlNodeList nodeList= xmldoc.ChildNodes.Item(1).ChildNodes.Item(0).ChildNodes;
                    XmlNode newNode = xmldoc.CreateElement("member");
                    for (var i = 0; i < nodeList.Count; i++)
                    {
                        var property = typeof(Member).GetProperty(nodeList.Item(i).Name);
                        XmlNode newNodeChild = xmldoc.CreateElement(nodeList.Item(i).Name);
                        if (nodeList.Item(i).Name.Equals("regiDt"))
                        {
                            newNodeChild.InnerText = DateTime.Now.ToString();
                        } else
                        {
                            newNodeChild.InnerText = property.GetGetMethod().Invoke(member, null) as string;
                        }
                        newNode.AppendChild(newNodeChild);
                    }

                    xmldoc.DocumentElement.AppendChild(newNode);
                }
                xmldoc.Save(xmlFilePath);
            } 
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public void deleteData(List<Member> memberList)
        {
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(xmlFilePath);
                XmlNodeList nodeList = xmldoc.ChildNodes.Item(1).ChildNodes.Item(0).ChildNodes;
                for (int i = 0; i < memberList.Count; i++)
                {
                    String numb = memberList[i].numb;
                    XmlNodeList numbList = xmldoc.GetElementsByTagName("numb");
                    for (int j = 0; j < numbList.Count; j++)
                    {
                        XmlNode target = numbList.Item(j);
                        if (target.InnerText.Equals(numb))
                        {
                            target.ParentNode.ChildNodes.Item(8).InnerText = DateTime.Now.ToString();
                        }
                        
                    }
                }

                xmldoc.Save(xmlFilePath);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public void updateData(List<Member> memberList)
        {
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(xmlFilePath);
                XmlNodeList nodeList = xmldoc.ChildNodes.Item(1).ChildNodes.Item(0).ChildNodes;
                for (int i = 0; i < memberList.Count; i++)
                {
                    Member member = memberList[i];
                    String numb = member.numb;
                    XmlNodeList numbList = xmldoc.GetElementsByTagName("numb");
                    for (int j = 0; j < numbList.Count; j++)
                    {
                        XmlNode target = numbList.Item(j);
                        if (target.InnerText.Equals(numb))
                        {
                            XmlNodeList targetProperties = target.ParentNode.ChildNodes;
                            targetProperties.Item(0).InnerText = member.name;
                            targetProperties.Item(1).InnerText = member.id;
                            targetProperties.Item(2).InnerText = member.pwd;
                            targetProperties.Item(3).InnerText = member.type;
                            targetProperties.Item(4).InnerText = member.emai;
                            targetProperties.Item(6).InnerText = member.phon;
                        }

                    }
                }
                xmldoc.Save(xmlFilePath);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }


        private void CreateXML()
        {
            FileStream fs = File.OpenWrite(xmlFilePath);
            XmlTextWriter textWriter = new XmlTextWriter(fs, Encoding.UTF8);
            textWriter.Formatting = Formatting.Indented;
            textWriter.WriteStartDocument();

            // 루트 설정
            textWriter.WriteStartElement("members");

            // 노드 안에 하위 노드 설정
            textWriter.WriteStartElement("member");

            textWriter.WriteStartElement("name");
            textWriter.WriteString("회원1");
            textWriter.WriteEndElement();

            textWriter.WriteStartElement("id");
            textWriter.WriteString("member1");
            textWriter.WriteEndElement();

            textWriter.WriteStartElement("pwd");
            textWriter.WriteString("");
            textWriter.WriteEndElement();

            textWriter.WriteStartElement("type");
            textWriter.WriteString("정회원");
            textWriter.WriteEndElement();

            textWriter.WriteStartElement("emai");
            textWriter.WriteString("");
            textWriter.WriteEndElement();

            textWriter.WriteStartElement("numb");
            textWriter.WriteString("1");
            textWriter.WriteEndElement();

            textWriter.WriteStartElement("phon");
            textWriter.WriteString("");
            textWriter.WriteEndElement();

            textWriter.WriteStartElement("regiDt");
            textWriter.WriteString(DateTime.Now.ToString());
            textWriter.WriteEndElement();

            textWriter.WriteStartElement("withDrawDt");
            textWriter.WriteString("");
            textWriter.WriteEndElement();

            textWriter.WriteEndElement();

            textWriter.Close();

        }
        /// <summary>
        /// XML 파일 읽기
        /// </summary>
        private List<Member> ReadXML()
        {
            List<Member> memberList = new List<Member>();
            try
            {
                if (!File.Exists(xmlFilePath))
                {
                    CreateXML();
                }
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(xmlFilePath);
                XmlElement root = xmldoc.DocumentElement;

                XmlNodeList nodes = root.ChildNodes;

                foreach (XmlNode node in nodes)
                {
                    Member member = new Member();
                    member.name = node["name"].InnerText;
                    member.id = node["id"].InnerText;
                    member.pwd = node["pwd"].InnerText;
                    member.type = node["type"].InnerText;
                    member.emai = node["emai"].InnerText;
                    member.numb = node["numb"].InnerText;
                    member.phon = node["phon"].InnerText;
                    member.regiDt = node["regiDt"].InnerText;
                    member.withDrawDt = node["withDrawDt"].InnerText;
                    if (member.withDrawDt.Equals(""))
                    {
                        memberList.Add(member);
                    }
                    lastMembNumb = Convert.ToInt64(member.numb);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return memberList;
        }

        //조회
        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            DataGrid dataGrid = FindName(Name = "DataGrid") as DataGrid;
            List<Member> memberList = ReadXML();
            dataGrid.ItemsSource = memberList;

        }
        //등록
        private void Button_Click_2(object sender, System.Windows.RoutedEventArgs e)
        {
            DataGrid dataGrid = FindName(Name = "DataGrid") as DataGrid;
            var selectedList = new List<Member>();
            Member member = new Member(Convert.ToString(lastMembNumb + 1));
            member.name = "새이름";
            member.phon = "새번호";
            member.id = "member" + Convert.ToString(lastMembNumb + 1);
            addData(member);
            Button_Click_1(this, new RoutedEventArgs());
        }
        //수정
        private void Button_Click_3(object sender, System.Windows.RoutedEventArgs e)
        {
            DataGrid dataGrid = FindName(Name = "DataGrid") as DataGrid;
            var selectedList = new List<Member>();
            for (int i = 0; i < dataGrid.Items.Count; i++)
            {
                var item = dataGrid.Items[i];
                if (((Member)item).CheckedVar == true)
                {
                    selectedList.Add((Member)item);
                }
            }
            updateData(selectedList);
            Button_Click_1(this, new RoutedEventArgs());
        }
        //삭제
        private void Button_Click_4(object sender, System.Windows.RoutedEventArgs e)
        {
            DataGrid dataGrid = FindName(Name = "DataGrid") as DataGrid;
            var selectedList = new List<Member>();
            for (int i = 0; i < dataGrid.Items.Count; i++)
            {
                var item = dataGrid.Items[i];
                if (((Member)item).CheckedVar == true)
                {
                   selectedList.Add((Member)item);
                }
            }
            deleteData(selectedList);
            Button_Click_1(this, new RoutedEventArgs());
        }
    }
}
