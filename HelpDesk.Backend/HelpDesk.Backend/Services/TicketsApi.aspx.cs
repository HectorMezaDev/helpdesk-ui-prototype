using System;
using System.Web.Services;
using HelpDesk.Backend.Data;
using HelpDesk.Backend.Models;
using HelpDesk.Backend.Utils;

namespace HelpDesk.Backend.Services
{
    public partial class TicketsApi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [WebMethod]
        public static object RegistrarTicket(Ticket ticket)
        {
            try
            {
                TicketRepository repo = new TicketRepository();

                string cadenaOriginal = ticket.SolicitanteID + ticket.Departamento + ticket.Titulo + DateTime.Now.ToString();
                ticket.HashSHA256 = SeguridadUtil.GenerarSHA256(cadenaOriginal);

                int nuevoId = repo.RegistrarTicket(ticket);

                if (nuevoId > 0)
                {
                    return new { success = true, message = "Ticket registrado con éxito", id = nuevoId, hash = ticket.HashSHA256 };
                }
                return new { success = false, message = "No se pudo registrar el ticket en la base de datos." };
            }
            catch (Exception ex)
            {
                return new { success = false, message = ex.Message };
            }
        }

        [WebMethod]
        public static object ActualizarEstadoTicket(int ticketId, int tecnicoId, string nuevoEstado)
        {
            try
            {
                TicketRepository repo = new TicketRepository();
                repo.ActualizarEstadoTicket(ticketId, tecnicoId, nuevoEstado);

                return new { success = true, message = "Estado actualizado correctamente." };
            }
            catch (Exception ex)
            {
                return new { success = false, message = ex.Message };
            }
        }
    }
}