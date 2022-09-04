using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RIK_Prooviülesanne__Taavi_Lepiko.Pages.Participants
{
    public class AddParticipantsModel : PageModel
    {
        public List<ParticipantInfo> listParticipants = new List<ParticipantInfo>();

        public EventInfo eventInfo = new EventInfo();
        public ParticipantInfo participantInfo = new ParticipantInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                String connectionString = "Data Source=(localdb)\\BluePrismLocalDB;Initial Catalog=Prooviylesanne;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Events WHERE EID = @eventId";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@eventId", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                eventInfo.EID = "" + reader.GetInt32(0);
                                eventInfo.ENAME = reader.GetString(1);
                                eventInfo.ETIME = reader.GetDateTime(2).ToString();
                                eventInfo.ELOCATION = reader.GetString(3);
                                eventInfo.EEXTRAINFO = reader.GetString(4);

                            }
                        }
                    }
                    String sql2 = "SELECT * FROM Participants WHERE EID = @eventId";
                    using (SqlCommand command = new SqlCommand(sql2, connection))
                    {
                        command.Parameters.AddWithValue("@eventId", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ParticipantInfo participantInfo = new ParticipantInfo();
                                participantInfo.PID = "" + reader.GetInt32(0);
                                participantInfo.PNAME = reader.GetString(1);
                                participantInfo.PCODE = "" + reader.GetInt64(3);

                                listParticipants.Add(participantInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
            participantInfo.PNAME = Request.Form["participantName"];
            participantInfo.PCODE = Request.Form["participantCode"];
            participantInfo.PPAYMENT = Request.Form["participantPayment"];
            participantInfo.PINFO = Request.Form["participantExtraInfo"];
            participantInfo.EID = Request.Query["id"];

            if (participantInfo.PNAME.Length == 0)
            {
                errorMessage = "Kõik väljad tuleb täita";
                return;
            }

            try
            {
                String connectionString = "Data Source=(localdb)\\BluePrismLocalDB;Initial Catalog=Prooviylesanne;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO Participants " +
                                 "(PNAME, PCODE, PPAYMENT, PINFO, EID) VALUES " +
                                 "(@participantName, @participantCode, @ParticipantPayment, @participantExtraInfo, @eventId);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@participantName", participantInfo.PNAME);
                        command.Parameters.AddWithValue("@participantCode", participantInfo.PCODE);
                        command.Parameters.AddWithValue("@participantPayment", participantInfo.PPAYMENT);
                        command.Parameters.AddWithValue("@participantExtraInfo", participantInfo.PINFO);
                        command.Parameters.AddWithValue("@eventId", Int32.Parse(participantInfo.EID));

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            String id = Request.Query["id"];
            Response.Redirect("/Participants/AddParticipants?id=" + @id);
        }
    }
}
