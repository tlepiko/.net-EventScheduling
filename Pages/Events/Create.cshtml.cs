using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RIK_Prooviülesanne__Taavi_Lepiko.Pages.Events
{
    public class CreateModel : PageModel
    {
        public EventInfo eventInfo = new EventInfo();

        public String errorMessage = "";

        public String successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            eventInfo.ENAME = Request.Form["eventName"];
            eventInfo.ETIME = Request.Form["eventTime"];
            eventInfo.ELOCATION = Request.Form["eventLocation"];
            eventInfo.EEXTRAINFO = Request.Form["eventExtraInfo"];

            if (eventInfo.ENAME.Length == 0 || eventInfo.ELOCATION.Length == 0 || eventInfo.EEXTRAINFO.Length == 0)
            {
                errorMessage = "Kõik väljad peavad olema täidetud";
                return;
            }
            if (DateTime.Parse(eventInfo.ETIME) < DateTime.Now)
            {
                errorMessage = "Üritusi saab broneerida vaid tuleviku kuupäevaga!";
                return;
            }

            try
            {
                String connectionString = "Data Source=(localdb)\\BluePrismLocalDB;Initial Catalog=Prooviylesanne;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO Events " +
                                 "(ENAME, ETIME, ELOCATION, EEXTRAINFO) VALUES " +
                                 "(@eventName, @eventTime, @eventLocation, @eventExtraInfo);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@eventName", eventInfo.ENAME);
                        command.Parameters.AddWithValue("@eventTime", DateTime.Parse(eventInfo.ETIME));
                        command.Parameters.AddWithValue("@eventLocation", eventInfo.ELOCATION);
                        command.Parameters.AddWithValue("@eventExtraInfo", eventInfo.EEXTRAINFO);

                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception)
            {

                errorMessage = "Sellise nimega üritus on juba olemas.";
                return;
            }

            eventInfo.ENAME = ""; eventInfo.ETIME = ""; eventInfo.ELOCATION = ""; eventInfo.EEXTRAINFO = "";
            successMessage = "Uus üritus on lisatud!";

            Response.Redirect("/Index");
        }
    }

    public class EventInfo
    {
        public String EID = "";
        public String ENAME = "";
        public String ETIME = "";
        public String ELOCATION = "";
        public String EEXTRAINFO = "";

    }
}
