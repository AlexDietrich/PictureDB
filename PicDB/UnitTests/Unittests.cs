using System.Linq;
using BIF.SWE2.Interfaces;
using NUnit.Framework;
using PicDB.Models;
using PicDB.ViewModels;

namespace PicDB.UnitTests
{
    [TestFixture]
    class Unittests
    {
        [Test]
        public void TestConectionStringFromConfigFileNotNull()
        {
            GlobalInformation.ReadConfigFile();
            var connectionString = GlobalInformation.ConnectionString;
            Assert.NotNull(connectionString);
        }

        [Test]
        public void TestPicturePathFromConfigFileNotNull()
        {
            GlobalInformation.ReadConfigFile();
            var path = GlobalInformation.Path;
            Assert.NotNull(path);
        }

        [Test]
        public void TestGlobalInformationConnectionstring()
        {
            var expectedConnctionstring = "Data Source=DESKTOP-V42FU8U;Initial Catalog=PicDB;Integrated Security=True"; 
            GlobalInformation.ReadConfigFile(); 
            var connectionString = GlobalInformation.ConnectionString;
            Assert.AreEqual(expectedConnctionstring, connectionString);
        }

        [Test]
        public void TestGlobalInformationPathToPicture()
        {
            var expectedPath = "C:\\Users\\Alexa\\Google Drive\\Studium\\4. Semester\\Software Engineering 2\\Projekt\\SWE2-CS\\deploy\\Personal_Pictures";
            GlobalInformation.ReadConfigFile();
            var path = GlobalInformation.Path; 
            Assert.AreEqual(expectedPath, path);
        }

        [Test]
        public void TestNumberOfPicturesFromDAL()
        {
            GlobalInformation.ReadConfigFile(); 
            var dal = new DataAccessLayer();
            var expectedNumberOfPictures = 6; 

            var picture = dal.GetPictures(null, null, null, null); 

            Assert.AreEqual(expectedNumberOfPictures, picture.ToList().Count);
        }

        [Test]
        public void TestNumberOfCamerasFromDAL()
        {
            GlobalInformation.ReadConfigFile();
            var dal = new DataAccessLayer();
            var expectedNumberOfCameras = 5; 

            var cameras = dal.GetCameras(); 

            Assert.AreEqual(expectedNumberOfCameras, cameras.ToList().Count);
        }

        [Test]
        public void TestGetSingleCameraFromDAL()
        {
            GlobalInformation.ReadConfigFile();
            var dal = new DataAccessLayer();
            var expectedIDofCamera = 1;

            var camera = dal.GetCamera(1); 

            Assert.AreEqual(expectedIDofCamera, camera.ID);
        }

        [Test]
        public void TestGetSingleCameraWithWrongID()
        {
            GlobalInformation.ReadConfigFile();
            var dal = new DataAccessLayer();
            var expectedIDofCamera = 2;

            var camera = dal.GetCamera(1);

            Assert.AreNotEqual(expectedIDofCamera, camera.ID);
        }

        [Test]
        public void TestNumberOfPhotographersFromDal()
        {
            GlobalInformation.ReadConfigFile();
            var dal = new DataAccessLayer();
            var expectedNumberOfPhotographers = 5;

            var photographer = dal.GetPhotographers(); 

            Assert.AreEqual(expectedNumberOfPhotographers, photographer.ToList().Count);
        }

        [Test]
        public void TestGetSinglePhotographerFromDAL()
        {
            GlobalInformation.ReadConfigFile();
            var dal = new DataAccessLayer();
            var expectedIdOfPhotographer = 1;

            var photographer = dal.GetPhotographer(1);

            Assert.AreEqual(expectedIdOfPhotographer, photographer.ID);
        }

        [Test]
        public void TestGetSinglePhotographerWithWrongID()
        {
            GlobalInformation.ReadConfigFile();
            var dal = new DataAccessLayer();
            var expectedIdOfPhotographer = 2;

            var photographer = dal.GetPhotographer(1);

            Assert.AreNotEqual(expectedIdOfPhotographer, photographer.ID);
        }


        [Test]
        public void TestExistCheckForPicturesFromDAL()
        {
            GlobalInformation.ReadConfigFile();
            var dal = new DataAccessLayer();
            var picture = dal.GetPicture(2018);

            var exists = dal.Exists(picture); 

            Assert.IsTrue(exists);
        }

        [Test]
        public void TestExistCheckForPhotographerFromDAL()
        {
            GlobalInformation.ReadConfigFile();
            var dal = new DataAccessLayer();
            var photographer = dal.GetPhotographer(1);

            var exists = dal.Exists(photographer); 

            Assert.IsTrue(exists);
        }

        [Test]
        public void TestIsoRatingGood()
        {
            var camera = new CameraModel {ISOLimitAcceptable = 1200, ISOLimitGood = 1000}; 
            var cameraViewModel = new CameraViewModel(camera);

            var isoRating = cameraViewModel.TranslateISORating(200); 

            Assert.AreEqual(ISORatings.Good, isoRating);
        }

        [Test]
        public void TestIsoRatingAcceptable()
        {
            var camera = new CameraModel { ISOLimitAcceptable = 1200, ISOLimitGood = 1000 };
            var cameraViewModel = new CameraViewModel(camera);

            var isoRating = cameraViewModel.TranslateISORating(1100);

            Assert.AreEqual(ISORatings.Acceptable, isoRating);
        }

        [Test]
        public void TestIsoRatingNoisey()
        {
            var camera = new CameraModel { ISOLimitAcceptable = 1200, ISOLimitGood = 1000 };
            var cameraViewModel = new CameraViewModel(camera);

            var isoRating = cameraViewModel.TranslateISORating(1300);

            Assert.AreEqual(ISORatings.Noisey, isoRating);
        }

        [Test]
        public void TestIsoRatingNotDefined()
        {
            var camera = new CameraModel { ISOLimitAcceptable = 1200, ISOLimitGood = 1000 };
            var cameraViewModel = new CameraViewModel(camera);

            var isoRating = cameraViewModel.TranslateISORating(0);

            Assert.AreEqual(ISORatings.NotDefined, isoRating);
        }
    }
}
