using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Ware.Data
{
    public interface WarInterface
    {
        Task<int> addToWareAsymc(Prod prodIN);
    }

    public class WarService : WarInterface
    {
        string conString = "Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s18290;Integrated Security=True";
        public Task<int> addToWareAsymc(Prod prodIn)
        {
           // int fin = 0;

            try
            {
                if (prodIn.Amount == 0)
                    throw new Exception("Error 404 - Ilość równa 0");

                int test = validator(prodIn);
               // Console.WriteLine(test);
                if (test == 12)
                    throw new Exception("Error 404 - Brak produktu");
                if (test == 21)
                    throw new Exception("Error 404 - Brak hurtowni");
                if (test == 30)
                    throw new Exception("Error 404 - Brak produktu i hurtowni");
                if (test == 3)
                {
                    if ((isOrder(prodIn).Result == 0))
                    {
                        throw new Exception("Error 404 - Brak zlecenia");
                       
                    }
                    else
                    {
                        tmpOrder tmp = orderGetter(prodIn).Result;
                        if (orderValExec(tmp).Result != 0)
                            throw new Exception("Error 404 - Zlecenie zrealizowane");
                        else
                        {
                            update(tmp);


                            ProdForTest inserted = whatProd(tmp.IdProduct);
                            return idOfLast(tmp, prodIn, inserted);

                        }


                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }




            return def();
        }

        public async Task<int> def()
        {
            int def = 0;
            return def;
        }


            public async Task<int> onlyId(){

           using SqlConnection con = new SqlConnection(conString);
            using SqlCommand com = new SqlCommand();
         
            com.CommandText = "select top 1 IdProductWarehouse as maxi from Product_Warehouse order by CreatedAt";
            com.Connection = con;
            await con.OpenAsync();
            SqlDataReader dr = await com.ExecuteReaderAsync();
            dr.Read();
            int maximun = int.Parse(dr["maxi"].ToString());

            con.Dispose();
            return maximun;



        }


    public async Task<int> idOfLast(tmpOrder orderIN, Prod prodReq, ProdForTest test)
        {
           using SqlConnection con = new SqlConnection(conString);
           using SqlCommand com = new SqlCommand();
            com.CommandText = "INSERT INTO Product_Warehouse (IdWarehouse,IdProduct,IdOrder,Amount,Price,CreatedAt)" +
                "VALUES (@a,@b,@c,@d,@e,@f);";
            com.Parameters.AddWithValue("@a", prodReq.IdWarehouse);
            com.Parameters.AddWithValue("@b", orderIN.IdProduct);
            com.Parameters.AddWithValue("@c", orderIN.IdOrder);
            com.Parameters.AddWithValue("@d", orderIN.Amount);
            com.Parameters.AddWithValue("@e", (orderIN.Amount * test.Price));
            com.Parameters.AddWithValue("@f", DateTime.Now);
            com.Connection = con;
            await con.OpenAsync();
            com.ExecuteNonQuery();
            con.Dispose();

            return onlyId().Result;
        }


        public ProdForTest whatProd(int Id)
        {
            List<ProdForTest> products = tmpProd().Result.ToList();
            foreach (ProdForTest which in products)
            {
                if (which.IdProduct.Equals(Id))
                {
                    return which;
                }
            }
            return null;

        }

        public async void update(tmpOrder orIn)
        {
           using SqlConnection con = new SqlConnection(conString);
           using  SqlCommand com = new SqlCommand();
            com.CommandText = "UPDATE \"Order\" SET IdProduct = @a, Amount = @v, CreatedAt = @c, FulfilledAt = @d WHERE IdOrder = @e;";
            com.Parameters.AddWithValue("@a", orIn.IdProduct);
            com.Parameters.AddWithValue("@v", orIn.Amount);
            com.Parameters.AddWithValue("@c", orIn.CreateAt);
            com.Parameters.AddWithValue("@d", DateTime.Now);
            com.Parameters.AddWithValue("@e", orIn.IdOrder);
          

            com.Connection = con;

            await con.OpenAsync();

            com.ExecuteNonQuery();
            con.Dispose();


        } 



        public int validator(Prod test)
        {
            int counter = 0;
            int counterp = 0;
            int counterw = 0;

            // counter = 3 -- ok
            // counter = 12 -- brak produktu
            // counter = 21 -- brak warchouse
            // counter = 30 -- brak war i produktu
            List<WarToTest> warh = tmpWar().Result.ToList();
            List<ProdForTest> products = tmpProd().Result.ToList();

            foreach (ProdForTest x in products)
            {
                if (x.IdProduct.Equals(test.IdProduct))
                {
                  //  Console.WriteLine("----------------------");
                  //  Console.WriteLine(x.IdProduct);
                 //   Console.WriteLine(test.IdProduct);
                    counterp += 1;
                   // Console.WriteLine("----------------------");
                }
            }
           // int tmpo = 0;
           // tmpo = counterp; /// products.Count();
           // Console.WriteLine(tmpo);
            counter += counterp; // products.Count();
            if (counter != 1)
                counter = 10;
            Console.WriteLine(counter);

            foreach (WarToTest x in warh)
            {
                if (x.IdWarehouse.Equals(test.IdWarehouse))
                {
                   // Console.WriteLine("----------------------");
                  // Console.WriteLine(x.IdWarehouse);
                  //  Console.WriteLine(test.IdWarehouse);
                  //  Console.WriteLine("----------------------");
                    counterw += 2;
                }
            }
            int tmp = 0;
            tmp = counterw; /// warh.Count();
            if (tmp != 2)
                tmp = 20;

          //  Console.WriteLine(tmp);
            counter += tmp;

            return counter;



        }

        public async Task<int> orderValExec(tmpOrder toTest)
        {
            using SqlConnection con = new SqlConnection(conString);
            using  SqlCommand com = new SqlCommand();
        
            com.CommandText = "SELECT COUNT(*) as isExec from dbo.Product_Warehouse WHERE idOrder=@a";
            com.Parameters.AddWithValue("@a", toTest.IdOrder);
            com.Connection = con;
            await con.OpenAsync();
            SqlDataReader dr = await com.ExecuteReaderAsync();
            dr.Read();
            int count = int.Parse(dr["isExec"].ToString());

            con.Dispose();
            return count;
        }



        public async Task<tmpOrder> orderGetter(Prod prod)
        {
            using SqlConnection con = new SqlConnection(conString);
            using SqlCommand com = new SqlCommand();
            com.CommandText = "SELECT top 1 * FROM \"Order\" WHERE IdProduct = @a and Amount = @b";
            com.Parameters.AddWithValue("@a", prod.IdProduct);
            com.Parameters.AddWithValue("@b", prod.Amount);
             com.Connection = con;

            await con.OpenAsync();
            SqlDataReader dr = await com.ExecuteReaderAsync();
            dr.Read();

            tmpOrder order = new tmpOrder
            {
                IdOrder = int.Parse(dr["IdOrder"].ToString()),
                IdProduct = int.Parse(dr["IdProduct"].ToString()),
                Amount = int.Parse(dr["Amount"].ToString()),
                CreateAt = DateTime.Parse(dr["CreatedAt"].ToString())
            };
            con.Dispose();
            return order;
        }


    

        public async Task<int> isOrder(Prod prod)
        {       
              using  SqlConnection con = new SqlConnection(conString);
              using SqlCommand com = new SqlCommand();            
                com.CommandText = "SELECT COUNT(*) as howMany FROM \"Order\" WHERE IdProduct = @a and Amount = @b";
                com.Parameters.AddWithValue("@a", prod.IdProduct);
                com.Parameters.AddWithValue("@b", prod.Amount);
                com.Connection = con;
                await con.OpenAsync();
                SqlDataReader dr = await com.ExecuteReaderAsync();
                dr.Read();
                int count = int.Parse(dr["howMany"].ToString());
                con.Dispose();
                return count;
        }







        public async Task<IEnumerable<WarToTest>> tmpWar()
        {
           using SqlConnection con = new SqlConnection(conString);
           using SqlCommand com = new SqlCommand();
           com.CommandText = "SELECT * FROM dbo.Warehouse";
           com.Connection = con;
           await con.OpenAsync();
           SqlDataReader dr = await com.ExecuteReaderAsync();
           
            List< WarToTest> list = new List<WarToTest>();
            while (await dr.ReadAsync())
            {
                await Task.Delay(300);
                list.Add(new WarToTest
                {
                    IdWarehouse = int.Parse(dr["IdWarehouse"].ToString()),
                    Name = dr["Name"].ToString(),
                    Address = dr["Address"].ToString()
                });
            }

            con.Dispose();
            return list;


        }

        public async Task<IEnumerable<ProdForTest>> tmpProd()
        {
            using SqlConnection con = new SqlConnection(conString);
            using SqlCommand com = new SqlCommand();
            com.CommandText = "SELECT * FROM dbo.Product";
            com.Connection = con;
            await con.OpenAsync();
            SqlDataReader dr = await com.ExecuteReaderAsync();
            List< ProdForTest> list = new List<ProdForTest>();
            while (await dr.ReadAsync())
            {
                await Task.Delay(300);
                list.Add(new ProdForTest
                {
                    IdProduct = int.Parse(dr["IdProduct"].ToString()),
                    Name = dr["Name"].ToString(),
                    Description = dr["Description"].ToString(),
                    Price = double.Parse(dr["Price"].ToString())
                });
            }

            con.Dispose();
            return list;
        }









    }


  

}
