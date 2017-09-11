using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace GatePass.Library
{
    public class DataLayerLibrary
    {
        public partial class DataLayer
        {

            /* 
             * One part of this class is to check whether the connectionstrings are valid 
             * and I hoping that the validation are smart enough to know what kind of connectionstring
             * did the developer passed. It has feature of parsing the connectionstring and check the connectionstrig
             * if it is available on our app.config or web.config file
             */

            #region ConnectionStrings Validation

            /// <summary>
            /// First, it parses the passed connectionstring, if it is a valid connectionstring,
            /// the method will call another method that will try to connect to our database.
            /// However, if the first step fails it will try to see if the connectionstring is name of a configuration file
            /// then reads the connectionstring and tries again to connect to our server.
            /// </summary>
            /// <param name="strConnectionString">string->Name of ConnectionString or ConnectionString itself</param>
            /// <param name="strValidConnectionString">string->returns the valid ConnectionString for the application</param>

            private static void ValidateConnectionString(string strConnectionString, out string strValidConnectionString)
            {
                strValidConnectionString = string.Empty;
                string regexSplitToParts = "([^=;]*)=([^=;]*)";
                RegexOptions options = RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled;
                Regex reg = new Regex(regexSplitToParts, options);

                try
                {
                    if ((reg.IsMatch(strConnectionString, 0)) && (ValidateConnection(strConnectionString)))
                        strValidConnectionString = strConnectionString;
                    else
                    {
                        if (ValidateConnection(ConfigurationManager.ConnectionStrings[strConnectionString].ConnectionString))
                            strValidConnectionString = ConfigurationManager.ConnectionStrings[strConnectionString].ConnectionString;
                    }
                }
                catch (NullReferenceException)
                {
                    throw new Exception("Unable to find your application configuration file");
                }
                catch (Exception)
                {
                    throw;
                }
            }

            /// <summary>
            /// Tries to see if we can connect to our database.
            /// </summary>
            /// <param name="strConnectionString">string->ConnectionString</param>
            /// <returns>bool->returns true if connected else false</returns>

            private static bool ValidateConnection(string strConnectionString)
            {
                bool bRet = false;

                try
                {
                    using (SqlConnection cn = new SqlConnection(strConnectionString))
                    {
                        cn.Open();
                        bRet = true;
                    }
                }
                catch (InvalidOperationException)
                {
                    throw new ApplicationException("Unable to connect to our database\r\nPlease check your connectionstring configuration");
                }
                catch (SqlException)
                {
                    throw new ApplicationException("Unable to connect to SQL Server(s)\r\nPlease check your connectionstring(s)");
                }
                catch (Exception)
                {
                    throw;
                }

                return bRet;
            }

            #endregion
        }


        /* 
         * This part of the class is actually gives the developer the power to lessen their
         * coding time in the datalayer of the application. Methods here actually are the most 
         * commonly used methods when using ADO.NET. The main purpose of this to help developers
         * inside Jin Framework Solutions to lessen their time in focusing on coding and focus more
         * on NUNIT Testing of the datalayer side. Any comments, suggestion are welcome.
         * Any changes that will be made to our library needs a approval from the management.
         * 
         */

        public partial class DataLayer
        {
            private string strConnectionString; // valid connectionstring for the entire application

            public DataLayer(string _strConnectionStrings)
            {
                //validates connectionstring upon initialization of the object/class
                ValidateConnectionString(_strConnectionStrings, out this.strConnectionString);
            }

            #region Execute Methods (Insert, Update, Delete)

            public bool executeQuery(string strQuery, SqlParameter params_, CommandType type_)
            {
                bool bolRet = false;

                try
                {
                    using (SqlConnection cn = new SqlConnection(this.strConnectionString))
                    {
                        cn.Open();

                        using (SqlTransaction trnscntn = cn.BeginTransaction())
                        using (SqlCommand cmd = new SqlCommand(strQuery, cn, trnscntn))
                        {
                            try
                            {
                                cmd.CommandType = type_;
                                cmd.Parameters.Add(params_);
                                //Changed from 0 to 1 for DELETING WITHOUT RETURN
                                bolRet = (cmd.ExecuteNonQuery() >= 0 ? true : false);

                                trnscntn.Commit();
                            }
                            catch (InvalidOperationException)
                            {
                                trnscntn.Rollback();
                                throw;
                            }
                            catch (SqlException)
                            {
                                trnscntn.Rollback();
                                throw;
                            }
                        }
                    }
                }
                catch (SqlException)
                {
                    throw;
                }

                return bolRet;
            }

            public bool executeQuery(string strQuery, SqlParameter[] params_, CommandType type_)
            {
                bool bolRet = false;

                try
                {
                    using (SqlConnection cn = new SqlConnection(this.strConnectionString))
                    {
                        cn.Open();

                        using (SqlTransaction trnscntn = cn.BeginTransaction())
                        using (SqlCommand cmd = new SqlCommand(strQuery, cn, trnscntn))
                        {
                            try
                            {
                                cmd.CommandType = type_;
                                cmd.Parameters.AddRange(params_);

                                bolRet = (cmd.ExecuteNonQuery() >= 1 ? true : false);

                                trnscntn.Commit();
                            }
                            catch (InvalidOperationException)
                            {
                                trnscntn.Rollback();
                                throw;
                            }
                            catch (SqlException)
                            {
                                trnscntn.Rollback();
                                throw;
                            }
                        }
                    }
                }
                catch (SqlException)
                {
                    throw;
                }

                return bolRet;
            }

            public bool executeQuery(string strQuery, SqlParameter[] params_, CommandType type_, ref object objValue)
            {
                bool bolRet = false;

                try
                {
                    using (SqlConnection cn = new SqlConnection(this.strConnectionString))
                    {
                        cn.Open();

                        using (SqlTransaction trnscntn = cn.BeginTransaction())
                        using (SqlCommand cmd = new SqlCommand(strQuery, cn, trnscntn))
                        {
                            try
                            {
                                cmd.CommandType = type_;

                                cmd.Parameters.AddRange(params_);

                                bolRet = (cmd.ExecuteNonQuery() >= 1 ? true : false);

                                trnscntn.Commit();

                                foreach (SqlParameter prm in params_)
                                {
                                    if (prm.Direction == ParameterDirection.Output)
                                    {
                                        objValue = prm.Value;
                                        break;
                                    }
                                }

                            }
                            catch (InvalidOperationException)
                            {
                                trnscntn.Rollback();
                                throw;
                            }
                            catch (SqlException)
                            {
                                trnscntn.Rollback();
                                throw;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }

                return bolRet;
            }

            #endregion

            #region Read Methods


            public object executeScalarQuery(string strQuery, CommandType type_)
            {
                object objValue = null;

                try
                {
                    using (SqlConnection cn = new SqlConnection(this.strConnectionString))
                    {
                        cn.Open();

                        using (SqlCommand cmd = new SqlCommand(strQuery, cn))
                        {
                            cmd.CommandType = type_;
                            cmd.CommandTimeout = 300;
                            try
                            {
                                object objTemp = null;

                                objTemp = cmd.ExecuteScalar();

                                if ((!DBNull.Value.Equals(objTemp)) && (objTemp != null))
                                    objValue = objTemp;

                            }
                            catch (SqlException)
                            {
                                throw;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                return objValue;
            }


            public object executeScalarQuery(string strQuery, CommandType type_, SqlParameter params_)
            {
                object objValue = null;

                try
                {
                    using (SqlConnection cn = new SqlConnection(this.strConnectionString))
                    {
                        cn.Open();

                        using (SqlCommand cmd = new SqlCommand(strQuery, cn))
                        {
                            cmd.CommandType = type_;
                            cmd.CommandTimeout = 300;
                            cmd.Parameters.Add(params_);

                            try
                            {
                                object objTemp = null;

                                objTemp = cmd.ExecuteScalar();

                                if ((!DBNull.Value.Equals(objTemp)) && (objTemp != null))
                                    objValue = objTemp;

                            }
                            catch (SqlException)
                            {
                                throw;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                return objValue;
            }

            public object executeScalarQuery(string strQuery, CommandType type_, SqlParameter[] params_)
            {
                object objValue = null;

                try
                {
                    using (SqlConnection cn = new SqlConnection(this.strConnectionString))
                    {
                        cn.Open();

                        using (SqlCommand cmd = new SqlCommand(strQuery, cn))
                        {
                            cmd.CommandType = type_;
                            cmd.CommandTimeout = 300;
                            cmd.Parameters.AddRange(params_);

                            try
                            {
                                object objTemp = null;

                                objTemp = cmd.ExecuteScalar();

                                if ((!DBNull.Value.Equals(objTemp)) && (objTemp != null))
                                    objValue = objTemp;

                            }
                            catch (SqlException)
                            {
                                throw;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                return objValue;
            }

            public DataTable executeSelectQuery(string strQuery, CommandType type_)
            {
                DataTable dt = new DataTable("tblSelectedQuery");

                try
                {
                    using (SqlConnection cn = new SqlConnection(this.strConnectionString))
                    {
                        cn.Open();

                        using (SqlCommand cmd = new SqlCommand(strQuery, cn))
                        {
                            cmd.CommandType = type_;
                            cmd.CommandTimeout = 300;
                            using (SqlDataAdapter adptr = new SqlDataAdapter(cmd))
                                adptr.Fill(dt);
                        }
                    }
                }
                catch (SqlException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
                }

                return dt;
            }

            public DataTable executeSelectQuery(string strQuery, CommandType type_, SqlParameter params_)
            {
                DataTable dt = new DataTable("tblSelectedQuery");

                try
                {
                    using (SqlConnection cn = new SqlConnection(this.strConnectionString))
                    {
                        cn.Open();

                        using (SqlCommand cmd = new SqlCommand(strQuery, cn))
                        {
                            cmd.CommandType = type_;

                            cmd.Parameters.Add(params_);

                            using (SqlDataAdapter adptr = new SqlDataAdapter(cmd))
                                adptr.Fill(dt);
                        }
                    }
                }
                catch (SqlException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
                }

                return dt;
            }

            public DataTable executeSelectQuery(string strQuery, CommandType type_, SqlParameter[] params_)
            {
                DataTable dt = new DataTable("tblSelectedQuery");

                try
                {
                    using (SqlConnection cn = new SqlConnection(this.strConnectionString))
                    {
                        cn.Open();

                        using (SqlCommand cmd = new SqlCommand(strQuery, cn))
                        {
                            cmd.CommandType = type_;

                            cmd.Parameters.AddRange(params_);

                            using (SqlDataAdapter adptr = new SqlDataAdapter(cmd))
                                adptr.Fill(dt);
                        }
                    }
                }
                catch (SqlException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
                }

                return dt;
            }

            public DataTable executeSelectQueryWithOutput(string strQuery, CommandType type_, SqlParameter[] params_, ref object objValue)
            {
                DataTable dt = new DataTable("tblSelectedQuery");

                try
                {
                    using (SqlConnection cn = new SqlConnection(this.strConnectionString))
                    {
                        cn.Open();

                        using (SqlCommand cmd = new SqlCommand(strQuery, cn))
                        {
                            cmd.CommandType = type_;

                            cmd.Parameters.AddRange(params_);

                            using (SqlDataAdapter adptr = new SqlDataAdapter(cmd))
                                adptr.Fill(dt);

                            foreach (SqlParameter prm in params_)
                            {
                                if (prm.Direction == ParameterDirection.Output)
                                {
                                    objValue = prm.Value;
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (SqlException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
                }

                return dt;
            }

            #endregion
        }
    }
}