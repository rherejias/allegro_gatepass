using Excel;
using GatePass.Models;
using GatePass.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;

namespace GatePass.Helpers
{
    public class UploaderHelper
    {
        private Dictionary<string, object> response = new Dictionary<string, object>();
        private CustomHelper custom_helper = new CustomHelper();
        public static string FileName { get; set; }
        public Dictionary<string, object> Uploader(HttpFileCollectionBase files, string dir_path, string browser)
        {
            response = new Dictionary<string, object>();
            List<string> filenames = new List<string>();
            try
            {
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    string fname;
                    //string newFname = "";
                    // Checking for Internet Explorer  
                    if (browser == "IE" || browser == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                        //newFname = fname.Replace("#", "_");
                    }

                    // Get the complete folder path and store the file inside it.  
                    //string pathFname = Path.Combine(HttpContext.Current.Server.MapPath("~/" + dir_path), fname);
                    string pathFname = Path.Combine(dir_path, fname);

                    file.SaveAs(pathFname);

                    //rename recently uploaded file
                    string newFileName = custom_helper.ConvertToTimestamp(DateTime.Now) + "_" + fname;
                    string a = pathFname;
                    //string b = Path.Combine(HttpContext.Current.Server.MapPath("~/" + dir_path), newFileName);
                    string b = Path.Combine(dir_path, newFileName);
                    File.Move(a, b);

                    filenames.Add(b);
                    FileName = newFileName;
                }
                // Returns message that successfully uploaded  
                response.Add("success", true);
                response.Add("error", false);
                response.Add("message", "File Uploaded Successfully!");
                response.Add("files", filenames);
            }
            catch (Exception e)
            {
                response.Add("success", false);
                response.Add("error", true);
                response.Add("message", "Upload failed! " + e.ToString());
            }
            return response;
        }

        public Dictionary<string, object> UploadTargets(string path_and_file)
        {
            try
            {
                //variable for logs
                int success = 0;
                int fail = 0;
                int total = 0;

                //get current workweek and year
                //di ko sure kung kailangan to
                //List<int> curr_ww = workweek_helper.GetCurrentWorkWeek();
                //int current_workweek = curr_ww[0];
                //int current_year = curr_ww[1];

                response = new Dictionary<string, object>();
                //kailangan ko to
                SupplierModels t = new SupplierModels();

                FileStream stream = File.Open(path_and_file, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader = null;
                string ext = Path.GetExtension(stream.Name);
                if (ext == ".xls")
                {
                    //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (ext == ".xlsx")
                {
                    //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                else
                {
                    throw new Exception("Invalid file type.");
                }

                //Choose one of either 3, 4, or 5
                //3. DataSet - The result of each spreadsheet will be created in the result.Tables
                //DataSet result = excelReader.AsDataSet();

                //4. DataSet - Create column names from first row
                excelReader.IsFirstRowAsColumnNames = true;
                DataSet result = excelReader.AsDataSet();

                //5. Data Reader methods
                DateTime dateCreated = DateTime.Now;
                int ctr = 0;
                string log_output = "";

                ////override function
                //if (allowOverride)
                //{
                //    t.DeleteTestTargets("WW" + current_workweek.ToString().PadLeft(2, '0'), current_year);
                //}

                while (excelReader.Read())
                {
                    if (ctr >= 1)
                    {
                        SupplierObject targets = new SupplierObject();

                        targets.Name = excelReader.IsDBNull(0) ? string.Empty : excelReader.GetString(0);
                        targets.Email = excelReader.IsDBNull(1) ? string.Empty : excelReader.GetString(1);
                        targets.ContactNbr = excelReader.IsDBNull(2) ? string.Empty : excelReader.GetString(2);
                        targets.UnitNbr = excelReader.IsDBNull(3) ? string.Empty : excelReader.GetString(3);
                        targets.StreetName = excelReader.IsDBNull(4) ? string.Empty : excelReader.GetString(4);
                        targets.Municipality = excelReader.IsDBNull(5) ? string.Empty : excelReader.GetString(5);
                        targets.City = excelReader.IsDBNull(6) ? string.Empty : excelReader.GetString(6);
                        targets.Country = excelReader.IsDBNull(7) ? string.Empty : excelReader.GetString(7);
                        targets.Zip = excelReader.IsDBNull(8) ? 0 : Convert.ToInt32(excelReader.GetString(8));
                        targets.ImpexRefNbr = excelReader.IsDBNull(9) ? string.Empty : excelReader.GetString(9);
                        targets.IsActive = true;
                        targets.AddedBy = Convert.ToInt32(HttpContext.Current.Session["userId_local"]);
                        targets.DateAdded = dateCreated;


                        t.SaveNewSupplier(targets);
                        total++;
                        success++;
                        //buburahin ko to titira ko lang "t.SaveTargets(targets)"
                        //if (targets.WorkWeek != "WW" + current_workweek.ToString().PadLeft(2, '0') || targets.FiscalYear != current_year)
                        //{
                        //    fail++;
                        //    log_output += "<p><span class='label label-danger'>[" + targets.ProductGroup + "] [" + targets.PackageGroupA + "] [" + targets.PackageGroupB + "] [" + targets.Target + "] [" + targets.WorkWeek + "] [" + targets.FiscalYear + "] = work week or year mismatch</span></p>";
                        //}
                        //else
                        //{
                        //    if (t.CheckExistingTestTarget(targets) > 0)
                        //    {
                        //        fail++;
                        //        log_output += "<p><span class='label label-danger'>[" + targets.ProductGroup + "] [" + targets.PackageGroupA + "] [" + targets.PackageGroupB + "] [" + targets.Target + "] [" + targets.WorkWeek + "] [" + targets.FiscalYear + "] = already exists</span></p>";
                        //    }
                        //    else
                        //    {
                        //        if (!t.SaveTargets(targets))
                        //        {
                        //            fail++;
                        //            log_output += "<p><span class='label label-danger'>[" + targets.ProductGroup + "] [" + targets.PackageGroupA + "] [" + targets.PackageGroupB + "] [" + targets.Target + "] [" + targets.WorkWeek + "] [" + targets.FiscalYear + "] = failed</span></p>";
                        //        }
                        //        else
                        //        {
                        //            hasSuccess = true;
                        //            success++;
                        //            log_output += "<p><span class='label label-success'>[" + targets.ProductGroup + "] [" + targets.PackageGroupA + "] [" + targets.PackageGroupB + "] [" + targets.Target + "] [" + targets.WorkWeek + "] [" + targets.FiscalYear + "] = added</span></p>";
                        //        }
                        //    }
                        //}
                        //hanggang dito
                        //total++;
                    }

                    ctr++;
                }

                //if (hasSuccess)
                //{
                //    t.RefreshData();
                //}

                //6. Free resources (IExcelDataReader is IDisposable)
                excelReader.Close();

                Dictionary<string, int> stats = new Dictionary<string, int>();
                stats.Add("success", success);
                stats.Add("fail", fail);

                Dictionary<string, string> msg = new Dictionary<string, string>();

                if (total <= 0)
                {
                    throw new Exception("Uploaded file is empty or is in the wrong format. Kindly check the guidelines for uploading.");
                }
                else
                {
                    if (success == total)
                    {
                        msg.Add("title", "Uploading Successful!");
                        msg.Add("body", "New targets uploaded successfully!");
                        msg.Add("type", "success");
                    }
                    else
                    {
                        msg.Add("title", "Uploading Unsuccessful!");
                        msg.Add("body", success + " / " + total + " uploaded successfully!");
                        msg.Add("type", "danger");
                    }
                }

                response.Add("success", true);
                response.Add("error", false);
                response.Add("stats", stats);
                response.Add("logs", log_output);
                response.Add("message", msg);
            }
            catch (Exception e)
            {
                response.Add("success", false);
                response.Add("error", true);
                response.Add("message", e.ToString());
            }

            return response;
        }

        //public Dictionary<string, object> UploadImages(string path_and_file)
        //{
        //    TransactionModels trans_models = new TransactionModels();
        //    try
        //    {
        //        response = new Dictionary<string, object>();

        //        FileStream stream = File.Open(path_and_file, FileMode.Open, FileAccess.Read);
        //        string ext = Path.GetExtension(stream.Name);
        //        string ext2 = Path.GetFileName(stream.Name);
        //        if (ext == ".png")
        //        {
        //            trans_models.uploadImage(ext2);
        //        }
        //        else if (ext == ".jpg")
        //        {
        //            trans_models.uploadImage(ext2);
        //        }
        //        else
        //        {
        //            throw new Exception("Invalid file type.");
        //        }
        //        response.Add("success", true);
        //        response.Add("error", false);
        //    }
        //    catch (Exception e)
        //    {
        //        response.Add("success", false);
        //        response.Add("error", true);
        //        response.Add("message", e.ToString());
        //    }

        //    return response;
        //}
    }
}