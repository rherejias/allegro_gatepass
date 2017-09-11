namespace GatePass.Jobs
{
    public class Foo : IJob
    {
        private EmailHelper email_helper = new EmailHelper();
        public void Execute(IJobExecutionContext context)
        {
            TransactionModels trans_model = new TransactionModels();
            DataTable dt = trans_model.ReadOverdueTransactionHeaders();

            foreach (DataRow row in dt.Rows)
            {
                //get header details
                string email_content = "<table style='width:100%; font-size:14; font-family:Arial, Helvetica, sans-serif;'>" +
                                                "<tr>" +
                                                    "<td style='background-color:#cccccc; padding:5px;'>Code</td>" +
                                                    "<td>" +
                                                        row["Code"] +
                                                    "</td>" +
                                                "</tr>" +
                                                "<tr>" +
                                                    "<td style='background-color:#cccccc; padding:5px;'>Requestor</td>" +
                                                    "<td>" +
                                                        row["Column1"] +
                                                    "</td>" +
                                                "</tr>" +
                                                "<tr>" +
                                                    "<td style='background-color:#cccccc; padding:5px;'>Purpose</td>" +
                                                    "<td>" +
                                                        row["Purpose"] +
                                                    "</td>" +
                                                "</tr>" +
                                                "<tr>" +
                                                    "<td style='background-color:#cccccc; padding:5px;'>Return Date</td>" +
                                                    "<td>" +
                                                        row["ReturnDate"] +
                                                    "</td>" +
                                                "</tr>" +
                                            "</table>";

                //get details
                email_content += "<br /><br /><table style='width:100%; font-size:14; font-family:Arial, Helvetica, sans-serif; border: 1px solid #12AFCB; border-collapse: collapse; font-size:12;' cellpadding='8'>" +
                                        "<tr>" +
                                            "<td style='border: 1px solid #12AFCB; border-collapse: collapse; background-color:#12AFCB; color:#ffffff'><strong>Item Name</strong></td>" +
                                            "<td style='border: 1px solid #12AFCB; border-collapse: collapse; background-color:#12AFCB; color:#ffffff'><strong>Quantity</strong></td>" +
                                            "<td style='border: 1px solid #12AFCB; border-collapse: collapse; background-color:#12AFCB; color:#ffffff'><strong>Item Type</strong></td>" +
                                            "<td style='border: 1px solid #12AFCB; border-collapse: collapse; background-color:#12AFCB; color:#ffffff'><strong>Status</strong></td>" +
                                        "</tr>";
                DataTable dt_items = trans_model.ReadOverdueTransactionItems(row["Code"].ToString());
                foreach (DataRow row_items in dt_items.Rows)
                {
                    string unreturned_style = (row_items["ReturnSlipStatus"].ToString() == "Not Returned") ? "background-color:#F25656; " : "";
                    email_content += "<tr>" +
                                        "<td style='border: 1px solid #12AFCB; border-collapse: collapse;'>" + row_items["ItemName"] + "</td>" +
                                        "<td style='border: 1px solid #12AFCB; border-collapse: collapse;'>" + row_items["Quantity"] + " " + row_items["UOMName"] + "</td>" +
                                        "<td style='border: 1px solid #12AFCB; border-collapse: collapse;'>" + row_items["ItemType"] + "</td>" +
                                        "<td style='" + unreturned_style + "border: 1px solid #12AFCB; border-collapse: collapse;'>" + row_items["ReturnSlipStatus"] + "</td>" +
                                     "</tr>";
                }
                email_content += "</table><br /><br /><br />";

                string sender = "GatePass Notification ampinoreply@allegromicro.com";

                using (var message = new MailMessage(sender, row["Email"].ToString()))
                {
                    message.Subject = "Items Not Returned!";
                    message.Body = email_helper.EmailTemplate(email_content);
                    message.IsBodyHtml = true;
                    message.Bcc.Add("aabasolo@allegromicro.com");
                    using (SmtpClient client = new SmtpClient
                    {
                        EnableSsl = false,
                        Host = "maoutlook.allegro.msad",
                        Port = 25,
                        Credentials = new NetworkCredential("ampinoreply@allegromicro.com", "@welcome1")
                    })
                    {
                        client.Send(message);
                    }
                }
            }

        }
    }
}