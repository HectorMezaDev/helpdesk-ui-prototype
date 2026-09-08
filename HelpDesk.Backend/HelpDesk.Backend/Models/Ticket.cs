namespace HelpDesk.Backend.Models
{
    public class Ticket
    {
        public int TicketID { get; set; }
        public int SolicitanteID { get; set; }
        public int? TecnicoAsignadoID { get; set; }
        public string Departamento { get; set; }
        public string Categoria { get; set; }
        public string Prioridad { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string RutaEvidencia { get; set; }
        public string Estado { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string HashSHA256 { get; set; }
        public string Solicitante { get; set; }
        public string TecnicoAsignado { get; set; }
    }
}