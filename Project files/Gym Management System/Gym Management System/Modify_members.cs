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


namespace Gym_Management_System
{
    
    public partial class Modify_members : Form
    {
        public string Id;
        private string Member_dp_path;
        public Modify_members()
        {
            InitializeComponent();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            textbox_Members_Id.Text = "";
            textBoxNIC.Text = "";
            textboxName.Text = "";
            textboxGender.Text = "";
            textboxBodyType.Text = "";
            richTextBoxAddress.Text = "";
            textboxMobileNumber.Text = "";
            richTextBoxHealthCondition.Text = "";
            textboxEmergencyContactName.Text = "";
            textboxEmergencyContactPhoneNumber.Text = "";
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            
            MessageBox.Show("Are you sure that you want to update this member ?");
            string NIC_forqr = textBoxNIC.Text;
            string Name_forqr = textboxName.Text;
            string mail_db = memEmail_tb.Text;

            Id = textbox_Members_Id.Text;
            string NIC = textBoxNIC.Text;
            string name = textboxName.Text;
            string Gender = textboxGender.Text;
            string Body_Type = textboxBodyType.Text;
            string Email = memEmail_tb.Text;
            string Address = richTextBoxAddress.Text;
            string Mobile_Number = textboxMobileNumber.Text;
            string Health_Condition = richTextBoxHealthCondition.Text;
            string Emergency_Contact_Name = textboxEmergencyContactName.Text;
            string Emergency_Contact_Number = (textboxEmergencyContactPhoneNumber.Text);
            string qrimgpath = Application.StartupPath.Substring(0, Application.StartupPath.Length - 10) + "\\Images\\Member QR\\" + Id + "memQR.jpg";

            DB_Connection dB_Connection = new DB_Connection();
            string query = "UPDATE Members SET NICorDL='"+NIC+"', MemberName='"+name+"' , Gender='"+Gender+"' , Body_Type='"+Body_Type+"' , Address='"+Address+"' , Mobile_Number='"+Mobile_Number+"' , Health_Condition='"+Health_Condition+"' , Emergency_Contact_Name='"+Emergency_Contact_Name+"' , Emergency_Contact_Number='"+Emergency_Contact_Number+ "', Member_dp='" + Member_dp_path + "' , Email = '" + Email + "' , QR_img_path='"+qrimgpath+"' WHERE Id='" + Id+"'";
            dB_Connection.update(query);

            
            if (NIC_forqr != NIC || Name_forqr != name || Email != mail_db)
            {
                QRmailSender qRmailSender = new QRmailSender();
                string qrsubject = (Id.ToString() + NIC + name).ToString();
                qRmailSender.qrgen(qrsubject, qrimgpath);
                qRmailSender.Emailgen(name, "member");
                qRmailSender.Emailsend(Email, qrimgpath);
            }
        }

        private void textbox_Members_Id_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                if (textbox_Members_Id.Text != "")
                {
                    try
                    {
                        Id = textbox_Members_Id.Text;
                        int id = int.Parse(textbox_Members_Id.Text);
                        DB_Connection dB_Connection = new DB_Connection();
                        SqlConnection con = new SqlConnection(dB_Connection.connectionstring);
                        con.Open();
                        string qry = "SELECT * FROM Members Where Id=@Id ";
                        SqlCommand cmd = new SqlCommand(qry, con);
                        cmd.Parameters.AddWithValue("@Id", id);
                        SqlDataReader da = dB_Connection.getDatausing_a(cmd);
                        if (da.HasRows)
                        {
                            while (da.Read())
                            {

                                textBoxNIC.Text = da.GetValue(1).ToString();
                                textboxName.Text = da.GetValue(2).ToString();
                                textboxGender.Text = da.GetValue(4).ToString();
                                textboxBodyType.Text = da.GetValue(5).ToString();
                                richTextBoxAddress.Text = da.GetValue(6).ToString();
                                textboxMobileNumber.Text = da.GetValue(7).ToString();
                                richTextBoxHealthCondition.Text = da.GetValue(8).ToString();
                                textboxEmergencyContactName.Text = da.GetValue(9).ToString();
                                textboxEmergencyContactPhoneNumber.Text = da.GetValue(10).ToString();
                                pictureBox1.Image = new Bitmap(da.GetValue(11).ToString());
                                Member_dp_path = da.GetValue(11).ToString();
                                memEmail_tb.Text = da.GetValue(12).ToString();
                            }
                            con.Close();
                        }
                        else
                        {
                            MessageBox.Show("There is no member by member id:" + id + "\nTry again with another Id");
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Are you sure that you want to Delete this member details ?");
            Id = textbox_Members_Id.Text;
            DB_Connection dB_Connection = new DB_Connection();
            string query = "DELETE FROM Members Where Id='"+Id+"'";
            dB_Connection.Delete(query);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (Id != null)
            {
                Add_mem_image_D_Box dialogbox = new Add_mem_image_D_Box();
                dialogbox.id = int.Parse(Id);
                dialogbox.file_Name = "Member DP";
                pictureBox1.Image.Dispose();
                dialogbox.ShowDialog();
                Member_dp_path = dialogbox.imgpath;
                Console.WriteLine(Member_dp_path);

                if (dialogbox.btnmemaddclick == true)
                {
                    if (Member_dp_path != null)
                    {
                        Console.WriteLine(Member_dp_path);
                        pictureBox1.Image = new Bitmap(Member_dp_path);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Enter the Id number");
            }
            

        }

        
    }
}
