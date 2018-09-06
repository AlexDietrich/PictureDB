using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using BIF.SWE2.Interfaces;
using BIF.SWE2.Interfaces.Models;
using PicDB.Models;

namespace PicDB
{
    class DataAccessLayer : IDataAccessLayer
    {
        private string ConnectionString { get; set; }

        public DataAccessLayer()
        {
            ConnectionString = GlobalInformation.ConnectionString;
        }

        public IEnumerable<IPictureModel> GetPictures(string namePart, IPhotographerModel photographerParts, IIPTCModel iptcParts,
            IEXIFModel exifParts)
        {
            var queryString =
                $"Select ID, FileName, fk_IPTC, fk_EXIF, fk_Camera, fk_Photographer from PictureModel";
            var liste = new List<IPictureModel>();

            using (var connection = new SqlConnection(ConnectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = queryString;

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var model = new PictureModel();
                        int.TryParse(reader["ID"].ToString(), out var tempInt);
                        model.ID = tempInt;
                        model.FileName = reader["FileName"].ToString();
                        model.IPTC = new IPTCModel();
                        model.EXIF = new EXIFModel();
                        model.Camera = reader["fk_Camera"] == DBNull.Value ? null : new CameraModel();
                        model.Photographer = reader["fk_Photographer"] == DBNull.Value ? null : new PhotographerModel();
                        liste.Add(model);
                    }
                }
            }
            return liste;
        }

        public IPictureModel GetPicture(int ID)
        {
            var picture = new PictureModel
            {
                IPTC = new IPTCModel(),
                EXIF = new EXIFModel()
            };
            var camID = -1;
            var photogrID = -1;
            var query = $"select PictureModel.ID, FileName, EXIFModel.Make, FNumber, ExposureTime, ISOValue, Flash, ExposureProgram, Keywords, ByLine, " +
                            $"CopyrightNotice, Headline, Caption, fk_Camera, fk_Photographer " +
                         $"from PictureModel " +
                         $"inner join EXIFModel on fk_EXIF = ExifModel.ID " +
                         $"inner join IPTCModel on fk_IPTC = IPTCModel.ID " +
                         $"where PictureModel.ID = @ID;";

            var idParameter = new SqlParameter("@ID", SqlDbType.Int) { Value = ID };

            using (var connection = new SqlConnection(ConnectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = query;
                command.Parameters.Add(idParameter); //Füge Parameter für Prepared Statemend zu den Parameter hinzu
                command.Prepare(); //Erstelle das Prepared statement
                using (var reader = command.ExecuteReader())
                {
                    reader.Read();

                    picture.ID = ID;
                    picture.FileName = reader["FileName"].ToString(); //NOT NULL in der Datenbank

                    //Speicher die IDs für späteres hinzufügen zur Instanz vom PictureModel
                    if (reader["fk_Camera"] != DBNull.Value) camID = int.Parse(reader["fk_Camera"].ToString()); //NOT NUll in der Datenbank
                    if (reader["fk_Photographer"] != DBNull.Value) photogrID = int.Parse(reader["fk_Photographer"].ToString()); //Not Null in der Datenbank

                    if (reader["ByLine"] != DBNull.Value) picture.IPTC.ByLine = reader["ByLine"].ToString();
                    if (reader["Caption"] != DBNull.Value) picture.IPTC.Caption = reader["Caption"].ToString();
                    if (reader["CopyrightNotice"] != DBNull.Value) picture.IPTC.CopyrightNotice = reader["CopyrightNotice"].ToString();
                    if (reader["Headline"] != DBNull.Value) picture.IPTC.Headline = reader["Headline"].ToString();
                    if (reader["Keywords"] != DBNull.Value) picture.IPTC.Keywords = reader["Keywords"].ToString();

                    if (reader["ExposureProgram"] != DBNull.Value) picture.EXIF.ExposureProgram = (ExposurePrograms)int.Parse(reader["ExposureProgram"].ToString());
                    if (reader["ExposureTime"] != DBNull.Value) picture.EXIF.ExposureTime = decimal.Parse(reader["ExposureTime"].ToString());
                    if (reader["FNumber"] != DBNull.Value) picture.EXIF.FNumber = decimal.Parse(reader["FNumber"].ToString());
                    if (reader["Flash"] != DBNull.Value) picture.EXIF.Flash = bool.Parse(reader["Flash"].ToString());
                    if (reader["ISOValue"] != DBNull.Value) picture.EXIF.ISOValue = decimal.Parse(reader["ISOValue"].ToString());
                    if (reader["Make"] != DBNull.Value) picture.EXIF.Make = reader["Make"].ToString();
                }
            }

            if (camID != -1) picture.Camera = GetCamera(camID);
            else picture.Camera = new CameraModel();
            if (photogrID != -1) picture.Photographer = GetPhotographer(photogrID);
            else picture.Photographer = new PhotographerModel();


            return picture;
        }

        public void Save(IPictureModel picture)
        {
            if (Exists(picture))
            {
                using (var connection = new SqlConnection(ConnectionString))
                using (var command = connection.CreateCommand())
                {

                    //get IPTC FK
                    var fk_IPTC = 0;
                    connection.Open();
                    command.CommandText = "Select fk_IPTC FROM PictureModel " +
                        "Where ID = @id;";

                    var idParam = new SqlParameter("@id", SqlDbType.Int) { Value = picture.ID };
                    command.Parameters.Add(idParam);

                    // Call Prepare after setting the Commandtext and Parameters.
                    command.Prepare();

                    // Change parameter values and call ExecuteNonQuery.
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        fk_IPTC = int.Parse(reader["fk_IPTC"].ToString());
                    }


                    // Create and prepare an SQL statement.
                    command.CommandText =
                    "UPDATE dbo.IPTCModel "
                    + "SET Keywords = @keywords, ByLine = @byline, CopyrightNotice = @copyrightnotice, Headline = @headline, Caption = @caption " +
                    "WHERE ID = @iptcid;";

                    var iptcidParam = new SqlParameter("@iptcid", SqlDbType.Int) { Value = fk_IPTC };
                    var keywordsParam = new SqlParameter("@keywords", SqlDbType.Text, 255) { Value = picture.IPTC.Keywords };
                    var bylineParam = new SqlParameter("@byline", SqlDbType.Text, 255) { Value = picture.IPTC.ByLine };
                    var copyrightnoticeParam = new SqlParameter("@copyrightnotice", SqlDbType.Text, 255) { Value = picture.IPTC.CopyrightNotice };
                    var headlineParam = new SqlParameter("@headline", SqlDbType.Text, 255) { Value = picture.IPTC.Headline };
                    var captionParam = new SqlParameter("@caption", SqlDbType.Text, 255) { Value = picture.IPTC.Caption };

                    command.Parameters.Add(iptcidParam);
                    command.Parameters.Add(keywordsParam);
                    command.Parameters.Add(bylineParam);
                    command.Parameters.Add(copyrightnoticeParam);
                    command.Parameters.Add(headlineParam);
                    command.Parameters.Add(captionParam);

                    // Call Prepare after setting the Commandtext and Parameters.
                    command.Prepare();

                    // Change parameter values and call ExecuteNonQuery.
                    command.ExecuteScalar();

                    var pictureModel = (PictureModel)picture;

                    //Wenn ein Photographer hinzugefügt wird, soll diese auch auf die richtige ID gesetzt werden
                    if (pictureModel.Photographer != null)
                    {
                        command.CommandText = "Update dbo.PictureModel SET fk_Photographer = @fk_photo WHERE ID = @id";
                        var paramCamID = new SqlParameter("@fk_photo", SqlDbType.Int) { Value = pictureModel.Photographer.ID };
                        
                        command.Parameters.Add(paramCamID);
                        command.Prepare();
                        command.ExecuteScalar();

                    }

                    //Wenn eine Camera hinzugefügt wird zum Bild, soll diese auch auf die richtige ID gesetzt werden
                    if (pictureModel.Camera != null)
                    {

                        command.CommandText = "Update dbo.PictureModel SET fk_Camera = @fk_Cam WHERE ID = @id";
                        var paramCamID = new SqlParameter("@fk_Cam", SqlDbType.Int) { Value = pictureModel.Camera.ID };
                        
                        command.Parameters.Add(paramCamID);
                        command.Prepare();
                        command.ExecuteScalar();
                    }

                    connection.Close();
                }
            }
            else
            {
                using (var connection = new SqlConnection(ConnectionString))
                using (var cmd = connection.CreateCommand())
                {
                    var itpc_fk = 0;
                    var exif_fk = 6; //Dummy Werte - Fremdschlüssel

                    connection.Open();

                    //create itpc and safe foreign key

                    cmd.CommandText = "INSERT INTO IPTCModel(Keywords, ByLine, CopyrightNotice, Headline, Caption) " +
                                      "output INSERTED.ID " +
                                      "Values (@keywords, @byline, @copyrightnotice, @headline, @caption)";



                    cmd.Parameters.AddWithValue("@keywords", picture.IPTC.Keywords ?? "fun");
                    cmd.Parameters.AddWithValue("@byline", picture.IPTC.ByLine ?? "uknown");
                    cmd.Parameters.AddWithValue("@copyrightnotice", picture.IPTC.CopyrightNotice ?? "no copyright");
                    cmd.Parameters.AddWithValue("@headline", picture.IPTC.Headline ?? "super fun nice pic yeah");
                    cmd.Parameters.AddWithValue("@caption", picture.IPTC.Caption ?? "bla bla bla bla");

                    //cmd.Prepare();

                    itpc_fk = (int)cmd.ExecuteScalar();


                    //Insert PictureModel
                    var command = new SqlCommand(null, connection)
                    {
                        CommandText = "INSERT INTO dbo.PictureModel(FileName, fk_IPTC, fk_EXIF)"
                                      + "VALUES(@filename, @fkiptc, @fkexif);"
                    };


                    var filenameParam = new SqlParameter("@filename", SqlDbType.Text, 255) { Value = picture.FileName };
                    var itpcParam = new SqlParameter("@fkiptc", SqlDbType.Int) { Value = itpc_fk };
                    var exifParam = new SqlParameter("@fkexif", SqlDbType.Int) { Value = exif_fk };

                    command.Parameters.Add(filenameParam);
                    command.Parameters.Add(itpcParam);
                    command.Parameters.Add(exifParam);

                    command.Prepare();

                    command.ExecuteScalar();
                    connection.Close();
                }
            }
        }

        public void DeletePicture(int ID)
        {
            var itcpFk = 0;
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                //select picture for itcp fk
                command.CommandText = "Select fk_IPTC FROM PictureModel " +
                                      "WHERE ID = @id;";
                var idParam = new SqlParameter("@id", SqlDbType.Int) { Value = ID };
                command.Parameters.Add(idParam);
                command.Prepare();
                using (var reader = command.ExecuteReader())
                {
                    reader.Read();
                    itcpFk = int.Parse(reader["fk_IPTC"].ToString());
                }

                //delete picture
                command.CommandText = "DELETE FROM PictureModel " +
                                      "WHERE ID = @id2;";
                idParam = new SqlParameter("@id2", SqlDbType.Int) { Value = ID };
                command.Parameters.Add(idParam);
                command.Prepare();
                command.ExecuteScalar();

                //delete itcp
                command.CommandText = "DELETE FROM IPTCModel " +
                                      "WHERE ID = @id3;";
                idParam = new SqlParameter("@id3", SqlDbType.Int) { Value = itcpFk };
                command.Parameters.Add(idParam);
                command.Prepare();
                command.ExecuteScalar();
                connection.Close();
            }
        }

        public IEnumerable<IPhotographerModel> GetPhotographers()
        {
            var photographers = new List<PhotographerModel>();

            var query = $"Select * FROM Photographermodel";

            using (var connection = new SqlConnection(ConnectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = query;
                command.Prepare();

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var photographer = new PhotographerModel { ID = int.Parse(reader["ID"].ToString()) };//ID = NOT DBNull.Value

                        if (reader["Firstname"] != DBNull.Value) photographer.FirstName = reader["Firstname"].ToString();
                        photographer.LastName = reader["Lastname"].ToString(); //Muss nicht überprüft werden, in der Datenbank NOT NULL
                        if (reader["Birthday"] != DBNull.Value) photographer.BirthDay = DateTime.Parse(reader["Birthday"].ToString());
                        if (reader["Notes"] != DBNull.Value) photographer.Notes = reader["Notes"].ToString();
                        photographers.Add(photographer);
                    }
                }
            }

            return photographers;
        }

        public IPhotographerModel GetPhotographer(int ID)
        {
            var photographer = new PhotographerModel();

            var query = $"Select Firstname, Lastname, Birthday, Notes " +
                        $"FROM Photographermodel WHERE  ID = @ID";
            var idParam = new SqlParameter("@ID", SqlDbType.Int) { Value = ID };

            using (var connection = new SqlConnection(ConnectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = query;
                command.Parameters.Add(idParam);
                command.Prepare();


                using (var reader = command.ExecuteReader())
                {
                    reader.Read();

                    photographer.ID = ID;
                    if (reader["Firstname"] != DBNull.Value) photographer.FirstName = reader["Firstname"].ToString();
                    photographer.LastName = reader["Lastname"].ToString(); //Muss nicht überprüft werden, da es in der Datenbank als Not Null abgebildet wird.
                    if (reader["Birthday"] != DBNull.Value) photographer.BirthDay = DateTime.Parse(reader["Birthday"].ToString());
                    if (reader["Notes"] != DBNull.Value) photographer.Notes = reader["Notes"].ToString();
                }
            }

            return photographer;
        }

        public void Save(IPhotographerModel photographer)
        {
            var query = String.Empty;
            if (Exists(photographer))
            {
                query = "UPDATE dbo.PhotographerModel " +
                        "SET FirstName = @firstname, LastName = @lastname, Birthday = @birthday, Notes = @notes " +
                        "WHERE ID = @id;";
            }
            else
            {
                query = "INSERT INTO dbo.PhotographerModel(FirstName, LastName, Birthday, Notes)"
                        + "VALUES(@firstname, @lastname, @birthday, @notes);";

            }

            // Create and prepare an SQL statement.
            var firstnameParam =
                new SqlParameter("@firstname", SqlDbType.VarChar, 100) { Value = photographer.FirstName };
            var lastnameParam =
                new SqlParameter("@lastname", SqlDbType.VarChar, 50) { Value = photographer.LastName };
            var birthdayParam =
                new SqlParameter("@birthday", SqlDbType.DateTime) { Value = photographer.BirthDay };
            var notesParam = new SqlParameter("@notes", SqlDbType.Text, 255) { Value = photographer.Notes };

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand(null, connection)
                {
                    CommandText = query
                };

                command.Parameters.Add(firstnameParam);
                command.Parameters.Add(lastnameParam);
                command.Parameters.Add(birthdayParam);
                command.Parameters.Add(notesParam);

                // Call Prepare after setting the Commandtext and Parameters.
                command.Prepare();

                // Change parameter values and call ExecuteNonQuery.
                command.ExecuteScalar();
                connection.Close();
            }
        }

        public void DeletePhotographer(int ID)
        {
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = "DELETE FROM PhotographerModel " +
                                      "WHERE ID = @id";


                var idParam = new SqlParameter("@id", SqlDbType.Int) { Value = ID };

                command.Parameters.Add(idParam);

                command.Prepare();

                command.ExecuteScalar();
                connection.Close();
            }
        }

        public IEnumerable<ICameraModel> GetCameras()
        {
            var queryString =
                $"Select ID, Producer, Make, BoughtOn, Notes, ISOLimitGood, ISOLimitAcceptable from CameraModel;";
            var liste = new List<ICameraModel>();

            using (var connection = new SqlConnection(ConnectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = queryString;

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var model = new CameraModel();
                        int.TryParse(reader["ID"].ToString(), out var tempInt);
                        model.ID = tempInt;
                        model.Producer = reader["Producer"].ToString();
                        model.Make = reader["Make"].ToString();
                        if (reader["BoughtOn"] != DBNull.Value) { model.BoughtOn = DateTime.Parse(reader["BoughtOn"].ToString()); }
                        if (reader["Notes"] != DBNull.Value) { model.Notes = reader["Notes"]?.ToString(); }
                        if (reader["ISOLimitGood"] != DBNull.Value) { model.ISOLimitGood = Decimal.Parse(reader["ISOLimitGood"].ToString()); }
                        if (reader["ISOLimitAcceptable"] != DBNull.Value) { model.ISOLimitAcceptable = Decimal.Parse(reader["ISOLimitAcceptable"].ToString()); }
                        liste.Add(model);
                    }
                }
            }
            return liste;
        }

        public ICameraModel GetCamera(int ID)
        {
            var queryString =
                $"Select ID, Producer, Make, BoughtOn, Notes, ISOLimitGood, ISOLimitAcceptable from CameraModel"
                + " WHERE ID = @id;";

            SqlParameter idparam = new SqlParameter("@id", SqlDbType.Int) { Value = ID };

            var model = new CameraModel();

            using (var connection = new SqlConnection(ConnectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = queryString;
                command.Parameters.Add(idparam);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    reader.Read();


                    int.TryParse(reader["ID"].ToString(), out var tempInt);
                    model.ID = tempInt;
                    model.Producer = reader["Producer"]?.ToString();
                    model.Make = reader["Make"]?.ToString();
                    if (reader["BoughtOn"] != DBNull.Value) { model.BoughtOn = DateTime.Parse(reader["BoughtOn"].ToString()); }
                    if (reader["Notes"] != DBNull.Value) { model.Notes = reader["Notes"]?.ToString(); }
                    if (reader["ISOLimitGood"] != DBNull.Value) { model.ISOLimitGood = Decimal.Parse(reader["ISOLimitGood"].ToString()); }
                    if (reader["ISOLimitAcceptable"] != DBNull.Value) { model.ISOLimitAcceptable = Decimal.Parse(reader["ISOLimitAcceptable"].ToString()); }

                }
            }
            return model;
        }

        private bool Exists(IPictureModel picture)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var cmd = new SqlCommand("SELECT COUNT(*) FROM PictureModel WHERE ID = @ID;", connection);

                cmd.Parameters.AddWithValue("@ID", picture.ID);

                var pCount = (int)cmd.ExecuteScalar();

                connection.Close();

                return pCount == 1;
            }
        }

        private bool Exists(IPhotographerModel photographer)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var cmd = new SqlCommand("SELECT COUNT(*) FROM PhotographerModel WHERE ID = @ID;", connection);

                cmd.Parameters.AddWithValue("@ID", photographer.ID);

                var pCount = (int)cmd.ExecuteScalar();

                connection.Close();

                return pCount == 1;
            }
        }

        public void SaveCamera(CameraModel camera)
        {
            //TODO: Save Camera to database
            var query = "INSERT INTO dbo.CameraModel(Producer, Make, BoughtOn, Notes, ISOLimitAcceptable, ISOLimitGood)"
                        + "VALUES(@producer, @make, @boughtOn, @notes, @isoAcc, @isoGood);";

            // Create and prepare an SQL statement.
            var producerParam =
                new SqlParameter("@producer", SqlDbType.Text, 255) { Value = camera.Producer };
            var makeParam =
                new SqlParameter("@make", SqlDbType.Text, 255) { Value = camera.Make };
            var boughtOnParam =
                new SqlParameter("@boughtOn", SqlDbType.DateTime) { Value = camera.BoughtOn };
            var notesParam = new SqlParameter("@notes", SqlDbType.Text, 255) { Value = camera.Notes };
            var isoAccLimit = new SqlParameter("@isoAcc", SqlDbType.Decimal) {Value = camera.ISOLimitAcceptable, Precision = 18, Scale = 0};
            var isoGood = new SqlParameter("@isoGood", SqlDbType.Decimal){Value = camera.ISOLimitGood, Precision = 18, Scale = 0};

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand(null, connection)
                {
                    CommandText = query
                };

                command.Parameters.Add(producerParam);
                command.Parameters.Add(makeParam);
                command.Parameters.Add(boughtOnParam);
                command.Parameters.Add(notesParam);
                command.Parameters.Add(isoAccLimit);
                command.Parameters.Add(isoGood);

                // Call Prepare after setting the Commandtext and Parameters.
                command.Prepare();

                // Change parameter values and call ExecuteNonQuery.
                command.ExecuteScalar();
                connection.Close();
            }
        }

        public void UpdateCamera(ICameraModel cameraModel)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var cmd = new SqlCommand("UPDATE dbo.CameraModel " +
                                         "SET Producer = @producer, Make = @make, BoughtOn = @boughton, Notes = @notes, ISOLimitGood = @isolimitgood, ISOLimitAcceptable = @isolimitacceptable " +
                                         "WHERE ID = @id;", connection);

                var ipParam = new SqlParameter("@id", SqlDbType.Int) { Value = cameraModel.ID };
                var producerParam = new SqlParameter("@producer", SqlDbType.Text, 255) { Value = cameraModel.Producer };
                var makeParam = new SqlParameter("@make", SqlDbType.Text, 255) { Value = cameraModel.Make };
                var boughtonParam = new SqlParameter("@boughton", SqlDbType.Date) { Value = cameraModel.BoughtOn };
                var notesParam = new SqlParameter("@notes", SqlDbType.Text, 255) { Value = cameraModel.Notes };
                var isolimitgoodParam = new SqlParameter("@isolimitgood", SqlDbType.Decimal) { Value = cameraModel.ISOLimitGood, Precision = 18};
                var isolimitacceptabeParam = new SqlParameter("@isolimitacceptable", SqlDbType.Decimal) { Value = cameraModel.ISOLimitAcceptable, Precision = 18};

                cmd.Parameters.Add(ipParam);
                cmd.Parameters.Add(producerParam);
                cmd.Parameters.Add(makeParam);
                cmd.Parameters.Add(boughtonParam);
                cmd.Parameters.Add(notesParam);
                cmd.Parameters.Add(isolimitgoodParam);
                cmd.Parameters.Add(isolimitacceptabeParam);

                cmd.Prepare();
                cmd.ExecuteScalar();

                connection.Close();
            }
        }

        public void DeleteCamera(int ID)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var cmd = new SqlCommand("DELETE FROM dbo.CameraModel " +
                                         "WHERE ID = @id;", connection);

                var idParam = new SqlParameter("@id", SqlDbType.Int) { Value = ID };

                cmd.Parameters.Add(idParam);

                cmd.Prepare();
                cmd.ExecuteScalar();

                connection.Close();
            }
        }
    }
}
