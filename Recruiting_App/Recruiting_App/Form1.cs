using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Recruiting_App
{
    public partial class Form1 : Form
    {
        Database_mapDataContext database = new Database_mapDataContext();

        public Form1()
        {
            InitializeComponent();

            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //ddPrimaryWhitworthCoach.DataSource = new Coach().Last_Name;

        }

        private void AthleteSearch_BTN_Click(object sender, EventArgs e)
        {
            DateTime contact;

            if (txtDateLastContacted.Text.Length > 0)
            {
                contact = Convert.ToDateTime(txtDateLastContacted.Text);      //if there is text in the date last contacted field, convert it to type DateTime (need to add data validation)
            }
            else
            {
                contact = DateTime.MinValue;                                  //if not date inputed then set date to minimum value
            }

            Athlete_dataview.DataSource = (from a in database.Athletes
                                          from c in database.Coaches
                                          from ev in database.Events
                                          from ae in database.Ath_Events
                                          where (txtFName.Text.Length == 0 || txtFName.Text == a.First_Name) &&            
                                                (txtLName.Text.Length == 0 || txtLName.Text == a.Last_Name) &&
                                                (txtHighSchool.Text.Length == 0 || txtHighSchool.Text == a.High_School) &&
                                                (txtCity.Text.Length == 0 || txtCity.Text == a.City) &&
                                                (txtState.Text.Length == 0 || txtState.Text == a.State) &&
                                                (ddSkillRating.Text.Length == 0 || ddSkillRating.Text == a.Skill_Rating) &&
                                                (ddClass.Text.Length == 0 || ddClass.Text == a.Class) &&
                                                (ddPrimaryWhitworthCoach.Text.Length == 0 || ddPrimaryWhitworthCoach.Text == c.Last_Name) &&
                                                (ddEvent.Text.Length == 0 || ddEvent.Text == ev.Event_Name) &&
                                                (a.Coach_id == c.Coach_id) &&
                                                (ev.Event_id == ae.Event_Id) &&
                                                (ae.Athlete_Id == a.Athlete_id) &&
                                                (txtDateLastContacted.Text.Length == 0 || contact >= a.Date_Last_Contacted) //our user wants to query for all athletes who have not been contacted
                                                                                                                            //since the inputed date.
                                                //the or statements make it so that the user can choose what he wants to query over. If he leaves a textbox blank, then the first part of the or
                                                //will return true. If text is entered in the textbox, then we want to query for results that match the inputed text.

                                          select new //select clause to output the desired data
                                          {
                                              Name = String.Format("{0} {1}", a.First_Name, a.Last_Name), //combine first and last name into one Name column
                                              Skill_Rating = a.Skill_Rating,
                                              Date_Last_Contacted = a.Date_Last_Contacted,
                                              Class = a.Class,
                                              High_School = a.High_School,
                                              High_School_Coach = a.High_School_Coach,
                                              Address = String.Format("{0} {1}, {2}", a.Address, a.City, a.State),
                                              Phone = a.Phone_Number,
                                              Email = a.Email,
                                              Height = a.Height,
                                              Weight = a.Weight,
                                              Whitworth_Coach = c.Last_Name,
                                          }).Distinct(); //without asking for only Distinct athletes, those athletes who are listed in the Ath_Event table for multiple events will 
                                                         //be displayed by this query multiple times

        }

        private void N_search_b_Click(object sender, EventArgs e)
        {
                Note_dataview.DataSource = (from a in database.Athletes
                                           from c in database.Coaches
                                           from nt in database.Note_Types
                                           from rn in database.Recruit_Notes
                                           where (N_txtFname.Text.Length == 0 || N_txtFname.Text == a.First_Name) &&
                                                 (N_txtLname.Text.Length == 0 || N_txtLname.Text == a.Last_Name) &&
                                                 (N_ddNoteType.Text.Length == 0 || N_ddNoteType.Text == nt.Note_Type_Name) && 
                                                 (c.Coach_id == rn.Coach_id) &&
                                                 (a.Athlete_id == rn.Athlete_id) &&
                                                 (nt.Note_Type_id == rn.Note_type_id) //&&
                                                 //(a.Coach_id == c.Coach_id)
                                                 //similar to the athlete query, we want the user to be able to enter whichever field he wants, and leave others blank
                                           select new
                                           {
                                               Name = String.Format("{0} {1}", a.First_Name, a.Last_Name),
                                               Note_Type = nt.Note_Type_Name,
                                               Date_of_Note = rn.Date_of_Note,
                                               Note = rn.Note,
                                               Written_By = c.Last_Name,
                                           }).Distinct();
        }

        private void Note_dataview_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            note_TB.Text = Note_dataview.Rows[e.RowIndex].Cells[3].Value.ToString(); //when the user clicks on a note in the Note Query form, that note will show up in the text box
                                                                                     //an idea is to do something similiar in the athlete query form, where a user can click on an athlete
                                                                                     //and populate a text box with the events they are being recruited for, or a particular type of note
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Validate();
            recruit_NoteBindingSource.EndEdit();
            database.SubmitChanges();
            //updates the database to reflect user changes/additions/deletions of recruit notes
        }

        private void atheditSearch_Click(object sender, EventArgs e)
        {
            athleteBindingSource.DataSource = from a in database.Athletes
                                              where (atheditTxtFname.Text.Length == 0 || atheditTxtFname.Text == a.First_Name) &&
                                                    (atheditTxtLname.Text.Length == 0 || atheditTxtLname.Text == a.Last_Name)
                                              orderby a.Athlete_id
                                              select a;
            //populates Athletes gridview by querying for athletes of particular name (or fields can be left blank)
        }

        private void noteeditSearch_Click(object sender, EventArgs e)
        {
            recruit_NoteBindingSource.DataSource = from n in database.Recruit_Notes
                                                   from a in database.Athletes
                                                   where (noteeditFname.Text.Length == 0 || noteeditFname.Text == a.First_Name) &&
                                                         (noteeditLname.Text.Length == 0 || noteeditLname.Text == a.Last_Name) &&
                                                         (a.Athlete_id == n.Athlete_id)
                                                         orderby n.Note_id
                                                   select n;
            //populates Recruit Notes gridview by querying for athletes of particular name (or fields can be left blank)
        }

        private void atheditUpdate_Click(object sender, EventArgs e)
        {
            Validate();
            athleteBindingSource.EndEdit();
            database.SubmitChanges();
            //updates the database to reflect user changes/additions/deletions of recruit notes
        }

        private void ath_eventSearch_Click(object sender, EventArgs e)
        {
            ath_EventBindingSource.DataSource = from ae in database.Ath_Events
                                                from a in database.Athletes
                                                from ev in database.Events
                                                where (ath_eventFname.Text.Length == 0 || ath_eventFname.Text == a.First_Name) &&
                                                         (ath_eventLname.Text.Length == 0 || ath_eventLname.Text == a.Last_Name) &&
                                                         (a.Athlete_id == ae.Athlete_Id) &&
                                                         (ev.Event_id == ae.Event_Id)
                                                orderby ae.Athlete_Id
                                                select ae;
            //populates Ath_Event datagrid Recruit Notes gridview by querying for athletes of particular name (or fields can be left blank)
                                                
        }

        private void ath_eventUpdate_Click(object sender, EventArgs e)
        {
            Validate();
            ath_EventBindingSource.EndEdit();
            database.SubmitChanges();
            //updates the database to reflect user changes/additions/deletions of recruit notes
            //NOTE: an athlete will not show up in the Athlete Query if they are not associated with an event in the ath_event table (they will still show up in the Edit athlete tables)
        }
    }
}
