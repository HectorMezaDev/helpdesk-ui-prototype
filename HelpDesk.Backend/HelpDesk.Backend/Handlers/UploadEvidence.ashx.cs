using System;
using System.IO;
using System.Web;

namespace HelpDesk.Backend.Handlers
{
    public class UploadEvidence : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            try
            {
                if (context.Request.Files.Count > 0)
                {
                    HttpPostedFile archivo = context.Request.Files[0];
                    string extension = Path.GetExtension(archivo.FileName).ToLower();

                    if (extension == ".pdf" || extension == ".png" || extension == ".jpg" || extension == ".jpeg")
                    {
                        if (archivo.ContentLength <= 5 * 1024 * 1024)
                        {
                            string nombreUnico = Guid.NewGuid().ToString() + extension;
                            string carpetaDestino = context.Server.MapPath("~/App_Data/Uploads/");

                            if (!Directory.Exists(carpetaDestino))
                            {
                                Directory.CreateDirectory(carpetaDestino);
                            }

                            string rutaFisica = Path.Combine(carpetaDestino, nombreUnico);
                            archivo.SaveAs(rutaFisica);

                            context.Response.Write("{\"success\": true, \"message\": \"Archivo subido correctamente\", \"ruta\": \"" + nombreUnico + "\"}");
                            return;
                        }
                    }
                }
                context.Response.Write("{\"success\": false, \"message\": \"Archivo inválido o excede el límite de 5MB.\"}");
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"success\": false, \"message\": \"" + ex.Message + "\"}");
            }
        }

        public bool IsReusable => false;
    }
}