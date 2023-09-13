using ado1.DAL.Entity;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ado1.DAL.DAO
{
    internal class ProductGroupDAO
    {
        private readonly SqlConnection _connection;
        public ProductGroupDAO(SqlConnection connection)
        {
            _connection = connection;
        }
        public List<Entity.ProductGroup> GetAll()
        {
            using SqlCommand command = new();
            command.Connection = _connection;
            command.CommandText = "SELECT * FROM ProductGroups WHERE DeleteDt IS NULL";
            try
            {
                using SqlDataReader reader = command.ExecuteReader();
                var ProductGroups = new List<Entity.ProductGroup>();
                while (reader.Read())  // get result's one row
                {
                    // columns.Add(
                    //     $"Id: {reader.GetGuid(0).ToString()[..4]}..., Name: {reader.GetString(1)}"
                    // );
                    ProductGroups.Add(new()
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        Picture = reader.GetString(3),
                        Price = reader.GetString(5),
                        Quantity = reader.GetString(6),
                    });
                }
                return ProductGroups;
            }
            catch { throw; }
        }
        public void Add(Entity.ProductGroup productGroup)
        {
            using SqlCommand command = new();
            command.Connection = _connection;
            command.CommandText = "INSERT INTO ProductGroups (Id, Name, Description, Picture,Price,Quantity) VALUES (@id, @name, @description, @picture,@price, @quantity)";
            command.Prepare();

            command.Parameters.Add(new SqlParameter("@id", SqlDbType.UniqueIdentifier));
            command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 50));
            command.Parameters.Add(new SqlParameter("@description", SqlDbType.NText));
            command.Parameters.Add(new SqlParameter("@picture", SqlDbType.NVarChar, 50));
            command.Parameters.Add(new SqlParameter("@price", SqlDbType.NVarChar, 50));
            command.Parameters.Add(new SqlParameter("@quantity", SqlDbType.NVarChar, 50));

            command.Parameters[0].Value = productGroup.Id;
            command.Parameters[1].Value = productGroup.Name;
            command.Parameters[2].Value = productGroup.Description;
            command.Parameters[3].Value = productGroup.Picture;
            command.Parameters[4].Value = productGroup.Price;
            command.Parameters[5].Value = productGroup.Quantity;
            command.ExecuteNonQuery();
        }

        public bool Delete(Entity.ProductGroup productGroup)
        {
            using SqlCommand command = new();
            command.Connection = _connection;
            command.CommandText = $@"UPDATE ProductGroups SET DeleteDt = @datetime WHERE Id = @id";
            command.Prepare();
            command.Parameters.Add(new SqlParameter("@id",SqlDbType.UniqueIdentifier));
            command.Parameters.Add(new SqlParameter("@datetime", SqlDbType.DateTime));

            command.Parameters[0].Value = productGroup.Id;
            command.Parameters[1].Value = DateTime.Now;

            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool SaveAndUpdate(Entity.ProductGroup productGroup)
        {
            using SqlCommand command = new();
            command.Connection = _connection;
            command.CommandText = $@"UPDATE ProductGroups SET Name = @name, Description = @description, Picture = @picture, Price = @price, Quantity = @quantity WHERE Id = @id";

            command.Parameters.Add(new SqlParameter("@id", SqlDbType.UniqueIdentifier));
            command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 50));
            command.Parameters.Add(new SqlParameter("@description", SqlDbType.NText));
            command.Parameters.Add(new SqlParameter("@picture", SqlDbType.NVarChar, 50));
            command.Parameters.Add(new SqlParameter("@price", SqlDbType.NVarChar, 50));
            command.Parameters.Add(new SqlParameter("@quantity", SqlDbType.NVarChar, 50));

            command.Parameters[0].Value = productGroup.Id;
            command.Parameters[1].Value = productGroup.Name;
            command.Parameters[2].Value = productGroup.Description;
            command.Parameters[3].Value = productGroup.Picture;
            command.Parameters[4].Value = productGroup.Price;
            command.Parameters[5].Value = productGroup.Quantity;

            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
