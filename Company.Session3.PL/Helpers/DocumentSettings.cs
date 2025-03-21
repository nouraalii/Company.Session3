namespace Company.Session3.PL.Helpers
{
    public static class DocumentSettings
    {
        //1.Uplode
        //ImageName
        public static string UplodeFile(IFormFile file , string folderName)
        {
            //1.Get Folder Location 

            //string folderPath = "D:\\routebackend\\MVC\\Company.Session3\\Company.Session3.PL\\wwwroot\\files\\"+folderName;

            //var folderPath = Directory.GetCurrentDirectory()+ "\\wwwroot\\files\\" + folderName;

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", folderName);

            //2. Get File Name and make it unique

            var fileName = $"{Guid.NewGuid()}{file.FileName}";

            //File Path

            var filePath = Path.Combine(folderPath, fileName);

            var fileStream = new FileStream(filePath ,FileMode.Create);

            file.CopyTo(fileStream);

            return fileName ;
        }


        //2.Delete

        public static void DeleteFile(string fileName , string folderName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", folderName, fileName);

            if (File.Exists(filePath)) 
            {
                File.Delete(filePath);
            }

        }

    }
}
