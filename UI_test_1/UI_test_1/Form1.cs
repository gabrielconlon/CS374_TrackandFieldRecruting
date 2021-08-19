using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UI_test_1
{
    public partial class main_FM : Form
    {
        test1DataContext database = new test1DataContext();


        public main_FM()
        {
            Athlete athlete = new Athlete();

            InitializeComponent();

            

        }
        
        private void A_Search_B_Click(object sender, EventArgs e)
        {
            //Data_View_A.DataSource = from a in database.Athletes
                                     //select a;
            string AthleteSQL;

            AthleteSQL = "SELECT DISTINCT A.[First Name], A.[Last Name], A.[Skill Rating], A.[Date Last Contacted], A.[Class], A.[High School], A.[High School Coach],A.[City], A.[State], A.[Address], A.[Phone Number], A.[Email], A.[Height], A.[Weight], C.[Last Name] ";
            AthleteSQL += "FROM Athlete A, Coach C, Event E, Ath_Event AE ";
            AthleteSQL += "WHERE A.[Coach_id]=C.[Coach_id] AND E.[Event_id]=AE.[Event_id] AND AE.[Athlete_id]=A.[Athlete_id]";

            if (A_FirstName_TB.Text.Length > 0)
            {
                AthleteSQL += " AND A.[First Name]='" + A_FirstName_TB.Text + "'";
            }

            if (A_LastName_TB.Text.Length > 0)
            {
                AthleteSQL += " AND A.[Last Name]='" + A_LastName_TB.Text + "'";
            }

            if (A_HighSchool_TB.Text.Length > 0)
            {
                AthleteSQL += " AND A.[High School]='" + A_HighSchool_TB.Text + "'";
            }

            if (A_ClassStanding_DD.Text.Length > 0)
            {
                AthleteSQL += " AND A.[Class Standing]='" + A_ClassStanding_DD.Text + "'";
            }

            if (A_City_TB.Text.Length > 0)
            {
                AthleteSQL += " AND A.[City]='" + A_City_TB.Text + "'";
            }

            if (A_State_TB.Text.Length > 0)
            {
                AthleteSQL += " AND A.[State]='" + A_State_TB.Text + "'";
            }

            if (A_DateLastContacted_TB.Text.Length > 0)
            {
                AthleteSQL += " AND A.[Date Last Contacted]<=" + A_DateLastContacted_TB.Text + " ";
            }

            if (A_SkillRating_DD.Text.Length > 0)
            {
                AthleteSQL += " AND A.[Skill Rating]='" + A_SkillRating_DD.Text + "' ";
            }

            if (A_PrimaryWhitworthCoach_DD.Text.Length > 0)
            {
                AthleteSQL += " AND C.[Last Name]='" + A_PrimaryWhitworthCoach_DD.Text + "'";
            }

            if (A_Event_DD.Text.Length > 0)
            {
                AthleteSQL += " AND E.[Event Name]='" + A_Event_DD.Text + "'";
            }

            //textBox11.Text = AthleteSQL;

            var tmp = from a in database.Athletes
                                             from c in database.Coaches
                                             from ev in database.Events
                                             from ae in database.Ath_Events
                                             where a.Coach_id == c.Coach_id && ev.Event_id == ae.Event_Id
                                             && ae.Athlete_Id == a.Athlete_id
                                             select a;

            athleteDataGridView.DataSource = tmp;

    //AthleteSQL += "WHERE A.[Coach_id]=C.[Coach_id] 
            //AND E.[Event_id]=AE.[Event_id] 
            //AND AE.[Athlete_id]=A.[Athlete_id]";

            
        }

        private void N_Search_B_Click(object sender, EventArgs e)
        {
            string NotesSQL;
            NotesSQL = "SELECT DISTINCT A.[First Name], A.[Last Name], NT.[Note Type Name], RN.[Date of Note], RN.[Note], C.[Last Name] AS [Written By] ";
            NotesSQL += "FROM Athlete A, Coach C, Event E, Note_Type NT, Recruit_Notes RN ";
            NotesSQL += "WHERE C.[Coach_id]=RN.[Coach_id] AND A.[Athlete_id]=RN.[Athlete_id] AND NT.[Note_type_id]=RN.[Note_type_id] AND A.[Coach_id]=C.[Coach_id]";

            if (N_FirstName_TB.Text.Length > 0)
            {
                NotesSQL += " AND A.[First Name]='" + N_FirstName_TB.Text + "'";
            }

            if (N_LastName_TB.Text.Length > 0)
            {
                NotesSQL += " AND A.[Last Name]='" + N_LastName_TB.Text + "'";
            }

            if (N_NoteType_DD.Text.Length > 0)
            {
                NotesSQL += " AND NT.[Note Type Name]='" + N_NoteType_DD.Text + "'";
            }

            textBox11.Text = NotesSQL;

        }

        private void main_FM_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'recruit_DatabaseDataSet.Athlete' table. You can move, or remove it, as needed.
            //this.athleteTableAdapter.Fill(this.recruit_DatabaseDataSet.Athlete);
            // TODO: This line of code loads data into the 'recruit_DatabaseDataSet.Coach' table. You can move, or remove it, as needed.
            //this.coachTableAdapter.Fill(this.recruit_DatabaseDataSet.Coach);
            // TODO: This line of code loads data into the 'recruit_DatabaseDataSet.Event' table. You can move, or remove it, as needed.
            //this.eventTableAdapter.Fill(this.recruit_DatabaseDataSet.Event);

        }

        private void Data_View_A_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

    }
}
