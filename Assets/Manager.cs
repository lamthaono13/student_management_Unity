using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.TableUI;
public class Manager : MonoBehaviour
{
    private int index = 0;
    private ADO db;
    private DataTable dt;

    [SerializeField] private InputField IDtxt;
    [SerializeField] private InputField firstNameTxt;
    [SerializeField] private InputField lastNameTxt;
    [SerializeField] private InputField cityTxt;
    [SerializeField] private InputField departementTxt;

    [SerializeField] private Button btnAdd;
    [SerializeField] private Button btnUpdate;
    [SerializeField] private Button btnDelete;
    [SerializeField] private Button btnExit;

    [SerializeField] private Button btnPre;
    [SerializeField] private Button btnNext;

    [SerializeField] private TableUI table;

    // Start is called before the first frame update
    void Start()
    {
        btnAdd.onClick.AddListener(OnClickBtnAdd);
        btnUpdate.onClick.AddListener(OnClickBtnUpdate);
        btnDelete.onClick.AddListener(OnClickBtnDelete);
        btnExit.onClick.AddListener(OnClickBtnExit);
        btnNext.onClick.AddListener(OnClickBtnNext);
        btnPre.onClick.AddListener(OnClickBtnPre);

        db = new ADO();

        dt = new DataTable();

        if (!this.db.Connect())
        {
            // err

            Debug.Log("Connect_failed");
        }
        else
        {
            Debug.Log("Connect_successed");
        }

        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Init()
    {
        table.GetCell(0, 0).text = "Id";
        table.GetCell(0, 1).text = "First Name";
        table.GetCell(0, 2).text = "LastName";
        table.GetCell(0, 3).text = "City";
        table.GetCell(0, 4).text = "Department";

        if (this.db.Con.State == ConnectionState.Open)
        {
            this.load_students();
            this.fill_TexBoxes(index);
        }
        else
        {
            // err
        }
    }

    private void load_students()
    {
        db.Cmd.CommandType = CommandType.Text;
        db.Cmd.CommandText = "Select * from Student";
        db.Dr = db.Cmd.ExecuteReader();
        dt.Clear();
        dt.Load(db.Dr);

        // Update Data Grid View

        //dataGridView1.DataSource = dt;

        table.Rows = dt.Rows.Count + 1;

        for(int j = 0; j < 5; j++)
        {
            for (int i = 1; i < table.Rows; i++)
            {
                table.GetCell(i, j).text = dt.Rows[i - 1].ItemArray[j].ToString();
        }
        }


    }

    // Filling Text Boxes With Students Info
    private void fill_TexBoxes(int i)
    {
        IDtxt.text = dt.Rows[i][0].ToString();
        firstNameTxt.text = dt.Rows[i][1].ToString();
        lastNameTxt.text = dt.Rows[i][2].ToString();
        cityTxt.text = dt.Rows[i][3].ToString();
        departementTxt.text = dt.Rows[i][4].ToString();
    }

    // Get Student From TextBoxes
    private Student get_student()
    {
        int id = -1;
        string f_name = firstNameTxt.text.Trim(),
            l_name = lastNameTxt.text.Trim(),
            city = cityTxt.text.Trim(),
            departement = departementTxt.text.Trim();

        try { id = int.Parse(IDtxt.text.Trim()); }
        catch { Debug.LogError("Enter a Valid ID"); }

        return new Student(id, f_name, l_name, city, departement);
    }

    // Adding a Student
    private void OnClickBtnAdd()
    {
        Student std = this.get_student();
        if (std.Id < 0)
            return;

        int added = std.AddStudent(db);
        if (added >= 0)
        {
            if (added == 1)
            {
                this.load_students();
                this.index = dt.Rows.Count - 1;
                this.fill_TexBoxes(index);
                Debug.Log("The Student Was Added Successfuly");
            }
            else
                Debug.Log("This Student already exist");
        }
        else
            Debug.Log("Connection Error");
    }

    // Updating a Student
    private void OnClickBtnUpdate()
    {
        Student std = this.get_student();
        if (std.Id < 0)
            return;

        int updated = std.UpdateStudent(db);
        if (updated >= 0)
        {
            if (updated == 1)
            {
                this.load_students();
                this.fill_TexBoxes(index);
                Debug.Log("The Student Was Updated Successfuly");
            }
            else
                Debug.Log("This Student Don't Exist");
        }
        else
            Debug.Log("Connection Error");
    }

    // Deleting a Student
    private void OnClickBtnDelete()
    {
        Student std = this.get_student();
        if (std.Id < 0)
            return;

        //if (MessageBox.Show("Are you sure", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
        //{
        //    int deleted = std.DeleteStudent(db);
        //    if (deleted >= 0)
        //    {
        //        if (deleted == 1)
        //        {
        //            this.load_students();
        //            this.index--;
        //            this.fill_TexBoxes(index);
        //            Debug.LogError("The Student Was Deleted Successfuly");
        //        }
        //        else
        //            Debug.LogError("This Student Don't Exist");
        //    }
        //    else
        //        Debug.LogError("Connection Error");
        //}

        int deleted = std.DeleteStudent(db);
        if (deleted >= 0)
        {
            if (deleted == 1)
            {
                this.load_students();
                this.index--;
                this.fill_TexBoxes(index);
                Debug.LogError("The Student Was Deleted Successfuly");
            }
            else
                Debug.LogError("This Student Don't Exist");
        }
        else
            Debug.LogError("Connection Error");
    }

    #region Navgation Buttons
    // Navigate to Next Student
    private void OnClickBtnNext()
    {
        index = index + 1 > dt.Rows.Count - 1 ? 0 : index + 1;
        fill_TexBoxes(index);
    }
    // Navigate to Previeus Student
    private void OnClickBtnPre()
    {
        index = index - 1 < 0 ? dt.Rows.Count - 1 : index - 1;
        fill_TexBoxes(index);
    }
    // Navigate to the first Student
    private void navigateToFirst()
    {
        index = 0;
        fill_TexBoxes(index);
    }
    // Navigate to the Last Student
    private void navigateToLast()
    {
        index = dt.Rows.Count - 1;
        fill_TexBoxes(index);
    }
    #endregion Navigation Buttons

    // Exiting The Application Event
    private void OnClickBtnExit()
    {
        this.db.Disconnect();
        Application.Quit();
    }
}
