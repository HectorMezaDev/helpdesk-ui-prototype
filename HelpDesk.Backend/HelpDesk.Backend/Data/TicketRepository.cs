using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using HelpDesk.Backend.Models;

namespace HelpDesk.Backend.Data
{
    public class TicketRepository
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["HelpDeskDB"].ConnectionString;

        public int RegistrarTicket(Ticket ticket)
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_RegistrarTicket", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SolicitanteID", ticket.SolicitanteID);
                    cmd.Parameters.AddWithValue("@Departamento", ticket.Departamento);
                    cmd.Parameters.AddWithValue("@Categoria", ticket.Categoria);
                    cmd.Parameters.AddWithValue("@Prioridad", ticket.Prioridad);
                    cmd.Parameters.AddWithValue("@Titulo", ticket.Titulo);
                    cmd.Parameters.AddWithValue("@Descripcion", ticket.Descripcion);
                    cmd.Parameters.AddWithValue("@HashSHA256", ticket.HashSHA256);

                    conexion.Open();
                    object resultado = cmd.ExecuteScalar();
                    return resultado != null ? Convert.ToInt32(resultado) : 0;
                }
            }
        }

        public void ActualizarEstadoTicket(int ticketId, int tecnicoId, string nuevoEstado)
        {
            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ActualizarEstadoTicket", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TicketID", ticketId);
                    cmd.Parameters.AddWithValue("@TecnicoAsignadoID", tecnicoId);
                    cmd.Parameters.AddWithValue("@NuevoEstado", nuevoEstado);

                    conexion.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}