using System;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.OleDb;
using System.Data;




namespace DSNWebApp
{
    public partial class LaunchSpacecraft : System.Web.UI.Page
    {
        string launchVehicleName, payloadName, payloadType;
        int launchVehicleOrbit;
        bool isLaunchInProgess = false;
        DataTable launchVehicleTable, payloadTable;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /**
         * Extracts the data from 2 CSV files and sends the request to DSN Service to launch the spacecraft
         */
        protected void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (isLaunchInProgess)
                {
                    return;
                }
                isLaunchInProgess = true;
                launchVehicleTable = UploadFile(FileUpload1);
                payloadTable = UploadFile(FileUpload2);
                PopulateData();
                DSNService.Service1Client dsnService = new DSNService.Service1Client();
                Boolean response = dsnService.launchSpacecraft(launchVehicleName, launchVehicleOrbit, payloadName, payloadType);
                if (response)
                {
                    SuccessLabel.Text = "Spacecraft launched successfully";
                    FailedLabel.Text = "";
                }
                else
                {
                    FailedLabel.Text = "Error in launching spacecraft";
                    SuccessLabel.Text = "";
                }
                isLaunchInProgess = false;
            }
            catch (Exception ex)
            {
                FailedLabel.Text = "Error in launching spacecraft, please provide the correct launch vehicle and payload files";
                SuccessLabel.Text = "";
            }
        }

        /**
         * Populate spacecraft data
         */
        private void PopulateData()
        {
            try {
                PopulateLaunchVehicleData();
                PopulatePayloadData();
            }
            catch (Exception e) { 
            }
        }

        /**
         * Populate launch vehicle data
         */
        private void PopulateLaunchVehicleData()
        {
            launchVehicleName = launchVehicleTable.Rows[0]["name"].ToString();
            launchVehicleOrbit = Int32.Parse(launchVehicleTable.Rows[0]["orbit"].ToString());
        }

        /**
         * Populate payload data
         */
        private void PopulatePayloadData()
        {
            payloadName = payloadTable.Rows[0]["name"].ToString();
            payloadType = payloadTable.Rows[0]["type"].ToString();
        }

        /**
         * Uploads the file to the upload folder of Web Application
         */
        private DataTable UploadFile(FileUpload file)
        {
            try {

                if (file.HasFile)
                {
                    int flag = 0;
                    string fileName = Path.GetFileName(file.PostedFile.FileName);
                    string randomName = DateTime.Now.ToFileTime().ToString();
                    string folderPath = "~/upload/";
                    string filePath = Server.MapPath(folderPath + randomName + fileName);
                    string[] filenames = Directory.GetFiles(Server.MapPath("~/upload"));

                    if (filenames.Length > 0)
                    {
                        foreach (string filename in filenames)
                        {
                            if (filePath == filename)
                            {
                                flag = 1;
                                break;
                            }
                        }
                        if (flag == 0)
                        {
                            file.SaveAs(filePath);
                            return ReadCSVFile(filePath);
                        }
                    }
                    else
                    {
                        file.SaveAs(filePath);
                        return ReadCSVFile(filePath);
                    }
                }
                else
                {
                    // !!TODO: Update the user that file has to be imported
                    string msg = "Select a file then try to import";
                }
                return new DataTable();
            }
            catch (Exception e) {
                return new DataTable();
            }
            
        }

        /**
         * Reads the CSV File and returns the DataTable as the output
         */
        public DataTable ReadCSVFile(string fileName)
        {
            DataTable csvTable = new DataTable();
            try
            {
                string connection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}\\;Extended Properties='Text;HDR=Yes;FMT=CSVDelimited'";
                connection = String.Format(connection, Path.GetDirectoryName(fileName));
                OleDbDataAdapter csvAdapter;
                csvAdapter = new OleDbDataAdapter("SELECT * FROM [" + Path.GetFileName(fileName) + "]", connection);
                if (File.Exists(fileName) && new FileInfo(fileName).Length > 0)
                {
                    try
                    {
                        csvAdapter.Fill(csvTable);
                        return csvTable;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(String.Format("Error reading Table {0}.\n{1}", Path.GetFileName(fileName), ex.Message));
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return csvTable;
        }
    }
}