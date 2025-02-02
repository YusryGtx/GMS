﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using QRCoder;
using System.Net;
using System.Net.Mail;
using System.Diagnostics;

namespace Gym_Management_System
{
    
    public partial class AddMembers : Form
    {
        public int mem_id;
        public string mem_imgpath;

        public AddMembers()
        {
            InitializeComponent();
            fillpackagecombo();
            Gender_cb.Items.AddRange(new object[]{"Male", "Female"});
            string idqry = "SELECT * FROM Members WHERE Id = (SELECT MAX(Id) from Members ) ";
            DB_Connection dB_Connection = new DB_Connection();
            SqlDataReader dr = dB_Connection.getData(idqry);
            if (dr.HasRows)
            {
                dr.Read();
                string num = dr["Id"].ToString();
                int num_1 = int.Parse(num);
                labelMID.Text = "Member id : "+(num_1 + 1).ToString();
                mem_id = num_1 + 1;
                


            }
            else
            {
                labelMID.Text = 1.ToString();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Add_mem_image_D_Box dialogbox = new Add_mem_image_D_Box();
            dialogbox.id = mem_id;
            dialogbox.file_Name = "Member Dp";
            dialogbox.ShowDialog();
            mem_imgpath = dialogbox.imgpath;
            if (dialogbox.btnmemaddclick == true)
            {
                if (mem_imgpath != null)
                {
                    pictureBox1.Image = new Bitmap(mem_imgpath);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string type = "Member";
            string NIC = textBoxNIC.Text;
            string name = textboxName.Text;
            string DOB = this.dateTimePicker1.Text;
            string Gender = Gender_cb.Text;
            string Body_Type = textboxBodyType.Text;
            string Address = richTextBoxAddress.Text;
            string Mobile_Number = textboxMobileNumber.Text;
            string Health_Condition = richTextBoxHealthCondition.Text;
            string Emergency_Contact_Name = textboxEmergencyContactName.Text;
            string Emergency_Contact_Number = (textboxEmergencyContactPhoneNumber.Text);
            string joineddate = DateTime.UtcNow.Date.ToString();
            string Email = memEmail_tb.Text;
            string qrimgpath = Application.StartupPath.Substring(0, Application.StartupPath.Length - 10) + "\\Images\\Member QR\\" + mem_id + "memQR.jpg";

            DB_Connection dB_Connection = new DB_Connection();
            string insertqry = "INSERT INTO Members(NICorDL,MemberName,DOB,Gender,Body_Type,Address,Mobile_Number,Health_Condition,Emergency_Contact_Name,Emergency_Contact_Number,Member_dp,Email,QR_img_path,Joined_Date) VALUES('" + NIC + "','" + name + "','"+DOB+"','" + Gender + "','" + Body_Type + "','" + Address + "'," + Mobile_Number + ",'" + Health_Condition + "','" + Emergency_Contact_Name + "','" + Emergency_Contact_Number + "','"+mem_imgpath+"','"+Email+"','"+qrimgpath+"','"+joineddate+"')";
            
            dB_Connection.InsertData(insertqry);

            
            
            Console.WriteLine(qrimgpath);
            string qrsubject =(mem_id+NIC+name).ToString();
            QRmailSender qRmailSender = new QRmailSender();

            qRmailSender.qrgen(qrsubject,qrimgpath);

            if (Email != null)
            {
                DialogResult dialog = MessageBox.Show("This member has not provided an Email Address! So, You can print the QR code. Do you want to print it ?","Print QR Code",MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    string filename = qrimgpath;
                    var p = new Process();
                    p.StartInfo.FileName = filename;
                    p.StartInfo.Verb = "print";
                    p.Start();
                }
            }
            else
            {
                qRmailSender.Emailgen(name, type);
                qRmailSender.Emailsend(Email, qrimgpath);
            }
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBoxNIC.Text = "";
            textboxName.Text = "";
            Gender_cb.Text = "";
            textboxBodyType.Text = "";
            richTextBoxAddress.Text = "";
            textboxMobileNumber.Text = "";
            richTextBoxHealthCondition.Text = "";
            textboxEmergencyContactName.Text = "";
            textboxEmergencyContactPhoneNumber.Text = "";
            mem_imgpath = "";
            memEmail_tb.Text = "";
        }

        private void fillpackagecombo()
        {
            string qry = "Select * from Packages";
            DB_Connection dB_Connection = new DB_Connection();
            SqlDataReader dr = dB_Connection.getData(qry);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    string pack_name = dr["Package_Name"].ToString();
                    Package_cb.Items.Add(pack_name);
                }
            }
        }

    }
}
