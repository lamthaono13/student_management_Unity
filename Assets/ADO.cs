using System.Data;
using System;
using System.Data.SqlClient;

public class ADO
{
    private const string serverName = "DESKTOP-C3770SE";
    private const string dataBaseName = "school";
    private const bool integratedSecurity = true;

    private SqlConnection con;
    private SqlCommand cmd;
    private SqlDataReader dr;
    private SqlDataAdapter sd;

    #region Encapsulation
    public SqlConnection Con { get => con; set => con = value; }
    public SqlCommand Cmd { get => cmd; set => cmd = value; }
    public SqlDataReader Dr { get => dr; set => dr = value; }

    public SqlDataAdapter Sd { get => sd; set => sd = value; }

    #endregion Encapsulation

    // Constructor
    public ADO()
    {
        string connectionString =
           "Server=DESKTOP-C3770SE;" + //Your SQLServer use with doble slash \\ for path as: MYPC\\SQLEXPRESS
           "Database=school;" + //Your database name
           "User ID=khang;" + //Your SQL user as SA or other
           "Password=123;"; //Your database user password

        this.con = new SqlConnection("Data Source = " + serverName + ";"
                        + "Integrated Security = " + integratedSecurity + ";" +
                        "Initial Catalog = " + dataBaseName + ""
                    );

        //this.con = new SqlConnection(connectionString);

        this.cmd = new SqlCommand();
        this.cmd.Connection = this.con;

        this.sd = new SqlDataAdapter(cmd);
    }


    // Method to Connect to the database
    public bool Connect()
    {
        if (this.con.State == ConnectionState.Closed || this.con.State == ConnectionState.Broken)
        {
            try { this.con.Open(); }
            catch { return false; }
        }
        return true;
    }

    // Method to close the connection to the database
    public void Disconnect()
    {
        if (this.con.State == ConnectionState.Open)
        {
            this.con.Close();
        }
    }
}
