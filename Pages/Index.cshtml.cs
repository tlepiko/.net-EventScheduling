using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RIK_Prooviülesanne__Taavi_Lepiko.Pages
{
    public class IndexModel : PageModel
    {
        public List<EventInfo> listEvents = new List<EventInfo>();
        public List<EventInfo> listEvents2 = new List<EventInfo>();

        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=(localdb)\\BluePrismLocalDB;Initial Catalog=Prooviylesanne;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Events";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            int i = 1;
                            int j = 1;
                            while (reader.Read())
                            {
                                EventInfo eventInfo = new EventInfo();
                                eventInfo.EID = "" + reader.GetInt32(0);
                                eventInfo.ENAME = reader.GetString(1);
                                eventInfo.ETIME = reader.GetDateTime(2).ToString();
                                eventInfo.ELOCATION = reader.GetString(3);
                                eventInfo.EEXTRAINFO = reader.GetString(4);

                                if (DateTime.Parse(eventInfo.ETIME) > DateTime.Now)
                                {
                                    eventInfo.queueNumber = i;
                                    listEvents.Add(eventInfo);
                                    i++;
                                }
                                else
                                {
                                    eventInfo.queueNumber = j;
                                    listEvents2.Add(eventInfo);
                                    j++;
                                }


                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception" + ex.ToString());

            }
        }
    }

    public class EventInfo
    {
        public String EID = "";
        public String ENAME = "";
        public String ETIME = "";
        public String ELOCATION = "";
        public String EEXTRAINFO = "";
        public int queueNumber;
        public int participants;

    }

    public class ParticipantInfo
    {
        public String PID = "";
        public String PNAME = "";
        public String EID = "";
        public String PCODE = "";
        public String PPAYMENT = "";
        public String PINFO = "";
    }
}
