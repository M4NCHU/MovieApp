namespace MovieApp.Respositories.Abstract
{
    // interfejs dowawania plików (zdjęć)
    public interface IFileService
    {
        // Tuple -  uporządkowany ciąg, niezmienny, stałej wielkości i różnorodnych obiektów
        // Zapisz zdjęcie
        public Tuple<int, string> SaveImage(IFormFile imageFile);
        // Usuń zdjęcie
        public bool DeleteImage(string imageFileName);
    }
}
