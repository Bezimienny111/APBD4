using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Ware.Data;

namespace Ware.Controllers
{
    [ApiController]
    [Route("api / warehouses2")]
    public class WarehousesController2 : ControllerBase
    {
        string conString = "Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s18290;Integrated Security=True";

        [HttpPost]
        public async Task<int> GetAllOWarehouses(Prod Id)
        {
            using var con = new SqlConnection(conString);
            using var com = new SqlCommand("AddProductToWarehouse", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@IdProduct", Id.IdProduct);
            com.Parameters.AddWithValue("@IdWarehouse", Id.IdWarehouse);
            com.Parameters.AddWithValue("@Amount", Id.Amount);
            com.Parameters.AddWithValue("@CreatedAt", Id.CreatedAt);

            await con.OpenAsync();
            var outParm = new SqlParameter("NewId", SqlDbType.Int);
            outParm.Direction = ParameterDirection.Output;
            con.Close();
            return 1;

        }


    }
}
