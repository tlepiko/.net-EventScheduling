using System.Data.SqlClient;
using System.Net.Security;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RIK_Prooviülesanne__Taavi_Lepiko.Pages.Participants
{
    public class ViewParticipantsModel : PageModel
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
        }
    }
}
