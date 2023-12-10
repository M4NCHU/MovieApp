using MovieApp.Respositories.Abstract;

namespace MovieApp.Respositories.Implementation
{
    // Implementacja interfejsu FileService
    public class FileService : IFileService
    {
        // IWebHostEnvironment - Zawiera informacje o środowisku hostingu internetowego, w którym działa aplikacja.
        private readonly IWebHostEnvironment environment;
        public FileService(IWebHostEnvironment env) 
        {
            this.environment = env;
        }
        // Tuple -  uporządkowany ciąg, niezmienny, stałej wielkości i różnorodnych obiektów
        // Zapisz zdjęcie
        public Tuple<int, string> SaveImage(IFormFile imageFile)
        {
            // Próbuj
            try
            {
                // Pobranie ścieżki do wwwroot
                var wwwPath = this.environment.WebRootPath;
                // Połącznie ścieżek (docelowo zapis zdjęć w folderze Uploads)
                var path = Path.Combine(wwwPath, "Uploads");
                // Utwórz folder jeżeli nie istnieje
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // Sprawdz dozwolone rozszerzenia
                var ext = Path.GetExtension(imageFile.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
                // Jeżeli plik posiada niedozwolone rozszerzenie
                if (!allowedExtensions.Contains(ext))
                {
                    string msg = string.Format("Tylko {0} są dozwolone", string.Join(",", allowedExtensions));
                    // Tuple -  uporządkowany ciąg, niezmienny, stałej wielkości i różnorodnych obiektów
                    return new Tuple<int, string>(0, msg);
                }
                // Guid - tworzy i wyświetla wartości dwóch GUIDs
                string uniqueString = Guid.NewGuid().ToString();
                // Utwórz unikatową nazwę zdjęcia
                var newFileName = uniqueString + ext;
                // połącz ścieżkę z unikatową nazwą pliku
                var fileWithPath = Path.Combine(path, newFileName);
                // Tworzenie ścieżki pliku
                var stream = new FileStream(fileWithPath, FileMode.Create);
                // Przekopiuj plik do danej ścieżki
                imageFile.CopyTo(stream);
                // Zamknij ścieżkę
                stream.Close();
                return new Tuple<int, string>(1, newFileName);
            }
            catch (Exception ex)
            {
                return new Tuple<int, string>(0, "Wystąpił błąd");
            }
        }

        // Usuń zdjęcie
        public bool DeleteImage(string imageFileName)
        {
            // Próbuj
            try
            {
                // Pobranie ścieżki do wwwroot
                var wwwPath = this.environment.WebRootPath;
                // Połącznie ścieżki z nazwą pliku
                var path = Path.Combine(wwwPath, "Uploads\\", imageFileName);
                // Sprawdz czy plik istnieje, jeżeli tak to usuń
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
