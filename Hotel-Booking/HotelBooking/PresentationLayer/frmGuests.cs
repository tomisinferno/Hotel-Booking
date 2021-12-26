/*
 * Author: Oluwatomi
 * Date: 2021/11/28
 * A program using an access database, to CRUD data*/

using System;
using System.Data.OleDb;
using System.Windows.Forms;
using HotelBooking.BusinessObjects;

namespace HotelBooking
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        //Access DB
        string sConnection = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=Cottages.accdb";
        OleDbConnection dbConn;

        private void frmMain_Load(object sender, EventArgs e)
        {
            //Populate Combo Box on Load
            PopulateGuestsCombo();
        }

        private void cmbSelectGuest_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedGuest = ((Guest)cmbSelectGuest.SelectedItem).GuestID;

            try
            {

                dbConn = new OleDbConnection(sConnection);
                //open connection to database
                dbConn.Open();
                string sql;

                //Added subquery to get the count of rows being returned
                //using parameter binding
                sql = "Select (Select count(GuestID) from Guests Where GuestID = @GuestID) as rowCount, * from Guests Where GuestID = @GuestID;";
                OleDbCommand dbCmd = new OleDbCommand();

                dbCmd.Parameters.AddWithValue("@GuestID", selectedGuest);

                //set command SQL string
                dbCmd.CommandText = sql;
                //set the command connection
                dbCmd.Connection = dbConn;
                //get number of rows
                //ExecuteScalar returns value from first column
                int numRows = (Int32)dbCmd.ExecuteScalar();
                //create OleDbDataReader dbReader
                OleDbDataReader dbReader;
                //Read data into dbReader
                dbReader = dbCmd.ExecuteReader();
                //Read first record
                dbReader.Read();
                if (dbReader.HasRows && numRows == 1)
                {
                    //get data from dbReader by column name and assing to text boxes
                    txtFirstName.Text = dbReader["FirstName"].ToString();
                    txtLastName.Text = dbReader["LastName"].ToString();
                    txtStreet.Text = dbReader["Street"].ToString();
                    txtCity.Text = dbReader["City"].ToString();
                    txtState.Text = dbReader["State"].ToString();
                    txtZip.Text = dbReader["Zip"].ToString();
                    txtPhone.Text = dbReader["Phone"].ToString();
                    txtEmail.Text = dbReader["Email"].ToString();
                    //dtpLastVisitDate.Value = DateTime.Parse(dbReader["LastVisitDate"].ToString());
                    dtpLastVisitDate.Text = dbReader["lastvisitdate"].ToString();
                    txtRoom.Text = dbReader["Room"].ToString();
                    txtGuestID.Text = dbReader["GuestID"].ToString();

                }

                dbReader.Close();
                dbConn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                DialogResult result = MessageBox.Show("Do you want to delete this record?", "Delete Record", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        dbConn = new OleDbConnection(sConnection);
                        dbConn.Open();
                        string sql;
                        sql = "Delete from Guests where GuestId = @GuestId";
                        OleDbCommand dbCmd = new OleDbCommand();
                        dbCmd.CommandText = sql;
                        dbCmd.Connection = dbConn;
                        dbCmd.Parameters.AddWithValue("@GuestId", txtGuestID.Text);
                        int rowCount = dbCmd.ExecuteNonQuery();
                        dbConn.Close();
                        if (rowCount == 1)
                        {
                            MessageBox.Show("Guest deleted successfully");
                            PopulateGuestsCombo();
                        }
                        else
                        {
                            MessageBox.Show("Error deleting record. Please try again.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }

        public void PopulateGuestsCombo()
        {
            ClearForm();

            try
            {
                dbConn = new OleDbConnection(sConnection);
                dbConn.Open();
                string sql;
                sql = "select * from Guests;";

                OleDbCommand dbCmd = new OleDbCommand();
                dbCmd.CommandText = sql;
                dbCmd.Connection = dbConn;
                OleDbDataReader dbReader;
                dbReader = dbCmd.ExecuteReader();
                while (dbReader.Read())
                {
                    Guest g = new Guest((int)dbReader["GuestId"], dbReader["FirstName"].ToString(), dbReader["LastName"].ToString());
                    //when displayed the combo box will call toString by default on the Person object.
                    //the toString only displays the FirstName of the person.
                    cmbSelectGuest.Items.Add(g);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void ClearForm()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtStreet.Text = "";
            txtCity.Text = "";
            txtState.Text = "";
            txtZip.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            dtpLastVisitDate.Value = DateTime.Now;
            txtRoom.Text = "";
            txtGuestID.Text = "";
            cmbSelectGuest.Text = "";
            cmbSelectGuest.Items.Clear();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                DialogResult result = MessageBox.Show("Do you want to Edit this record?", "Delete Record", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        dbConn = new OleDbConnection(sConnection);
                        dbConn.Open();
                        string sql;
                        sql = "Update Guests set LastName = @LastName, FirstName = @FirstName, Street = @Street, City = @City, " +
                            "State = @State, Zip = @Zip, Phone = @Phone, email = @Email, Room = @Room, LastVisitDate = @LastVisitDate " +
                            "where GuestID = @GuestID;";

                        OleDbCommand dbCmd = new OleDbCommand();
                        dbCmd.CommandText = sql;
                        dbCmd.Connection = dbConn;

                        dbCmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
                        dbCmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                        dbCmd.Parameters.AddWithValue("@Street", txtStreet.Text);
                        dbCmd.Parameters.AddWithValue("@City", txtCity.Text);
                        dbCmd.Parameters.AddWithValue("@State", txtState.Text);
                        dbCmd.Parameters.AddWithValue("@Zip", txtZip.Text);
                        dbCmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                        dbCmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                        dbCmd.Parameters.AddWithValue("@Room", txtRoom.Text);
                        dbCmd.Parameters.AddWithValue("@LastVisitDate", dtpLastVisitDate.Text);
                        dbCmd.Parameters.AddWithValue("@GuestID", txtGuestID.Text);
                        int rowCount = dbCmd.ExecuteNonQuery();
                        dbConn.Close();

                        if (rowCount == 1)
                        {
                            MessageBox.Show("Guest Edited successfully");
                            ClearForm();
                            PopulateGuestsCombo();
                        }
                        else
                        {
                            MessageBox.Show("Error updating record. Please try again.");
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }

        }

        private bool ValidateForm()
        {
            if (string.IsNullOrEmpty(txtFirstName.Text) || string.IsNullOrEmpty(txtLastName.Text) || string.IsNullOrEmpty(txtStreet.Text) ||
                string.IsNullOrEmpty(txtCity.Text) || string.IsNullOrEmpty(txtState.Text) || string.IsNullOrEmpty(txtZip.Text) || string.IsNullOrEmpty(txtPhone.Text) ||
                string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtRoom.Text))
            {
                MessageBox.Show("Fill form properly");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {

                if (!String.IsNullOrEmpty(txtGuestID.Text))
                {
                    //txtFirstName.Text = "";
                    //txtLastName.Text = "";
                    //txtStreet.Text = "";
                    //txtCity.Text = "";
                    //txtState.Text = "";
                    //txtZip.Text = "";
                    //txtPhone.Text = "";
                    //txtEmail.Text = "";
                    //dtpLastVisitDate.Value = DateTime.Now;
                    //txtRoom.Text = "";
                    //txtGuestID.Text = "";
                    //cmbSelectGuest.Text = "";
                    ClearForm();
                    PopulateGuestsCombo();
                }
                else
                {
                    if (ValidateForm())
                    {
                        try
                        {
                            dbConn = new OleDbConnection(sConnection);
                            dbConn.Open();
                            string sql;
                            sql = "insert into Guests(LastName, FirstName, Street, City, State, Zip, Phone, Email, LastVisitDate, Room) Values (@LastName, @FirstName, @Street, @City, " +
                                "@State, @Zip, @Phone, @Email, @LastVisitDate, @Room);";

                            OleDbCommand dbCmd = new OleDbCommand();

                            dbCmd.CommandText = sql;
                            dbCmd.Connection = dbConn;

                            dbCmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                            dbCmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
                            dbCmd.Parameters.AddWithValue("@Street", txtStreet.Text);
                            dbCmd.Parameters.AddWithValue("@City", txtCity.Text);
                            dbCmd.Parameters.AddWithValue("@State", txtState.Text);
                            dbCmd.Parameters.AddWithValue("@Zip", txtZip.Text);
                            dbCmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                            dbCmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                            dbCmd.Parameters.AddWithValue("@LastVisitDate", dtpLastVisitDate.Text);
                            dbCmd.Parameters.AddWithValue("@Room", txtRoom.Text);
                            int rowCount = dbCmd.ExecuteNonQuery();

                            dbConn.Close();
                            if (rowCount == 1)
                            {
                                MessageBox.Show("Guest added successfully");
                                //update frmGuests

                                ClearForm();
                                PopulateGuestsCombo();
                            }
                            else
                            {
                                MessageBox.Show("Error adding guest. Please try again.");
                            }


                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                }
            }
        }
    }
}
