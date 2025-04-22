namespace Examen1_LeonardoMadrigal.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalLibros { get; set; }
        public int TotalUsuariosActivos { get; set; }
        public int TotalUsuariosInactivos { get; set; } // 🔹 AGREGAR ESTA LÍNEA
        //public int AsistentesMesActual { get; set; }
        //public int NoAsistentesMesActual { get; set; }
        public List<TopEventoViewModel> TopEventos { get; set; }
    }

    public class TopEventoViewModel
    {
        public string Titulo { get; set; }
        public int TotalStock { get; set; }
    }
}
