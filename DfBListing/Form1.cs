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
using System.Net;
using Newtonsoft.Json;
using System.Web;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;


namespace DfBListing
{
  
    [DataContract]
    class Profile
    {
        //profile:
        [DataMember]
        public string given_name { get; set; }
        [DataMember]
        public string surname { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string member_id { get; set; }
        [DataMember]
        public string email { get; set; }
        [DataMember]
        public string external_id { get; set; }
        [DataMember]
        public List<string> groups { get; set; }
    }
    
    [DataContract]
    class Permissions
    {
        //permissions:
        [DataMember]
        public bool is_admin { get; set; }
    }

    [DataContract]
    class Member
    //member is made up of profile and permissions components
    {
        [DataMember]
        public Profile profile { get; set; }
        [DataMember]
        public Permissions permissions { get; set; }
    }

    [DataContract]
    class Team
    {
        //team is made up of members and has a cursor and has_more attribute
        [DataMember]
        public List<Member> members { get; set; }
        [DataMember]
        public string cursor { get; set; }
        [DataMember]
        public bool has_more { get; set; } 
    }

    
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtboxToken.Text = "32Wp8V0dZ28AAAAAAACiCt2gcRLGDd-2IeDXLTuS3sV6NiQSCucUZ5u8RFECzRdQ";
            
            //
            // This is the first node in the view.
            //
            TreeNode treeNode = new TreeNode("Windows");
            treeView1.Nodes.Add(treeNode);
            //
            // Another node following the first node.
            //
            treeNode = new TreeNode("Linux");
            treeView1.Nodes.Add(treeNode);
            //
            // Create two child nodes and put them in an array.
            // ... Add the third node, and specify these as its children.
            //
            TreeNode node2 = new TreeNode("C#");
            TreeNode node3 = new TreeNode("VB.NET");
            TreeNode[] array = new TreeNode[] { node2, node3 };
            //
            // Final node.
            //
            treeNode = new TreeNode("Dot Net Perls", array);
            treeView1.Nodes.Add(treeNode);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = txtboxToken.Text;
            

            //call the API
            string testtext = dfb_members_list();

            textBox1.Text = testtext;

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Team));
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(testtext));
            Team obj = (Team)ser.ReadObject(stream);

            int totalMembers = obj.members.Count;

            for (int i = 0; i < totalMembers; i++)
            {
                string fullname = obj.members[i].profile.given_name + " " + obj.members[i].profile.surname;
                comboBox1.Items.Add(fullname);
            }
        }

        private string dfb_members_list()
        {
            String page = "https://api.dropbox.com/1/team/members/list";
            WebRequest request = WebRequest.Create(page);

            request.Method = "POST";
            request.ContentType = "application/json";
            String authheader = "Authorization:Bearer " + txtboxToken.Text;
            request.Headers.Add(authheader);

            // Create POST data and convert it to a byte array.
            string postData = "{}";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;

            // Get the request stream.
            Stream ds = request.GetRequestStream();

            // Write the data to the request stream.
            ds.Write(byteArray, 0, byteArray.Length);

            // Close the Stream object.
            ds.Close();

            // Get the response.
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // Display the status.
                Console.WriteLine(response.StatusDescription);

                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();

                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);

                // Read the content. 
                string responseFromServer = reader.ReadToEnd();

                // Cleanup the streams and the response.
                reader.Close();
                dataStream.Close();
                response.Close();

                return responseFromServer;
            }


            catch (WebException error)
            {
                return "We hit an error in basicAPIcall()";
            }

        
        }









    //class - Form1
    }

//namespace DfBListing
}
