using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace cliniPetApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegServicioController : Controller
    {
        private readonly IConfiguration _configuration;

        public RegServicioController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarServicio([FromBody] ServicioRequest request)
        {
            if (request == null) 
            { 
                return BadRequest(new { error = "The request field is required." }); 
            }

            string connectionString = _configuration.GetConnectionString("BDConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync(); using (SqlCommand command = new SqlCommand("RegistrarServicio", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    //parametros de procedimiento almacenado
                    command.Parameters.AddWithValue("@Nombre", request.Nombre);
                    command.Parameters.AddWithValue("@Descripción", request.Descripcion);
                    command.Parameters.AddWithValue("@Costo", request.Costo);
                    command.Parameters.AddWithValue("@IDMascota", request.IDMascota);
                    command.Parameters.AddWithValue("@IDCita", request.IDCita);

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        return Ok(new { mensaje = "Servicio registrado exitosamente" });
                    }
                    catch (SqlException ex)
                    {
                        return BadRequest(new { error = ex.Message });
                    }
                }
            }
        }
    }

    //clase para recibir los datos del cliente
    public class ServicioRequest 
    { 
        public string Nombre { get; set; } 
        public string Descripcion { get; set; } 
        public decimal Costo { get; set; } 
        public int IDMascota { get; set; } 
        public int IDCita { get; set; } 
    }
}



