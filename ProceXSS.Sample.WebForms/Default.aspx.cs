using System;

namespace ProceXSS.Sample.WebForms
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Request.Form["txtXss"]!=null)
            {
                Response.Write(Request.Form["txtXss"]);
            }
        }
    }
}
